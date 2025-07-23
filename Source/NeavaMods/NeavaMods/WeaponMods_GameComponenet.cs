using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class WeaponMods_GameComponenet : GameComponent
    {

        public WeaponMods_GameComponenet(Game game)
        {
            Instance = this;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref num, "test", 0);
        }

        public int num = 0;

        public static WeaponMods_GameComponenet Instance;
    }
}
