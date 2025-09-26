using AsmResolver.Collections;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
//using Il2CppSystem;
using Il2CppSystem.Threading.Tasks;
using JetBrains.Annotations;
using Last.Data;
using Last.Data.Master;
using Last.Data.User;
using Last.Entity.Field;
using Last.Event;
using Last.Interpreter;
using Last.Interpreter.Instructions;
using Last.Interpreter.Instructions.SystemCall;
using Last.Management;
using Last.Map;
using Last.Systems.Indicator;
using Last.Systems.Message;
using Last.UI;
using Last.UI.KeyInput;
using Prime31;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using static Last.Interpreter.Instructions.External;
using static Last.Map.LoadData;
//using static Serial.FF1.Map.TransportationEvent;

namespace FF1PRAP
{//[HarmonyPatch("GetExp")]
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
			InternalLogger.LogInfo($"new LocationData() {{ Content = {prop.ContentId}, Qty = {prop.ContentNum}, Id = {prop.EntityId}, Flag = {prop.FlagId}, Type = LocationType.Treasure, Name = \"\", Script = {prop.ScriptId}, Map = \"{GameData.CurrentMap}\", Access = new() {{ }} }},");
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
				GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 31, 1);
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

			InternalLogger.LogInfo($"Running {scriptName} on {GameData.CurrentMap}");
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


		private static void ItemContainer(ref string masterLine)
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

			GameData.UserData = __instance;
			InternalLogger.LogInfo($"Init: {__instance.CurrentLocation}");


			//			GameStateTracker.Fi
		}

		public static void StartNewGame(Serial.FF1.UI.KeyInput.NewGameWindowController __instance)
		{
			InternalLogger.LogInfo($"New game started: {__instance.stateMachine.current.ToString()}");
		}

		public static void GetLatestSaveSlotData(SaveSlotManager __instance, int slotId)
		{

			//FF1PR.SaveManager = __instance;
			//FF1PR.CurrentSlot = slotId;
			InternalLogger.LogInfo($"Slot Loaded: {slotId}");

			// Load randomization data here
			//			GameStateTracker.Fi
		}

		public static void GetLatestSaveSlotData2(SaveSlotManager __instance, SaveSlotData saveData)
		{

			//FF1PR.SaveManager = __instance;
			//FF1PR.CurrentSlot = saveData.id;
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

			if (GameData.StateTracker is null)
			{
				GameData.StateTracker = GameObject.Find("GameStateTracker").GetComponent<GameStateTracker>();
			}
			/*
			var dataManager = FF1PR.UserData;
			
			//UserDataManager dataManager = GameObject.Fi);

			if (FF1PR.RandoData.Counter == 60)
			{
				FF1PR.RandoData.Counter = 0;
				
				//InternalLogger.LogInfo($"State: {GameData.StateTracker.CurrentState} /  {GameData.StateTracker.CurrentSubState}");

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
			var itemNameKey = GameData.MasterManager.GetList<Content>()[FF1PR.RandoData.CurrentItem].MesIdName;
			var itemName = GameData.MessageManager.GetMessage(itemNameKey);

			//GameData.MessageManager.GetMessage("");
			GameData.MessageManager.RemoveValue("MSG_CUSTM_01");
			GameData.MessageManager.AddMessage("MSG_CUSTM_01", $"You obtained {itemName}? Hope you're happy with it!");
			GameData.MessageManager.GetMessageDictionary()["MSG_CUSTM_01"] = $"You obtained {itemName}? Hope you're happy with it!";

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

		public static void Parse_Text_Pre(SystemIndicator.Mode mode)
		{
			InternalLogger.LogInfo($"SystIndicator {mode}");

		}

		public static void SetMessagePost(Last.Message.MessageWindowController __instance, string message)
		{
			InternalLogger.LogInfo($"WindowController: {message} - {__instance.displayIndex}");

		}

		public static void OnCreate_Pre(Last.Message.MessageWindowManager __instance)
		{

			InternalLogger.LogInfo($"MessageWindowManager: {__instance.GetInstanceID()} - {__instance.gameObject.name}");
			InternalLogger.LogInfo($"2: Auto: {__instance.isAuto} - Field: {__instance.isFieldWindow} ");
			InternalLogger.LogInfo($"3: NextWait: {__instance.isNextMessageWait} - Playing: {__instance.isPlaying} ");
			InternalLogger.LogInfo($"4: Speed: {__instance.messageSpeed} - Name: {__instance.name} ");
			InternalLogger.LogInfo($"5: Newline: {__instance.NewLineTag} - NextState: {__instance.nextState} ");
			InternalLogger.LogInfo($"6: PrevIndex: {__instance.prevMessageIndex} - Tag: {__instance.tag} ");
			InternalLogger.LogInfo($"7: waitlist: {__instance.timeWaitList.Count} - WaitTag: {__instance.WaitTag} ");
		}
		




		public static void IntegratorExit_Post(Integrator __instance)
		{
			//FF1PR.ScriptIntegrator = __instance;
			//InternalLogger.LogInfo($"Exiting script {__instance.scriptName}.");
		}

		public static void IntegratorPurge_Post(Integrator __instance)
		{
			InternalLogger.LogInfo($"Integrator: Purging {__instance.scriptName}.");
		}
		public static void IntegratorReset_Post(Integrator __instance)
		{
			InternalLogger.LogInfo($"Integrator: Reset {__instance.scriptName}.");
		}

		public static void SetEntitiesEnable_Post(bool enable)
		{
			InternalLogger.LogInfo($"MapModel: Event Entites Enable {enable}.");
		}
		public static void SetActive_Post(bool active)
		{
			InternalLogger.LogInfo($"MapModel: Sec active {active}.");
		}

		public static void InitPlayerStatePlay_Post()
		{
			InternalLogger.LogInfo($"FieldController: Player State Init.");
		}

		public static void CreateSlotListData_Post(SaveSlotData __instance)
		{
			InternalLogger.LogInfo($"Saveslot {__instance.id}: {__instance.configData} - {__instance.CurrentArea}");
			InternalLogger.LogInfo($"Saveslot {__instance.id} - 2: {__instance.CurrentLocation} - {__instance.dataStorage}");
			InternalLogger.LogInfo($"Saveslot {__instance.id} - 3: {__instance.clearFlag} - {__instance.timeStamp}");
			InternalLogger.LogInfo($"Saveslot {__instance.id} - 4: {__instance.mapData} ");
			InternalLogger.LogInfo($"Saveslot {__instance.id} - 5: {__instance.playTime} - {__instance.timeStamp}");
			InternalLogger.LogInfo($"Saveslot {__instance.id} - 6: {__instance.userData}");

		}

		private static void SetSlotNumText_Post(ref Last.UI.KeyInput.SaveContentController __instance)
		{
			InternalLogger.LogInfo($"Saveslot Num: {__instance.slotName} - {__instance.slotNum}");

			__instance.slotNum = "weeeeee";
		}

		private static void SetSlotNumText_Pre(ref string name)
		{
			InternalLogger.LogInfo($"Saveslot Num: {name}");
			name = "weee";
		}

		private static void InitSaveView_Post(ref SaveContentController __result)
		{
			InternalLogger.LogInfo($"SaveContentCreate: {__result.slotName} - {__result.slotNum}");
			__result.slotName = "riiiiiight";
			//__instance.slotNumText.text = "yooo";
		}

		private static void SetData_Pre()
		{

			InternalLogger.LogInfo($"SaveSlot data create!!!!");

			/*
			if (__result.SaveSlotDataList != null)
			{
				__result.SaveSlotDataList[3].timeStamp = "wawawoooo";


			}*/

			//___TimeStamp = "George";

			/*

			SaveSlotData data = new();
			if (__instance.SaveSlotDataList != null)
			{
				foreach (var save in __instance.SaveSlotDataList)
				{

					InternalLogger.LogInfo($"SlotManagerSlot: {save.timeStamp} - {save.IsSuccess}");
				}

			}*/



			//__instance.slotName = "riiiiiight";
			//__instance.slotNumText.text = "yooo";
		}

		private static void SetDSlotName_Post(ref SaveContentView __instance)
		{
			InternalLogger.LogInfo($"ContentView: {__instance.slotNumText.text} - {__instance.slotNameText.text}");
			//__instance.slotName = "riiiiiight";
			//__instance.slotNumText.text = "yooo";
		}

		private static void SetActive_Post(SaveListController.Mode mode)
		{
			InternalLogger.LogInfo($"SaveListView: {mode}");
		}

		public static void TaskCheckComplete_Post(ref bool __result, ResourceLoadTask __instance)
		{
			/*
			if (__result)
			{
				var name = __instance.GetAssetName();



			}*/


			InternalLogger.LogInfo($"TaskLoad Check: {__instance.GetAssetName()} - {__result}");
		}



		public static void CheckGroupLoadAssetCompleted2_Post(bool __result, string groupName, string assetName)
		{
			InternalLogger.LogInfo($"RessourceManager check: {assetName} - {__result}.");
		}



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

		public static void SetContentData_Pre(ref SaveSlotData data, int index, SaveListController.Mode mode)
		{
			//InternalLogger.LogInfo($"SaveSloData: {index} - {data.id}");
			GameData.SaveInfoState.CurrentSlot = data.id;
		}

		public static void CheckFairyShop_Post()
		{
			InternalLogger.LogInfo($"Fairy: {Last.Interpreter.Instructions.SystemCall.Current.kFairyBottleId} - {Last.Interpreter.Instructions.SystemCall.Current.kFairyBottleEventFlag}");

			//Last.Interpreter.Instructions.SystemCall.Current.kFairyBottleId = FF1PR.PlacedItems[(int)TreasureFlags.Caravan].Id;
		}

		private static void BuyItemProduct_Post(ShopProductData data, int count, bool __result)
		{
			if (SessionManager.GameMode == GameModes.Randomizer)
			{
				// 141 is the caravan shop product id
				if (data.ProductId == 141 && __result)
				{
					GameData.DataStorage.Set(Last.Interpreter.DataStorage.Category.kTreasureFlag1, (int)TreasureFlags.Caravan, 1);
				}
			}
			else if (SessionManager.GameMode == GameModes.Archipelago)
			{
				if (data.ProductId == 141 && __result)
				{
					GameData.DataStorage.Set(Last.Interpreter.DataStorage.Category.kTreasureFlag1, (int)TreasureFlags.Caravan, 1);
					Archipelago.instance.ActivateCheck(Logic.FlagToLocationName[(int)TreasureFlags.Caravan]);
				}
			}
		}

		private static void InitializeCommandList_Post(Last.UI.KeyInput.ConfigActualDetailsControllerBase __instance)
		{
			ConfigCommandController boostcommand = null;
			foreach (var command in __instance.commandList)
			{
				if (command.NameText.text == "Boost")
				{
					boostcommand = command;
				}
				
				InternalLogger.LogInfo($"Commands: {command.NameText.text}");
			
			}
			foreach (var command in __instance.SelectedContentNameList)
			{
				InternalLogger.LogInfo($"ContentName: {command.Value}");
			}


			if (boostcommand != null)
			{ 
			
			}
			__instance.commandList.Remove(boostcommand);
		}

		private static void CreateConfig_Post(MainMenuController __instance, ConfigController __result)
		{
			foreach (var menu in __instance.subMenuList)
			{
				InternalLogger.LogInfo($"SubMenu: {menu.Key} - {menu.Value.CallerCommandId}");
			}
		}

		private static void AddSelectedCommandMessage_Post(ConfigCommandType type, string message, bool isRemove = false)
		{
			InternalLogger.LogInfo($"AddCommand:{type} - {message} - {isRemove}");
		
		}
		private static void BuyItemInt_Post(int productId, int count, bool __result)
		{
			if (productId == Randomizer.Data.PlacedItems[(int)TreasureFlags.Caravan].Id && __result)
			{
				InternalLogger.LogInfo($"Item bought! Int");
			}
			else
			{
				InternalLogger.LogInfo($"Item bought? Int {__result} {productId}");
			}
		}

		private static void GetSubtractSteps_Post(ref int __result)
		{
			InternalLogger.LogInfo($"Map Steps: {__result}");
			//__result = 1; 
		}

		private static void GetRequiredStepsRange_Post(MapModel __instance, ref RequiredStepsRange __result)
		{
			float multiplier = Randomizer.Data.DungeonEncounterRate;

			if (__instance.GetMapName() == "Map_10010")
			{
				multiplier = Randomizer.Data.OverworldEncounterRate;
			}


			__result = new RequiredStepsRange((int)(__result.Min * multiplier), (int)(__result.Max * multiplier));
			//InternalLogger.LogInfo($"Step Range for {__instance.GetMapName()}: {__result.Min} - {__result.Max}");
			
		}
		private static void GameBooster_Pre(Action<ISubMenuArgument> value)
		{

			InternalLogger.LogInfo($"Command: {value.Method.Name}");
		}

		private static void CreateTelepoPointList_Post(Il2CppReferenceArray<TelepoPointData> telepoPoints)
		{

			InternalLogger.LogInfo($"---{telepoPoints.Count}---");

			foreach (var telepo in telepoPoints)
			{
				InternalLogger.LogInfo($"TelePo: {telepo.mapId} - {telepo.property.EntityId} - {telepo.property.GotoEntityId} - {telepo.pointInEntity.Property.EntityId}");

			}
		
		}

		private static void NextMapProperty_Pre(LoadData __instance, ref PropertyGotoMap property)
		{

			// alright, so Map-EntityId is the identifier, then update MapId, PointId, AssetGroup and AssetName
			// i guess we'll have to mine manually
			// so

			InternalLogger.LogInfo($"NextMapProperty: {GameData.CurrentMap};{property.EntityId};{property.MapId};{property.PointId};{property.AssetGroupName};{property.AssetName}");
			

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
		public static void CastTelepo_Post()
		{

			Randomizer.Teleporting = true;
			InternalLogger.LogInfo($"Casting Teleportation.");

			//InternalLogger.LogInfo($"Telepo Data: {Last.Interpreter.Instructions.External.Telepo.kMapId} - {Last.Interpreter.Instructions.External.Telepo.kPointInId}");

		}

		public static void TelepoCache_Add_Point(TelepoPointData telepo)
		{
			InternalLogger.LogInfo($"Telepo Cache Point: Adding ({telepo.mapId}, {telepo.pointInEntity})");
		}
		public static void TelepoCache_Add_Item(TelepoCacheItem item)
		{
			InternalLogger.LogInfo($"Telepo Cache Item: Adding ({item.MapId}, {item.PointInObjectId}) - {item.KeyName}");
		}
		public static void TelepoCache_Add_Int(int mapId, int pointInObjectId)
		{
			InternalLogger.LogInfo($"Telepo Cache Int: Adding ({mapId}, {pointInObjectId})");
		}
		public static void TelepoCache_Pop(TelepoCacheItem __result)
		{
			InternalLogger.LogInfo($"Telepo Cache Pop: Popping ({__result.MapId}, {__result.PointInObjectId})");
			Randomizer.Teleporting = true;
		}

		public static void TelepoCache_Peek(TelepoCache __instance, TelepoCacheItem __result)
		{
			if (GameData.TelepoCache == null)
			{
				GameData.TelepoCache = __instance;
			}
			InternalLogger.LogInfo($"Telepo Cache Peeking ({__result.MapId}, {__result.PointInObjectId})");
		}

		public static void TelepoCache_Remove()
		{
			InternalLogger.LogInfo($"Deleting Cache");
		}

		public static void TelepoCache_Get()
		{
			InternalLogger.LogInfo($"Telepo Cache Get.");
			Randomizer.Teleporting = true;
		}

		public static void AccessorGotoMap(LoadData newCache, bool skipFade, bool skipBgm)
		{
			InternalLogger.LogInfo($"GotoMap Access: {newCache.MapId} - {newCache.Point}");
		}
		public static void ProcedureGotoMap(LoadData loadData, bool skipFade, bool skipBgm )
		{
			InternalLogger.LogInfo($"GotoMap Procedure: {loadData.MapId} - {loadData.Point}");
		}

		public static void InitializeOwnedTransportation_Pre()
		{
			InternalLogger.LogInfo($"Owned Transport Initialized.");
		}

		public static void AddOwnedTransportationList_Pre()
		{
			InternalLogger.LogInfo($"Owned Transport List Added.");
		}

		public static void GetCat(DataStorage.Category c, int index)
		{
			InternalLogger.LogInfo($"Data Get: {c} - {index}");
		}
		public static void GetString(string c, int index)
		{
			InternalLogger.LogInfo($"Data Get: {c} - {index}");
		}
		public static void GetFlag(DataStorage.Flags f, int index, int segment)
		{
			InternalLogger.LogInfo($"Data Get: {f} - {index} - {segment}");
		}
		public static void ActionGotoMap(ref PropertyGotoMap propertyGotoMap)
		{
			InternalLogger.LogInfo($"On Cache: {propertyGotoMap.MapId} - {propertyGotoMap.PointId} - {propertyGotoMap.Gimmick}");
			/*
			if (propertyGotoMap.MapId == 1)
			{
				propertyGotoMap.MapId = 255;
			}*/

			InternalLogger.LogInfo($"NextMapProperty: {GameData.CurrentMap};{propertyGotoMap.EntityId};{propertyGotoMap.MapId};{propertyGotoMap.PointId};{propertyGotoMap.AssetGroupName};{propertyGotoMap.AssetName}");

			bool wasOverworld = false;
			if (propertyGotoMap.MapId == 1)
			{
				wasOverworld = true;
			}

			if (Randomizer.PointToTeleporters.TryGetValue((propertyGotoMap.MapId, propertyGotoMap.PointId), out var currentteleporter))
			{
				if (Randomizer.Data.Entrances.TryGetValue(currentteleporter.Name, out var entrance))
				{
					var newpoint = Randomizer.NameToTeleporters[entrance];

					InternalLogger.LogInfo($"NextMapProperty: Found {currentteleporter.Name}, replacing by {newpoint.Name}");
					//InternalLogger.LogInfo($"NextMapProperty: Found {(property.MapId, property.PointId)}, replacing by {(newpoint.MapId, newpoint.PointId)}");

					propertyGotoMap.MapId = newpoint.MapId;
					propertyGotoMap.PointId = newpoint.PointId;
					propertyGotoMap.AssetGroupName = newpoint.MapGroup;
					propertyGotoMap.AssetName = newpoint.MapName;


					if (wasOverworld && propertyGotoMap.MapId != 1)
					{
						var teledata = GameData.TelepoCache.Peek();
						if (teledata != null)
						{
							if (teledata.MapId == propertyGotoMap.MapId && teledata.PointInObjectId == propertyGotoMap.PointId)
							{
								GameData.TelepoCache.Pop();
							}
						}
					}
				}
			}
		}
		private static void NextMapInt_Pre(ref int mapId, ref int point)
		{
			InternalLogger.LogInfo($"NextMapInt: {mapId} - {point}");

			if (mapId == 94)
			{
				if (Randomizer.Data.OrdealsMaze.TryGetValue(point, out var newpoint))
				{
					InternalLogger.LogInfo($"NextMapInt: Hijack {point} > {newpoint}");
					point = newpoint;
				}
			}

			
			//InternalLogger.LogInfo($"NextMapInt: {GameData.CurrentMap};{property.EntityId};{property.MapId};{property.PointId};{property.AssetGroupName};{property.AssetName}");
			/*
			if (mapId == 94 && point == 3)
			{
				mapId = 94;
				point = 2;
			}*/
		}
		private static void SetMapHandle_Pre(ref IMapAccessor accessor)
		{
			if (GameData.MapAccessor == null)
			{
				InternalLogger.LogTesting($"Getting MapAccessor");
				GameData.MapAccessor = accessor;
			}
		}
		private static void SetDataList_Pre(IMapAccessor mapAccessor, List<OwnedTransportationData> ownedDataList, bool onlyEnable, bool isInit)
		{
			InternalLogger.LogTesting($"SetDataList: {onlyEnable} - {isInit}");
		}
		private static void SetDataData_Pre(IMapAccessor mapAccessor, OwnedTransportationData ownedData, bool onlyEnable, bool isInit)
		{
			InternalLogger.LogTesting($"SetDataData: {ownedData.Id}; {ownedData.flagNumber} - {onlyEnable} - {isInit}");
		}
		private static void CheckOkList_Post(int transportationId, int attribute, bool __result, TransportationController __instance)
		{
			InternalLogger.LogTesting($"Checking Transport: {transportationId}, {attribute} > {__result}");

			foreach (var data in __instance.infoData.modelList)
			{
				InternalLogger.LogTesting($"ModelList: {data.key} - {data.value.id};{data.value.groupId};{data.value.type}");

			}
		}

		private static void TransportationEnabled_Pre(int transportationId, bool enable, int stayMapId, bool isScript)
		{
			InternalLogger.LogTesting($"Transport Enabled: {transportationId}, {enable}, {stayMapId}, {isScript}");

		}
		private static void NextMapVector_Pre(int mapId, Vector3 cellPos, int transportationId, int direction)
		{
			InternalLogger.LogInfo($"NextMapVector: {mapId} - {cellPos} - {transportationId} - {direction}");
		}

		private static void GetTileMapData_Post(ref TileMapData __result)
		{
			/*
			InternalLogger.LogInfo($"TileMapData: {__result.type}");

			foreach (var layer in __result.layers)
			{
				InternalLogger.LogInfo($"TileMapLayerGroupData: {layer.name} - {layer.id}");

				if (layer.id == 3)
				{
					InternalLogger.LogInfo($"TileMapLayerData?");
					for (int y = 0; y < 128; y++)
					{ 
						for (int x = 0; x < 128; x++)
						{
							layer.layers[0].data[y * 256 + x] = 0;
						}
					}

					//InternalLogger.LogInfo($"TileMapLayerData: ");
				}
			}*/
		}

		public static void ConfigLoadStart()
		{
			InternalLogger.LogInfo("Load Start");
		}
		public static void ConfigReloadScene()
		{
			InternalLogger.LogInfo("Config Reload");
		}
		public static void ConfigTitleBack()
		{
			InternalLogger.LogInfo("Title Back");
		}
		public static void GetAsset_Post(string assetName, ref string __result)
		{
			InternalLogger.LogInfo($"End Roll: {assetName} - {__result}");
		}
	}
	public enum SaveInfoModes
	{ 
		MainMenu,
		LoadMenu,
		SaveMenu,
	}
}
