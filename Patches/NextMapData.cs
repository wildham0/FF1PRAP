using Last.Interpreter;
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
		private static void NextMapProperty_Pre(LoadData __instance, ref PropertyGotoMap property)
		{

			// alright, so Map-EntityId is the identifier, then update MapId, PointId, AssetGroup and AssetName
			// i guess we'll have to mine manually
			// so

			//InternalLogger.LogInfo($"NextMapProperty: {GameData.CurrentMap};{property.EntityId};{property.MapId};{property.PointId};{property.AssetGroupName};{property.AssetName}");


			/*
			if (property.EntityId == 52)
			{
				property.MapId = 7;
				property.AssetName = "Map_20021_3";
				//FF1PR.storedGotoMap = property;
			}/*
			else if (property.EntityId == 55)
			{
				property = FF1PR.storedGotoMap;
			}*/

		}
		private static void NextMapInt_Pre(ref int mapId, ref int point)
		{
			//InternalLogger.LogInfo($"NextMapInt: {mapId} - {point}");

			// Shuffled Citadel of Trials' Maze
			if (mapId == 94 && Randomizer.Data.OrdealsMaze.Any())
			{
				if (Randomizer.Data.OrdealsMaze.TryGetValue(point, out var newpoint))
				{
					point = newpoint;
				}
			}
		}
	}
}
