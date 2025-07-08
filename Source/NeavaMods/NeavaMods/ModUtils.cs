using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    [StaticConstructorOnStartup]
    internal static class ModUtils
    {
        public static void Msg(object o)
        {
            Log.Message("[Neava's Mods] " + ((o != null) ? o.ToString() : null));
        }

        public static void Warn(object o)
        {
            Log.Warning("[Neava's Mods] " + ((o != null) ? o.ToString() : null));
        }

        public static void Error(object o)
        {
            Log.Error("[Neava's Mods] " + ((o != null) ? o.ToString() : null));
        }

        public static Thing GenerateSpecificMod(ModEffectDef effect)
        {
            Thing modThing = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("WeaponModBasic", errorOnFail: false));

            if (modThing is WeaponModComp weaponMod)
            {
                weaponMod.effect = effect;

                return modThing;
            }
            else
            {
                return null;
            }
        }

        public static List<ModEffectDef> GetMatchingEffects(ModificationDef modDef)
        {
            if ((modDef.Tags == null || modDef.Tags.Count == 0) &&
                (modDef.TagsExclude == null || modDef.TagsExclude.Count == 0) &&
                (modDef.TagsAny == null || modDef.TagsAny.Count == 0))
            {
                Error($"ModificationDef '{modDef.defName}' has no tag filters defined (Tags, TagsExclude, TagsAny).");
                return new List<ModEffectDef>();
            }

            return DefDatabase<ModEffectDef>.AllDefs
                .Where(effect =>
                {
                    if (effect.Tags == null)
                        return false;

                    if (modDef.TagsExclude != null &&
                        modDef.TagsExclude.Any(tag => effect.Tags.Contains(tag)))
                        return false;

                    if (modDef.Tags != null &&
                        !modDef.Tags.All(tag => effect.Tags.Contains(tag)))
                        return false;

                    if (modDef.TagsAny != null && modDef.TagsAny.Count > 0 &&
                        !modDef.TagsAny.Any(tag => effect.Tags.Contains(tag)))
                        return false;

                    return true;
                })
                .ToList();
        }

        public static ModEffectDef GenerateModEffect(ModificationDef modDef)
        {
            var candidates = GetMatchingEffects(modDef);
            return candidates.TryRandomElement(out var chosen) ? chosen : null;
        }
    }
}
