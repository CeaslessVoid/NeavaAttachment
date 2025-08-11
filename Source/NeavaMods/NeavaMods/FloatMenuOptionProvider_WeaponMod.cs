using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;
using static NeavaMods.CompProperties_WeaponModContainer;

namespace NeavaMods
{
    public class FloatMenuOptionProvider_WeaponMod : FloatMenuOptionProvider
    {
        protected override bool Drafted => true;
        protected override bool Undrafted => true;
        protected override bool Multiselect => false;


        //public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
        //{
        //    Pawn pawn = context.FirstSelectedPawn;
        //    if (pawn == null || pawn.equipment?.Primary == null)
        //        yield break;
        //    var equippedWeapon = pawn.equipment.Primary;
        //    if (equippedWeapon.TryGetComp<CompWeaponModContainer>() == null)
        //        yield break;
        //    if (clickedThing is not Building_WeaponMod modStation)
        //        yield break;
        //    if (modStation.HoldingItem != null || !modStation.PowerOn)
        //        yield break;
        //    yield return MakeInsertWeaponMenu(pawn, equippedWeapon, modStation);
        //}

        //public static FloatMenuOption MakeInsertWeaponMenu(Pawn pawn, ThingWithComps weapon, Building_WeaponMod station)
        //{
        //    string label = "InsertItem".Translate(weapon.LabelCap, station.LabelCap);

        //    FloatMenuOption option = new FloatMenuOption(label, () =>
        //    {
        //        Job job = JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("InsertEquippedWeapon"), pawn.equipment.Primary, station);
        //        job.count = 1;
        //        pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
        //    }, MenuOptionPriority.High);

        //    return FloatMenuUtility.DecoratePrioritizedTask(option, pawn, station);
        //}
    }
}
