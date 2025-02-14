using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    [StaticConstructorOnStartup]
    public static class Main
    {
        static Main()
        {
            Main.HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

        }

        public static Harmony HarmonyInstance = new Harmony("NeavaMods.GunAttachments.Base");


        public static bool IsCEActive()
        {
            return ModLister.HasActiveModWithName("Combat Extended");
        }

    }

    [StaticConstructorOnStartup]
    public static class DebugAttachmentDefs
    {
        static DebugAttachmentDefs()
        {
            foreach (var def in DefDatabase<ModDef>.AllDefsListForReading)
            {
                Log.Message($"Loaded AttachmentDef: {def.defName}");
            }
        }
    }

}
