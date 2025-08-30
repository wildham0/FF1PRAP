using Last.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF1PRAP
{
	partial class Patches
	{
		public static List<ScenarioFlags> FiendsFlags = new() { ScenarioFlags.LichDefeated, ScenarioFlags.KaryDefeated, ScenarioFlags.KrakenDefeated, ScenarioFlags.TiamatDefeated };
		public static void Gameflags_Postfix(string c, int index, int value)
		{
			if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
			{
				InternalLogger.LogInfo($"Setting flag: {c} - {index}");
				if (c == "TreasureFlag1" && value == 1)
				{
					Archipelago.instance.ActivateCheck(Randomizer.FlagToLocationName[index]);
				}
			}

			// Process Crystal Requirements
			if (c == "ScenarioFlag1" && FiendsFlags.Contains((ScenarioFlags)index))
			{
				int crystalCount = 1;

				foreach (var flag in FiendsFlags)
				{
					if (FF1PR.DataStorage.Get(DataStorage.Category.kScenarioFlag1, (int)flag) == 1)
					{
						crystalCount++;
					}
				}

				if (crystalCount >= Randomizer.RandomizerData.RequiredCrystals)
				{
					FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.BlackOrbReqCompleted, 1);
				}
			}
		}
	}
}
