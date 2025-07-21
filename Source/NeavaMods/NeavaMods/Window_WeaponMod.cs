using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace NeavaMods
{
    public class Window_WeaponMod : Window
    {
        public override Vector2 InitialSize => new Vector2(1200f, 900f);

        public Window_WeaponMod(Building_WeaponMod building)
        {
            forcePause = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = true;
            doCloseX = true;
            draggable = true;

            internalBuilding = building;
        }
        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Medium;
            Widgets.Label(inRect.TopPart(0.2f), "Weapon Modding Window");

            Text.Font = GameFont.Small;
            Widgets.Label(inRect.ContractedBy(10f).BottomPart(0.8f), "This will be your GUI for modifying weapons.");
        }

        public static void ToggleWindow(Building_WeaponMod building)
        {
            Window_WeaponMod window = (Window_WeaponMod)Enumerable.FirstOrDefault<Window>(
                Find.WindowStack.Windows, x => x is Window_WeaponMod);
            if (window != null)
            {
                SoundDefOf.TabOpen.PlayOneShotOnCamera(building.Map);
                window.internalBuilding = building;
                return;
            }

            Find.WindowStack.Add(new Window_WeaponMod(building));
        }


        private Building_WeaponMod internalBuilding;
    }
}
