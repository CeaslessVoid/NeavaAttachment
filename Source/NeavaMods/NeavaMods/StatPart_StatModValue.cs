using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class StatPart_StatModValue : StatPart
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            if (!req.HasThing)
                return;

            Thing thing = req.Thing;

            if (thing is WeaponModComp weaponMod)
            {
                if (weaponMod.effect?.statOffsets != null && weaponMod.effect.statOffsets.Count > 0)
                {
                    if (weaponMod.effect?.statOffsetDict != null &&
                        weaponMod.effect.statOffsetDict.TryGetValue(this.parentStat, out float offset))
                    {
                        if (this.parentStat == StatDefOf.MarketValue)
                        {
                            val += offset;
                        }
                        else
                        {
                            val += val * offset;
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

            if (thing is WeaponModComp weaponMod)
            {
                StringBuilder explanation = new StringBuilder();

                if (weaponMod.effect?.statOffsets != null)
                {
                    foreach (var statOffset in weaponMod.effect?.statOffsets)
                    {
                        if (statOffset.stat == StatDefOf.MarketValue)
                        {
                            explanation.AppendLine($"{weaponMod.effect.label}: {statOffset.value:+#0.##;-#0.##}");
                        }
                    }

                    return explanation.ToString();
                }
            }

            return null;
        }
    }
}
