using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace FerLiebor;

[HarmonyPatch]
public static class Patch_PregnancyUtility_GetInheritedGenes
{
	private static MethodBase TargetMethod()
	{
		return typeof(PregnancyUtility).GetMethod("GetInheritedGenes", BindingFlags.Static | BindingFlags.Public, null, new Type[3]
		{
			typeof(Pawn),
			typeof(Pawn),
			typeof(bool).MakeByRefType()
		}, null);
	}

	private static bool Prefix(ref List<GeneDef> __result, Pawn father, Pawn mother, ref bool success)
	{
		if (mother == null && father == null)
		{
			return true;
		}
		Pawn pawn = null;
		if (father != null && FL_Common.IsBun(father) && father.genes.HasActiveGene(FL_GeneDefOf.Fer_Liebor_Pregnancy))
		{
			pawn = father;
		}
		else if (mother != null && FL_Common.IsBun(mother) && mother.genes.HasActiveGene(FL_GeneDefOf.Fer_Liebor_Pregnancy))
		{
			pawn = mother;
		}
		if (pawn != null)
		{
			success = true;
			__result = pawn.genes.GenesListForReading.Select((Gene g) => g.def).ToList();
			return false;
		}
		return true;
	}
}
