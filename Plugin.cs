using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using UnityEngine;

using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using static FF1PRAP.MyPatches;
using Last.Management;
using Il2CppSystem;
using System.Collections.Generic;
using System.IO;
using Il2CppInterop.Runtime;
using Last.Interpreter.Instructions.SystemCall;
using System.Runtime.InteropServices;
using Last.Data;
using Last.Interpreter;
using Last.Data.Master;
using Last.Map;
using static PostProcessLite;
using System.Xml;
using Last.Scene;
using Last.Data.User;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Last.Message;

namespace FF1PRAP;

public class PluginInfo
{
	public const string NAME = "FF1 Pixel Remaster AP";
	public const string VERSION = "0.1.12";
	public const string GUID = "wildham.ff1pr.randomizer";
}

[BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
public class FF1PR : BasePlugin
{
	// Instances
	public static UserDataManager UserData;
	public static GameStateTracker StateTracker;
	public static DataStorage DataStorage;
	public static MessageManager MessageManager;
	public static MasterManager MasterManager;
	public static MapManager MapManager;
	public static FieldController FieldController;
	public static OwnedItemClient OwnedItemsClient;
	public static MainGame MainGame;
	public static ResourceManager ResourceManager;
	public static Integrator ScriptIntegrator;

	// save stuff at save load 
	public static SaveSlotManager SaveManager;
	public static SaveSlotData CurrentSave;

	// Loading/Saving Menu Stuff
	public static Last.UI.KeyInput.TitleWindowController TitleWindowController;
	public static Last.UI.KeyInput.LoadGameWindowController LoadGameWindowController;
	public static Last.UI.KeyInput.MainMenuController MainMenuController;
	public static Last.UI.KeyInput.SaveListController SaveListController;
	public static Last.UI.KeyInput.LoadWindowController LoadWindowController;
	public static Last.UI.KeyInput.SaveWindowController SaveWindowController;



	public static int CurrentSlot;

	// Settings
	public static SessionManager SessionManager;
	public static string CurrentMap => FF1PR.MapManager != null ? FF1PR.MapManager.CurrentMapModel.AssetData.MapName : "None";
	public static Dictionary<int, ItemData> PlacedItems;
	public static GameStates GameState => Monitor.instance != null ? (GameStates)Monitor.instance.GetGameState() : GameStates.Title;
	//public static GameStates GameState => GameStates.Title;
	public override void Load()
	{
		// Plugin startup logic
		InternalLogger.SetLogger(base.Log);
		InternalLogger.LogInfo($"Plugin {PluginInfo.NAME} v{PluginInfo.VERSION} is loaded! ({PluginInfo.GUID})");

		SessionManager = new SessionManager();

		ClassInjector.RegisterTypeInIl2Cpp<Archipelago>();
		ClassInjector.RegisterTypeInIl2Cpp<Monitor>();
		ClassInjector.RegisterTypeInIl2Cpp<ApItemWindow>();
		
		RegisterTypeAndCreateObject(typeof(SettingsWindow), "settings gui");
		RegisterTypeAndCreateObject(typeof(SaveInfoWindow), "save info");

		//Application.runInBackground = Settings.RunInBackground;

		Harmony harmony = new Harmony(PluginInfo.GUID);

		// Xp patch
		//harmony.Patch(AccessTools.Method(typeof(Last.Data.Master.Monster), "get_Exp"), null, new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "get_Exp")));

		// Treasure Patch
		harmony.Patch(AccessTools.Method(typeof(Last.Map.EventActionTreasure), "CreateTask"), new HarmonyMethod(AccessTools.Method(typeof(Patches), "Treasure_Prefix")));

		// Item Patch
		harmony.Patch(AccessTools.Method(typeof(Last.Management.OwnedItemClient), "AddOwnedItem", [typeof(Content), typeof(int)]), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "Items_Postfix")));

		// Gameflag Patch
		harmony.Patch(AccessTools.Method(typeof(Last.Interpreter.DataStorage), "Set", [typeof(string), typeof(int), typeof(int)]), new HarmonyMethod(AccessTools.Method(typeof(Patches), "Gameflags_Postfix")));

		// Script Patch
		harmony.Patch(AccessTools.Method(typeof(Last.Map.MapAssetData), "GetScript"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "GetScript_Postfix")));

		// Entities Patch
		//harmony.Patch(AccessTools.Method(typeof(MapUtility), "ParseMapObjectGroupData"), new HarmonyMethod(AccessTools.Method(typeof(Patches), "ParseMapObjectGroupData_Prefix")));
		harmony.Patch(AccessTools.Method(typeof(Last.Map.FieldController), "SetupEntities"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "SetupEntities_Post")));

		// Instances
		harmony.Patch(AccessTools.Method(typeof(Last.Management.MessageManager), "Initialize"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "MessageManagerInit_Postfix")));
		harmony.Patch(AccessTools.Method(typeof(Last.Interpreter.DataStorage), "Initialize"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "DataStorageInit_Postfix")));
		harmony.Patch(AccessTools.Method(typeof(Last.Management.GameStateTracker), "Start"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "GameStateTrackerInit_Postfix")));
		harmony.Patch(AccessTools.Method(typeof(Last.Management.UserDataManager), "Initialize"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "UserDataManagerInit_Postfix")));
		harmony.Patch(AccessTools.Method(typeof(Last.Data.Master.MasterManager), "Initialize"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "MasterManagerInit_Postfix")));
		harmony.Patch(AccessTools.Method(typeof(Last.Map.MapManager), "InitInsitance"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "MapManagerInit_Postfix")));
		harmony.Patch(AccessTools.Method(typeof(Last.Map.FieldController), "Initialize"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "FieldControllerInit_Postfix")));
		harmony.Patch(AccessTools.Method(typeof(Last.Management.OwnedItemClient), "AddOwnedItem", [typeof(Content), typeof(int)]), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "OwnedItemClientInit_Postfix")));

		// Init injected classes
		harmony.Patch(AccessTools.Method(typeof(SceneTitleScreen), "CreateInstance"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "CreateInstance_Post")));

		// Saving/Loading game
		harmony.Patch(AccessTools.Method(typeof(Last.Management.SaveSlotManager), "LoadSlot"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "LoadSaveDataFromBoot_Postfix")));
		harmony.Patch(AccessTools.Method(typeof(Last.Management.SaveSlotManager), "GotoLoadSaveData"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "LoadSaveDataFromMenu_Postfix")));
		harmony.Patch(AccessTools.Method(typeof(Last.Management.SaveSlotManager), "SaveSlot"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "GetSavePath_Post")));
		harmony.Patch(AccessTools.Method(typeof(Last.Management.SaveSlotManager), "Save", [typeof(int), typeof(SaveSlotData)]), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "GetSavePath_Post")));
		harmony.Patch(AccessTools.Method(typeof(Last.Management.SaveSlotManager), "Save", [typeof(int), typeof(Action<bool>), typeof(bool)]), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "GetSavePath_Post")));

		// New game
		harmony.Patch(AccessTools.Method(typeof(Serial.FF1.UI.KeyInput.NewGameWindowController), "UpdateStartWait"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "NewGame_Postfix")));
		harmony.Patch(AccessTools.Method(typeof(Serial.FF1.UI.Touch.NewGameWindowController), "UpdateStartWait"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "NewGame_Postfix")));
		
		// Loading State
		harmony.Patch(AccessTools.Method(typeof(Last.Systems.Indicator.SystemIndicator), "Activate"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "GetLoadingState_Post")));

		// Resource Manager
		harmony.Patch(AccessTools.Method(typeof(Last.Management.ResourceManager), "Initialize"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "GetResourceManager_Post")));
		//harmony.Patch(AccessTools.Method(typeof(Last.Management.ResourceManager), "AddLoadingTask"), new HarmonyMethod(AccessTools.Method(typeof(Patches), "AddLoadingTask_Pre")));
		//harmony.Patch(AccessTools.Method(typeof(Last.Management.ResourceLoadTask), "CheckComplete"), null, new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "TaskCheckComplete_Post")));
		harmony.Patch(AccessTools.Method(typeof(Last.Management.ResourceManager), "CheckCompleteAsset", [typeof(string)]), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "CheckGroupLoadAssetCompleted_Post")));
		//harmony.Patch(AccessTools.Method(typeof(Last.Management.ResourceManager), "CheckLoadAssetCompleted", [typeof(string), typeof(string)]), null, new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "CheckGroupLoadAssetCompleted2_Post")));

		// Loadsing/Saving Screen State
		harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.SaveListController), "CreateDataList"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "SaveListControllerCreateContentList_Post")));
		harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.TitleWindowController), "Initialize"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "TitleWindowControllerInitialize_Post")));
		harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.MainMenuController), "SetNextState"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "MainMenuControllerInitialize_Post")));
		harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.LoadGameWindowController), "Initialize"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "LoadGameWindowControllerInitialize_Post")));
		harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.LoadWindowController), "SetActive"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "LoadWindowControllerInitialize_Post")));
		harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.SaveWindowController), "SetActive"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "SaveWindowControllerInitialize_Post")));


		//harmony.Patch(AccessTools.Method(typeof(Last.Management.ResourceManager), "GetAsset"), new HarmonyMethod(AccessTools.Method(typeof(Patches), "GetAsset_Pre")));
		//harmony.Patch(AccessTools.Method(typeof(Last.Map.FieldController), "EntitiesSetup"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "EntitiesSetup_Post")));

		//harmony.Patch(AccessTools.Method(typeof(Last.Map.FieldController), "InitPlayerStatePlay"), null, new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "InitPlayerStatePlay_Post")));

		/*
		harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.SaveContentController), "SetCharaNameText"), new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "SetSlotNumText_Pre")));

		harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.SaveListController), "CreateContent"), null, new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "InitSaveView_Post")));
		harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.SaveContentController), "SetData"), null, new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "SetData_Post")));
		harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.SaveContentView), "SetSlotNumText"), new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "SetDSlotName_Post")), new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "SetDSlotName_Post")));*/
		//harmony.Patch(AccessTools.Method(typeof(SaveSlotManager), "CreateSlotListUniTask"), new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "SetData_Post")), new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "SetData_Post"))); 



	}
	private static void RegisterTypeAndCreateObject(System.Type type, string name)
	{
		ClassInjector.RegisterTypeInIl2Cpp(type);
		UnityEngine.Object.DontDestroyOnLoad(new GameObject(name, new Il2CppSystem.Type[]
		{
				Il2CppType.From(type)
		})
		{
			hideFlags = HideFlags.HideAndDontSave
		});
	}

}
public class InternalLogger
{

	private static ManualLogSource Logger;

	public static void LogInfo(string message)
	{
		Logger.LogInfo(message);
	}

	public static void LogWarning(string message)
	{
		Logger.LogWarning(message);
	}

	public static void LogError(string message)
	{
		Logger.LogError(message);
	}

	public static void LogDebug(string message)
	{
		Logger.LogDebug(message);
	}

	public static void SetLogger(ManualLogSource logger)
	{
		Logger = logger;
	}

	// set this to true to trigger logging for all log testing messages
	public static bool Testing = false;
	public static void LogTesting(string message)
	{
		if (Testing)
		{
			Logger.LogInfo(message);
		}
	}
}
