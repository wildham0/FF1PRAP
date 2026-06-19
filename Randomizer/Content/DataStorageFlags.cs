using Last.Data.Master;
using Last.Interpreter;
using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Bindings;
using UnityEngine;

namespace FF1PRAP
{
	partial class Randomizer
    {
		public static Dictionary<(DataStorage.Category type, int flag), string> DataStorageFlags = new()
		{
			{ (DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.PrincessSaved), "Garland Defeated" },
			{ (DataStorage.Category.kTreasureFlag1, (int)TreasureFlags.Bikke), "Bikke Defeated" },
			{ (DataStorage.Category.kTreasureFlag1, (int)TreasureFlags.Astos), "Astos Defeated" },
			{ (DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.VampireDefeated), "Vampire Defeated" },
			{ (DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.LichDefeated), "Lich Defeated" }, // Also Earth Crystal 
			{ (DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.KaryDefeated), "Kary Defeated" }, // Also Fire Crystal
			{ (DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.KrakenDefeated), "Kraken Defeated" }, // Also Water Crystal
			{ (DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.TiamatDefeated), "Tiamat Defeated" }, // Also Wind Crystal

			{ (DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.Bridge), "Bridge Built" },
			{ (DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.Canal), "Canal Opened" },
			{ (DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.TitanFed), "Titan Fed" },
			{ (DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.Airship), "Airship Acquired" },
			{ (DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.FaerieReleased), "Faerie Released" },
			{ (DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.StoneTranslated), "Rosetta Stone Translated" },
		};
	}
}
