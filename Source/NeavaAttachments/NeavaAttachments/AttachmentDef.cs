using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaAttachments
{
    public class AttachmentDef : Def
    {
        public string location;

        public float bulk; //CE Only

        public List<StatModifier> statOffsets; // Diffrent from main

        public string extensionClass;
    }
}
