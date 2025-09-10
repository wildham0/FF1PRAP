using Last.Interpreter;
using Last.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF1PRAP
{
	partial class Patches
	{
		// Various TelepoCache function to monitor how we're moving around
		public static void TelepoCache_Peek(TelepoCache __instance, TelepoCacheItem __result)
		{
			/*
			if (GameData.TelepoCache == null)
			{
				GameData.TelepoCache = __instance;
			}*/
			//InternalLogger.LogInfo($"Telepo Cache Peeking ({__result.MapId}, {__result.PointInObjectId})");
		}
		public static void TelepoCache_Get()
		{
			InternalLogger.LogTesting($"Telepo Cache Get.");
			Randomizer.Teleporting = true;
		}
		public static void TelepoCache_Pop(TelepoCacheItem __result)
		{
			InternalLogger.LogTesting($"Telepo Cache Pop: Popping ({__result.MapId}, {__result.PointInObjectId})");
			Randomizer.Teleporting = true;
		}
		public static void OnCacheTelePoint_Post(ref PropertyGotoMap propertyGotoMap)
		{
			InternalLogger.LogTesting($"OnCacheTelePoint: Don't Teleport");
			Randomizer.Teleporting = false;
		}
		public static void OnCacheTelePoint_Pre(ref PropertyGotoMap propertyGotoMap)
		{
			InternalLogger.LogTesting($"OnCacheTelePoint: {GameData.CurrentMap};{propertyGotoMap.EntityId};{propertyGotoMap.MapId};{propertyGotoMap.PointId};{propertyGotoMap.AssetGroupName};{propertyGotoMap.AssetName}");

			bool wasOverworld = (propertyGotoMap.MapId == 1);

			if (Randomizer.PointToTeleporters.TryGetValue((propertyGotoMap.MapId, propertyGotoMap.PointId), out var currentteleporter))
			{
				if (Randomizer.Data.Entrances.TryGetValue(currentteleporter.Name, out var entrance))
				{
					var newpoint = Randomizer.NameToTeleporters[entrance];

					InternalLogger.LogTesting($"OnCacheTelePoint: Found {currentteleporter.Name}, replacing by {newpoint.Name}");
					//InternalLogger.LogInfo($"NextMapProperty: Found {(property.MapId, property.PointId)}, replacing by {(newpoint.MapId, newpoint.PointId)}");

					propertyGotoMap.MapId = newpoint.MapId;
					propertyGotoMap.PointId = newpoint.PointId;
					propertyGotoMap.AssetGroupName = newpoint.MapGroup;
					propertyGotoMap.AssetName = newpoint.MapName;

					// A bit hacky, but this ensure we don't desync the tele cache when the old entrance was aiming at the Overworld.
					if (wasOverworld && propertyGotoMap.MapId != 1)
					{
						var teledata = GameData.FieldController.telepoCache.Peek();
						if (teledata != null)
						{
							if (teledata.MapId == propertyGotoMap.MapId && teledata.PointInObjectId == propertyGotoMap.PointId)
							{
								GameData.FieldController.telepoCache.Pop();
							}
						}
					}
				}
			}
		}


		private static void NextMapProperty_Pre(LoadData __instance, ref PropertyGotoMap property)
		{

			//InternalLogger.LogInfo($"NextMap Prop: {property.MapId}")

			// alright, so Map-EntityId is the identifier, then update MapId, PointId, AssetGroup and AssetName
			// i guess we'll have to mine manually
			// so

			if (property.MapId == 255)
			{
				property.MapId = 1;
			}

			InternalLogger.LogTesting($"NextMapProperty: {GameData.CurrentMap};{property.EntityId};{property.MapId};{property.PointId};{property.AssetGroupName};{property.AssetName}");

			if (Randomizer.PointToTeleporters.TryGetValue((property.MapId, property.PointId), out var currentteleporter))
			{
				if (Randomizer.Data.Entrances.TryGetValue(currentteleporter.Name, out var entrance))
				{
					var newpoint = Randomizer.NameToTeleporters[entrance];
					
					InternalLogger.LogTesting($"NextMapProperty: Found {currentteleporter.Name}, replacing by {newpoint.Name}");

					property.MapId = newpoint.MapId;
					property.PointId = newpoint.PointId;
					property.AssetGroupName = newpoint.MapGroup;
					property.AssetName = newpoint.MapName;
				}
			}
			/*
			if (Randomizer.Data.Teleporters.TryGetValue((property.MapId, property.PointId), out var newteleporter))
			{
				InternalLogger.LogInfo($"NextMapProperty: Found {(property.MapId, property.PointId)}, replacing by {(newteleporter.MapId, newteleporter.PointId)}");

				property.MapId = newteleporter.MapId;
				property.PointId = newteleporter.PointId;
				property.AssetGroupName = newteleporter.MapGroup;
				property.AssetName = newteleporter.MapName;
			}*/


			/*
			if (property.MapId == 4 && property.PointId == 1)
			{
				property.MapId = 32;
				property.PointId = 1;
				property.AssetName = "Map_20071_1";
				//property.AssetGroupName = "map_20071";
			
			}*/


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
			InternalLogger.LogTesting($"NextMapInt: {mapId} - {point}");

			if (Randomizer.Teleporting)
			{
				InternalLogger.LogTesting($"Player is teleporting, cancelling.");
				Randomizer.Teleporting = false;
			}
			else if (Randomizer.PointToTeleporters.TryGetValue((mapId, point), out var currentteleporter))
			{
				if (Randomizer.Data.Entrances.TryGetValue(currentteleporter.Name, out var entrance))
				{
					var newpoint = Randomizer.NameToTeleporters[entrance];

					InternalLogger.LogTesting($"NextMapProperty: Found {currentteleporter.Name}, replacing by {newpoint.Name}");

					mapId = newpoint.MapId;
					point = newpoint.PointId;
				}
			}

			// Allow Teleporter to destroy cache when reaching the Overworld
			if (mapId == 1)
			{
				GameData.FieldController.telepoCache.RemoveAll();
			}

			/*
			if (Randomizer.Data.Teleporters.TryGetValue((mapId, point), out var newteleporter))
			{
				InternalLogger.LogInfo($"NextMapProperty: Found {(mapId, point)}, replacing by {(newteleporter.MapId, newteleporter.PointId)}");

				mapId = newteleporter.MapId;
				point = newteleporter.PointId;
			}*/

			// Shuffled Citadel of Trials' Maze
			/*
			if (mapId == 94 && Randomizer.Data.OrdealsMaze.Any())
			{
				if (Randomizer.Data.OrdealsMaze.TryGetValue(point, out var newpoint))
				{
					point = newpoint;
				}
			}*/
		}
	}
}
