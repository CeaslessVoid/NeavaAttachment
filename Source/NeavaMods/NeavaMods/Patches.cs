//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NeavaMods
//{
//    using HarmonyLib;
//    using RimWorld;
//    using UnityEngine;
//    using Verse;
//    using Verse.Sound;
//    using static RimWorld.PsychicRitualRoleDef;

//    public class ProjectilInfoExteder : IExposable
//    {

//        public static Dictionary<Projectile, Thing> projectileData = new Dictionary<Projectile, Thing>();
//        public void ExposeData()
//        {
//            Scribe_Collections.Look(ref projectileData, "projectileData");
//        }

//        public static Thing GetEquipment(Projectile projectile)
//        {
//            if (projectileData.TryGetValue(projectile, out Thing equipment))
//            {
//                return equipment;
//            }
//            return null;
//        }

//        public static void SetEquipment(Projectile projectile, Thing equipment)
//        {
//            projectileData[projectile] = equipment;
//        }
//    }

//    [StaticConstructorOnStartup]
//    public static class HarmonyPatches
//    {
//        static HarmonyPatches()
//        {
//            Main.HarmonyInstance.Patch(
//            original: AccessTools.Method(
//                typeof(Projectile),
//                "Launch",
//                new Type[] {
//                    typeof(Thing),       // launcher
//                    typeof(Vector3),     // origin
//                    typeof(LocalTargetInfo), // usedTarget
//                    typeof(LocalTargetInfo), // intendedTarget
//                    typeof(ProjectileHitFlags), // hitFlags
//                    typeof(bool),        // preventFriendlyFire
//                    typeof(Thing),       // equipment
//                    typeof(ThingDef)        // targetCoverDef
//                }),
//            prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(OnHitPreCheckLaunch))
//        );

//            Main.HarmonyInstance.Patch(
//                original: AccessTools.Method(typeof(Bullet), "Impact"),
//                prefix: new HarmonyMethod(typeof(HarmonyPatches), nameof(OnHitPreCheck))
//            );

//            Main.HarmonyInstance.Patch(
//                original: AccessTools.Method(typeof(Verb_MeleeAttackDamage), "DamageInfosToApply"),
//                postfix: new HarmonyMethod(typeof(HarmonyPatches), nameof(OnHitPreCheckMelee))
//            );

//        }

//        public static void OnHitPreCheckLaunch(Projectile __instance, Thing launcher, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, bool preventFriendlyFire = false, Thing equipment = null)
//        {
//            if (launcher is Pawn pawn && (pawn.Faction == Faction.OfPlayer || NeavaMods_GameComponent.Settings.enemyUseMods))
//            {
//                ProjectilInfoExteder.SetEquipment(__instance, equipment);
//            }
//        }

//        public static bool OnHitPreCheck(Bullet __instance, Thing hitThing, bool blockedByShield = false)
//        {

//            if (ProjectilInfoExteder.projectileData.TryGetValue(__instance, out Thing equipment))
//            {
//                ProjectilInfoExteder.projectileData.Remove(__instance);

//                if (__instance.Launcher is Pawn pawn && (pawn.Faction == Faction.OfPlayer || NeavaMods_GameComponent.Settings.enemyUseMods) && hitThing is Pawn pawn2)
//                {
//                    var compMods = equipment.TryGetComp<CompWeaponMods>();

//                    if (compMods != null)
//                    {
//                        float damageMod = 1;

//                        var onHitEffects = compMods.GetEffectsByProcType(ProcType.OnHitPreCheck);
//                        if (onHitEffects != null)
//                        {
//                            foreach (var effect in onHitEffects)
//                            {
//                                effect.ApplyEffect(pawn, pawn2, __instance, null, ref damageMod);
//                            }
//                        }

//                        // Hijack

//                        Map map = __instance.Map;
//                        IntVec3 position = __instance.Position;

//                        // Projectile class
//                        GenClamor.DoClamor(__instance, 12f, ClamorDefOf.Impact);
//                        if (!blockedByShield && __instance.def.projectile.landedEffecter != null)
//                        {
//                            __instance.def.projectile.landedEffecter.Spawn(__instance.Position, __instance.Map, 1f).Cleanup();
//                        }
//                        __instance.Destroy(DestroyMode.Vanish);

//                        BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(__instance.Launcher, hitThing, __instance.intendedTarget.Thing, __instance.EquipmentDef, __instance.def, ThingDefOf.Barricade);
//                        Find.BattleLog.Add(battleLogEntry_RangedImpact);

//                        // Notify Impact
//                        BulletImpactData impactData = new BulletImpactData
//                        {
//                            bullet = __instance,
//                            hitThing = hitThing,
//                            impactPosition = position
//                        };
//                        if (hitThing != null)
//                        {
//                            hitThing.Notify_BulletImpactNearby(impactData);

//                            //we can actually throw the rest in here
//                            Pawn pawn3;
//                            bool instigatorGuilty = (pawn3 = (__instance.Launcher as Pawn)) == null || !pawn.Drafted;

