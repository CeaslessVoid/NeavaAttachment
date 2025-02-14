using LudeonTK;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public static class Debug
    {
        [DebugAction(null, null, false, false, false, false, 0, false, category = CATEGORY, name = "TestSPawnAssultRifle", requiresRoyalty = false, requiresIdeology = false, requiresBiotech = false, actionType = 0, allowedGameStates = LudeonTK.AllowedGameStates.Playing)]
        private static void TestSpawnAssultRifle()
        {

            ThingWithComps weapon = (ThingWithComps)ThingMaker.MakeThing(ThingDef.Named("Gun_AssaultRifle"));
            var comp = weapon.TryGetComp<CompAttachments>();
            if (comp != null)
            {
                var testAttachment = DefDatabase<AttachmentDef>.GetNamed("Attachment_Test");
                comp.TryAddAttachment(testAttachment);
            }
            GenPlace.TryPlaceThing(weapon, UI.MouseCell(), Find.CurrentMap, ThingPlaceMode.Near);
            Messages.Message("Spawned an Assault Rifle with the Test Attachment!", MessageTypeDefOf.TaskCompletion, false);
        }

        private const string CATEGORY = "Neava's Attachments";
    }
}
