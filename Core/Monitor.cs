using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using Il2CppSystem;
using Last.Data.Master;
using Last.Entity.Field;
using Last.Interpreter;
using Last.Management;
using Last.Message;
using Last.Systems.Indicator;
using Last.UI.KeyInput;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Il2CppSystem.Uri;
using static UnityEngine.GridBrushBase;
using static UnityEngine.InputSystem.Users.InputUser;
using static Last.Management.SubSceneManagerMainGame;

namespace FF1PRAP
{	public enum ProcessStates
	{ 
		Reset,
		InitGame,
		LoadGame,
		PostLoadGame,
		NewGame,
		ResetAndInitGame,
		None
	}
	public class MonitorTool
	{
		public GameStates GameState = GameStates.Title;
		public ProcessStates ProcessState = ProcessStates.InitGame;
		public State MainState = State.Init;
		public SystemIndicator.Mode LoadingState = SystemIndicator.Mode.kNone;
		public Last.Defaine.MenuCommandId MainMenuState = Last.Defaine.MenuCommandId.Non;
		private bool newGameProcessed = false;
		private bool nowLoading = false;
		public List<string> AssetsToPatch = new();
		public List<int> JobQueue = new();
		public bool ShipWarp = false;
		public bool CanoeWarp = false;
		public MonitorTool() { }

