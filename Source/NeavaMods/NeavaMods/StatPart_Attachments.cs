using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaAttachments
{
    public class StatPart_Attachments : StatPart
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            if (!req.HasThing)
                return;

            var thing = req.Thing;
            var comp = thing.TryGetComp<CompAttachments>();
            if (comp == null)
                return;

            foreach (var attachment in comp.Attachments.Values)
            {
                if (attachment.statOffsets != null)
                {
                    foreach (var statOffset in attachment.statOffsets)
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
            var comp = thing.TryGetComp<CompAttachments>();
            if (comp == null || comp.Attachments.Count == 0)
                return null;

            StringBuilder explanation = new StringBuilder();
            foreach (var attachment in comp.Attachments.Values)
            {
                if (attachment.statOffsets != null)
                {
                    foreach (var statOffset in attachment.statOffsets)
                    {
                        if (statOffset.stat == this.parentStat)
                        {
                            if (this.parentStat == StatDefOf.Mass)
                            {
                                explanation.AppendLine($"{attachment.label}: {statOffset.value:+#0.##;-#0.##}");
                            }
                            else
                            {
                                explanation.AppendLine($"{attachment.label}: {(statOffset.value * 100):+#0.##;-#0.##}%");
                            }
                        }
                    }
                }
            }

            return explanation.ToString();
        }
    }

    public class StatPart_AttachmentsHuman : StatPart
        {
            public override void TransformValue(StatRequest req, ref float val)
            {
                if (req.Thing is Pawn pawn && pawn.equipment?.Primary != null)
                {
                    var comp = pawn.equipment.Primary.TryGetComp<CompAttachments>();
                    if (comp != null)
                    {
                        foreach (var attachment in comp.Attachments.Values)
                        {
                            if (attachment.statOffsets != null)
                            {
                                foreach (var statModifier in attachment.statOffsets)
                                {
                                    if (statModifier.stat == this.parentStat)
                                    {
                                        val += (val * statModifier.value);
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
                    var comp = pawn.equipment.Primary.TryGetComp<CompAttachments>();
                    if (comp != null)
                    {
                        StringBuilder explanation = new StringBuilder();
                        foreach (var attachment in comp.Attachments.Values)
                        {
                            if (attachment.statOffsets != null)
                            {
                                foreach (var statModifier in attachment.statOffsets)
                                {
                                    if (statModifier.stat == this.parentStat)
                                    {
                                        explanation.AppendLine($"{attachment.label}: {statModifier.value}%");
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

