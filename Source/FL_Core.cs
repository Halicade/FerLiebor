using HarmonyLib;
using Verse;


public class FL_Core : Mod
{

	public FL_Core(ModContentPack content) : base(content) {
		var harmony = new Harmony("SinnerPen.FerLiebor");
		harmony.PatchAll();
	}
}
