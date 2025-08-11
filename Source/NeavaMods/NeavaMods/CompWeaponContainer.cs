//using RimWorld;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using Verse;
//using Verse.Sound;
//using static NeavaMods.CompProperties_WeaponModContainer;

//namespace NeavaMods
//{
//    public class CompWeaponContainer : CompThingContainer
//    {
//        public override bool Accepts(Thing thing)
//        {
//            return !(thing is Building) && thing.TryGetComp<CompWeaponModContainer>() != null;
//        }

//        public override bool Accepts(ThingDef thingDef)
//        {
//            return !Empty && thingDef.HasComp<CompWeaponModContainer>();
//        }

//        public override string CompInspectStringExtra()
//        {
//            TaggedString label = base.Empty ? "Nothing".Translate() : base.ContainedThing.LabelCap;
//            return ("Contents".Translate() + ": " + label).Trim();
//        }
//    }

//    public class Building_WeaponMod : Building
//    {
//        public float ScanRadius => 10f;
//        public bool PowerOn
//        {
//            get
//            {
//                if (power == null)
//                {
//                    power = base.GetComp<CompPowerTrader>();
//                }
//                CompPowerTrader compPowerTrader = power;
//                return compPowerTrader != null && compPowerTrader.PowerOn;
//            }
//        }

//        //public override void DrawExtraSelectionOverlays()
//        //{
//        //    base.DrawExtraSelectionOverlays();

//        //    GenDraw.DrawRadiusRing(Position, ScanRadius);   
//        //}

//        public ThingWithComps HoldingItem
//        {
//            get
//            {
//                CompThingContainer containerComp = ContainerComp;
//                return ((containerComp != null) ? containerComp.ContainedThing : null) as ThingWithComps;
//            }
//        }

//        public CompThingContainer ContainerComp
//        {
//            get
//            {
//                if (container == null)
//                {
//                    container = base.GetComp<CompThingContainer>();
//                }
//                return container;
//            }
//        }

//        public CompWeaponModContainer WeaponMod
//        {
//            get
//            {
//                ThingWithComps holdingItem = HoldingItem;
//                if (holdingItem == null)
//                {
//                    return null;
//                }
//                return holdingItem.GetComp<CompWeaponModContainer>();
//            }
//        }

//        public List<ModEffectDef> Modifications
//        {
//            get
//            {
//                return WeaponMod?.ModsSlotsListForReading;
//            }
//        }

//        protected CompPowerTrader power;

//        protected CompThingContainer container;

//        public override void ExposeData()
//        {
//            base.ExposeData();
//        }

//        public void InsertItem(ThingWithComps item)
//        {
//            ContainerComp.innerContainer.Take(item);
//        }

//        public virtual void ExtractItem()
//        {
//            if (ContainerComp.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, true))
//            {
//                return;
//            }
//            SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
//        }

//        public override IEnumerable<Gizmo> GetGizmos()
//        {
//            List<Gizmo> list = Enumerable.ToList<Gizmo>(base.GetGizmos());
//            list.Add(CreateModifyGizmo());
//            list.Add(CreateExtractItemGizmo());

//            return list;
//        }

//        protected Gizmo CreateModifyGizmo()
//        {
//            return new Command_Action
//            {
//                Disabled = (!PowerOn || HoldingItem == null),
//                action = delegate ()
//                {
//                    Window_WeaponMod.ToggleWindow(this);
//                }
//            };
//        }

//        protected Gizmo CreateExtractItemGizmo()
//        {
//            return new Command_Action
//            {
//                icon = ContentFinder<Texture2D>.Get("UI/Designators/Open", false),
//                defaultLabel = "TakeOut".Translate(),
//                defaultDesc = "TakeOutDesc".Translate(),
//                Disabled = (HoldingItem == null),
//                disabledReason = "Empty".Translate(),
//                action = delegate ()
//                {
//                    ExtractItem();
//                    SoundDefOf.DropElement.PlayOneShot(this);
//                }
//            };
//        }

//    }

//}
