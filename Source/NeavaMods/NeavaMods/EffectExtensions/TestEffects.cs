using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods.EffectExtensions
{
    public class TestEffects1 : PreHit
    {
        public override void Apply(Pawn shooter, Thing victim, ref DamageInfo dinfo)
        {
            ModUtils.Msg("PreHitApplied");

            dinfo.SetAmount(dinfo.Amount * 10);
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

    public class TestEffects3 : PreHit
    {
        public override void Apply(Pawn shooter, Thing victim, ref DamageInfo dinfo)
        {
            ModUtils.Msg("PreHitApplied");
            dinfo.SetAmount(dinfo.Amount * 10);
        }
    }

}
