using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class WeaponModComp : ThingWithComps
    {
        public ModEffectDef effect;
        public override string DescriptionDetailed
        {
            get
            {
                return DescriptionFlavor;
            }
        }

        public override string DescriptionFlavor
        {
            get
            {
                string baseDesc = this.def.description ?? "";

                if (effect == null)
                    return baseDesc;

                string effectLabel = effect.label ?? "Unknown";
                string effectDesc = effect.description ?? "";

                return $"{baseDesc}\n\n" +
                       $"{effectLabel} ({effect.Drain})\n" +
                       $"{effectDesc}";
            }
        }

        public override string LabelNoCount
        {
            get
            {
                string baseLabel = GenLabel.ThingLabel(this, 1, true, true);

                if (effect == null || string.IsNullOrEmpty(effect.label))
                    return baseLabel;

                return $"{baseLabel} [{effect.label}]";
            }
        }

        public override string GetInspectString()
        {
            string text = base.GetInspectString();

            if (effect != null)
            {
                string effectDesc = effect.description ?? "";

                return "Drain".Translate(effect.Drain) + "\n" + effectDesc;
            }

            return text;
        }

        public override void PostMake()
        {
            base.PostMake();
            this.effect = ModUtils.GenerateModEffect((ModificationDef)this.def);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<ModEffectDef>(ref this.effect, "effect");
        }
    }
}
