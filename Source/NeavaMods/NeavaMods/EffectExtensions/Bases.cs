using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods.EffectExtensions
{

    public abstract class BaseModEffect : IExposable
    {
        public virtual EffectCategory Category => throw new NotImplementedException();

        public virtual void ExposeData()
        {
        }
    }

    //public abstract class RangePreHit : BaseModEffect
    //{
    //    public override EffectCategory Category => EffectCategory.RangePreHit;

    //    public virtual void Apply(Pawn shooter, Thing victim, ref Projectile projectile, ref float damageMod) { }
    //}

    //public abstract class MeleePreHit : BaseModEffect
    //{
    //    public override EffectCategory Category => EffectCategory.MeleePreHit;

    //    public virtual void Apply(Pawn attacker, Thing victim, ref Verb_MeleeAttackDamage verb, ref DamageInfo dinfo, ref float damageMod) { }
    //}

    public abstract class PreHit : BaseModEffect
    {
        public override EffectCategory Category => EffectCategory.PreApplyDamage;

        public virtual void Apply(Pawn attacker, Thing victim, ref DamageInfo dinfo) { }
    }

    public abstract class PostHit : BaseModEffect
    {
        public override EffectCategory Category => EffectCategory.PostApplyDamage;

        public virtual void Apply(Pawn attacker, Thing victim, DamageInfo dinfo, float totalDamageDealt) { }
    }

    public abstract class OnKill : BaseModEffect
    {
        public override EffectCategory Category => EffectCategory.OnKill;

        public virtual void Apply(Pawn attacker, Thing victim, DamageInfo dinfo, float totalDamageDealt) { }
    }

    public enum EffectCategory
    {
        PreApplyDamage,
        PostApplyDamage,
        OnKill,
    }

}
