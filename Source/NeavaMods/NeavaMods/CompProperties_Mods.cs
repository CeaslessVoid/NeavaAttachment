using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class CompProperties_Mods : CompProperties
    {
        public CompProperties_Mods()
        {
            compClass = typeof(CompMods);
        }
    }

    public class CompMods : ThingComp
    {
        public const int SlotCount = 8;

        public Dictionary<int, ModSlot> slots = new Dictionary<int, ModSlot>();

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Collections.Look(ref slots, "slots", LookMode.Value, LookMode.Deep);
        }

        public bool TryAddMod(int slotIndex, ModDef mod, Polarity? polarity = null)
        {
            if (slotIndex < 0 || slotIndex >= SlotCount)
            {
                ModUtils.Warn($"Invalid slot index: {slotIndex}. Must be between 0 and {SlotCount - 1}.");
                return false;
            }

            if (slots.ContainsKey(slotIndex))
            {
                ModUtils.Warn($"Slot {slotIndex} is already occupied. Remove the existing mod first.");
                return false;
            }
            // Fix later

            slots[slotIndex] = new ModSlot(mod, polarity);
            return true;
        }

        public bool RemoveMod(int slotIndex)
        {
            if (slots.ContainsKey(slotIndex))
            {
                slots.Remove(slotIndex);
                return true;
            }
            return false;
        }

        //public (ModDef mod, Polarity? polarity) GetSlot(int slotIndex)
        //{
        //    return slots.TryGetValue(slotIndex, out var slot) ? slot : (null, null);
        //}
    }

}


