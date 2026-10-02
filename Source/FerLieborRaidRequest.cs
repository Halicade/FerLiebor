using System.Collections.Generic;
using Verse;

namespace FerLiebor;


public class FerLieborRaidRequest : IExposable
{
    public int tickToSpawn;

    public float points;

    public int groupCount;

    public bool spawnAnywhereIfNoGoodCell;

    public List<IntVec3> emergencePoints = [];

    public FerLieborRaidRequest()
    {
    }

    public FerLieborRaidRequest(int tickToSpawn, float points, int groupCount, bool spawnAnywhereIfNoGoodCell, List<IntVec3> emergencePoints)
    {
        this.tickToSpawn = tickToSpawn;
        this.points = points;
        this.groupCount = groupCount;
        this.spawnAnywhereIfNoGoodCell = spawnAnywhereIfNoGoodCell;
        this.emergencePoints = emergencePoints ?? [];
    }

    public void ExposeData()
    {
        Scribe_Values.Look(ref tickToSpawn, "tickToSpawn", 0);
        Scribe_Values.Look(ref points, "points", 0f);
        Scribe_Values.Look(ref groupCount, "groupCount", 1);
        Scribe_Values.Look(ref spawnAnywhereIfNoGoodCell, "spawnAnywhereIfNoGoodCell", defaultValue: false);
        Scribe_Collections.Look(ref emergencePoints, "emergencePoints", LookMode.Value);
        if (Scribe.mode == LoadSaveMode.PostLoadInit && emergencePoints == null)
        {
            emergencePoints = new List<IntVec3>();
        }
    }
}
