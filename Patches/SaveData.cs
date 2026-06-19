using Last.Data.Master;
using Last.Data.User;
using Last.Entity.Field;
using Last.Interpreter;
using Last.Management;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

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
		public static void GetSavePathAction_Post(SaveSlotManager __instance, int slotId, Action<bool> callback, bool isClear)
		{
			//InternalLogger.LogTesting("Saving Action at Slot: " + slotId + " - " + isClear);

			InternalLogger.LogTesting($"Saving at Slot: {slotId}/{__instance.CurrentSlotId}");
			var currentslot = __instance.GetLatestSaveSlotData(true, true);
			//InternalLogger.LogTesting($"Checking: {currentslot.id}/{currentslot.mapData}");
			
			if (Randomizer.Data.Entrances != null &&
				Randomizer.Data.Entrances.TryGetValue(EntranceNames.overworld_chaos_shrine, out var entrance) &&
				isClear)
			{
				var newpoint = Randomizer.NameToTeleporters[entrance];
				var splitdata = currentslot.mapData.Split(",\"transportationId\"");
				var splitvalues = splitdata[0].Split(":");
				string testdata = splitvalues[0] + ":" + splitvalues[1] + ":" + newpoint.PointId + ",\"transportationId\"" + splitdata[1];
				currentslot.mapData = testdata;

				__instance.Save(slotId, currentslot);
			}

			SaveGame(__instance.CurrentSlotId);
		}
		public static void GetSavePathSlot_Post(SaveSlotManager __instance, int slotId)
		{
			InternalLogger.LogTesting("Saving Slot at Slot: " + slotId);
			SaveGame(__instance.CurrentSlotId);
		}
		public static void GetSavePath_Post(SaveSlotManager __instance)
		{
			SaveGame(__instance.CurrentSlotId);
		}

		public static void GetSavePath_Pre(SaveSlotManager __instance, int slotId, Action<bool> callback, bool isClear)
		{
			InternalLogger.LogTesting($"Saving at Slot Pre: {slotId}/{__instance.CurrentSlotId}");
			var currentslot = __instance.GetLatestSaveSlotData(true, true);
			InternalLogger.LogTesting($"Checking: {currentslot.id}/{currentslot.mapData}");


			var splitdata = currentslot.mapData.Split(",\"transportationId\"");
			InternalLogger.LogTesting($"Checking: {splitdata[0]}");
			var splitvalues = splitdata[0].Split(":");
			InternalLogger.LogTesting($"Checking: {splitvalues[0]}");

			currentslot.mapData = splitvalues[0] + ":" + splitvalues[1] + ":" + "5" + ",\"transportationId\"" + splitdata[1];
			InternalLogger.LogTesting($"Checking: {currentslot.id}/{currentslot.mapData}");



			SaveGame(__instance.CurrentSlotId);

			//var currentslot = __instance.GetLatestSaveSlotData(true, true);
			//InternalLogger.LogTesting("Slot data location: " + currentslot.mapData + "; " + currentslot.CurrentArea + "; " + currentslot.userData);
			/*
			var splitdata = currentslot.mapData.Split(",\"transportationId\"");
			var splitvalues = splitdata[0].Split(":");
			//ar pointvalue = split

			currentslot.mapData = splitvalues[0] + ":" + "5" + ",\"transportationId\"" + splitdata[1];*/
		}
		public static void GetSavePath_Post_2(SaveSlotManager __instance, int slotId, SaveSlotData slotData)
		{
			//InternalLogger.LogTesting("Saving at Slot: " + slotId + " - " + slotData.userData);
			InternalLogger.LogTesting($"Saving at Slot Post 2: {slotId}/{__instance.CurrentSlotId}");
			var currentslot = __instance.GetLatestSaveSlotData(true, true);
			InternalLogger.LogTesting($"Checking: {currentslot.id}/{currentslot.mapData}");


			SaveGame(__instance.CurrentSlotId);

			//var currentslot = __instance.GetLatestSaveSlotData(true, true);
			//InternalLogger.LogTesting("Slot data location: " + currentslot.mapData + "; " + currentslot.CurrentArea + "; " + currentslot.userData);
			/*
			var splitdata = currentslot.mapData.Split(",\"transportationId\"");
			var splitvalues = splitdata[0].Split(":");
			//ar pointvalue = split

			currentslot.mapData = splitvalues[0] + ":" + "5" + ",\"transportationId\"" + splitdata[1];*/
		}

		public static void SaveSlotPre(ref SaveSlotManager __instance)
		{
			InternalLogger.LogTesting("Saving at Slot: " + __instance.CurrentSlotId + " - " + GameData.CurrentMap);
			//SaveGame(__instance.CurrentSlotId);

			var currentslot = __instance.GetLatestSaveSlotData(true, true);
			//InternalLogger.LogTesting("Slot data location: " + currentslot.mapData + "; " + currentslot.CurrentArea + "; " + currentslot.userData);
			/*
			var splitdata = currentslot.mapData.Split(",\"transportationId\"");
			var splitvalues = splitdata[0].Split(":");
			//ar pointvalue = split

			currentslot.mapData = splitvalues[0] + ":" + splitvalues[1] + ":" + "5" + ",\"transportationId\"" + splitdata[1];*/
		}
		public static void LoadGame(int slotid)
		{
			SessionManager.CurrentSlot = slotid;
			Monitor.instance.SetProcess((int)ProcessStates.LoadGame);
		}

		public static void SaveGame(int slotid)
		{
			SessionManager.CurrentSlot = slotid;

			SessionManager.WriteSessionInfo();
			SessionManager.UpdateSlotInfo();
		}
		// Read save data and get Session Info from it, then create info string for the save menu.
		public static void CreateSlotListData(SaveSlotData __instance)
		{
			string json = __instance.userData;

			if (json.Contains("session_info"))
			{
				JsonNode originalJson = JsonNode.Parse(json);
				JsonObject sessionData = originalJson.AsObject()["session_info"].AsObject();

				SessionManager.SetSlotInfo(sessionData.ToJsonString(), __instance.id);
				InternalLogger.LogTesting($"SaveData Slot Info found for: {__instance.id}");
			}
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
		// Inject Session Info into save file json
		public static void UserDataManager_ToJSON_Post(bool isClear, ref string __result)
		{
			if (SessionManager.GameMode != GameModes.Vanilla)
			{
				JsonNode originalJson = JsonNode.Parse(__result);

				var options = new JsonSerializerOptions
				{
					WriteIndented = false,
					Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
				};

				originalJson.AsObject().Add("session_info", SessionManager.GetSessionInfo());

				__result = originalJson.ToJsonString(options);
				//InternalLogger.LogInfo($"Session Info: {SessionManager.GetSessionInfo()}");
			}
		}
		// Extract Session Info from save file json
		public static void UserDataManager_FromJsonAsync_Pre(ref string json, bool isClear, bool isExtraLibrary)
		{
			if (json.Contains("session_info"))
			{
				JsonNode originalJson = JsonNode.Parse(json);
				JsonObject sessionData = originalJson.AsObject()["session_info"].AsObject();
				originalJson.AsObject().Remove("session_info");

				var options = new JsonSerializerOptions
				{
					WriteIndented = false,
					Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
				};

				json = originalJson.ToJsonString(options);

				SessionManager.SetSessionInfo(sessionData.ToJsonString());
			}
		}
	}
}
