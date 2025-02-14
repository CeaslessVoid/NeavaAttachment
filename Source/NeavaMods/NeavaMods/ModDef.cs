using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class ModDef : Def
    {

        public List<StatModifier> statOffsets; // Diffrent from main

        public string extensionClass;

        public List<string> requiredWeaponTags;

        public bool matchAllntneeded; // If this then only 1 tag is needed

        public bool not; // Not these tags

        public List<string> requiredDefs; // Match only 1 needed

        public Polarity polarity;

        public int drain;
    }
}
