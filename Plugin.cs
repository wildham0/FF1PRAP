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

namespace FF1PRAP;

public class PluginInfo
{
	public const string NAME = "FF1 Pixel Remaster";
	public const string VERSION = "0.1.3";
	public const string GUID = "wildham.ff1pr.randomizer";
}

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class FF1PR : BasePlugin
{
	internal static new ManualLogSource Log;

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

	// save stuff at save load 
	public static SaveSlotManager SaveManager;
	public static SaveSlotData CurrentSave;

	public static int CurrentSlot;

	// Settings
	public static SessionManager SessionManager;
	public static string CurrentMap => FF1PR.MapManager.CurrentMapModel.AssetData.MapName;
	public static Dictionary<int, ItemData> PlacedItems;

	public static GameStates GameState => Monitor.instance.GetGameState();
	public override void Load()
	{
		// Plugin startup logic
		InternalLogger.SetLogger(base.Log);
		InternalLogger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} - {MyPluginInfo.PLUGIN_VERSION} is loaded!");

		SessionManager = new SessionManager();

		ClassInjector.RegisterTypeInIl2Cpp<Archipelago>();
		ClassInjector.RegisterTypeInIl2Cpp<Monitor>();
		ClassInjector.RegisterTypeInIl2Cpp<ApItemWindow>();
		//ClassInjector.RegisterTypeInIl2Cpp<QuickSettings>();
		
		RegisterTypeAndCreateObject(typeof(QuickSettings), "quick settings gui");
		//RegisterTypeAndCreateObject(typeof(ApItemWindow), "ap item window");

		//Application.runInBackground = Settings.RunInBackground;

		Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);

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
		harmony.Patch(AccessTools.Method(typeof(MapUtility), "ParseMapObjectGroupData"), new HarmonyMethod(AccessTools.Method(typeof(Patches), "ParseMapObjectGroupData_Prefix")));

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
		
		//
		harmony.Patch(AccessTools.Method(typeof(Last.Systems.Indicator.SystemIndicator), "Activate"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "GetLoadingState_Post")));


		//harmony.Patch(AccessTools.Method(typeof(Last.Message.MessageWindowController), "SetMessage"), null, new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "SetMessagePost")));
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
