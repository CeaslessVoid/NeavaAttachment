using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace NeavaMods
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches2
    {
        static HarmonyPatches2()
        {
            //Main.HarmonyInstance.Patch(
            //original: AccessTools.Method(typeof(FloatMenuMakerMap), "GetProviderOptions"),
            //postfix: new HarmonyMethod(typeof(HarmonyPatches2), nameof(AddWeaponModFloatMenuOptions))
            //);
        }

        // > Doesn't work without harmony patch
        // > Makes harmony patch
        // > Now works with both normal and harmony patch
        // WTF is this bullshit

        public static void AddWeaponModFloatMenuOptions(FloatMenuContext context, List<FloatMenuOption> options)
        {
            try
            {
                if (!context.ValidSelectedPawns.Any()) return;

                var provider = new FloatMenuOptionProvider_WeaponMod();

                if (!provider.Applies(context)) return;

                foreach (var thing in context.ClickedThings)
                {
                    if (provider.TargetThingValid(thing, context))
                    {
                        Thing target = thing;
                        if (thing.TryGetComp<CompSelectProxy>() is CompSelectProxy proxy && proxy.thingToSelect != null)
                            target = proxy.thingToSelect;

                        foreach (var opt in provider.GetOptionsFor(target, context))
                        {
                            if (opt.iconThing == null) opt.iconThing = target;
                            opt.targetsDespawned = !target.Spawned;
                            options.Add(opt);
                        }
                    }
                }

                foreach (var pawn in context.ClickedPawns)
                {
                    if (provider.TargetPawnValid(pawn, context))
                    {
                        foreach (var opt in provider.GetOptionsFor(pawn, context))
                        {
                            if (opt.iconThing == null) opt.iconThing = pawn;
                            opt.targetsDespawned = !pawn.Spawned;
                            options.Add(opt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error injecting float menu options: {ex}");
            }
        }
    }

}
