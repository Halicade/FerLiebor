using System.Collections.Generic;
using Verse;

namespace FerLiebor;

public class MapComponent_FerLieborRaidScheduler(Map map) : MapComponent(map)
{
    private List<FerLieborRaidRequest> _pendingRaids = [];

    private const int MoteIntervalTicks = 12;

    public override void MapComponentTick()
    {
        base.MapComponentTick();
        if (_pendingRaids == null || _pendingRaids.Count == 0)
        {
            return;
        }
        int ticksGame = Find.TickManager.TicksGame;
        for (int num = _pendingRaids.Count - 1; num >= 0; num--)
        {
            FerLieborRaidRequest ferLieborRaidRequest = _pendingRaids[num];
            if (ticksGame >= ferLieborRaidRequest.tickToSpawn)
            {
                FerLieborSpawner.SpawnFerLieborGroup(map, ferLieborRaidRequest.points, ferLieborRaidRequest.groupCount, ferLieborRaidRequest.emergencePoints);
                _pendingRaids.RemoveAt(num);
            }
            else if (ticksGame % 12 == 0)
            {
                FerLieborSpawner.EmitBurrowDirt(map, ferLieborRaidRequest.emergencePoints);
            }
        }
    }

    public List<IntVec3> ScheduleRaid(int delayTicks, float points, int groupCount, bool spawnAnywhereIfNoGoodCell)
    {
        List<IntVec3> list = FerLieborSpawner.FindEmergencePointsForRaid(map, 3);
        if (list == null || list.Count == 0)
        {
            Log.Warning("FerLiebor: Failed to find emergence points for scheduled raid.");
            return null;
        }
        _pendingRaids.Add(new FerLieborRaidRequest(Find.TickManager.TicksGame + delayTicks, points, groupCount, spawnAnywhereIfNoGoodCell, list));
        FerLieborSpawner.EmitBurrowDirt(map, list);
        return list;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Collections.Look(ref _pendingRaids, "pendingRaids", LookMode.Deep);
        if (Scribe.mode == LoadSaveMode.PostLoadInit && _pendingRaids == null)
        {
            _pendingRaids = [];
        }
    }
}
