using HarmonyLib;
using RimWorld;
using Verse;

namespace FerLiebor;

[HarmonyPatch(typeof(PregnancyUtility), "ApplyBirthOutcome")]
public static class Patch_PregnancyUtility_ApplyBirthOutcome
{
	private static void Postfix(Thing __result, Pawn geneticMother, Pawn father)
	{
		if (__result is Pawn pawn)
		{
			Pawn pawn2 = null;
			if (father != null && FL_Common.IsBun(father) && father.genes.HasActiveGene(FL_GeneDefOf.Fer_Liebor_Pregnancy))
			{
				pawn2 = father;
			}
			else if (geneticMother != null && FL_Common.IsBun(geneticMother) && geneticMother.genes.HasActiveGene(FL_GeneDefOf.Fer_Liebor_Pregnancy))
			{
				pawn2 = geneticMother;
			}
			if (pawn2?.genes?.Xenotype != null)
			{
				pawn.genes.SetXenotypeDirect(pawn2.genes.Xenotype);
				pawn.genes.xenotypeName = pawn2.genes.xenotypeName;
				pawn.genes.iconDef = pawn2.genes.iconDef;
				pawn.genes.hybrid = false;
			}
		}
	}
}
