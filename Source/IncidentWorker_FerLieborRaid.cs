using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace FerLiebor;


public class IncidentWorker_FerLieborRaid : IncidentWorker
{
	public static readonly SimpleCurve PointsFactorCurve = new SimpleCurve
	{
		new CurvePoint(0f, 0.7f),
		new CurvePoint(5000f, 0.45f)
	};

	private const int DefaultDelayTicks = 3200;

	protected override bool CanFireNowSub(IncidentParms parms)
	{
		if (!base.CanFireNowSub(parms))
		{
			return false;
		}
		if (!(parms.target is Map map))
		{
			return false;
		}
		
		Faction faction = Find.FactionManager.FirstFactionOfDef(FL_GeneDefOf.FeralWarren);
		if (faction == null)
		{
			return false;
		}
		if (!faction.HostileTo(Faction.OfPlayer))
		{
			return false;
		}
		IntVec3 cell;
		return FerLieborCellFinder.TryFindCell(out cell, map);
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		if (parms.target is not Map map)
		{
			Log.Error("IncidentWorker_FerLieborRaid: target was not a Map.");
			return false;
		}
		if (parms.points <= 0f)
		{
			parms.points = StorytellerUtility.DefaultThreatPointsNow(map);
		}
		parms.points *= PointsFactorCurve.Evaluate(parms.points);
		int groupCount = Mathf.Max(1, GenMath.RoundRandom(parms.points / 120f));
		MapComponent_FerLieborRaidScheduler component = map.GetComponent<MapComponent_FerLieborRaidScheduler>();
		if (component == null)
		{
			Log.Error("FerLiebor: Could not find MapComponent_FerLieborRaidScheduler.");
			return false;
		}
		List<IntVec3> list = component.ScheduleRaid(3200, parms.points, groupCount, spawnAnywhereIfNoGoodCell: true);
		if (list == null || list.Count == 0)
		{
			return false;
		}
		if (parms.sendLetter && !parms.silent)
		{
			List<TargetInfo> list2 = list.Select((IntVec3 cell) => new TargetInfo(cell, map)).ToList();
			SendStandardLetter(def.letterLabel, def.letterText, def.letterDef, parms, list2);
		}
		Find.TickManager.slower.SignalForceNormalSpeedShort();
		return true;
	}
}
