﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeavaMods
{
    public class ModSlot
    {
        public WeaponModDef Mod;
        public Polarity? Polarity;

        public ModSlot() { }
        public ModSlot(WeaponModDef mod, Polarity? polarity)
        {
            Mod = mod;
            Polarity = polarity;
        }
    }

}
