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
        public override Vector2 InitialSize => new Vector2(1000f, 600f);

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

        private const float ModBoxWidth = 160f;
        private const float ModBoxHeight = 60f;
        private const float ModSpacing = 10f;
        private const float ModListTopPadding = 10f;

        private const int GridColumns = 4;
        private const int GridRows = 2;

        private Vector2 scrollPos;
        private int dragGroupID = -1;

        //private List<WeaponModComp> discoveredMods = new List<WeaponModComp>();
        private List<ModEffectDef> discoveredMods = new List<ModEffectDef>();

        public override void DoWindowContents(Rect inRect)
        {
            Rect contentRect = inRect.ContractedBy(Padding);

            float slotGridHeight = (ModBoxHeight + ModSpacing) * GridRows + 120f;
            float modListHeight = 180f;

            Rect slotGridRect = new Rect(contentRect.x, contentRect.y, contentRect.width, slotGridHeight);
            Rect modListRect = new Rect(contentRect.x, contentRect.yMax - modListHeight, contentRect.width, modListHeight);

            dragGroupID = DragAndDropWidget.NewGroup(null);

            //RefreshNearbyMods();
            DrawModList(modListRect);
            DrawModSlotGrid(slotGridRect);
            DrawDragGhost();
        }

        private void DrawModSlotGrid(Rect rect)
        {
            Widgets.DrawMenuSection(rect);

            float totalWidth = GridColumns * ModBoxWidth + (GridColumns - 1) * ModSpacing;
            float startX = rect.x + (rect.width - totalWidth) / 2f;
            float startY = rect.y + 10f;

            var dragging = DragAndDropWidget.CurrentlyDraggedDraggable();

            for (int row = 0; row < GridRows; row++)
            {
                for (int col = 0; col < GridColumns; col++)
                {
                    int slotIndex = row * GridColumns + col;

                    Rect cellRect = new Rect(
                        startX + col * (ModBoxWidth + ModSpacing),
                        startY + row * (ModBoxHeight + ModSpacing),
                        ModBoxWidth,
                        ModBoxHeight
                    );

                    Widgets.DrawBoxSolid(cellRect, new Color(0.2f, 0.2f, 0.2f, 0.6f));

                    DragAndDropWidget.DropArea(dragGroupID, cellRect, obj =>
                    {
                        if (obj is ModEffectDef dropped)
                            Log.Message($"Dropped mod {dropped.defName} into slot {slotIndex}");
                    }, null);

                    if (dragging is ModEffectDef && DragAndDropWidget.HoveringDropAreaRect(dragGroupID, null) is Rect hover && hover == cellRect)
                        Widgets.DrawHighlight(cellRect);
                }
            }
        }

        private void DrawModList(Rect rect)
        {
            Widgets.DrawMenuSection(rect);

            int modsPerRow = Mathf.FloorToInt((rect.width - Padding * 2) / (ModBoxWidth + ModSpacing));
            if (modsPerRow < 1) modsPerRow = 1;

            int totalRows = Mathf.CeilToInt((float)DefDatabase<ModEffectDef>.AllDefs.Count() / modsPerRow);
            float contentWidth = rect.width - Padding * 2;
            float viewHeight = totalRows * (ModBoxHeight + ModSpacing) + ModListTopPadding;
            Rect viewRect = new Rect(0f, 0f, contentWidth, viewHeight);

            float totalRowWidth = modsPerRow * (ModBoxWidth + ModSpacing) - ModSpacing;
            float startX = (viewRect.width - totalRowWidth) / 2f;

            Widgets.BeginScrollView(rect, ref scrollPos, viewRect);

            int i = 0;
            foreach (var effectDef in DefDatabase<ModEffectDef>.AllDefs)
            {
                if (effectDef == null) continue;

                int row = i / modsPerRow;
                int col = i % modsPerRow;

                Rect modRect = new Rect(
                    startX + col * (ModBoxWidth + ModSpacing),
                    ModListTopPadding + row * (ModBoxHeight + (ModSpacing * 2)),
                    ModBoxWidth,
                    ModBoxHeight
                );

                if (DragAndDropWidget.Draggable(dragGroupID, modRect, effectDef, null, null))
                {
                    // Dragging
                }
                else
                {
                    Widgets.DrawBoxSolid(modRect, new Color(0.3f, 0.3f, 0.3f, 0.6f));

                    Rect drainRect = new Rect(modRect.x + 6f, modRect.y + 4f, 40f, 20f);
                    Text.Font = GameFont.Tiny;
                    Text.Anchor = TextAnchor.UpperLeft;
                    Widgets.Label(drainRect, $"{effectDef.Drain}");

                    Text.Font = GameFont.Small;

                    Rect labelRect = modRect.ContractedBy(4f);
                    labelRect.y += 12f;

                    Text.Anchor = TextAnchor.MiddleCenter;
                    Widgets.Label(labelRect, effectDef.LabelCap);
                    Text.Anchor = TextAnchor.UpperLeft;
                }

                i++;
            }

            Widgets.EndScrollView();
        }


        private void DrawDragGhost()
        {
            var dragging = DragAndDropWidget.CurrentlyDraggedDraggable();
            if (dragging is ModEffectDef def)
            {
                Vector2 size = new Vector2(ModBoxWidth, 30f);
                Vector2 pos = Event.current.mousePosition;

                Rect dragRect = new Rect(pos.x - size.x / 2, pos.y - size.y / 2, size.x, size.y);
                Widgets.DrawBoxSolid(dragRect, new Color(0.4f, 0.4f, 0.4f, 0.8f));
                Widgets.NoneLabelCenteredVertically(dragRect.ContractedBy(4f), def.LabelCap);
            }
        }

        //private void RefreshNearbyMods()
        //{
        //    discoveredMods.Clear();
        //    IntVec3 center = internalBuilding.Position;
        //    Map map = internalBuilding.Map;
        //    float radius = internalBuilding.ScanRadius;

        //    foreach (var thing in GenRadial.RadialDistinctThingsAround(center, map, radius, useCenter: true))
        //    {
        //        if (thing.def is ModificationDef && thing is WeaponModComp modItem && modItem.effect != null)
        //        {
        //            discoveredMods.Add(modItem);
        //        }
        //    }
        //}

        //private void RefreshNearbyMods()
        //{
        //    discoveredMods.Clear();
        //    foreach (var def in DefDatabase<ModEffectDef>.AllDefs)
        //    {
        //        discoveredMods.Add(def);
        //    }
        //}

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
