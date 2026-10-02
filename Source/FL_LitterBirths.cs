using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace FerLiebor;

[StaticConstructorOnStartup]
public static class FL_LitterBirths
{
	private static readonly Dictionary<string, int> litterBirthCounts;

	static FL_LitterBirths()
	{
		litterBirthCounts = new Dictionary<string, int>();
		Harmony harmony = new Harmony("FerLiebor.LitterBirths");
		harmony.Patch(AccessTools.Method(typeof(Hediff_Labor), "PostRemoved"), null, new HarmonyMethod(typeof(FL_LitterBirths), "Labor_PostRemoved_Postfix"));
		harmony.Patch(AccessTools.Method(typeof(Hediff_LaborPushing), "PostRemoved"), null, new HarmonyMethod(typeof(FL_LitterBirths), "LaborPushing_PostRemoved_Postfix"));
	}

	public static void Labor_PostRemoved_Postfix(Hediff_Labor __instance)
	{
		Pawn pawn = __instance.pawn;
		Pawn_GeneTracker genes = pawn.genes;
		if (genes != null && genes.HasActiveGene(FL_GeneDefOf.Fer_Liebor_Litters) && !litterBirthCounts.ContainsKey(pawn.ThingID))
		{
			int value = Rand.RangeInclusive(1, 4);
			litterBirthCounts[pawn.ThingID] = value;
		}
	}

	public static void LaborPushing_PostRemoved_Postfix(Hediff_LaborPushing __instance)
	{
		Pawn pawn = __instance.pawn;
		string thingID = pawn.ThingID;
		if (litterBirthCounts.TryGetValue(thingID, out var value))
		{
			value--;
			if (value <= 0)
			{
				litterBirthCounts.Remove(thingID);
				return;
			}
			litterBirthCounts[thingID] = value;
			Hediff_Labor hediff_Labor = (Hediff_Labor)pawn.health.AddHediff(HediffDefOf.PregnancyLabor);
			hediff_Labor.SetParents(pawn, __instance.Father, PregnancyUtility.GetInheritedGeneSet(__instance.Father, pawn));
		}
	}
}
