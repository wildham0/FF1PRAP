using Last.Data.Master;
using Last.Interpreter;
using Last.Management;
using Last.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine.SocialPlatforms;
using static FF1PRAP.Patches;
//using Il2CppSystem.Collections.Generic;

namespace FF1PRAP
{
	public static class GameData
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
		public static ResourceManager ResourceManager;

		// Loading/Saving Menu Stuff
		public static Last.UI.KeyInput.TitleWindowController TitleWindowController;

		public static SaveInfoState SaveInfoState = new();

		public static string CurrentMap => GameData.MapManager != null ? (GameData.MapManager.CurrentMapModel != null ? GameData.MapManager.CurrentMapModel.AssetData.MapName : "None") : "None";
		public static GameStates GameState => Monitor.instance != null ? (GameStates)Monitor.instance.GetGameState() : GameStates.Title;

		//public static int CurrentSlot;
		//public static Integrator ScriptIntegrator;
		//public static MainGame MainGame;
		// save stuff at save load 
		//public static SaveSlotManager SaveManager;
		//public static SaveSlotData CurrentSave;
	}
}
