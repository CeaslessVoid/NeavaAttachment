using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaAttachments
{
    public class CompProperties_Attachments : CompProperties
    {
        public CompProperties_Attachments()
        {
            compClass = typeof(CompAttachments);
        }
    }

    public class CompAttachments : ThingComp
    {
        private static readonly List<string> ValidAttachmentSlots = new List<string> { "Optics", "Barrel", "Underbarrel", "Other" };

        private Dictionary<string, AttachmentDef> attachments = new Dictionary<string, AttachmentDef>();

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Collections.Look(ref attachments, "attachments", LookMode.Value, LookMode.Def);
        }

        public bool TryAddAttachment(AttachmentDef attachment)
        {
            if (!ValidAttachmentSlots.Contains(attachment.location))
            {
                ModUtils.Warn($"Attachment {attachment.defName} cannot be applied. Invalid slot: {attachment.location}");
                return false;
            }

            attachments[attachment.location] = attachment;
            return true;
        }

        public bool RemoveAttachment(string slot)
        {
            if (attachments.ContainsKey(slot))
            {
                attachments.Remove(slot);
                return true;
            }
            return false;
        }

        public AttachmentDef GetAttachment(string slot)
        {
            return attachments.TryGetValue(slot, out var attachment) ? attachment : null;
        }

        public override void Notify_Equipped(Pawn pawn)
        {
            base.Notify_Equipped(pawn);
            RecalculateStats();
        }

        public void RecalculateStats()
        {
            foreach (var attachment in attachments.Values)
            {
                if (attachment.statOffsets != null)
                {
                    foreach (var statModifier in attachment.statOffsets)
                    {
                        parent.GetStatValue(statModifier.stat, true);
                    }
                }
            }
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
        {
            foreach (var kvp in attachments)
            {
                yield return new StatDrawEntry(StatCategoryDefOf.Weapon, $"Attachment ({kvp.Key})", kvp.Value.label, kvp.Value.description, 0);
            }
        }
    }
}


