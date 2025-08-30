using Last.Entity.Field;
using Last.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF1PRAP
{
	partial class Patches
	{
		private static void GetRequiredStepsRange_Post(MapModel __instance, ref RequiredStepsRange __result)
		{
			float multiplier = Randomizer.Data.DungeonEncounterRate;

			if (__instance.GetMapName() == "Map_10010")
			{
				multiplier = Randomizer.Data.OverworldEncounterRate;
			}

			__result = new RequiredStepsRange((int)(__result.Min * multiplier), (int)(__result.Max * multiplier));
			//InternalLogger.LogInfo($"Step Range for {__instance.GetMapName()}: {__result.Min} - {__result.Max} ({multiplier})");
		}
	}
}