		public void Update()
		{
			ProcessGameState();
			ProcessPatches();
			ProcessItemQueue();
			ProcessJobQueue();

			if (SessionManager.GameMode == GameModes.Vanilla)
			{
				return;
			}

			//InternalLogger.LogInfo($"Process State: {ProcessState}; Loading: {LoadingState}; Game: {GameState}: StateTracker: {(GameData.StateTracker != null ? GameData.StateTracker.CurrentState + " + " + GameData.StateTracker.CurrentSubState : "null")}");

			if (ProcessState == ProcessStates.Reset)
			{
				InternalLogger.LogInfo($"Randomizer Settings Reset.");
				ProcessState = ProcessStates.None;
				Randomizer.Teleporting = false;
				GameData.OwnedItemsClient = null;
				newGameProcessed = false;
			}
			else if (GameState == GameStates.Title && ProcessState == ProcessStates.InitGame)
			{
				Initialization.ApplyBaseGameModifications();
				ProcessState = ProcessStates.None;
			}
			else if (GameState == GameStates.Title && ProcessState == ProcessStates.ResetAndInitGame)
			{
				InternalLogger.LogInfo($"Randomizer Settings Reset + Game Intialization.");
				GameData.OwnedItemsClient = null;
				newGameProcessed = false;
				SessionManager.RandomizerInitialized = false;
				Initialization.ApplyBaseGameModifications();
				ProcessState = ProcessStates.None;
			}
			else if (ProcessState == ProcessStates.NewGame)
			{
				ProcessState = ProcessStates.PostLoadGame;

				if (!newGameProcessed)
				{
					newGameProcessed = true;
					if (SessionManager.GameMode == GameModes.Randomizer)
					{
						Initialization.ApplyRandomizedFeatures(Randomizer.Data);
					}
					else if (SessionManager.GameMode == GameModes.Archipelago)
					{
						Randomizer.Data = new();
						SessionManager.Data.Reset();
						InternalLogger.LogInfo($"Loading saved randomization data.");
						if (!Randomizer.Load(SessionManager.FolderPath, "ap_" + SessionManager.Data.Player + "_" + SessionManager.Data.WorldSeed))
						{
							InternalLogger.LogInfo($"File not found, generating randomization data.");
							Randomizer.Randomize();
						}
						Initialization.ApplyRandomizedFeatures(Randomizer.Data);
					}
					Initialization.InitializeNewGame();
				}
			}
			else if (ProcessState == ProcessStates.LoadGame)
			{
				ProcessState = ProcessStates.PostLoadGame;
				InternalLogger.LogInfo($"Process Load Game - Game State:{GameState}");

				if (SessionManager.GameMode == GameModes.Archipelago)
				{
					InternalLogger.LogInfo($"Loading saved randomization data.");
					if (!Randomizer.Load(SessionManager.FolderPath, "ap_" + SessionManager.Data.Player + "_" + SessionManager.Data.WorldSeed))
					{
						InternalLogger.LogInfo($"File not found, generating randomization data.");
						Randomizer.Randomize();
					}
					//Archipelago.instance.RestoreState();

					Initialization.ApplyRandomizedFeatures(Randomizer.Data);
				}
				else
				{
					if (!Randomizer.Load(SessionManager.FolderPath, SessionManager.Data.Seed + "_" + SessionManager.Data.Hashstring))
					{
						InternalLogger.LogWarning($"File not found, gameplay might be unstable. Generate a game first in the Solo Randomizer Settings Menu.");
					}

					//FF1PR.PlacedItems = Randomizer.Data.PlacedItems;
					Initialization.ApplyRandomizedFeatures(Randomizer.Data);
				}
			}
			else if (ProcessState == ProcessStates.PostLoadGame && MainState == SubSceneManagerMainGame.State.Player)
			{
				if (SessionManager.GameMode == GameModes.Archipelago)
				{
					InternalLogger.LogInfo($"Post Load Processs for AP.");
					Archipelago.instance.RestoreState();

					foreach (var check in SessionManager.Data.LocationsChecked)
					{
						InternalLogger.LogInfo($"Sending check: {check}");
						Archipelago.instance.ActivateCheck(check);
					}
					ProcessApDataStorage();
				}

				ProcessFlags();
				ProcessState = ProcessStates.None;
			}

			if (ShipWarp && MainState == State.Player)
			{
				var shipscript = new Last.Interpreter.ScriptSandbox("sc_ship_warp");
				ShipWarp = false;
			}
			else if (CanoeWarp && MainState == State.Player)
			{
				var canoescript = new Last.Interpreter.ScriptSandbox("sc_canoe_warp");
				CanoeWarp = false;
			}
		}
		private void ProcessPatches()
		{
			List<string> patchToRemove = new();

			foreach (var patchdata in AssetsToPatch)
			{
				if (GameData.ResourceManager.completeAssetDic.ContainsKey(patchdata))
				{
					var assettext = GameData.ResourceManager.completeAssetDic[patchdata].Cast<TextAsset>().text;
					var assetnameparts = patchdata.Split('/');
					var assetname = assetnameparts.Last();
					var filename = assetnameparts[assetnameparts.Count() - 2] + "/" + assetnameparts[assetnameparts.Count() - 1];
					InternalLogger.LogTesting($"MapPatcher: Patching {filename}.");

					if (Randomizer.MapAssetsToPatch.TryGetValue(patchdata, out var mappatches))
					{
						assettext = MapPatcher.Patch(assettext, mappatches);
					}
					else if (Randomizer.EntityAssetsToPatch.TryGetValue(patchdata, out var entitypatches))
					{
						assettext = EntityPatcher.Patch(assettext, entitypatches);
					}

					var textasset = new TextAsset(UnityEngine.TextAsset.CreateOptions.CreateNativeObject, assettext);

					GameData.ResourceManager.completeAssetDic[patchdata] = textasset;
					patchToRemove.Add(patchdata);
				}
			}

			AssetsToPatch = AssetsToPatch.Except(patchToRemove).ToList();
		}
		private void ProcessGameState()
		{
			//var stateTrackerState = Last.Management.GameStates.Boot;
			var currentState = Last.Management.GameStates.Boot;

			if (GameData.StateTracker != null)
			{
				currentState = GameData.StateTracker.CurrentState;
				//InternalLogger.LogInfo($"State: {GameData.StateTracker.CurrentState}");
			}

			if (currentState == GameStates.Title && GameState == GameStates.InGame)
			{
				ProcessState = ProcessStates.Reset;
			}
			else if(currentState == GameStates.Splash && (GameState == GameStates.InGame || GameState == GameStates.Title))
			{
				ProcessState = ProcessStates.ResetAndInitGame;
			}

			nowLoading = LoadingState != SystemIndicator.Mode.kNone;

			GameState = nowLoading ? GameStates.None : currentState;
		}

