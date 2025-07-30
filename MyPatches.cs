using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Last.Data;
using Last.Data.Master;
using Last.Entity.Field;
using Last.Event;
using Last.Map;
using Prime31;
using UnityEngine;
using System.Reflection;
using System.IO;
using Last.Management;
using Last.Interpreter;
using System.Diagnostics;
using Last.Interpreter.Instructions;
using Il2CppInterop.Runtime.Runtime;
using static UnityEngine.InputSystem.Users.InputUser;
using Last.Data.User;
using Last.Interpreter.Instructions.SystemCall;
using Last.Systems.Message;
using System.Text.Json;
using static Last.Map.LoadData;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using JetBrains.Annotations;
using Steamworks;

namespace FF1PRAP
{
	//[HarmonyPatch("GetExp")]
	//[HarmonyPostfix]
	class MyPatches
	{

		//static RandoDataStorage test;

		//[HarmonyPatch(typeof(BattleResultData), nameof(BattleResultData.GetExp))]
		//[HarmonyPostfix]
		public static void get_Exp(ref int __result)
		{
			__result = 65000;
		}

		public static void Weapon_Attack_get_Postfix(ref int __result)
		{
			__result = 99;
		}

		public static void TreasureCheck_Postfix(FieldTresureBox tresureBoxEntity)
		{
			var entityid = tresureBoxEntity.tresureBoxProperty.EntityId;
			var flagid = tresureBoxEntity.tresureBoxProperty.FlagId;
			var contentid = tresureBoxEntity.tresureBoxProperty.ContentId;
			var contentnum = tresureBoxEntity.tresureBoxProperty.ContentNum;
			var treasurename = tresureBoxEntity.tresureBoxProperty.Name;
			var script = tresureBoxEntity.tresureBoxProperty.ScriptId;

			var prop = tresureBoxEntity.tresureBoxProperty;

			//InternalLogger.LogInfo($"Treasure {treasurename} opened: {entityid} - {flagid}; Content: {contentid} - {contentnum}");
			InternalLogger.LogInfo($"new LocationData() {{ Content = {prop.ContentId}, Qty = {prop.ContentNum}, Id = {prop.EntityId}, Flag = {prop.FlagId}, Type = LocationType.Treasure, Name = \"\", Script = {prop.ScriptId}, Map = \"{FF1PR.CurrentMap}\", Access = new() {{ }} }},");
		}

		public static void TransportationData(ref OwnedTransportationData data)
		{


			if (data.flagNumber == 518 && !data.Enable)
			{
				data.Position = new Vector3(144, 159, 149);
				data.MapId = 1;
				data.SetDataStorageFlag(true);
			}

			//InternalLogger.LogInfo($"Transport Data - Flag : {data.flagNumber} | Position: {data.Position} | Enabled? {data.Enable} | MapId: {data.MapId}");


			//InternalLogger.LogInfo($"Faire Data : groupid {shopgroupid} | shop {shop} | bottle {bottleid} | flag:  {flag}");

			// 518 = airship (144, 159)
			// 517 = ship
			// z = 149
		}


		public static void InspectBottle(MainCore mc, int __result)
		{

			InternalLogger.LogInfo($"Fairy Data : {Last.Interpreter.Instructions.SystemCall.Current.kFairyBottleId} | {__result}");
			//InternalLogger.LogInfo($"Fairy Shop called.");
		}

		public static void InspectBottle2()
		{
			InternalLogger.LogInfo($"Caravan Shop called.");
		}


		public static void BuyItem(ShopProductData data, int count)
		{

			if (data.ContentId == 59)
			{
				// slab !
				FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 31, 1);
			}
			InternalLogger.LogInfo($"{data.ProductId} - {data.ContentId} - {count}");
		}

		public static void GetItem_Pre(ref MainCore mc)
		{

			InternalLogger.LogInfo($"Get Item called");

			InternalLogger.LogInfo($"MC: {mc.i0}, {mc.i1}");

			if (mc.i0 == 500)
			{
				mc.i0 = 52;
			}
			else if (mc.i0 == 15)
			{
				mc.i0 = 46;
			}

			InternalLogger.LogInfo($"Updated MC: {mc.i0}, {mc.i1}");
		}

