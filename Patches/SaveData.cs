using Last.Data.Master;
using Last.Data.User;
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
		public static void LoadSaveDataFromBoot_Postfix(SaveSlotManager __instance, int slotId)
		{
			InternalLogger.LogInfo($"Loading at Slot: {slotId}");
			LoadGame(slotId);

			//InitializeRando();
		}
		public static void LoadSaveDataFromMenu_Postfix(SaveSlotManager __instance, SaveSlotData saveData)
		{
			InternalLogger.LogInfo($"Loading at Slot: {saveData.id}");
			LoadGame(saveData.id);

			//InitializeRando();
		}
		public static void GetSavePath_Post(SaveSlotManager __instance)
		{
			InternalLogger.LogInfo("Saving at Slot: " + __instance.CurrentSlotId);
			SaveGame(__instance.CurrentSlotId);

			//__instance.SaveSlotDataList[__instance.CurrentSlotId].timeStamp = "weeeeee";
		}
		public static void LoadGame(int slotid)
		{
			FF1PR.SessionManager.CurrentSlot = slotid;
			FF1PR.SessionManager.LoadSessionInfo(slotid);
			Monitor.instance.SetProcess((int)ProcessStates.LoadGame);
		}

		public static void SaveGame(int slotid)
		{
			FF1PR.SessionManager.CurrentSlot = slotid;
			if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
			{
				Archipelago.instance.GetLocationsToSend();
			}

			FF1PR.SessionManager.WriteSessionInfo();
			FF1PR.SessionManager.LoadSaveSlotInfoData();
		}

		public static void NewGame_Postfix()
		{
			Monitor.instance.SetProcess((int)ProcessStates.NewGame);
		}
	}
}
