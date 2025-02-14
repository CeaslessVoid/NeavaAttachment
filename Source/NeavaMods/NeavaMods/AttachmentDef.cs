using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class AttachmentDef : Def
    {
        public string location;

        public float bulk; //CE Only

        public List<StatModifier> statOffsets; // Diffrent from main, weight is in here

        public string extensionClass;

        public TechLevel minTechLevel;

        public TechLevel maxTechLevel; // Not used, but modders may enjoy

        public List<string> requiredWeaponTags;

        public bool matchAllntneeded; // If this then only 1 tag is needed

        public bool not; // Not these tags

        public List<string> requiredDefs;
    }
}
