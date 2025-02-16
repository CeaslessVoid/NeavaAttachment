using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public interface IModEffect
    {
        ProcType ProcType { get; }
        void ApplyEffect(Pawn pawn, Pawn victim, Projectile projectile, Verb_MeleeAttackDamage verb, ref float damageMod);

    }

    public enum ProcType
    {
        OnHitPreCheck, // Damage Calc
        OnHit, // After damage
        OnKill
    }


}
