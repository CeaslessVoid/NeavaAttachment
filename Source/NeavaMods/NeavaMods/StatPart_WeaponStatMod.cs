using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class StatPart_WeaponStatMod : StatPart
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            if (!req.HasThing)
                return;

            var thing = req.Thing;
            var comp = thing.TryGetComp<CompWeaponMods>();
            if (comp == null)
                return;

            foreach (var modSlot in comp.slots.Values)
            {
                if (modSlot.Mod?.statOffsets != null)
                {
                    foreach (var statOffset in modSlot.Mod.statOffsets)
                    {
                        if (statOffset.stat == this.parentStat)
                        {
                            if (this.parentStat == StatDefOf.Mass || this.parentStat == StatDefOf.MarketValue)
                            {
                                val += statOffset.value;
                            }
                            else
                            {
                                val += val * statOffset.value;
                            }
                        }
                    }
                }
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (!req.HasThing)
                return null;

            var thing = req.Thing;
            var comp = thing.TryGetComp<CompWeaponMods>();
            if (comp == null || comp.slots.Count == 0)
                return null;

            StringBuilder explanation = new StringBuilder();
            foreach (var modSlot in comp.slots.Values)
            {
                if (modSlot.Mod?.statOffsets != null)
                {
                    foreach (var statOffset in modSlot.Mod.statOffsets)
                    {
                        if (statOffset.stat == this.parentStat)
                        {
                            if (this.parentStat == StatDefOf.Mass || this.parentStat == StatDefOf.MarketValue)
                            {
                                explanation.AppendLine($"{modSlot.Mod.label}: {statOffset.value:+#0.##;-#0.##}");
                            }
                            else
                            {
                                explanation.AppendLine($"{modSlot.Mod.label}: {(statOffset.value * 100):+#0.##;-#0.##}%");
                            }
                        }
                    }
                }
            }

            return explanation.ToString();
        }
    }

}

