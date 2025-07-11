using NeavaMods.EffectExtensions;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            public List<ModEffectDef> modSlots = new List<ModEffectDef>(8);
            public readonly Dictionary<EffectCategory, List<BaseModEffect>> cachedEffects = new Dictionary<EffectCategory, List<BaseModEffect>>();
            public override void PostExposeData()
            {
                base.PostExposeData();
                Scribe_Collections.Look(ref modSlots, "modSlots", LookMode.Def);

                if (Scribe.mode == LoadSaveMode.PostLoadInit)
                {
                    CacheEffects();
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
        }
    }
}
