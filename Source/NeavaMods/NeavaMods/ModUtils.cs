using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    internal static class ModUtils
    {
        public static void Msg(object o)
        {
            Log.Message("[Neava's Mods] " + ((o != null) ? o.ToString() : null));
        }

        public static void Warn(object o)
        {
            Log.Warning("[Neava's Mods] " + ((o != null) ? o.ToString() : null));
        }

        public static void Error(object o)
        {
            Log.Error("[Neava's Mods] " + ((o != null) ? o.ToString() : null));
        }

    }
}
