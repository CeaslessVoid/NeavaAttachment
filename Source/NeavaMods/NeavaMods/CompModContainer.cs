using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static NeavaMods.CompProperties_WeaponModContainer;

namespace NeavaMods
{
    public class CompProperties_ModContainer : CompProperties
    {
        public CompProperties_ModContainer()
        {
            compClass = typeof(CompModContainer);
        }

    }
    public class CompModContainer : ThingComp, IThingHolder, ISearchableContents
    {
        public CompProperties_ModContainer Props
        {
            get
            {
                return (CompProperties_ModContainer)props;
            }
        }


        public ThingOwner SearchableContents
        {
            get
            {
                return innerContainer;
            }
        }

        public List<ThingWithComps> ContainedWeaponMods
        {
            get
            {
                return innerContainer
                    .OfType<ThingWithComps>()
                    .Where(t => t.def is ModificationDef)
                    .ToList();
            }
        }

        public override void PostPostMake()
        {
            base.PostPostMake();
            innerContainer = new ThingOwner<Thing>(this);
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return innerContainer;
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
            base.PostDestroy(mode, previousMap);
        }

        public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
        {
            if (mode != DestroyMode.WillReplace)
            {
                EjectContents(map);
            }
            for (int i = 0; i < leftToLoad.Count; i++)
            {
                ((WeaponModComp)leftToLoad[i]).targetContainer = null;
            }
            leftToLoad.Clear();
        }

        public override void PostDrawExtraSelectionOverlays()
        {
            for (int i = 0; i < leftToLoad.Count; i++)
            {
                if (leftToLoad[i].Map == parent.Map)
                {
                    GenDraw.DrawLineBetween(parent.DrawPos, leftToLoad[i].DrawPos);
                }
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (this.parent.Faction == Faction.OfPlayer && this.innerContainer.Any)
            {
                yield return new Command_Action
                {
                    defaultLabel = "EjectAll".Translate(),
                    defaultDesc = "EjectAllDesc".Translate(),
                    icon = CompModContainer.EjectTex.Texture,
                    action = delegate ()
                    {
                        EjectContents(parent.Map);
                    }
                };
            }
            yield break;
        }

        public void EjectContents(Map destMap = null)
        {
            if (destMap == null)
            {
                destMap = parent.Map;
            }
            IntVec3 dropLoc = parent.def.hasInteractionCell ? parent.InteractionCell : parent.Position;
            innerContainer.TryDropAll(dropLoc, destMap, ThingPlaceMode.Near, null, null, true);
        }

        public override string CompInspectStringExtra()
        {
            return "WeaponModsStored".Translate() + string.Format(": {0} \n", innerContainer.Count) + "CasketContains".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Deep.Look<ThingOwner>(ref innerContainer, "innerContainer", new object[]
            {
                this
            });
            Scribe_Collections.Look<Thing>(ref leftToLoad, "leftToLoad", LookMode.Reference, Array.Empty<object>());
        }

        public List<Thing> leftToLoad = new List<Thing>();

        public ThingOwner innerContainer;

        private static readonly CachedTexture EjectTex = new CachedTexture("UI/Gizmos/EjectAll");

    }
}
