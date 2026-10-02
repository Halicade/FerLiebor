using RimWorld;
using Verse;

namespace FerLiebor;

public class FL_Thoughts_Patch : ThoughtWorker
{
	protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
	{
		if (def.gender != Gender.None && otherPawn.gender != def.gender)
		{
			return ThoughtState.Inactive;
		}
		if (RelationsUtility.PawnsKnowEachOther(p, otherPawn))
		{
			FL_Extension modExtension = def.GetModExtension<FL_Extension>();
			if (modExtension.nullifyingGenes != null)
			{
				foreach (Gene item in p.genes.GenesListForReading)
				{
					if (modExtension.nullifyingGenes.Contains(item.def))
					{
						return ThoughtState.Inactive;
					}
				}
			}
			if (modExtension.hatedGenes != null)
			{
				foreach (Gene item2 in otherPawn.genes.GenesListForReading)
				{
					if (modExtension.hatedGenes.Contains(item2.def))
					{
						return ThoughtState.ActiveAtStage(0);
					}
				}
			}
			else
			{
				Log.Error(def + " doesn't have any hated genes, meaning it will always be inactive");
			}
		}
		return ThoughtState.Inactive;
	}
}
