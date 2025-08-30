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
		public enum EarlyProgressionModes
		{ 
			BikkesShip = 0,
			MarshPath
		}
		public static void InitializeEarlyProgression()
		{
			if (Randomizer.Data.EarlyProgression == Randomizer.EarlyProgressionModes.MarshPath)
			{
				GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.WestwardProgressionMode, 1);
				GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 6, 0); // Bridge 
			}
			else
			{
				GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.WestwardProgressionMode, 0);
				GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 6, 1); // Bridge 
			}
		}
		public static void InitializeNorthernDocks()
		{
			if (Randomizer.Data.NorthernDocks)
			{
				GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.NorthernDocks, 1);
			}
			else
			{
				GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.NorthernDocks, 0);
			}
		}
	}
}
