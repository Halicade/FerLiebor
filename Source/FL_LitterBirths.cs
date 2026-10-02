using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace FerLiebor;

[HarmonyPatch]
public static class FL_LitterBirths
{
    private static Pawn _currentlyPregnantPawn;

    private static int _pawnsToGenerate;

    private static bool pawnFinishedGivingBirth;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Hediff_Labor), nameof(Hediff_Labor.PostRemoved))]
    public static void Labor_PostRemoved_Postfix(Hediff_Labor __instance) {
        if (pawnFinishedGivingBirth) {
            pawnFinishedGivingBirth = false;
            _currentlyPregnantPawn = null;
            return;
        }

        Pawn pawn = __instance.pawn;
        if (pawn == _currentlyPregnantPawn) {
            return;
        }

        if (_pawnsToGenerate > 0) {
            return;
        }

        Pawn_GeneTracker genes = pawn.genes;
        if (genes != null && genes.HasActiveGene(FL_GeneDefOf.Fer_Liebor_Litters)) {
            _currentlyPregnantPawn = pawn;
            _pawnsToGenerate = Rand.RangeInclusive(0, 4);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Hediff_LaborPushing), nameof(Hediff_LaborPushing.PostRemoved))]
    public static void LaborPushing_PostRemoved_Postfix(Hediff_LaborPushing __instance) {
        Pawn pawn = __instance.pawn;
        if (pawn == _currentlyPregnantPawn && _pawnsToGenerate > 0) {
            _pawnsToGenerate--;
            Hediff_LaborPushing hediffLabor = (Hediff_LaborPushing)pawn.health.AddHediff(HediffDefOf.PregnancyLaborPushing);
            hediffLabor.SetParents(pawn, __instance.Father, PregnancyUtility.GetInheritedGeneSet(__instance.Father, pawn));

        }

        if (_pawnsToGenerate <= 0) {
            pawnFinishedGivingBirth = true;
            _currentlyPregnantPawn = null;
        }
    }
}