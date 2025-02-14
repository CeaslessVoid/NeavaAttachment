using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaAttachments
{
    //[StaticConstructorOnStartup]
    //public static class CompAttachmentInjector
    //{
    //    static CompAttachmentInjector()
    //    {
    //        var harmony = new Harmony("NeavaAttachments.CompAttachmentInjector");
    //        harmony.Patch(
    //            original: AccessTools.Method(typeof(ThingDef), nameof(ThingDef.PostLoad)),
    //            postfix: new HarmonyMethod(typeof(CompAttachmentInjector), nameof(AddAttachmentComp))
    //        );
    //    }

    //    public static void AddAttachmentComp(ThingDef __instance)
    //    {
    //        ModUtils.Msg("----------Trying to add Comp----------");
    //        if (__instance.IsRangedWeapon)
    //        {
    //            ModUtils.Msg("1");
    //            if (__instance.comps == null)
    //            {
    //                __instance.comps = new List<CompProperties>();
    //            }
    //            ModUtils.Msg("2");

    //            if (!__instance.HasComp(typeof(CompAttachments)))
    //            {
    //                __instance.comps.Add(new CompProperties
    //                {
    //                    compClass = typeof(CompAttachments)
    //                });

    //                ModUtils.Msg($"Added CompAttachments to weapon: {__instance.defName}");
    //            }
    //        }
    //    }
    //}
}
