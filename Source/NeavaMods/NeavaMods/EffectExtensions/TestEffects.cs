using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods.EffectExtensions
{
    public class TestEffects1 : RangePreHit
    {
        public override void Apply(Pawn shooter, Thing victim, ref Projectile projectile, ref float damageMod)
        {
            ModUtils.Msg("PreHitApplied");
            damageMod += 10;
        }
    }

    public class TestEffects2 : PostHit
    {
        public override void Apply(Pawn attacker, Thing victim, DamageInfo dinfo, float totalDamageDealt)
        {
            ModUtils.Msg("PostHitApplied");
            ModUtils.Msg($"Damage: {totalDamageDealt}");
            ModUtils.Msg($"Victim: {victim.Label}");
        }
    }

    public class TestEffects3 : MeleePreHit
    {
        public override void Apply(Pawn attacker, Thing victim, ref Verb_MeleeAttackDamage verb, ref DamageInfo dinfo, ref float damageMod)
        {
            ModUtils.Msg("PreHitApplied");
            damageMod += 10;
        }
    }

}
