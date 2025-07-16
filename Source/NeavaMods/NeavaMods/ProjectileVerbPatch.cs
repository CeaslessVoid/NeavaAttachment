using HarmonyLib;
using NeavaMods.EffectExtensions;
using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Diagnostics;
using Verse;
using Verse.Sound;
using static NeavaMods.CompProperties_WeaponModContainer;

namespace NeavaMods
{
    public class ProjectileInfoExteder : GameComponent
    {
        public static Dictionary<Projectile, Thing> projectileEquipment = new Dictionary<Projectile, Thing>();

        public static readonly Dictionary<Thing /*victim*/, Thing /*equipment*/> VictimToEquip = new Dictionary<Thing, Thing>();

        public ProjectileInfoExteder(Game game)
        {

        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref projectileEquipment, "projectileEquipment", LookMode.Reference, LookMode.Reference);
            //Scribe_Collections.Look(ref VictimToEquip, "VictimToEquip", LookMode.Reference, LookMode.Reference);
        }

        public static void SetEquipment(Projectile projectile, Thing equipment)
        {
            projectileEquipment[projectile] = equipment;
        }


        public static void RegisterVictim(Thing victim, Thing equip)
        {
            VictimToEquip[victim] = equip;
        }



        public static void Remove(Projectile projectile)
        {
            projectileEquipment.Remove(projectile);
        }

