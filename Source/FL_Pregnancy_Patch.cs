using FerLiebor;
using HarmonyLib;
using Verse;
/*
[HarmonyPatch]
internal static class FL_Pregnancy_Patch
{
	[HarmonyPatch(typeof(Hediff), "Tick")]
	public static class Hediff_Pregnant_Tick_Patch
	{
		[HarmonyPrefix]
		public static void Prefix(Hediff __instance, out float __state)
		{
			__state = (__instance as Hediff_Pregnant)?.GestationProgress ?? 0f;
		}

		[HarmonyPostfix]
		public static void Postfix(Hediff __instance, float __state)
		{
			if (__instance is Hediff_Pregnant hediff && FL_Common.IsPregnantWithBun(hediff.pawn))
			{
				FL_Utils.GestationProgress(ref hediff, __state);
			}
		}
	}
}
*/