﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    internal class NeavaMods_GameComponent : GameComponent
    {
        public static Settings Settings { get; private set; }

        public static List<ModDef> ModDefs = new List<ModDef>();
        public NeavaMods_GameComponent(Game game)
        {
            Settings = LoadedModManager.GetMod<NeavaMods_Mod>().GetSettings<Settings>();
        }

        public static void InitOnNewGame()
        {
            ModDefs = new List<ModDef>();
        }

        public static void InitOnLoad()
        {
            if (ModDefs == null)
            {
                ModDefs = new List<ModDef>();
            }

            if (!ModDefs.Contains(NeavaModsDefOfs.Mod_Test))
            {
                ModDefs.Add(NeavaModsDefOfs.Mod_Test);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref ModDefs, "ModDefs", LookMode.Deep);

        }
    }
}
