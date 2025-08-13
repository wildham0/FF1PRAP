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
using Last.Systems;
using System.Reflection;
using System.Linq;

namespace FF1PRAP;

public class PluginInfo
{
	public const string NAME = "FF1 Pixel Remaster AP";
	public const string VERSION = "0.2.4";
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

	public static int CurrentSlot;
	public static SaveInfoState SaveInfoState = new();

	public static PropertyGotoMap storedGotoMap;

	public static string TMOverworld;

	// Settings
	public static SessionManager SessionManager;
	public static string CurrentMap => FF1PR.MapManager != null ? (FF1PR.MapManager.CurrentMapModel != null ? FF1PR.MapManager.CurrentMapModel.AssetData.MapName : "None") : "None";
	public static Dictionary<int, ItemData> PlacedItems;
	public static GameStates GameState => Monitor.instance != null ? (GameStates)Monitor.instance.GetGameState() : GameStates.Title;
	//public static GameStates GameState => GameStates.Title;
	public override void Load()
	{
		// Create Logger
		InternalLogger.SetLogger(base.Log);
		InternalLogger.LogInfo($"Plugin {PluginInfo.NAME} v{PluginInfo.VERSION} is loaded! ({PluginInfo.GUID})");

		// Create Session Manager
		SessionManager = new SessionManager();

		// Create Behaviours
		ClassInjector.RegisterTypeInIl2Cpp<Archipelago>();
		ClassInjector.RegisterTypeInIl2Cpp<Monitor>();
		ClassInjector.RegisterTypeInIl2Cpp<ApItemWindow>();
		
		RegisterTypeAndCreateObject(typeof(SettingsWindow), "settings gui");

		//Application.runInBackground = Settings.RunInBackground;

		// Apply all the patches
		Harmony harmony = new Harmony(PluginInfo.GUID);

		// Treasure Patch
		harmony.Patch(AccessTools.Method(typeof(Last.Map.EventActionTreasure), "CreateTask"), new HarmonyMethod(AccessTools.Method(typeof(Patches), "Treasure_Prefix")));

		// Item Patch
		harmony.Patch(AccessTools.Method(typeof(Last.Management.OwnedItemClient), "AddOwnedItem", [typeof(Content), typeof(int)]), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "Items_Postfix")));
		harmony.Patch(AccessTools.Method(typeof(ShopUtility), "BuyItem", [typeof(ShopProductData), typeof(int)]), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "BuyItemProduct_Post")));

		// Gameflag Patch
		harmony.Patch(AccessTools.Method(typeof(Last.Interpreter.DataStorage), "Set", [typeof(string), typeof(int), typeof(int)]), new HarmonyMethod(AccessTools.Method(typeof(Patches), "Gameflags_Postfix")));

		// Script Patch
		harmony.Patch(AccessTools.Method(typeof(Last.Map.MapAssetData), "GetScript"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "GetScript_Postfix")));
		
		// Encounter rate Patch
		harmony.Patch(AccessTools.Method(typeof(Last.Map.MapModel), "GetRequiredStepsRange"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "GetRequiredStepsRange_Post")));

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

		// Resource Manager + Assets
		harmony.Patch(AccessTools.Method(typeof(Last.Management.ResourceManager), "Initialize"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "GetResourceManager_Post")));
		harmony.Patch(AccessTools.Method(typeof(Last.Management.ResourceManager), "CheckCompleteAsset", [typeof(string)]), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "CheckGroupLoadAssetCompleted_Post")));
		//harmony.Patch(AccessTools.Method(typeof(Last.Management.ResourceManager), "CheckGroupLoadAssetCompleted", [typeof(string)]), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "CheckLoadAssetCompleted")));

		// Loading/Saving Screen State
		harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.TitleWindowController), "Initialize"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "TitleWindowControllerInitialize_Post")));
		harmony.Patch(AccessTools.Method(typeof(MessageManager), "ReplaceKeyToValue", [typeof(string), typeof(Il2CppSystem.Collections.Generic.Dictionary<string, string>)]), null, new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "ReplaceKey_Post")));
		harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.SaveListController), "SetContentData"), new HarmonyMethod(AccessTools.Method(typeof(Patches), "SetContentData_Pre")));

		// Boost Menu
		harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.ConfigController), "InitializeGameBoosterSetting"), null, new HarmonyMethod(AccessTools.Method(typeof(Patches), "GameBooster_Post")));

		// Loading Map coordinates, we'll need at least the first for EF shuffle
		//harmony.Patch(AccessTools.Method(typeof(Last.Map.LoadData), "NextMapData", [typeof(PropertyGotoMap), typeof(ViewType)]), new HarmonyMethod(AccessTools.Method(typeof(Patches), "NextMapProperty_Pre")));
		harmony.Patch(AccessTools.Method(typeof(Last.Map.LoadData), "NextMapData", [typeof(int), typeof(int), typeof(ViewType)]), new HarmonyMethod(AccessTools.Method(typeof(Patches), "NextMapInt_Pre")));
		//harmony.Patch(AccessTools.Method(typeof(Last.Map.LoadData), "NextMapData", [typeof(int), typeof(Vector3), typeof(int), typeof(int), typeof(ViewType)]), new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "NextMapVector_Pre")));


		harmony.Patch(AccessTools.Method(typeof(Last.Map.MapAssetData), "GetTileMapData"), null, new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "GetTileMapData_Post")));

		//harmony.Patch(AccessTools.Method(typeof(Last.Map.MapModel), "SetTelepoPoints"), new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "CreateTelepoPointList_Post")));
		//harmony.Patch(AccessTools.Method(typeof(Last.UI.KeyInput.ConfigController), "add_OnNextMenu"), new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "GameBooster_Pre")));
		//harmony.Patch(AccessTools.Method(typeof(Last.Map.MapModel), "GetSubtractSteps"), null, new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "GetSubtractSteps_Post")));
		/*
		harmony.Patch(AccessTools.Method(typeof(Last.Interpreter.Instructions.SystemCall.Current), "FairyShop"), null, new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "CheckFairyShop_Post")));
		harmony.Patch(AccessTools.Method(typeof(ShopUtility), "BuyItem", [typeof(int), typeof(int)]), null, new HarmonyMethod(AccessTools.Method(typeof(MyPatches), "BuyItemInt_Post")));*/
		/*
		string scriptfile = "";
		var assembly = Assembly.GetExecutingAssembly();
		string filepath = assembly.GetManifestResourceNames().Single(str => str.EndsWith("tm_overworld.json"));
		using (Stream logicfile = assembly.GetManifestResourceStream(filepath))
		{
			using (StreamReader reader = new StreamReader(logicfile))
			{
				scriptfile = reader.ReadToEnd();
			}
		}

		var split = scriptfile.Split('\n');

		InternalLogger.LogInfo($"Split length: {split.Count()}");
		int[,] pattern =
		{
				{ 000, 000, 773, 000, 000, 000, 000, 000, 000, 000, 000, 000 },
				{ 901, 901, 966, 773, 966, 773, 965, 965, 000, 000, 000, 000 },
				{ 901, 965, 965, 901, 901, 966, 773, 902, 000, 000, 000, 000 },
				{ 902, 902, 773, 773, 901, 773, 966, 966, 966, 965, 000, 000 },
				{ 773, 902, 902, 966, 901, 902, 902, 773, 965, 773, 773, 000 },
				{ 901, 965, 965, 902, 902, 773, 773, 965, 902, 902, 965, 000 },
				{ 000, 965, 902, 902, 773, 773, 901, 901, 966, 966, 965, 000 },
				{ 000, 000, 000, 000, 000, 000, 901, 000, 000, 000, 000, 000 },
		};


		(int x, int y) target = (118, 128);

		for (int y = 0; y < 8; y++)
		{
			for (int x = 0; x < 12; x++)
			{
				InternalLogger.LogInfo($"Pos: {x}, {y}");
				if (pattern[y, x] != 0)
				{
					int line = (target.x + x) + ((target.y + y) * 256) + 248 - 1;
					split[line] = pattern[y, x].ToString() + ",";
				}
			}
		}

		TMOverworld = String.Join("", split);
		*/
		/*


		for (int y = 0; y < 256; y++)
		{
			string line = "";
			for (int x = 0; x < 256; x++)
			{
				line += split[x + 256 * y].Last();
			}
			map.Add(line);
		}


		InternalLogger.LogInfo("---map4--");
		foreach (var line in map)
		{
			InternalLogger.LogInfo(line);
		
		}*/
		/*
		var teststring = new string(MapPatches.TestMap);
		InternalLogger.LogInfo(teststring);
		var result = MapPatcher.Patch(teststring, 0, MapPatches.Test, 4, 4);

		InternalLogger.LogInfo(result);*/
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
