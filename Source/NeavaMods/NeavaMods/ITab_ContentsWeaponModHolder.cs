using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    public class ITab_ContentsWeaponModHolder : ITab_ContentsBase
    {
        public override IList<Thing> container
        {
            get
            {
                return ContainerThing.innerContainer;
            }
        }

        public CompWeaponContainer ContainerThing
        {
            get
            {
                return base.SelThing.TryGetComp<CompWeaponContainer>();
            }
        }

        public ITab_ContentsWeaponModHolder()
        {
            labelKey = "TabCasketContents";
            containedItemsKey = "TabCasketContents";
        }
    }
}
