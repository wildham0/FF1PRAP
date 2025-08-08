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

		// For adding game date info in loading/saving screen
		public static void ReplaceKey_Post(ref string __result, string message, Dictionary<string, string> dictionary)
		{
			if (FF1PR.SaveInfoState.CurrentSlot != FF1PR.SaveInfoState.PreviousSlot)
			{
				if (message.Contains("File") && FF1PR.SessionManager.GetSlotInfo(FF1PR.SaveInfoState.CurrentSlot) != "")
				{
					__result = $"{FF1PR.SessionManager.GetSlotInfo(FF1PR.SaveInfoState.CurrentSlot)}   File";
					FF1PR.SaveInfoState.PreviousSlot = FF1PR.SaveInfoState.CurrentSlot;
				}
				else if (message.Contains("Autosave") && FF1PR.SessionManager.GetSlotInfo(FF1PR.SaveInfoState.CurrentSlot) != "")
				{
					__result = $"{FF1PR.SessionManager.GetSlotInfo(FF1PR.SaveInfoState.CurrentSlot)}   Autosave";
					FF1PR.SaveInfoState.PreviousSlot = FF1PR.SaveInfoState.CurrentSlot;
				}
				else if (message.Contains("Quick Save") && FF1PR.SaveInfoState.CurrentSlot == 22 && FF1PR.SessionManager.GetSlotInfo(FF1PR.SaveInfoState.CurrentSlot) != "")
				{
					__result = $"{FF1PR.SessionManager.GetSlotInfo(FF1PR.SaveInfoState.CurrentSlot)}   Quick Save";
					FF1PR.SaveInfoState.PreviousSlot = FF1PR.SaveInfoState.CurrentSlot;
				}
			}
		}

		public static void SetContentData_Pre(ref SaveSlotData data, int index)
		{
			//InternalLogger.LogInfo($"SaveSloData: {index} - {data.id}");
			FF1PR.SaveInfoState.CurrentSlot = data.id;
		}
	}

	public class SaveInfoState
	{
		public int CurrentSlot = 0;
		public int PreviousSlot = 255;
	}
}
