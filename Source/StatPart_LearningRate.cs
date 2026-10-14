using RimWorld;
using Verse;

namespace FerLiebor;

public class StatPart_LearningRate : StatPart
{

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
    private SimpleCurve curve;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

    public override void TransformValue(StatRequest req, ref float val) {
        if (req is { HasThing: true, Thing: Pawn pawn } && IsFerLiebor(pawn)) {
            val *= FerLieborAgeFactor(pawn);
        }
    }


    public override string ExplanationPart(StatRequest req) {
        if (req is { HasThing: true, Thing: Pawn pawn } && IsFerLiebor(pawn)) {
            return "FL_StatsReportIsFerLiebor".Translate() + (": x" + FerLieborAgeFactor(pawn).ToStringPercent());
        }

        return null;
    }

    private bool IsFerLiebor(Pawn pawn) {
        return pawn.genes?.Xenotype == FL_GeneDefOf.Fer_Liebor;
    }

    private float FerLieborAgeFactor(Pawn pawn) {
        float ageBiologicalYearsFloat = pawn.ageTracker.AgeBiologicalYearsFloat;
        return curve.Evaluate(ageBiologicalYearsFloat);
    }
}