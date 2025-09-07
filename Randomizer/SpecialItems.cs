using Il2CppSystem.Linq;
using Last.Data.Master;
using Last.Interpreter;
using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Bindings;

namespace FF1PRAP
{
	partial class Randomizer
    {
		public static void ProcessSpecialItems(int itemid)
		{
			Randomizer.ProcessJobItem(itemid);

			if (itemid == (int)Items.Ship)
			{
				var ship = GameData.UserData.OwnedTransportationList.GetTransport(517);

				if (ship == null)
				{
					return;
				}

				// Coneria dock is 145, 162
				// Pravoka dock is 203, 146

				(int x, int y) shipSpawn = (203, 146);

				// Check if we spawn at Coneria
				if (GameData.DataStorage.Get(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.WestwardProgressionMode) == 1 || (SessionManager.Options.TryGetValue("spawn_ship", out var spawnship) && spawnship == Options.Enable))
				{
					shipSpawn = (145, 162);
				}

				ship.Position = new UnityEngine.Vector3(shipSpawn.x, shipSpawn.y, 149);
				ship.MapId = 1;
				ship.Direction = 2;
				ship.SetDataStorageFlag(true);
			}
			else if (itemid == (int)Items.Canoe)
			{
				var canoe = GameData.UserData.OwnedTransportationList.GetTransport(516);

				if (canoe == null)
				{
					return;
				}

				canoe.Position = new UnityEngine.Vector3(1000, 1000, 0);
				canoe.MapId = 1;
				canoe.Direction = 2;
				canoe.SetDataStorageFlag(true);
			}
			else if (itemid == (int)Items.Lute)
			{
				if (Randomizer.Data.RequiredTablatures == 0)
				{
					GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.LuteAccessCompleted, 1);
				}
			}
			else if (itemid == (int)Items.LuteTablature)
			{
				if (GameData.UserData.ImportantOwendItemList.ToArray().TryFind(i => i.ContentId == (int)Items.LuteTablature, out var result))
				{
					if (result.Count >= Randomizer.Data.RequiredTablatures)
					{
						GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.LuteAccessCompleted, 1);
					}
					InternalLogger.LogInfo($"Tablature Count: {result.Count}");
				}
			}
		}
	}
}