		public static void AddOwnedItemInt(int contentId, int count)
		{
			InternalLogger.LogInfo($"Add Owned Int: {contentId}, {count}");
		}

		public static void AddOwnedItemContent(Content targetData, int count)
		{
			InternalLogger.LogInfo($"Add Owned Content: {targetData.Id} - {targetData.MesIdName}, {count}");
		}
		public static void TreasureUtility_Postfix(string masterLine)
		{
			InternalLogger.LogInfo($"{masterLine}");
		}

		public static void EventDict_Postfix(ref Dictionary<string, EventTag> __result)
		{
			InternalLogger.LogInfo($"---- Event read ---");

			foreach (var entry in __result)
			{
				InternalLogger.LogInfo($"{entry.Key}");
			}
		}

		public static void EventCreate_Postfix(string actionName)
		{
			InternalLogger.LogInfo($"{actionName}");
		}

		public static void Interpreter_Postfix(string json)
		{
			InternalLogger.LogInfo($"{json}");
		}

		public static void InterpreterNext(ref string __result)
		{
			InternalLogger.LogInfo($"{__result}");
		}

		public static void LoadCompleted()
		{
			InternalLogger.LogInfo($"Map data load completed");
		}

		public static void SectScriptPost(string scriptName, TextAsset textAsset, string assetGroupName, string assetName)
		{
			InternalLogger.LogInfo($"{scriptName}; {assetGroupName}; {assetName} – {textAsset.name}");
		}

		public static void CheckForScriptAndInject(string scriptName, MapAssetData __instance)
		{

			InternalLogger.LogInfo($"Running {scriptName} on {FF1PR.CurrentMap}");
			/*
			if (scriptName == "sc_e_0051")
			{
				if (!__instance.scriptList.ContainsKey("sc_e_0051"))
				{
					InternalLogger.LogInfo($"Injection attempt");

					string scriptfile = "";
					var assembly = Assembly.GetExecutingAssembly();
					string filepath = assembly.GetManifestResourceNames().Single(str => str.EndsWith("sc_e_0051.json"));
					using (Stream logicfile = assembly.GetManifestResourceStream(filepath))
					{
						using (StreamReader reader = new StreamReader(logicfile))
						{
							scriptfile = reader.ReadToEnd();
						}
					}

					InternalLogger.LogInfo($"File opened???");

					TextAsset adamantscript = new TextAsset(scriptfile);


					InternalLogger.LogInfo($"{scriptfile}");

					__instance.SetScript("sc_e_0051", adamantscript, "sc_e_0051", "sc_e_0051");

					InternalLogger.LogInfo($"Done");
				}
			}*/
		}

		public static void InjectDialogue(MessageManager __instance)
		{
			/*
			InternalLogger.LogInfo($"---Message---");
			foreach (var dialogue in __instance.messageDictionary)
			{

				InternalLogger.LogInfo($"{dialogue.Key}: {dialogue.Value}");
			}*/

			/*
			InternalLogger.LogInfo($"---Speaker---");

			foreach (var dialogue in __instance.speakerDictionary)
			{

				InternalLogger.LogInfo($"{dialogue.Key}: {dialogue.Value}");
			}*/


			__instance.SetValue("MSG_LUTE_02", "Rah rah oh lalala.<P>I'm Lady Gaga.");
			__instance.RemoveValue("MSG_LUTE_03");
			__instance.AddMessage("MSG_LUTE_03", "Come on, come on.");

			InternalLogger.LogInfo($"{__instance.GetMessage("MSG_LUTE_03")}");

			var systemMes = __instance.GetMessage("MSG_MAGIC_NAME_57");

			InternalLogger.LogInfo($"{systemMes}");

		}


