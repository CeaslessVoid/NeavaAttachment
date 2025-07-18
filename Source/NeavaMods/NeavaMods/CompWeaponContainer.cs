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
    public class CompWeaponContainer : CompThingContainer
    {
        public override bool Accepts(Thing thing)
        {
            return !(thing is Building) && thing.TryGetComp<CompWeaponModContainer>() != null;
        }

        public override bool Accepts(ThingDef thingDef)
        {
            return !Empty && thingDef.HasComp<CompWeaponModContainer>();
        }

        //public override string CompInspectStringExtra()
        //{
        //    TaggedString label = base.Empty ? "Nothing".Translate() : base.ContainedThing.LabelCap;
        //    return ("Contents".Translate() + ": " + label).Trim();
        //}
    }
}
