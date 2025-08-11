using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods.Restrictions
{
    public class RestrictionDef : Def
    {
        [NoTranslate]
        public List<string> Tags;

        [NoTranslate]
        public List<string> TagsExclude;

        [NoTranslate]
        public List<string> TagsAny;

        [NoTranslate]
        public List<string> Def;

        [NoTranslate]
        public List<IRestrictEffect> Extension;
    }

    public interface IRestrictEffect
    {
        bool CheckAvailable(Thing target);
    }
}
