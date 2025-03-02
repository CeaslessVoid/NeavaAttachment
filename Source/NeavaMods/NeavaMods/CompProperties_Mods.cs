using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
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
            compClass = typeof(CompWeaponMods);
        }
    }

    public class CompWeaponMods : ThingComp
    {
        public const int SlotCount = 8;

        public Dictionary<int, ModSlot> slots = new Dictionary<int, ModSlot>();

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Collections.Look(ref slots, "slots", LookMode.Value, LookMode.Deep);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                RebuildEffectCache();
            }
        }

        public bool TryAddMod(int slotIndex, WeaponModDef mod, Polarity? polarity = null)
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
            RebuildEffectCache();
            return true;
        }

        public bool RemoveMod(int slotIndex)
        {
            if (slots.ContainsKey(slotIndex))
            {
                slots.Remove(slotIndex);
                return true;
            }
            RebuildEffectCache();
            return false;
        }

        public void RebuildEffectCache()
        {
            cachedEffects.Clear();

            foreach (var slot in slots.Values)
            {
                var modEffect = slot.Mod.GetExtensionClassInstance();
                if (modEffect != null)
                {
                    if (!cachedEffects.ContainsKey(modEffect.ProcType))
                    {
                        cachedEffects[modEffect.ProcType] = new List<IModEffect>();
                    }
                    cachedEffects[modEffect.ProcType].Add(modEffect);
                }
            }
        }

        public List<IModEffect> GetEffectsByProcType(ProcType procType)
        {
            return cachedEffects.TryGetValue(procType, out var effects) ? effects : null;
        }

        private readonly Dictionary<ProcType, List<IModEffect>> cachedEffects = new Dictionary<ProcType, List<IModEffect>>();

    }

}


