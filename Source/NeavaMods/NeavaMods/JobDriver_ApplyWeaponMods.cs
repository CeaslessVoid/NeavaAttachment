using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace NeavaMods
{
    public class JobDriver_ApplyWeaponMods : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return ReservationUtility.Reserve(this.pawn, this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
        }
        protected override IEnumerable<Toil> MakeNewToils()
        {
            Thing weapon = this.pawn.equipment.Primary;

            yield return ToilFailConditions.FailOnDespawnedOrNull<Toil>(Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false), TargetIndex.A);

            yield return Toils_General.Do(delegate ()
            {
                Find.WindowStack.Add(ModUtils.GetWeaponModGUI(weapon, this.job.GetTarget(TargetIndex.A).Thing, this.pawn, null));
            });

            yield break;
        }
    }
}
