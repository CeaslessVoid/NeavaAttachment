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

        private const float Padding = 10f;
        private const float LeftWidthPct = 0.4f;

        private Vector2 scrollPosLeft;
        private float scrollViewHeightLeft;

        private const float ModBoxWidth = 150f;
        private const float ModBoxHeight = 40f;
        private const int ModColumnCount = 2;

        private const float GridBoxSize = 100f;
        private const int GridColumns = 4;
        private const int GridRows = 2;
        public override void DoWindowContents(Rect inRect)
        {
            Rect contentRect = inRect.ContractedBy(Padding);

            float leftWidth = contentRect.width * LeftWidthPct;
            Rect leftRect = new Rect(contentRect.x, contentRect.y, leftWidth - Padding, contentRect.height);
            Rect rightRect = new Rect(leftRect.xMax + Padding, contentRect.y, contentRect.width - leftWidth - Padding, contentRect.height);

            DrawLeftScrollArea(leftRect);
            DrawRightGrid(rightRect);
        }

        private void DrawLeftScrollArea(Rect rect)
        {
            Widgets.DrawMenuSection(rect);

            Rect viewRect = new Rect(0f, 0f, rect.width - 20f, scrollViewHeightLeft);
            Widgets.BeginScrollView(rect, ref scrollPosLeft, viewRect);

            float curY = 0f;
            int column = 0;
            float columnWidth = viewRect.width / ModColumnCount;

            foreach (var def in DefDatabase<ModificationDef>.AllDefs)
            {
                if (def.effectDef == null)
                    continue;

                Rect modRect = new Rect(column * columnWidth, curY, columnWidth - 10f, ModBoxHeight);
                Widgets.DrawBoxSolid(modRect, new Color(0.2f, 0.2f, 0.2f, 0.5f));
                Widgets.Label(modRect.ContractedBy(4f), def.effectDef.LabelCap);

                column++;
                if (column >= ModColumnCount)
                {
                    column = 0;
                    curY += ModBoxHeight + 6f;
                }
            }

            scrollViewHeightLeft = curY + ModBoxHeight;
            Widgets.EndScrollView();
        }

        private void DrawRightGrid(Rect rect)
        {
            Widgets.DrawMenuSection(rect);

            float spacing = 10f;
            float totalWidth = GridColumns * GridBoxSize + (GridColumns - 1) * spacing;
            float totalHeight = GridRows * GridBoxSize + (GridRows - 1) * spacing;

            float startX = rect.x + (rect.width - totalWidth) / 2f;
            float startY = rect.y + (rect.height - totalHeight) / 2f;

            for (int row = 0; row < GridRows; row++)
            {
                for (int col = 0; col < GridColumns; col++)
                {
                    float x = startX + col * (GridBoxSize + spacing);
                    float y = startY + row * (GridBoxSize + spacing);
                    Rect boxRect = new Rect(x, y, GridBoxSize, GridBoxSize);
                    Widgets.DrawBoxSolidWithOutline(boxRect, new Color(0.3f, 0.3f, 0.3f, 0.3f), Color.gray);
                    Widgets.Label(boxRect.ContractedBy(5f), $"Slot {row * GridColumns + col + 1}");
                }
            }
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
