using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace NeavaMods
{
    public class Building_TableMachining : Building_WorkTable
    {
        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
            {
                yield return floatMenuOption;
            }

            if (selPawn.equipment?.Primary is ThingWithComps mainWeapon)
            {
                var compMods = mainWeapon.GetComp<CompWeaponMods>();
                if (compMods != null)
                {
                    yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(TranslatorFormattedStringExtensions.Translate("ApplyWeaponMods", mainWeapon.Label).CapitalizeFirst(), delegate ()
                    {
                        selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(NeavaModsDefOfs.ApplyWeaponMods, this, mainWeapon), new JobTag?(0), false);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this, "ReservedBy", null);
                }
                else
                {
                    yield return new FloatMenuOption(TranslatorFormattedStringExtensions.Translate("CannotUseReason", Translator.Translate("noWeaponModComp").CapitalizeFirst()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
                }
            }
        }
    }
}
