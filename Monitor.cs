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
		public SaveWindowController.State SaveMenuState = SaveWindowController.State.None;
		private bool newGameProcessed = false;

		public List<AssetTask> tasksToMonitor;
		public MonitorTool()
		{
			//FF1PR.SessionManager.GameMode = GameModes.Archipelago;
			tasksToMonitor = new();
		}

		public void Update()
		{
			ProcessGameState();
			ProcessAssets();

			/*
			if(FF1PR.MapManager != null && FF1PR.MapManager.currentMapModel != null)
			{
				InternalLogger.LogInfo($"Map: {FF1PR.MapManager.currentMapModel.GetMapName()} - {FF1PR.MapManager.IsAllCompleted()}");
			}

			if (FF1PR.ScriptIntegrator != null)
			{
				InternalLogger.LogInfo($"Map: {FF1PR.ScriptIntegrator.scriptName} - {FF1PR.ScriptIntegrator.working}");
			}*/


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
				Initialization.InitializeRando();
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
						//var seed = (uint)System.DateTime.Now.Ticks;
						//FF1PR.SessionManager.SetValue("seed", seed.ToString());
						//FF1PR.PlacedItems = Randomizer.DoItemPlacement(seed);
						//FF1PR.SessionManager.SetRandomizedGame(FF1PR.PlacedItems);
						Initialization.InitializeRandoItems(Randomizer.RandomizerData);
					}
				}
			}
			else if (ProcessState == ProcessStates.LoadGame)
			{
				ProcessState = ProcessStates.None;
				//var test = GameObject.Find("save info");
				//test.enabled = false;

				if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
				{
					Archipelago.instance.RestoreState();
					RandomizerData randoData = new();
					randoData.Load(FF1PR.SessionManager.folderPath, "ap_" + FF1PR.SessionManager.Data.Player + "_" + FF1PR.SessionManager.Data.WorldSeed);
				}
				else
				{
					RandomizerData randoData = new();
					randoData.Load(FF1PR.SessionManager.folderPath, FF1PR.SessionManager.Data.Seed + "_" + FF1PR.SessionManager.Data.Hashstring);

					//FF1PR.PlacedItems = FF1PR.SessionManager.GetPlacedItems();
					FF1PR.PlacedItems = randoData.PlacedItems;
					Initialization.InitializeRandoItems(randoData);
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

		private void ProcessAssets()
		{

			//tasksToMonitor = tasksToMonitor.Where(t => !t.Done).ToList();

			foreach (var task in tasksToMonitor.Where(t => !t.Done).ToList())
			{
				task.Ready = task.Task.CheckComplete();
				
				if (task.Ready)
				{
					var asset = FF1PR.ResourceManager.GetAsset<TextAsset>(task.Name);
					if (asset != null)
					{
						FF1PR.ResourceManager.completeAssetDic[task.Name] = task.Asset;
						task.Done = true;
						InternalLogger.LogInfo($"Asset {task.Name} modified.");
					}
				}
				InternalLogger.LogInfo($"AssetTask: {task.Ready}");
			}
		}
		public bool IsTaskDone(string assetName)
		{
			if (tasksToMonitor.TryFind(t => t.Name == assetName, out var task))
			{
				return task.Done;
			}
			else
			{
				return true;
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

		public void SetAssetTask(string name, TextAsset asset, Last.Management.ResourceLoadTask task)
		{
			tool.tasksToMonitor.Add(new AssetTask(name, task, asset));
		}
		public int GetGameState()
		{
			return (int)tool.GameState;
		}

		public bool IsTaskDone(string assetName)
		{
			return tool.IsTaskDone(assetName);
		}

		public void SetMainMenuState(Last.Defaine.MenuCommandId state)
		{
			tool.MainMenuState = state;
			InternalLogger.LogInfo($"MainMenu: {state}");
		}

		public void SetSaveMenuState(SaveWindowController.State state)
		{
			tool.SaveMenuState = state;
			InternalLogger.LogInfo($"SaveMenu: {state}");
		}
	}

}
