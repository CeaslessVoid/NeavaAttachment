using NeavaMods.Restrictions;
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
    public class ModificationDef : ThingDef
    {
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

        [NoTranslate]
        public string imagePath;

        [Unsaved(false)]
        private Texture2D cachedImage;

        [Unsaved]
        public Dictionary<StatDef, float> statOffsetDict;

        [NoTranslate]
        public string colorHex;

        [Unsaved]
        public Color? color;

        public RestrictionDef restrictionDef;

        public Texture2D Image
        {
            get
            {
                if (cachedImage == null)
                {
                    if (imagePath.NullOrEmpty())
                    {
                        cachedImage = BaseContent.BadTex;
                    }
                    else
                    {
                        cachedImage = (ContentFinder<Texture2D>.Get(imagePath, true) ?? BaseContent.BadTex);
                    }
                }
                return cachedImage;
            }
        }

        public bool AvailableOn(Thing thing)
        {
            if (restrictionDef == null)
                return true;

            var thingTagsList = thing.def.weaponTags;
            string defName = thing.def.defName;

            HashSet<string> thingTags = thingTagsList != null ? new HashSet<string>(thingTagsList) : new HashSet<string>();

            if (restrictionDef.Tags != null)
            {
                foreach (var tag in restrictionDef.Tags)
                {
                    if (!thingTags.Contains(tag))
                        return false;
                }
            }

            if (restrictionDef.TagsExclude != null)
            {
                foreach (var tag in restrictionDef.TagsExclude)
                {
                    if (thingTags.Contains(tag))
                        return false;
                }
            }

            if (restrictionDef.TagsAny != null && restrictionDef.TagsAny.Count > 0)
            {
                bool foundAny = false;
                foreach (var tag in restrictionDef.TagsAny)
                {
                    if (thingTags.Contains(tag))
                    {
                        foundAny = true;
                        break;
                    }
                }
                if (!foundAny)
                    return false;
            }

            if (restrictionDef.Def != null && restrictionDef.Def.Count > 0)
            {
                if (string.IsNullOrEmpty(defName) || !restrictionDef.Def.Contains(defName))
                    return false;
            }

            if (restrictionDef.Extension != null)
            {
                foreach (var ext in restrictionDef.Extension)
                {
                    if (ext != null && !ext.CheckAvailable(thing))
                        return false;
                }
            }

            return true;
        }


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
                    Log.Error($"Failed to parse color '{colorHex}' in {defName}: {e.Message}");
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
