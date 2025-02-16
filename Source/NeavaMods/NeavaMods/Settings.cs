using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class Settings : ModSettings
    {
        public override void ExposeData()
        {
            base.ExposeData();
        }

        public bool enemyUseMods = false;

    }

    internal class NeavaMods_Mod : Mod
    {
        public NeavaMods_Mod(ModContentPack content) : base(content)
        {
            this._settings = GetSettings<Settings>();
        }

        private Settings _settings;
    }
}
