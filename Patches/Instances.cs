using Last.Entity.Field;
using Last.Interpreter;
using Last.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FF1PRAP
{
	partial class Patches
	{
		// Message Manager to inject Dialogues
		public static void MessageManagerInit_Postfix(MessageManager __instance)
		{
			FF1PR.MessageManager = __instance;
		}
		// DataStorage to manipulate flags
		public static void DataStorageInit_Postfix(DataStorage __instance)
		{
			FF1PR.DataStorage = __instance;
		}
		// GameStateTracker to read current state
		public static void GameStateTrackerInit_Postfix(GameStateTracker __instance)
		{
			//InternalLogger.LogInfo($"Stat Tracker: Do we ever init? {__instance.CurrentState}");
			FF1PR.StateTracker = __instance;
		}
		// UserDataManager, set starting transportation
		public static void UserDataManagerInit_Postfix(UserDataManager __instance)
		{
			FF1PR.UserData = __instance;
		}
		// Master Manager, to manipulate game data (items, class, attack, whatever)
		private static void MasterManagerInit_Postfix(Last.Data.Master.MasterManager __instance)
		{
			FF1PR.MasterManager = __instance;
		}
		// Map Manager, for map data?
		private static void MapManagerInit_Postfix(Last.Map.MapManager __instance)
		{
			FF1PR.MapManager = __instance;
		}
		// FieldController
		private static void FieldControllerInit_Postfix(Last.Map.FieldController __instance)
		{
			FF1PR.FieldController = __instance;
		}
		// OwnedItems
		private static void OwnedItemClientInit_Postfix(Last.Management.OwnedItemClient __instance)
		{
			if (FF1PR.OwnedItemsClient == null)
			{
				FF1PR.OwnedItemsClient = __instance;
			}
		}
		// Resource Manager
		public static void GetResourceManager_Post(ResourceManager __instance)
		{
			FF1PR.ResourceManager = __instance;
		}

	}
}
