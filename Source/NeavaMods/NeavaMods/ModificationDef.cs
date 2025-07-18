﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
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

        public List<StatModifier> statFactors;

        public List<StatModifier> equippedStatOffsets;

        public int Drain;

        [NoTranslate]
        public List<string> Tags;

        public List<string> Extensions;

        [Unsaved]
        public Dictionary<StatDef, float> statOffsetDict;

        [NoTranslate]
        public string colorHex;

        [Unsaved]
        public Color? color;

        public override void ResolveReferences()
        {
            base.ResolveReferences();

            if (!string.IsNullOrEmpty(colorHex))
            {
                try
                {
                    color = GenColor.FromHex(colorHex);
                }
                catch (Exception e)
                {
                    Log.Error($"[NeavaMods] Failed to parse color '{colorHex}' in {defName}: {e.Message}");
                }
            }

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
