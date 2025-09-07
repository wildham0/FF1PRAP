using Last.Data.Master;
using Last.Interpreter;
using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Bindings;

namespace FF1PRAP
{
	partial class Randomizer
    {
		public static List<ScenarioFlags> FiendsFlags = new() { ScenarioFlags.LichDefeated, ScenarioFlags.KaryDefeated, ScenarioFlags.KrakenDefeated, ScenarioFlags.TiamatDefeated };
		public static void ProcessCrystals(string c, int index)
		{
			if (c == "ScenarioFlag1" && FiendsFlags.Contains((ScenarioFlags)index))
			{
				int crystalCount = 1;

				foreach (var flag in FiendsFlags)
				{
					if (GameData.DataStorage.Get(DataStorage.Category.kScenarioFlag1, (int)flag) == 1)
					{
						crystalCount++;
					}
				}

				if (crystalCount >= Randomizer.Data.RequiredCrystals)
				{
					GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.BlackOrbReqCompleted, 1);
				}
			}
		}
	}
}
