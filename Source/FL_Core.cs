using HarmonyLib;
using Verse;


public class FL_Core : Mod
{

    public FL_Core(ModContentPack content) : base(content) {
        LongEventHandler.QueueLongEvent(action: HarmonyPatches,
            textKey: null,
            doAsynchronously: true,
            exceptionHandler: null
        );


    }

    private static void HarmonyPatches() {
        var harmony = new Harmony("SinnerPen.FerLiebor");
        harmony.PatchAll();

    }
}