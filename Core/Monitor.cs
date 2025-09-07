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
using Last.Management;

namespace FF1PRAP
{	public enum ProcessStates
	{ 
		Reset,
		InitGame,
		LoadGame,
		NewGame,
		ResetAndInitGame,
		None
	}
	public class MonitorTool
	{
		public GameStates GameState = GameStates.Title;
		public ProcessStates ProcessState = ProcessStates.InitGame;
		public SystemIndicator.Mode LoadingState = SystemIndicator.Mode.kNone;
		public Last.Defaine.MenuCommandId MainMenuState = Last.Defaine.MenuCommandId.Non;
		private bool newGameProcessed = false;
		private bool nowLoading = false;
		public List<string> AssetsToPatch = new();
		public MonitorTool() { }

		public void Update()
		{
			ProcessGameState();
			ProcessPatches();

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
				ProcessState = ProcessStates.None;

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
				ProcessState = ProcessStates.None;

				if (SessionManager.GameMode == GameModes.Archipelago)
				{
					Archipelago.instance.RestoreState();
					InternalLogger.LogInfo($"Loading saved randomization data.");
					if (!Randomizer.Load(SessionManager.FolderPath, "ap_" + SessionManager.Data.Player + "_" + SessionManager.Data.WorldSeed))
					{
						InternalLogger.LogInfo($"File not found, generating randomization data.");
						Randomizer.Randomize();
					}

					Initialization.ApplyRandomizedFeatures(Randomizer.Data);
				}
				else
				{
					if (!Randomizer.Load(SessionManager.FolderPath, SessionManager.Data.Seed + "_" + SessionManager.Data.Hashstring))
					{
						InternalLogger.LogInfo($"File not found, gameplay might be unstable. Generate a game first in the Solo Randomizer Settings Menu.");
					}
					
					//FF1PR.PlacedItems = Randomizer.Data.PlacedItems;
					Initialization.ApplyRandomizedFeatures(Randomizer.Data);
				}
			}
		}
		/*
		private void ProcessMapData()
		{
			foreach (var mapdata in MapDataUpdate)
			{
				if (mapdata.Value)
				{
					if (GameData.ResourceManager.completeAssetDic.ContainsKey(mapdata.Key))
					{

						var assettext = GameData.ResourceManager.completeAssetDic[mapdata.Key].Cast<TextAsset>().text;
						var assetnameparts = mapdata.Key.Split('/');
						var assetname = assetnameparts.Last();
						var filename = assetnameparts[assetnameparts.Count() - 2] + "/" + assetnameparts[assetnameparts.Count() - 1];
						InternalLogger.LogInfo($"MapPatcher: Patching {filename}.");

						assettext = MapPatcher.Patch(assettext, MapDataPatches[mapdata.Key]);

						//var assetfile = MapPatcher.Patch(, 0, MapPatches.Westward, 256, 256);
						var textasset = new TextAsset(UnityEngine.TextAsset.CreateOptions.CreateNativeObject, assettext);
						
						GameData.ResourceManager.completeAssetDic[mapdata.Key] = textasset;
						MapDataUpdate[mapdata.Key] = false;
					}
				}
			}
		}*/
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
					InternalLogger.LogInfo($"MapPatcher: Patching {filename}.");


					if (Randomizer.MapAssetsToPatch.TryGetValue(patchdata, out var mappatches))
					{
						assettext = MapPatcher.Patch(assettext, mappatches);
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
		/*
		public void CheckForMap(string address)
		{
			if (tool.MapDataUpdate.TryGetValue(address, out bool result))
			{
				if (!result)
				{
					tool.MapDataUpdate[address] = true;
				}
			}
		}*/
		public void AddPatchesToProcess(string address)
		{
			tool.AssetsToPatch.Add(address);
		}
	}

}
