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
            draggable = false;

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

        private int dragGroupID = -1;
        public override void DoWindowContents(Rect inRect)
        {
            Rect contentRect = inRect.ContractedBy(Padding);

            float leftWidth = contentRect.width * LeftWidthPct;
            Rect leftRect = new Rect(contentRect.x, contentRect.y, leftWidth - Padding, contentRect.height);
            Rect rightRect = new Rect(leftRect.xMax + Padding, contentRect.y, contentRect.width - leftWidth - Padding, contentRect.height);

            DrawRightGrid(rightRect);
            DrawLeftScrollArea(leftRect);
        }

        private void DrawLeftScrollArea(Rect rect)
        {
            Widgets.DrawMenuSection(rect);

            dragGroupID = DragAndDropWidget.NewGroup(null);

            Rect viewRect = new Rect(0f, 0f, rect.width - 20f, scrollViewHeightLeft);
            Widgets.BeginScrollView(rect, ref scrollPosLeft, viewRect);

            float curY = 0f;
            int column = 0;
            float columnWidth = viewRect.width / ModColumnCount;

            foreach (var effectDef in DefDatabase<ModEffectDef>.AllDefs)
            {
                if (effectDef == null)
                    continue;

                Rect modRect = new Rect(column * columnWidth, curY, columnWidth - 10f, columnWidth - 10f);

                if (DragAndDropWidget.Draggable(dragGroupID, modRect, effectDef, null, null))
                {
                    Rect dragRect = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 160f, 30f);
                    Widgets.DrawBoxSolid(dragRect, new Color(0.4f, 0.4f, 0.4f, 0.8f));
                    Widgets.Label(dragRect.ContractedBy(4f), effectDef.LabelCap);
                }
                else
                {
                    Widgets.DrawBoxSolid(modRect, new Color(0.2f, 0.2f, 0.2f, 0.5f));
                    Widgets.Label(modRect.ContractedBy(4f), effectDef.label);
                }

                column++;
                if (column >= ModColumnCount)
                {
                    column = 0;
                    curY += columnWidth - 10f + 6f;
                }
            }

            scrollViewHeightLeft = curY + columnWidth - 10f;
            Widgets.EndScrollView();
        }

        private void DrawRightGrid(Rect rect)
        {
            Widgets.DrawMenuSection(rect);

            float spacing = 10f;
            float totalWidth = GridColumns * GridBoxSize + (GridColumns - 1) * spacing;
            float startX = rect.x + (rect.width - totalWidth) / 2f;
            float startY = rect.y + 20f;

            for (int row = 0; row < GridRows; row++)
            {
                for (int col = 0; col < GridColumns; col++)
                {
                    Rect cellRect = new Rect(
                        startX + col * (GridBoxSize + spacing),
                        startY + row * (GridBoxSize + spacing),
                        GridBoxSize,
                        GridBoxSize
                    );

                    Widgets.DrawBoxSolid(cellRect, new Color(0.15f, 0.15f, 0.15f, 0.5f));

                    int capturedRow = row;
                    int capturedCol = col;

                    DragAndDropWidget.DropArea(dragGroupID, cellRect, obj =>
                    {
                        if (obj is ModEffectDef dropped)
                        {
                            Log.Message($"Dropped: {dropped.defName} into slot [{capturedRow}, {capturedCol}]");
                        }
                    }, null);

                    if (DragAndDropWidget.CurrentlyDraggedDraggable() is ModEffectDef)
                    {
                        var hovering = DragAndDropWidget.HoveringDropAreaRect(dragGroupID, null);
                        if (hovering.HasValue && hovering.Value == cellRect)
                        {
                            Widgets.DrawHighlight(cellRect);
                        }
                    }
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
