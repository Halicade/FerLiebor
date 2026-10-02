using RimWorld;
using Verse;

namespace FerLiebor;

public class StatPart_LearningRate : StatPart
{

    private SimpleCurve curve;

    public override void TransformValue(StatRequest req, ref float val) {
        if (req.HasThing && req.Thing is Pawn pawn && IsFerLiebor(pawn)) {
            val *= FerLieborAgeFactor(pawn);
        }
    }


    public override string ExplanationPart(StatRequest req) {
        if (req.HasThing && req.Thing is Pawn pawn && IsFerLiebor(pawn)) {
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