using HarmonyLib;
using Verse;

namespace FerLiebor;

[HarmonyPatch(typeof(Pawn_AgeTracker), "GrowthPointsPerDay", MethodType.Getter)]
public static class GrowthPointsPatch
{
    private const float GrowthMultiplier = 6f;

    [HarmonyPostfix]
    public static void Postfix(ref float __result, Pawn_AgeTracker __instance, Pawn ___pawn) {
        if (__result > 0f &&
        ___pawn.genes?.Xenotype == FL_GeneDefOf.Fer_Liebor) {
            __result *= GrowthMultiplier;
        }
    }
}