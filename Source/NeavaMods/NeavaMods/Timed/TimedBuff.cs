using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods.Timed
{
    public class TimedBuff : IExposable
    {
        public string id;
        public float power;
        public int expireTick;

        public void ExposeData()
        {
            Scribe_Values.Look(ref id, "buffId", id, true);
            Scribe_Values.Look(ref power, "buffpower", power, true);
            Scribe_Values.Look(ref expireTick, "buffexpireTick", expireTick, true);
        }
    }

}
