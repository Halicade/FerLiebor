using RimWorld;
using Verse;

namespace FerLiebor;

[StaticConstructorOnStartup]
internal static class FL_Common
{
	public static bool IsBun(Pawn pawn)
	{
		return pawn?.genes?.Xenotype == FL_GeneDefOf.Fer_Liebor;
	}

	internal static bool IsPregnantWithBun(Pawn pawn) {
		return PregnancyUtility.GetPregnancyHediff(pawn) is Hediff_Pregnant hediff_Pregnant && (IsBun(pawn) || IsBun(hediff_Pregnant.Father));
	}
}
