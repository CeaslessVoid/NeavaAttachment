using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods.Restrictions
{
    public class RestrictionRangedOnly : IRestrictEffect
    {
        public bool CheckAvailable(Thing target)
        {
            return target.def.IsRangedWeapon;
        }
    }
}
