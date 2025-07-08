using LudeonTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class Debug
    {
        [DebugAction("NeavaMods", "Spawn WeaponModBasic with TestEffect", allowedGameStates = AllowedGameStates.PlayingOnMap)]
        public static void SpawnWeaponModWithEffect()
        {
            Map map = Find.CurrentMap;
            if (map == null)
            {
                ModUtils.Error("No current map to spawn item.");
                return;
            }

            IntVec3 pos = UI.MouseCell();

            Thing modThing = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("WeaponModBasic", errorOnFail: false));

            if (modThing is WeaponModComp weaponMod)
            {
                if (weaponMod.effect == null)
                {
                    ModUtils.Warn("WeaponModBasic spawned with null effect. Aborting spawn.");
                    return;
                }
            }
            else
            {
                ModUtils.Warn("Spawned thing is not a WeaponModComp.");
            }

            GenPlace.TryPlaceThing(modThing, pos, map, ThingPlaceMode.Near);
        }
    }
}
