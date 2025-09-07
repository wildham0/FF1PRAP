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
	public class SaveInfoState
	{
		public int CurrentSlot = 0;
		public int PreviousSlot = 255;
	}

	partial class Patches
	{
		public static void LoadSaveDataFromBoot_Postfix(SaveSlotManager __instance, int slotId)
		{
			InternalLogger.LogTesting($"Loading at Slot: {slotId}");
			LoadGame(slotId);
		}
		public static void LoadSaveDataFromMenu_Postfix(SaveSlotManager __instance, SaveSlotData saveData)
		{
			InternalLogger.LogTesting($"Loading at Slot: {saveData.id}");
			LoadGame(saveData.id);
		}
		public static void GetSavePath_Post(SaveSlotManager __instance)
		{
			InternalLogger.LogTesting("Saving at Slot: " + __instance.CurrentSlotId);
			SaveGame(__instance.CurrentSlotId);
		}
		public static void LoadGame(int slotid)
		{
			SessionManager.CurrentSlot = slotid;
			SessionManager.LoadSessionInfo(slotid);
			Monitor.instance.SetProcess((int)ProcessStates.LoadGame);
		}

		public static void SaveGame(int slotid)
		{
			SessionManager.CurrentSlot = slotid;
			if (SessionManager.GameMode == GameModes.Archipelago)
			{
				Archipelago.instance.GetLocationsToSend();
			}

			SessionManager.WriteSessionInfo();
			SessionManager.LoadSaveSlotInfoData();
		}
		public static void NewGame_Postfix()
		{
			Monitor.instance.SetProcess((int)ProcessStates.NewGame);
		}

		// For adding game date info in loading/saving screen
		public static void ReplaceKey_Post(ref string __result, string message, Dictionary<string, string> dictionary)
		{
			if (GameData.SaveInfoState.CurrentSlot != GameData.SaveInfoState.PreviousSlot)
			{
				if (message.Contains("File") && SessionManager.GetSlotInfo(GameData.SaveInfoState.CurrentSlot) != "")
				{
					__result = $"{SessionManager.GetSlotInfo(GameData.SaveInfoState.CurrentSlot)}   File";
					GameData.SaveInfoState.PreviousSlot = GameData.SaveInfoState.CurrentSlot;
				}
				else if (message.Contains("Autosave") && SessionManager.GetSlotInfo(GameData.SaveInfoState.CurrentSlot) != "")
				{
					__result = $"{SessionManager.GetSlotInfo(GameData.SaveInfoState.CurrentSlot)}   Autosave";
					GameData.SaveInfoState.PreviousSlot = GameData.SaveInfoState.CurrentSlot;
				}
				else if (message.Contains("Quick Save") && GameData.SaveInfoState.CurrentSlot == 22 && SessionManager.GetSlotInfo(GameData.SaveInfoState.CurrentSlot) != "")
				{
					__result = $"{SessionManager.GetSlotInfo(GameData.SaveInfoState.CurrentSlot)}   Quick Save";
					GameData.SaveInfoState.PreviousSlot = GameData.SaveInfoState.CurrentSlot;
				}
			}
		}

		public static void SetContentData_Pre(ref SaveSlotData data, int index)
		{
			//InternalLogger.LogInfo($"SaveSloData: {index} - {data.id}");
			GameData.SaveInfoState.CurrentSlot = data.id;
		}
	}
}
