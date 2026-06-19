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
		public static void Gameflags_Postfix(string c, int index, int value)
		{
			if (SessionManager.GameMode == GameModes.Archipelago)
			{
				InternalLogger.LogTesting($"Setting flag: {c} - {index}");
				
				// Send check when a treasureflag is set
				if (c == "TreasureFlag1" && value == 1)
				{
					SessionManager.Data.LocationsChecked.Add(Logic.FlagToLocationName[index]);
					Archipelago.instance.ActivateCheck(Logic.FlagToLocationName[index]);
				}

				// Send Event when if 
				DataStorage.Category flagtype = DataStorage.Category.kTreasureFlag4;
				if (c == "TreasureFlag1")
				{
					flagtype = DataStorage.Category.kTreasureFlag1;
				}
				else if (c == "ScenarioFlag1")
				{
					flagtype = DataStorage.Category.kScenarioFlag1;
				}

				if (Randomizer.DataStorageFlags.TryGetValue((flagtype, index), out string datastorageflag))
				{
					Archipelago.instance.UpdateDataStorage(datastorageflag, value == 1);
				}
			}

			Randomizer.ProcessCrystals(c, index);
		}
	}
}
