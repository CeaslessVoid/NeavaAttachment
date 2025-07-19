using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace NeavaMods
{
    public class JobDriver_InsertEquippedWeapon : JobDriver
    {
        private ThingWithComps Weapon => job.GetTarget(TargetIndex.A).Thing as ThingWithComps;
        private Building_WeaponMod ModStation => job.GetTarget(TargetIndex.B).Thing as Building_WeaponMod;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed) &&
                   pawn.Reserve(job.GetTarget(TargetIndex.B), job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            var weapon = Weapon;
            var station = ModStation;

            this.FailOn(() => weapon == null || station == null || !station.PowerOn || station.HoldingItem != null);

            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell);
            yield return Toil_InsertWeaponToModStation(weapon, station);
        }

        private Toil Toil_InsertWeaponToModStation(ThingWithComps weapon, Building_WeaponMod station)
        {
            var toil = ToilMaker.MakeToil("NeavaMods.InsertWeapon");

            toil.initAction = () =>
            {
                pawn.equipment?.TryTransferEquipmentToContainer(weapon, station.ContainerComp.innerContainer);
            };

            toil.defaultCompleteMode = ToilCompleteMode.Instant;

            return toil;
        }
    }

}
