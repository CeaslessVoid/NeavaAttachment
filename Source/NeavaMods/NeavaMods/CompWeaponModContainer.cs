using NeavaMods.EffectExtensions;
using NeavaMods.Timed;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace NeavaMods
{
    public class CompProperties_WeaponModContainer : CompProperties
    {
        public CompProperties_WeaponModContainer()
        {
            this.compClass = typeof(CompWeaponModContainer);
        }

        public class CompWeaponModContainer : ThingComp
        {
            private List<ModEffectDef> modSlots = Enumerable.Repeat<ModEffectDef>(null, 8).ToList();
            public readonly Dictionary<EffectCategory, List<BaseModEffect>> cachedEffects = new Dictionary<EffectCategory, List<BaseModEffect>>();

            public List<ModEffectDef> ModsSlotsListForReading
            {
                get
                {
                    return modSlots;
                }
            }

            public void AddModToSlot(ModEffectDef mod, int slot)
            {
                modSlots[slot] = mod;
            }
            public override void PostExposeData()
            {
                base.PostExposeData();
                Scribe_Collections.Look(ref modSlots, "modSlots", LookMode.Def);
                Scribe_Collections.Look(ref activeBuffs, "activeBuffs", LookMode.Value, LookMode.Deep);
                if (Scribe.mode == LoadSaveMode.PostLoadInit)
                {
                    CacheEffects();
                    RebuildExpirySchedule();
                }
            }

            public void CacheEffects()
            {
                cachedEffects.Clear();
                foreach (ModEffectDef def in modSlots)
                {
                    if (def?.Extensions == null) continue;

                    foreach (string className in def.Extensions)
                    {
                        Type type = GenTypes.GetTypeInAnyAssembly(className);
                        if (type == null || !typeof(BaseModEffect).IsAssignableFrom(type)) continue;

                        if (Activator.CreateInstance(type) is BaseModEffect effect)
                        {
                            if (!cachedEffects.TryGetValue(effect.Category, out var list))
                            {
                                list = new List<BaseModEffect>();
                                cachedEffects[effect.Category] = list;
                            }

                            list.Add(effect);
                        }
                        else
                        {
                            ModUtils.Error($"Could not find effect {className}");
                        }
                    }
                }

            }
            public IEnumerable<T> GetEffectsOfType<T>(EffectCategory category) where T : BaseModEffect
            {
                if (cachedEffects.TryGetValue(category, out var list))
                {
                    foreach (var effect in list)
                    {
                        if (effect is T typed) yield return typed;
                    }
                }
            }

            public bool CanAddMod(ModEffectDef effect) //TODO
            {
                return true;
            }

            public override float GetStatOffset(StatDef stat)
            {
                float num = 0f;
                foreach (ModEffectDef effect in ModsSlotsListForReading)
                {
                    if (effect != null)
                        num += effect.statOffsets.GetStatOffsetFromList(stat);
                }
                return num;
            }

            public override float GetStatFactor(StatDef stat)
            {
                float num = 1f;
                foreach (ModEffectDef effect in ModsSlotsListForReading)
                {
                    if (effect != null)
                        num *= effect.statFactors.GetStatFactorFromList(stat);
                }
                return num;
            }

            public override void GetStatsExplanation(StatDef stat, StringBuilder sb, string whitespace = "")
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (ModEffectDef effect in ModsSlotsListForReading)
                {
                    if (effect != null)
                    {
                        float statOffsetFromList = effect.statOffsets.GetStatOffsetFromList(stat);
                        if (!Mathf.Approximately(statOffsetFromList, 0f))
                        {
                            stringBuilder.AppendLine(whitespace + "    " + effect.LabelCap + ": " + stat.Worker.ValueToString(statOffsetFromList, false, ToStringNumberSense.Offset));
                        }
                        float statFactorFromList = effect.statFactors.GetStatFactorFromList(stat);
                        if (!Mathf.Approximately(statFactorFromList, 1f))
                        {
                            stringBuilder.AppendLine(whitespace + "    " + effect.LabelCap + ": " + stat.Worker.ValueToString(statFactorFromList, false, ToStringNumberSense.Factor));
                        }
                    }
                }
                if (stringBuilder.Length == 0)
                {
                    return;
                }
                sb.AppendLine(whitespace + "StatsReport_WeaponMods".Translate() + ":");
                sb.Append(stringBuilder.ToString());
            }

            public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
            {
                if (!modSlots.NullOrEmpty<ModEffectDef>())
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("Stat_ThingWeaponMods_Desc".Translate());
                    stringBuilder.AppendLine();
                    for (int i = 0; i < modSlots.Count; i++)
                    {
                        ModEffectDef effect = modSlots[i];

                        if (effect != null)
                        {
                            stringBuilder.AppendLine(effect.LabelCap.Colorize(effect.color ?? Color.white));
                            stringBuilder.AppendLine(effect.description);

                            if (!effect.statOffsets.NullOrEmpty<StatModifier>())
                            {
                                stringBuilder.Append((from x in effect.statOffsets
                                                      select string.Format("{0} {1}", x.stat.LabelCap, x.stat.Worker.ValueToString(x.value, false, ToStringNumberSense.Offset))).ToLineList(" - ", false));
                                stringBuilder.AppendLine();
                            }
                            if (!effect.statFactors.NullOrEmpty<StatModifier>())
                            {
                                stringBuilder.Append((from x in effect.statFactors
                                                      select string.Format("{0} {1}", x.stat.LabelCap, x.stat.Worker.ValueToString(x.value, false, ToStringNumberSense.Factor))).ToLineList(" - ", false));
                                stringBuilder.AppendLine();
                            }

                            // Verb tools

                            if (i < modSlots.Count - 1)
                            {
                                stringBuilder.AppendLine();
                            }
                        }

                        yield return new StatDrawEntry(this.parent.def.IsMeleeWeapon ? StatCategoryDefOf.Weapon_Melee : StatCategoryDefOf.Weapon_Ranged, "Stat_ThingWeaponMods_Label".Translate(),
                            (from x in modSlots where x != null select x.label).ToCommaList(false, false).CapitalizeFirst(), stringBuilder.ToString(), 1104, null, null, false, false);

                    }
                }

                yield break;
            }

            public override string CompTipStringExtra()
            {
                return "Stat_ThingWeaponMods_Label".Translate() + ": " + (from x in this.ModsSlotsListForReading where x != null select x.label).ToCommaList(false, false).CapitalizeFirst();
            }

            public override string CompInspectStringExtra()
            {
                if (this.modSlots.NullOrEmpty<ModEffectDef>())
                {
                    return null;
                }
                return "Stat_ThingWeaponMods_Label".Translate() + ": " + (from x in this.ModsSlotsListForReading where x != null select x.label).ToCommaList(false, false).CapitalizeFirst();
            }

            // Buff Tracker
            private Dictionary<string, TimedBuff> activeBuffs = new();
            private SortedList<int, List<string>> expirySchedule = new();

            private int lastCleanupTick = 0;
            private const int CleanupInterval = 60000;

            private void RebuildExpirySchedule()
            {
                expirySchedule.Clear();
                foreach (var kvp in activeBuffs)
                {
                    var buff = kvp.Value;
                    if (!expirySchedule.TryGetValue(buff.expireTick, out var list))
                    {
                        list = new List<string>();
                        expirySchedule[buff.expireTick] = list;
                    }
                    list.Add(buff.id);
                }
            }

            public void AddBuff(string id, int durationTicks, float power)
            {
                int expireTick = Find.TickManager.TicksGame + durationTicks;

                if (activeBuffs.TryGetValue(id, out var existing))
                {
                    RemoveBuff(id);
                }

                var buff = new TimedBuff
                {
                    id = id,
                    power = power,
                    expireTick = expireTick
                };

                activeBuffs[id] = buff;

                if (!expirySchedule.TryGetValue(expireTick, out var list))
                {
                    list = new List<string>();
                    expirySchedule[expireTick] = list;
                }
                list.Add(id);
            }

            public float GetBuffPower(string id)
            {
                int now = Find.TickManager.TicksGame;

                if (activeBuffs.TryGetValue(id, out var buff))
                {
                    if (buff.expireTick < now)
                    {
                        RemoveBuff(id);
                        return 0f;
                    }
                    return buff.power;
                }
                return 0f;
            }

            public void RemoveBuff(string id)
            {
                if (!activeBuffs.TryGetValue(id, out var buff))
                    return;

                activeBuffs.Remove(id);

                if (expirySchedule.TryGetValue(buff.expireTick, out var list))
                {
                    list.Remove(id);
                    if (list.Count == 0)
                        expirySchedule.Remove(buff.expireTick);
                }
            }

            public void Tick()
            {
                int now = Find.TickManager.TicksGame;

                if (now - lastCleanupTick >= CleanupInterval)
                {
                    lastCleanupTick = now;

                    List<int> expiredKeys = new();
                    foreach (var kv in expirySchedule)
                    {
                        if (kv.Key > now) break;

                        foreach (string id in kv.Value)
                            activeBuffs.Remove(id);

                        expiredKeys.Add(kv.Key);
                    }

                    foreach (int key in expiredKeys)
                        expirySchedule.Remove(key);
                }
            }

            public bool HasBuff(string id) => GetBuffPower(id) > 0f;
        }
    }
}