		private static void ItemContainer(Item __instance, ref string masterLine)
		{
			//masterLine += "43,43,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0\n";
			//FF1PR.ItemMaster = __instance;
			InternalLogger.LogInfo($"{masterLine}");
		}
		private static string LoadScriptFile(string name)
		{

			string scriptfile = "";
			var assembly = Assembly.GetExecutingAssembly();
			string filepath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name + ".json"));
			using (Stream logicfile = assembly.GetManifestResourceStream(filepath))
			{
				using (StreamReader reader = new StreamReader(logicfile))
				{
					scriptfile = reader.ReadToEnd();
				}
			}

			return scriptfile;
		}


		public static void InjectAdamant_Script(ref MapAssetData __result)
		{
			InternalLogger.LogInfo($"Injection attempt");

			string scriptfile = "";
			var assembly = Assembly.GetExecutingAssembly();
			string filepath = assembly.GetManifestResourceNames().Single(str => str.EndsWith("sc_e_0051.json"));
			using (Stream logicfile = assembly.GetManifestResourceStream(filepath))
			{
				using (StreamReader reader = new StreamReader(logicfile))
				{
					scriptfile = reader.ReadToEnd();
				}
			}

			InternalLogger.LogInfo($"File opened???");

			TextAsset adamantscript = new TextAsset("scriptfile");

			__result.SetScript("sc_e_0051", adamantscript, "sc_e_0051", "sc_e_0051");

			InternalLogger.LogInfo($"Done");
		}

		public static void EventRunScript(string actionName)
		{
			InternalLogger.LogInfo($"Action: {actionName}");
		}

		public static void OpenSavegame(string fileName)
		{
			InternalLogger.LogInfo($"Savegame: {fileName}");
		}
		public static void AnalyseCallBack(UserDataManager __instance)
		{

			FF1PR.UserData = __instance;
			InternalLogger.LogInfo($"Init: {__instance.CurrentLocation}");


			//			GameStateTracker.Fi
		}

		public static void StartNewGame(Serial.FF1.UI.KeyInput.NewGameWindowController __instance)
		{
			InternalLogger.LogInfo($"New game started: {__instance.stateMachine.current.ToString()}");
		}

		public static void GetLatestSaveSlotData(SaveSlotManager __instance, int slotId)
		{

			FF1PR.SaveManager = __instance;
			FF1PR.CurrentSlot = slotId;
			InternalLogger.LogInfo($"Slot Loaded: {slotId}");

			// Load randomization data here
			//			GameStateTracker.Fi
		}

		public static void GetLatestSaveSlotData2(SaveSlotManager __instance, SaveSlotData saveData)
		{

			FF1PR.SaveManager = __instance;
			FF1PR.CurrentSlot = saveData.id;
			InternalLogger.LogInfo($"Slot Loaded 2: {__instance.CurrentSlotId} / {saveData.id}");

			// Load randomization data here
			//			GameStateTracker.Fi
		}

		public static void SearchHack(ref Dictionary<string, Il2CppSystem.Object> list)
		{

			foreach (var item in list)
			{
				InternalLogger.LogInfo($"MasterBase: {item.Key}");
			}



			// Load randomization data here
			//			GameStateTracker.Fi
		}

		public static void GetMessageBox(MessageParser.SortMatchData data, BaseContent __result)
		{
			InternalLogger.LogInfo($"{__result.ContentText} - {__result.Type}");
		}

		public static void OnExit(Core c, int __result)
		{

			InternalLogger.LogInfo($"Core Exit: {c.coreID}: {__result}");
		}

		public static void GetNextMnemonic(string __result)
		{
			InternalLogger.LogInfo($"Core GetNext: {__result}");
		}


		public static void LoadEventEntity(bool __result, ref MapAssetData __instance)
		{

			InternalLogger.LogInfo($"Is Completed {__result}");
			if (!__result)
			{
				return;
			}

			var entitylist = __instance.GetEventEntityList();
			//InternalLogger.LogInfo($"Load Entity: {name}");

			if (entitylist.TryGetValue("ev_e_0003", out var entityInfo))
			{
				InternalLogger.LogInfo($"Entity found");
				var garland = entityInfo.ObjectMap.layers[0].objects.ToList().Find(o => o.id == 173);
				garland.GetPropertyByName("turn_around").value = "true";
				InternalLogger.LogInfo($"id: {garland.id}, direction: {garland.GetPropertyByName("direction").value}");
				garland = entityInfo.ObjectMap.layers[0].objects.ToList().Find(o => o.id == 173);
				//garland.GetPropertyByName("direction").value = "4";
				InternalLogger.LogInfo($"id: {garland.id}, direction: {garland.GetPropertyByName("direction").value}");
			}
		}

		public static void EventRunScript2(Integrator __instance)
		{
			//FF1PR.RandoData.Counter++;
			//InternalLogger.LogInfo($"Entity Update : {Plugin.RandoData.Counter} - Running? {__instance.working}");

			if (FF1PR.StateTracker is null)
			{
				FF1PR.StateTracker = GameObject.Find("GameStateTracker").GetComponent<GameStateTracker>();
			}
			/*
			var dataManager = FF1PR.UserData;
			
			//UserDataManager dataManager = GameObject.Fi);

			if (FF1PR.RandoData.Counter == 60)
			{
				FF1PR.RandoData.Counter = 0;
				
				//InternalLogger.LogInfo($"State: {FF1PR.StateTracker.CurrentState} /  {FF1PR.StateTracker.CurrentSubState}");

				if (dataManager is not null)
				{
					//InternalLogger.LogInfo($"State: {dataManager.CurrentAreaId} - {dataManager.CurrentArea} / {dataManager.CurrentMapId}");
				}

				if (FF1PR.CurrentSave is not null)
				{
					//InternalLogger.LogInfo($"Save filename: {FF1PR.SaveManager.GetSavePath()} / {FF1PR.SaveManager.SystemFileName}");
					InternalLogger.LogInfo($"Save Slot: {FF1PR.CurrentSave.id} / Lodaing? {FF1PR.CurrentSave.IsLoad}");
				}

			}*/

			/*
			if (FF1PR.RandoData.Counter == 1500 && FF1PR.RandoData.CurrentItem < 61)
			{


				FF1PR.RandoData.Counter = 1000;

				var tresureDummy = new FieldTresureBox();
				var treasureProp = new PropertyTresureBox(new MapObjectProperty(0));
				treasureProp.ContentId = 10;
				treasureProp.ContentNum = 1;

				tresureDummy.tresureBoxProperty = treasureProp;
			*/
			//Last.Map.EventActionTreasure.CreateTask(tresureDummy, false, true);

			InternalLogger.LogInfo($"Script load");
			//var integrator = new Integrator();
			//var callback = new ICallbacks()
			//integrator.Execute(LoadScriptFile("scripttest"), null);

			ScriptBuilder newitemscript = new();

			/* this is interesting, need some work tho
			var test = new Message.MessageWindowParams(1, 2);
			Message.PlayMessageCommon("MSG_WND_DAN_04", test, true);
			*/
			/*
			var itemNameKey = FF1PR.MasterManager.GetList<Content>()[FF1PR.RandoData.CurrentItem].MesIdName;
			var itemName = FF1PR.MessageManager.GetMessage(itemNameKey);

			//FF1PR.MessageManager.GetMessage("");
			FF1PR.MessageManager.RemoveValue("MSG_CUSTM_01");
			FF1PR.MessageManager.AddMessage("MSG_CUSTM_01", $"You obtained {itemName}? Hope you're happy with it!");
			FF1PR.MessageManager.GetMessageDictionary()["MSG_CUSTM_01"] = $"You obtained {itemName}? Hope you're happy with it!";

			newitemscript.AddSegment(new List<string>()
			{
				"[Main]",
				"Nop",
				$"GetItem {FF1PR.RandoData.CurrentItem} 1",
				"Msg MSG_CUSTM_01",
				"Exit",
				"Nop",
				"Exit"
			});

			var result = newitemscript.Output("funscript");

			InternalLogger.LogInfo(result);
			//var scriptsandbox = new ScriptSandbox(LoadScriptFile("scripttest"));
			__instance.Execute(newitemscript.Output("funscript"), __instance.callbackHandler);
			//InternalLogger.LogInfo($"Script Update?");
			//scriptsandbox.Update();


			FF1PR.RandoData.CurrentItem++;

		}*/


		}

		public static void PlayerUpdate_Post(ref Player __instance)
		{

			InternalLogger.LogInfo($"Dash from Provider: {__instance.moveSpeed}");
		}
		public static void PlayFlag_Post(ref float __result)
		{

			InternalLogger.LogInfo($"Dash from Flags: {__result}");
			//Last.Entity.Field.FieldPlayer
			//__result *= 2;
		}


	}

	public class RandoDataStorage
	{
		public int Counter { get; set; } = 0;
		public int CurrentItem { get; set; } = 44;
		public bool InitializationDone = false;

		public void Initialize()
		{
			Counter = 0;
		}

		public void Increment()
		{
			Counter++;
		}
	}
}
