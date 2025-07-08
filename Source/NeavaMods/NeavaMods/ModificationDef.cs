using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class ModificationDef : ThingDef
    {
        public ModEffectDef effectDef;

        [NoTranslate]
        public List<string> Tags;

        [NoTranslate]
        public List<string> TagsExclude;

        [NoTranslate]
        public List<string> TagsAny;
    }

    public class ModEffectDef : Def
    {
        public List<StatModifier> statOffsets;

        public int Drain;

        [NoTranslate]
        public List<string> Tags;

        [Unsaved]
        public Dictionary<StatDef, float> statOffsetDict;

        public override void ResolveReferences()
        {
            base.ResolveReferences();

            if (statOffsets != null && statOffsets.Count > 0)
            {
                statOffsetDict = new Dictionary<StatDef, float>();

                foreach (var mod in statOffsets)
                {
                    if (mod?.stat == null)
                    {
                        Log.Warning($"[NeavaMods] Skipping null stat in {defName}'s statOffsets.");
                        continue;
                    }

                    statOffsetDict[mod.stat] = mod.value;
                }
            }
        }


    }
}
