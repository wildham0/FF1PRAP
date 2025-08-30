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
		public static void InitializeTransportation()
		{
			if (FF1PR.SessionManager.Options.TryGetValue("spawn_airship", out var spawnairship) && spawnairship == Options.Enable)
			{
				var airship = FF1PR.UserData.OwnedTransportationList.GetTransport(518);
				airship.Position = new Vector3(144, 159, 149);
				airship.MapId = 1;
				airship.Direction = 2;
				airship.SetDataStorageFlag(true);
				FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.Airship, 1);
			}

			if (FF1PR.SessionManager.Options.TryGetValue("spawn_ship", out var spawnship) && spawnship == Options.Enable)
			{
				var airship = FF1PR.UserData.OwnedTransportationList.GetTransport(517);
				airship.Position = new Vector3(145, 162, 149);
				airship.MapId = 1;
				airship.Direction = 2;
				airship.SetDataStorageFlag(true);
				FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.Ship, 1);
			}
		}
	}
}
