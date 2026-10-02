using System.Collections.Generic;
using RimWorld;
using Verse;

namespace FerLiebor;

public static class FerLieborCellFinder
{
    public static bool TryFindCell(out IntVec3 cell, Map map) {
        if (TryFindCellInHomeArea(out cell, map)) {
            return true;
        }
        if (TryFindEnclosedIndoorCell(out cell, map)) {
            return true;
        }
        if (TryFindRoofedCell(out cell, map)) {
            return true;
        }
        if (TryFindAnyValidCell(out cell, map)) {
            return true;
        }
        cell = IntVec3.Invalid;
        return false;
    }

    private static bool TryFindCellInHomeArea(out IntVec3 cell, Map map) {
        cell = IntVec3.Invalid;
        if (map == null || map.areaManager == null) {
            return false;
        }
        Area_Home home = map.areaManager.Home;
        if (home == null) {
            return false;
        }
        List<IntVec3> list = new List<IntVec3>();
        for (int i = 0; i < map.Size.z; i++) {
            for (int j = 0; j < map.Size.x; j++) {
                IntVec3 intVec = new IntVec3(j, 0, i);
                try {
                    if (home[intVec] && IsValidEmergenceCell(intVec, map)) {
                        list.Add(intVec);
                    }
                }
                catch { }
            }
        }
        if (list.Count > 0) {
            cell = list.RandomElement();
            return true;
        }
        return false;
    }

    private static bool TryFindEnclosedIndoorCell(out IntVec3 cell, Map map) {
        cell = IntVec3.Invalid;
        List<IntVec3> list = new List<IntVec3>();
        for (int i = 0; i < map.Size.z; i++) {
            for (int j = 0; j < map.Size.x; j++) {
                IntVec3 intVec = new IntVec3(j, 0, i);
                if (intVec.InBounds(map) && intVec.Roofed(map) && IsValidEmergenceCell(intVec, map)) {
                    Room room = intVec.GetRoom(map);
                    if (room != null && !room.TouchesMapEdge) {
                        list.Add(intVec);
                    }
                }
            }
        }
        if (list.Count > 0) {
            cell = list.RandomElement();
            return true;
        }
        return false;
    }

    private static bool TryFindRoofedCell(out IntVec3 cell, Map map) {
        cell = IntVec3.Invalid;
        List<IntVec3> list = new List<IntVec3>();
        for (int i = 0; i < map.Size.z; i++) {
            for (int j = 0; j < map.Size.x; j++) {
                IntVec3 intVec = new IntVec3(j, 0, i);
                if (intVec.Roofed(map) && IsValidEmergenceCell(intVec, map)) {
                    list.Add(intVec);
                }
            }
        }
        if (list.Count > 0) {
            cell = list.RandomElement();
            return true;
        }
        return false;
    }

    private static bool TryFindAnyValidCell(out IntVec3 cell, Map map) {
        cell = IntVec3.Invalid;
        List<IntVec3> list = new List<IntVec3>();
        for (int i = 0; i < map.Size.z; i++) {
            for (int j = 0; j < map.Size.x; j++) {
                IntVec3 intVec = new IntVec3(j, 0, i);
                if (IsValidEmergenceCell(intVec, map)) {
                    list.Add(intVec);
                }
            }
        }
        if (list.Count > 0) {
            cell = list.RandomElement();
            return true;
        }
        return false;
    }

    private static bool IsValidEmergenceCell(IntVec3 c, Map map) {
        if (!c.InBounds(map)) {
            return false;
        }
        if (!c.Walkable(map)) {
            return false;
        }
        if (c.Fogged(map)) {
            return false;
        }
        if (c.GetFirstPawn(map) != null) {
            return false;
        }
        if (c.GetFirstThing(map, ThingDefOf.Hive) != null) {
            return false;
        }
        if (c.GetFirstThing(map, ThingDef.Named("TunnelHiveSpawner")) != null) {
            return false;
        }
        List<Thing> thingList = c.GetThingList(map);
        for (int i = 0; i < thingList.Count; i++) {
            Thing thing = thingList[i];
            if (thing is Pawn) {
                return false;
            }
            if (thing.def.category == ThingCategory.Building && thing.def.passability == Traversability.Impassable) {
                return false;
            }
        }
        float temperature = c.GetTemperature(map);
        if (temperature < -40f) {
            return false;
        }
        return true;
    }
}