//                            DamageInfo dinfo = new DamageInfo(__instance.def.projectile.damageDef, __instance.DamageAmount * damageMod, __instance.ArmorPenetration, __instance.ExactRotation.eulerAngles.y, __instance.Launcher, null, __instance.EquipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, __instance.intendedTarget.Thing, instigatorGuilty, true, QualityCategory.Normal, true);
//                            QualityCategory quality;
//                            equipment.TryGetQuality(out quality);
//                            dinfo.SetWeaponQuality(quality);

//                            hitThing.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_RangedImpact);
//                            Pawn pawn4 = hitThing as Pawn;
//                            if (pawn4 != null)
//                            {
//                                Pawn_StanceTracker stances = pawn2.stances;
//                                if (stances != null)
//                                {
//                                    stances.stagger.Notify_BulletImpact(__instance);
//                                }
//                            }
//                            if (__instance.def.projectile.extraDamages != null)
//                            {
//                                foreach (ExtraDamage extraDamage in __instance.def.projectile.extraDamages)
//                                {
//                                    if (Rand.Chance(extraDamage.chance))
//                                    {
//                                        DamageInfo dinfo2 = new DamageInfo(extraDamage.def, extraDamage.amount, extraDamage.AdjustedArmorPenetration(), __instance.ExactRotation.eulerAngles.y, __instance.Launcher, null, __instance.EquipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, __instance.intendedTarget.Thing, instigatorGuilty, true, QualityCategory.Normal, true);
//                                        hitThing.TakeDamage(dinfo2).AssociateWithLog(battleLogEntry_RangedImpact);
//                                    }
//                                }
//                            }
//                            if (Rand.Chance(__instance.def.projectile.bulletChanceToStartFire) && (pawn2 == null || Rand.Chance(FireUtility.ChanceToAttachFireFromEvent(pawn2))))
//                            {
//                                hitThing.TryAttachFire(__instance.def.projectile.bulletFireSizeRange.RandomInRange, __instance.Launcher);
//                                return false;
//                            }

//                        }
//                        else
//                        {
//                            if (!blockedByShield)
//                            {
//                                SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(__instance.Position, map, false));
//                                if (__instance.Position.GetTerrain(map).takeSplashes)
//                                {
//                                    FleckMaker.WaterSplash(__instance.ExactPosition, map, Mathf.Sqrt((float)__instance.DamageAmount) * 1f, 4f);
//                                }
//                                else
//                                {
//                                    FleckMaker.Static(__instance.ExactPosition, map, FleckDefOf.ShotHit_Dirt, 1f);
//                                }
//                            }
//                            if (Rand.Chance(__instance.def.projectile.bulletChanceToStartFire))
//                            {
//                                FireUtility.TryStartFireIn(__instance.Position, map, __instance.def.projectile.bulletFireSizeRange.RandomInRange, __instance.Launcher, null);
//                            }
//                        }
//                        int num = 9;
//                        for (int i = 0; i < num; i++)
//                        {
//                            IntVec3 c = position + GenRadial.RadialPattern[i];
//                            if (c.InBounds(map))
//                            {
//                                List<Thing> thingList = c.GetThingList(map);
//                                for (int j = 0; j < thingList.Count; j++)
//                                {
//                                    if (thingList[j] != hitThing)
//                                    {
//                                        thingList[j].Notify_BulletImpactNearby(impactData);
//                                    }
//                                }
//                            }
//                        }

//                        return false;
//                    }
//                }
//            }

//            return true;
//        }

//        public static void OnHitPreCheckMelee(Verb_MeleeAttackDamage __instance, ref IEnumerable<DamageInfo> __result, LocalTargetInfo target)
//        {
//            __result = WrapEnumerator(__instance, __result, target);
//        }

//        private static IEnumerable<DamageInfo> WrapEnumerator(Verb_MeleeAttackDamage __instance, IEnumerable<DamageInfo> original, LocalTargetInfo target)
//        {
//            foreach (var damageInfo in original)
//            {

//                if (damageInfo.Instigator is Pawn pawn && (pawn.Faction == Faction.OfPlayer || NeavaMods_GameComponent.Settings.enemyUseMods) && target.Pawn != null)
//                {
//                    ThingWithComps weapon = __instance.EquipmentSource;

//                    var compMods = weapon.TryGetComp<CompWeaponMods>();

//                    if (compMods != null)
//                    {
//                        float damageMod = 1;

//                        var onHitEffects = compMods.GetEffectsByProcType(ProcType.OnHitPreCheck);
//                        if (onHitEffects != null)
//                        {
//                            foreach (var effect in onHitEffects)
//                            {
//                                effect.ApplyEffect(pawn, target.Pawn, null, __instance, ref damageMod);
//                            }
//                        }

//                        damageInfo.SetAmount(damageInfo.Amount * damageMod);
//                    }
//                }

//                yield return damageInfo;

//            }
//        }

//    }

//}
