using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace FerLiebor;

public class FerLieborSpawner
{

    public static void SpawnFerLieborGroup(Map map, float points, int groupCount, List<IntVec3> emergencePoints) {
        if (map == null) {
            return;
        }
        if (emergencePoints == null || emergencePoints.Count == 0) {
            Log.Warning("FerLieborSpawner: No valid emergence points.");
            return;
        }

        Faction faction = Find.FactionManager.FirstFactionOfDef(FL_GeneDefOf.FeralWarren);

        if (faction == null) {
            Log.Error("FerLieborSpawner: Could not find FeralWarren faction.");
            return;
        }
        if (points <= 0f) {
            points = groupCount > 0 ? groupCount * 120f : 200f;
        }
        PawnGroupMakerParms parms = new PawnGroupMakerParms
        {
            faction = faction,
            points = points,
            tile = map.Tile,
            generateFightersOnly = true,
            groupKind = PawnGroupKindDefOf.Combat
        };
        List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(parms).ToList();
        if (list.Count == 0) {
            Log.Warning("FerLieborSpawner: FeralWarren generated no pawns.");
            return;
        }
        for (int i = 0; i < list.Count; i++) {
            Pawn newThing = list[i];
            IntVec3 intVec = emergencePoints[i % emergencePoints.Count];
            IntVec3 intVec2 = CellFinder.RandomClosewalkCellNear(intVec, map, 2);
            if (!intVec2.InBounds(map) || !intVec2.Walkable(map)) {
                intVec2 = intVec;
            }
            if (intVec2.InBounds(map) && intVec2.Walkable(map)) {
                GenSpawn.Spawn(newThing, intVec2, map, Rot4.Random);
            }
        }
        IncidentParms incidentParms = new IncidentParms
        {
            target = map,
            faction = faction,
            points = points,
            pawnGroupKind = PawnGroupKindDefOf.Combat,
            raidStrategy = RaidStrategyDefOf.ImmediateAttack,
            canTimeoutOrFlee = true,
            canSteal = true,
            canKidnap = true
        };
        incidentParms.raidStrategy.Worker.MakeLords(incidentParms, list);
    }

    public static List<IntVec3> FindEmergencePointsForRaid(Map map, int desiredCount) {
        List<IntVec3> list = [];
        if (!FerLieborCellFinder.TryFindCell(out var cell, map)) {
            return list;
        }
        foreach (IntVec3 item in GenRadial.RadialCellsAround(cell, 15f, useCenter: true)) {
            if (item.InBounds(map) && item.Walkable(map) && !item.Fogged(map) && item.Roofed(map) && item.GetFirstPawn(map) == null) {
                list.Add(item);
            }
        }
        List<IntVec3> list2 = [];
        while (list2.Count < desiredCount && list.Count > 0) {
            IntVec3 chosen = list.RandomElement();
            list2.Add(chosen);
            list.RemoveAll(c => c.DistanceTo(chosen) < 8f);
        }
        return list2;
    }

    public static void EmitBurrowDirt(Map map, List<IntVec3> emergencePoints) {
        if (map == null || emergencePoints == null || emergencePoints.Count == 0) {
            return;
        }
        foreach (IntVec3 emergencePoint in emergencePoints) {
            int num = Rand.RangeInclusive(2, 4);
            for (int i = 0; i < num; i++) {
                ThingDef fleckToSpawn = Rand.RangeInclusive(1, 3) switch
                {
                    1 => FL_GeneDefOf.BurrowDirtSmall,
                    2 => FL_GeneDefOf.BurrowDirtLarge,
                    _ => FL_GeneDefOf.BurrowDust
                };

                Vector3 loc = emergencePoint.ToVector3Shifted();
                loc.x += Rand.Range(-0.35f, 0.35f);
                loc.z += Rand.Range(-0.35f, 0.35f);
                MoteMaker.MakeStaticMote(loc, map, fleckToSpawn, Rand.Range(0.5f, 0.9f));
            }
        }
    }
}