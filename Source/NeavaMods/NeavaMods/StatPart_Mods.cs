using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class StatPart_Mods : StatPart
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            if (!req.HasThing)
                return;

            var thing = req.Thing;
            var comp = thing.TryGetComp<CompMods>();
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
                            if (this.parentStat == StatDefOf.Mass)
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
            var comp = thing.TryGetComp<CompMods>();
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
                            if (this.parentStat == StatDefOf.Mass)
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

    public class StatPart_ModsHuman : StatPart
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            if (req.Thing is Pawn pawn && pawn.equipment?.Primary != null)
            {
                var comp = pawn.equipment.Primary.TryGetComp<CompMods>();
                if (comp != null)
                {
                    foreach (var modSlot in comp.slots.Values)
                    {
                        if (modSlot.Mod?.statOffsets != null)
                        {
                            foreach (var statOffset in modSlot.Mod.statOffsets)
                            {
                                if (statOffset.stat == this.parentStat)
                                {
                                    if (this.parentStat == StatDefOf.Mass)
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
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (req.Thing is Pawn pawn && pawn.equipment?.Primary != null)
            {
                var comp = pawn.equipment.Primary.TryGetComp<CompMods>();
                if (comp != null)
                {
                    StringBuilder explanation = new StringBuilder();
                    foreach (var modSlot in comp.slots.Values)
                    {
                        if (modSlot.Mod?.statOffsets != null)
                        {
                            foreach (var statOffset in modSlot.Mod.statOffsets)
                            {
                                if (statOffset.stat == this.parentStat)
                                {
                                    if (this.parentStat == StatDefOf.Mass)
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
            return null;
        }
    }
}