        public static void RemveVictim(Thing victim)
        {
            VictimToEquip.Remove(victim);
        }

    }

    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            Main.HarmonyInstance.Patch(
                original: AccessTools.Method(
                    typeof(Projectile),
                    "Launch",
                    new Type[] {
                            typeof(Thing),           // launcher
                            typeof(Vector3),         // origin
                            typeof(LocalTargetInfo), // usedTarget
                            typeof(LocalTargetInfo), // intendedTarget
                            typeof(ProjectileHitFlags), // hitFlags
                            typeof(bool),           // preventFriendlyFire
                            typeof(Thing),          // equipment
                            typeof(ThingDef)        // targetCoverDef
                    }),
                prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(OnHitPreCheckLaunch))
            );

            Main.HarmonyInstance.Patch(
                original: AccessTools.Method(typeof(Bullet), "Impact"),
                prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(ProjectileImpact_Patch))
            );

            //Main.HarmonyInstance.Patch(
            //    original: AccessTools.Method(typeof(Verb_MeleeAttackDamage), "DamageInfosToApply"),
            //    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Verb_MeleeAttackDamage_Patch))
            //);

            //Main.HarmonyInstance.Patch(
            //    original: AccessTools.Method(typeof(Verb_MeleeAttackDamage), "ApplyMeleeDamageToTarget"),
            //    prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Verb_MeleeAttackDamage_AfterPatch))
            //);

            //Main.HarmonyInstance.Patch(
            //    original: AccessTools.Method(typeof(Thing), "PostApplyDamage"),
            //    postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(Verb_ThingPostApplyDamage_Patch))
            //);
        }


        public static void OnHitPreCheckLaunch(Projectile __instance, Thing launcher, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, bool preventFriendlyFire = false, Thing equipment = null)
        {
            
            if (launcher is Pawn pawn && equipment.TryGetComp<CompWeaponModContainer>() != null)
            {
                ProjectileInfoExteder.SetEquipment(__instance, equipment);
            }
        }

        public static bool ProjectileImpact_Patch(Bullet __instance, Thing hitThing, bool blockedByShield = false)
        {
            if (ProjectileInfoExteder.projectileEquipment.TryGetValue(__instance, out Thing equipment))
            {
                ProjectileInfoExteder.Remove(__instance);
                ProjectileInfoExteder.RegisterVictim(hitThing, equipment);
            }

            return true;
        }

        //public static bool ProjectileImpact_Patch(Bullet __instance, Thing hitThing, bool blockedByShield = false)
        //{
        //    if (ProjectileInfoExteder.projectileEquipment.TryGetValue(__instance, out Thing equipment))
        //    {
        //        ProjectileInfoExteder.Remove(__instance);

        //        Pawn pawn = __instance.Launcher as Pawn;
        //        var compMods = equipment.TryGetComp<CompWeaponModContainer>();
        //        float damageMod = 1;

        //        if (compMods != null)
        //        {
        //            Projectile projectile = __instance;

        //            foreach (RangePreHit effect in compMods.GetEffectsOfType<RangePreHit>(EffectCategory.RangePreHit))
        //            {
        //                effect.Apply(pawn, hitThing, ref projectile, ref damageMod);
        //            }

        //        }

        //        Map map = __instance.Map;
        //        IntVec3 position = __instance.Position;

        //        GenClamor.DoClamor(__instance, 12f, ClamorDefOf.Impact);
        //        if (!blockedByShield && __instance.def.projectile.landedEffecter != null)
        //        {
        //            __instance.def.projectile.landedEffecter.Spawn(__instance.Position, __instance.Map, 1f).Cleanup();
        //        }
        //        __instance.Destroy(DestroyMode.Vanish);

        //        BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(__instance.Launcher, hitThing, __instance.intendedTarget.Thing, __instance.EquipmentDef, __instance.def, ThingDefOf.Barricade);
        //        Find.BattleLog.Add(battleLogEntry_RangedImpact);

        //        BulletImpactData impactData = new BulletImpactData
        //        {
        //            bullet = __instance,
        //            hitThing = hitThing,
        //            impactPosition = position
        //        };

        //        if (hitThing != null)
        //        {
        //            hitThing.Notify_BulletImpactNearby(impactData);

        //            Pawn pawn3;
        //            bool instigatorGuilty = (pawn3 = (__instance.Launcher as Pawn)) == null || !pawn.Drafted;

        //            DamageInfo dinfo = new DamageInfo(__instance.def.projectile.damageDef, __instance.DamageAmount * damageMod, __instance.ArmorPenetration, __instance.ExactRotation.eulerAngles.y, __instance.Launcher, null, __instance.EquipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, __instance.intendedTarget.Thing, instigatorGuilty, true, QualityCategory.Normal, true);
        //            QualityCategory quality;
        //            equipment.TryGetQuality(out quality);
        //            dinfo.SetWeaponQuality(quality);

        //            ProjectileInfoExteder.RegisterVictim(hitThing, equipment);
        //            hitThing.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_RangedImpact);

        //            Pawn pawn4 = hitThing as Pawn;
        //            if (pawn4 != null)
        //            {
        //                Pawn_StanceTracker stances = pawn4.stances;
        //                if (stances != null)
        //                {
        //                    stances.stagger.Notify_BulletImpact(__instance);
        //                }
        //            }
        //            if (base.ExtraDamages == null)
        //            {
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            if (!blockedByShield)
        //            {
        //                SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(__instance.Position, map, false));
        //                if (__instance.Position.GetTerrain(map).takeSplashes)
        //                {
        //                    FleckMaker.WaterSplash(__instance.ExactPosition, map, Mathf.Sqrt((float)__instance.DamageAmount) * 1f, 4f);
        //                }
        //                else
        //                {
        //                    FleckMaker.Static(__instance.ExactPosition, map, FleckDefOf.ShotHit_Dirt, 1f);
        //                }
        //            }
        //            if (Rand.Chance(__instance.def.projectile.bulletChanceToStartFire))
        //            {
        //                FireUtility.TryStartFireIn(__instance.Position, map, __instance.def.projectile.bulletFireSizeRange.RandomInRange, __instance.Launcher, null);
        //            }
        //        }
        //        int num = 9;
        //        for (int i = 0; i < num; i++)
        //        {
        //            IntVec3 c = position + GenRadial.RadialPattern[i];
        //            if (c.InBounds(map))
        //            {
        //                List<Thing> thingList = c.GetThingList(map);
        //                for (int j = 0; j < thingList.Count; j++)
        //                {
        //                    if (thingList[j] != hitThing)
        //                    {
        //                        thingList[j].Notify_BulletImpactNearby(impactData);
        //                    }
        //                }
        //            }
        //        }



        //        return false;
        //    }
        //    return true;
        //}

        //public static void Verb_MeleeAttackDamage_Patch(Verb_MeleeAttackDamage __instance, ref IEnumerable<DamageInfo> __result, LocalTargetInfo target)
        //{
        //    __result = WrapEnumerator(__instance, __result, target);

        //}

        //private static IEnumerable<DamageInfo> WrapEnumerator(Verb_MeleeAttackDamage __instance, IEnumerable<DamageInfo> original, LocalTargetInfo target)
        //{
        //    ThingWithComps weapon = __instance.EquipmentSource;
        //    Pawn attacker = __instance.CasterPawn;
        //    Thing victim = target.Thing;

        //    float damageMod = 1f;
        //    bool firstDone = false;
        //    DamageInfo firstInfo = default;

        //    if (weapon != null && attacker != null && victim != null)
        //    {
        //        var compMods = weapon.TryGetComp<CompWeaponModContainer>();
        //        if (compMods != null)
        //        {
        //            firstInfo = original.FirstOrDefault();

        //            foreach (var effect in compMods.GetEffectsOfType<MeleePreHit>(EffectCategory.MeleePreHit))
        //            {
        //                effect.Apply(attacker, victim, ref __instance, ref firstInfo, ref damageMod);
        //            }
        //        }
        //    }

        //    foreach (DamageInfo dinfo in original)
        //    {
        //        if (!firstDone)
        //        {
        //            firstDone = true;

        //            firstInfo.SetAmount(firstInfo.Amount * damageMod);
        //            ProjectileInfoExteder.RegisterVictim(target.Thing, __instance.EquipmentSource);
        //            yield return firstInfo;
        //        }
        //        else
        //        {
        //            yield return dinfo;
        //        }
        //    }
        //}
        
        public static void Verb_ThingPostApplyDamage_Patch(Thing __instance, DamageInfo dinfo, float totalDamageDealt)
        {
            // Thing here is the target being hit

            if (ProjectileInfoExteder.VictimToEquip.TryGetValue(__instance, out Thing equipment))
            {
                ProjectileInfoExteder.RemveVictim(__instance);

                var compMods = equipment.TryGetComp<CompWeaponModContainer>();

                if (compMods != null)
                {
                    foreach (PostHit effect in compMods.GetEffectsOfType<PostHit>(EffectCategory.PostHit))
                    {
                        effect.Apply((Pawn)dinfo.Instigator, __instance, dinfo, totalDamageDealt);
                    }

                    if (__instance is Pawn pawn && pawn.health.ShouldBeDead())
                    {
                        foreach (OnKill effect in compMods.GetEffectsOfType<OnKill>(EffectCategory.OnKill))
                        {
                            effect.Apply((Pawn)dinfo.Instigator, __instance, dinfo, totalDamageDealt);
                        }
                    }

                }

            }
        }
    }
}
