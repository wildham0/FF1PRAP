using Archipelago.MultiClient.Net.Helpers;
using Last.Interpreter;
using Last.Systems.Indicator;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Last.Message;
using static UnityEngine.GridBrushBase;
using Il2CppSystem;
using Last.Data.Master;
using Last.UI.KeyInput;

namespace FF1PRAP
{
	public enum GameStates
	{
		Title,
		Loading,
		InGame
	}
	public enum ProcessStates
	{ 
		Reset,
		InitGame,
		LoadGame,
		NewGame,
		None
	}
	public class AssetTask
	{
		public string Name;
		public bool Ready;
		public bool Done;
		public Last.Management.ResourceLoadTask Task;
		public TextAsset Asset;

		public AssetTask(string name, Last.Management.ResourceLoadTask task, TextAsset asset)
		{
			Name = name;
			Ready = false;
			Task = task;
			Asset = asset;
			Done = false;
		}
	}
	public class MonitorTool
	{

		public GameStates GameState = GameStates.Title;
		public ProcessStates ProcessState = ProcessStates.InitGame;
		public SystemIndicator.Mode LoadingState = SystemIndicator.Mode.kNone;
		public Last.Defaine.MenuCommandId MainMenuState = Last.Defaine.MenuCommandId.Non;
		//public SaveWindowController.State SaveMenuState = SaveWindowController.State.None;
		private bool newGameProcessed = false;

		public List<AssetTask> tasksToMonitor;
		public MonitorTool()
		{
			tasksToMonitor = new();
		}

		public void Update()
		{
			ProcessGameState();

			if (FF1PR.SessionManager.GameMode == GameModes.Vanilla)
			{
				return;
			}

			if (ProcessState == ProcessStates.Reset)
			{
				ProcessState = ProcessStates.None;
				FF1PR.OwnedItemsClient = null;
				newGameProcessed = false;
			}
			else if (GameState == GameStates.Title && ProcessState == ProcessStates.InitGame)
			{
				Initialization.ApplyBaseGameModifications();
				ProcessState = ProcessStates.None;
			}
			else if (ProcessState == ProcessStates.NewGame)
			{
				ProcessState = ProcessStates.None;
				if (!newGameProcessed)
				{
					newGameProcessed = true;
					Initialization.InitializeNewGame();
					if (FF1PR.SessionManager.GameMode == GameModes.Randomizer)
					{
						Initialization.InitializeRandoItems(Randomizer.RandomizerData);
						Initialization.ApplyRandomizedFeatures(Randomizer.RandomizerData);
					}
					else if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
					{
						Randomizer.RandomizerData = new();
						InternalLogger.LogInfo($"Loading saved randomization data.");
						if (!Randomizer.RandomizerData.Load(FF1PR.SessionManager.folderPath, "ap_" + FF1PR.SessionManager.Data.Player + "_" + FF1PR.SessionManager.Data.WorldSeed))
						{
							InternalLogger.LogInfo($"File not found, generating randomization data.");
							Randomizer.ArchipelagoRandomize(FF1PR.SessionManager.Data.Player + FF1PR.SessionManager.Data.WorldSeed);
						}
						Initialization.ApplyRandomizedFeatures(Randomizer.RandomizerData);
					}
				}
			}
			else if (ProcessState == ProcessStates.LoadGame)
			{
				ProcessState = ProcessStates.None;

				if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
				{
					Archipelago.instance.RestoreState();
					Randomizer.RandomizerData = new();
					InternalLogger.LogInfo($"Loading saved randomization data.");
					if (!Randomizer.RandomizerData.Load(FF1PR.SessionManager.folderPath, "ap_" + FF1PR.SessionManager.Data.Player + "_" + FF1PR.SessionManager.Data.WorldSeed))
					{
						InternalLogger.LogInfo($"File not found, generating randomization data.");
						Randomizer.ArchipelagoRandomize(FF1PR.SessionManager.Data.Player + FF1PR.SessionManager.Data.WorldSeed);
					}

					Initialization.ApplyRandomizedFeatures(Randomizer.RandomizerData);
				}
				else
				{
					Randomizer.RandomizerData = new();
					if (!Randomizer.RandomizerData.Load(FF1PR.SessionManager.folderPath, FF1PR.SessionManager.Data.Seed + "_" + FF1PR.SessionManager.Data.Hashstring))
					{
						InternalLogger.LogInfo($"File not found, gameplay might be unstable. Generate a game first in the Solo Randomizer Settings Menu.");
					}
					
					FF1PR.PlacedItems = Randomizer.RandomizerData.PlacedItems;
					Initialization.InitializeRandoItems(Randomizer.RandomizerData);
					Initialization.ApplyRandomizedFeatures(Randomizer.RandomizerData);
				}
			}
		}
		private void ProcessGameState()
		{
			var stateTrackerState = Last.Management.GameStates.Boot;

			if (FF1PR.StateTracker != null)
			{
				stateTrackerState = FF1PR.StateTracker.CurrentState;
			}

			if (FF1PR.StateTracker.CurrentState == Last.Management.GameStates.Title && GameState == GameStates.InGame)
			{
				ProcessState = ProcessStates.Reset;
				GameState = GameStates.Title;
			}
			else if (LoadingState != SystemIndicator.Mode.kNone)
			{
				GameState = GameStates.Loading;
			}
			else if(LoadingState == SystemIndicator.Mode.kNone && stateTrackerState == Last.Management.GameStates.Title)
			{
				GameState = GameStates.Title;
			}
			else if (LoadingState == SystemIndicator.Mode.kNone && stateTrackerState == Last.Management.GameStates.InGame)
			{
				GameState = GameStates.InGame;
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
			InternalLogger.LogInfo($"MainMenu: {state}");
		}
	}

}
