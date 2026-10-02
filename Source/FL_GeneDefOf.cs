using RimWorld;
using Verse;

namespace FerLiebor;

[DefOf]
public static class FL_GeneDefOf
{
	public static XenotypeDef Fer_Liebor;

	public static GeneDef Fer_Liebor_Savage;

	public static GeneDef Fer_Liebor_Pregnancy;

	public static GeneDef Fer_Liebor_Litters;

	static FL_GeneDefOf()
	{
		DefOfHelper.EnsureInitializedInCtor(typeof(FL_GeneDefOf));
	}
}
