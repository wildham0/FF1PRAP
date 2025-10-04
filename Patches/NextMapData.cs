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
		public static void TelepoCache_Peek_Pre(TelepoCache __instance)
		{
			/*
			if (GameData.TelepoCache == null)
			{
				GameData.TelepoCache = __instance;
			}*/
			InternalLogger.LogInfo($"Telepo Cache Try Peeking?");
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

		// Unused
		private static void NextMapProperty_Pre(LoadData __instance, ref PropertyGotoMap property)
		{

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

			// Special check to pop the telepo stack when using the sub back
			if (mapId == 52 && point == 101)
			{
				InternalLogger.LogTesting($"Coming back from sub.");
				var teledata = GameData.FieldController.telepoCache.Peek();
				if (teledata != null)
				{
					InternalLogger.LogTesting($"Was on stack {teledata.MapId} - {teledata.PointInObjectId}.");
					if (teledata.MapId == mapId && teledata.PointInObjectId == point)
					{
						GameData.FieldController.telepoCache.Pop();
					}
				}
			}

			// special check to reset ordeals when we get out
			if (mapId == 93 && point == 2)
			{
				bool reached1f = false;
				InternalLogger.LogTesting($"Going back to Trials 1F.");
				while (!reached1f)
				{
					var teledata = GameData.FieldController.telepoCache.Peek();
					if (teledata.MapId == 94 || teledata.MapId == 95 || teledata.MapId == 93)
					{
						GameData.FieldController.telepoCache.Pop();
					}
					else
					{
						reached1f = true;
					}
				}
			}
		}
	}
}
