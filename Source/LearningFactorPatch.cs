using HarmonyLib;
using RimWorld;
using Verse;

namespace FerLiebor;
/*
 Moved this to Statparts
[HarmonyPatch(typeof(StatWorker), "GetValueUnfinalized")]
public static class LearningFactorPatch
{
	private static readonly SimpleCurve LearningCurve = new SimpleCurve
	{
		new CurvePoint(0f, 4f),
		new CurvePoint(7f, 3f),
		new CurvePoint(13f, 2f),
		new CurvePoint(18f, 1.5f),
		new CurvePoint(20f, 0.8f),
		new CurvePoint(30f, 0.5f)
	};

	[HarmonyPostfix]
	public static void Postfix(ref float __result, StatRequest req, StatDef ___stat)
	{
		
		if (req.Pawn?.genes?.Xenotype == FL_GeneDefOf.Fer_Liebor)
		{
			if (___stat == StatDefOf.GlobalLearningFactor || ___stat == StatDefOf.LearningRateFactor)
			{
				float ageBiologicalYearsFloat = req.Pawn!.ageTracker.AgeBiologicalYearsFloat;
				float num = LearningCurve.Evaluate(ageBiologicalYearsFloat);
				__result *= num;
			}
		}
	}
}
*/