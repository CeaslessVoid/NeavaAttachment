using LudeonTK;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using static NeavaMods.CompProperties_WeaponModContainer;

namespace NeavaMods
{
    public class Debug
    {
        [DebugAction("NeavaMods", "Spawn Bolt-Action Rifle with TestEffect", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void SpawnBoltActionWithTestEffect()
        {
            Map map = Find.CurrentMap;
            if (map == null)
            {
                ModUtils.Error("No current map found.");
                return;
            }

            IntVec3 pos = UI.MouseCell();

            ThingDef rifleDef = DefDatabase<ThingDef>.GetNamed("Gun_BoltActionRifle", errorOnFail: false);
            Thing rifle = ThingMaker.MakeThing(rifleDef);

            var modComp = rifle.TryGetComp<CompWeaponModContainer>();

            if (NeavaDefOf.TestEffect == null)
            {
                ModUtils.Error("TestEffect not found in NeavaDefOf.");
                return;
            }

            modComp.AddMod(NeavaDefOf.TestEffect);

            modComp.CacheEffects();

            GenPlace.TryPlaceThing(rifle, pos, map, ThingPlaceMode.Near);
        }

        [DebugAction("NeavaMods", "Give Longsword with TestEffectt", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void GiveLongswordWithTestEffect()
        {
            Map map = Find.CurrentMap;
            if (map == null)
            {
                ModUtils.Error("No current map found.");
                return;
            }

            IntVec3 pos = UI.MouseCell();

            ThingDef rifleDef = DefDatabase<ThingDef>.GetNamed("MeleeWeapon_LongSword", errorOnFail: false);
            Thing rifle = ThingMaker.MakeThing(rifleDef);

            var modComp = rifle.TryGetComp<CompWeaponModContainer>();

            if (NeavaDefOf.TestEffect == null)
            {
                ModUtils.Error("TestEffect not found in NeavaDefOf.");
                return;
            }

            modComp.AddMod(NeavaDefOf.TestEffectt);

            modComp.CacheEffects();

            GenPlace.TryPlaceThing(rifle, pos, map, ThingPlaceMode.Near);
        }

    }
}
