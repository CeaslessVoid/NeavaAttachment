using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class DoubleDamageToMechsEffect : IModEffect
    {
        public ProcType ProcType => ProcType.OnHitPreCheck;
        public void ApplyEffect(Pawn shooter, Pawn victim, Projectile projectile, Verb_MeleeAttackDamage verb, ref float damageMod)
        {
            ModUtils.Msg("Hit");
            if (victim.RaceProps.IsMechanoid)
            {
                damageMod += 99f;
            }

        }
    }


}