		private void ProcessItemQueue()
		{
			if (GameState == GameStates.InGame && SessionManager.Data.ItemsQueue.Any())
			{
				(string name, bool message) item = SessionManager.Data.ItemsQueue.First();
				var handleResult = Patches.GiveItem(item.name, item.message);

				switch (handleResult)
				{
					case Patches.ItemResults.Success:
						SessionManager.Data.ItemsQueue.RemoveAt(0);
						break;
					case Patches.ItemResults.Busy:
						break;
					case Patches.ItemResults.Invalid:
						SessionManager.Data.ItemsQueue.RemoveAt(0);
						break;
				}
			}
		}
		private void ProcessJobQueue()
		{
			if (GameState == GameStates.InGame && JobQueue.Any())
			{
				if (GameData.StateTracker != null)
				{
					var subState = GameData.StateTracker.CurrentSubState;
					if(subState != GameSubStates.InGame_Battle_Start && subState != GameSubStates.InGame_Battle_InProgress)
					{
						foreach (var job in JobQueue)
						{
							Randomizer.AddJobs(job);
						}

						JobQueue.Clear();
					}
				}
			}
		}
		private void ProcessFlags()
		{
			var itemList = GameData.UserData.GetImportantOwnedItemsClone();

			foreach (var item in itemList)
			{
				if (item.Content != null)
				{
					InternalLogger.LogInfo($"Flag Process: {item.Content.Id} - {item.Content.MesIdName} - {item.ContentId} - {item.NaturalName}");
				}

				if (Randomizer.ItemIdToFlag.TryGetValue(item.Id, out var flag))
				{
					GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, flag, 1);

					InternalLogger.LogTesting($"Flag {flag} set by {(Items)item.Id}");
				}

				Randomizer.ProcessSpecialItems(item.Id);
			}

			Patches.UpdateEntities();
		}
		private void ProcessApDataStorage()
		{
			foreach (var gameevent in Randomizer.DataStorageFlags)
			{
				Archipelago.instance.UpdateDataStorage(gameevent.Value, GameData.DataStorage.Get(gameevent.Key.type, gameevent.Key.flag) == 1);
			}
		}
	}

	public class Monitor : MonoBehaviour
	{
		public static Monitor instance { get; set; }

		public MonitorTool tool;

        public void Start() {
			tool = new MonitorTool();
			InternalLogger.LogInfo($"Monitor tool started.");
		}
        public void Update() {
            tool.Update();
        }
        public void OnDestroy() {

        }
		public void SetProcess(int process)
		{
			tool.ProcessState = (ProcessStates)process;
		}
		public void SetLoadingState(SystemIndicator.Mode mode)
		{
			tool.LoadingState = mode;
		}
		public int GetGameState()
		{
			return (int)tool.GameState;
		}
		public void SetMainMenuState(Last.Defaine.MenuCommandId state)
		{
			tool.MainMenuState = state;
			InternalLogger.LogTesting($"MainMenu: {state}");
		}
		public void SetMainState(State state)
		{
			tool.MainState = state;
		}
		public void AddPatchesToProcess(string address)
		{
			tool.AssetsToPatch.Add(address);
		}
		public void QueueJob(int jobId)
		{ 
			tool.JobQueue.Add(jobId);
		}

		public void SetShipWarp()
		{
			tool.ShipWarp = true;
		}
		public void SetCanoeWarp()
		{
			tool.CanoeWarp = true;
		}
	}

}
