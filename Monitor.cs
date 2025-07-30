using Archipelago.MultiClient.Net.Helpers;
using Last.Interpreter;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FF1PRAP
{

	public class MonitorTool
	{
		public MonitorTool()
		{
			counter = 0;
			FF1PR.SessionManager.GameMode = GameModes.Archipelago;
		}

		private int counter;
		
		public void Update()
		{

			//InternalLogger.LogInfo($"State: {FF1PR.StateTracker.CurrentState} - {FF1PR.StateTracker.CurrentSubState}");
			if (FF1PR.SessionManager.GameMode == GameModes.Vanilla)
			{
				return;
			}

			if (FF1PR.StateTracker.CurrentState == Last.Management.GameStates.Title && FF1PR.SessionManager.GameState == GameStates.InGame)
			{
				FF1PR.SessionManager.GameState = GameStates.Title;
				FF1PR.OwnedItemsClient = null;
				if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
				{
					//Archipelago.instance.Disconnect();
				}
			}

			if (FF1PR.SessionManager.GameState == GameStates.Title)
			{
				Initialization.InitializeRando();
			}
			else if (FF1PR.SessionManager.GameState == GameStates.NewGame)
			{
				FF1PR.SessionManager.GameState = GameStates.WaitingForStart;
				Initialization.InitializeNewGame();
				if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
				{
					// connection data was set with menu or host file???
					/*
					FF1PR.SessionManager.SetValue("archipelago", true);
					FF1PR.SessionManager.SetValue("player", "testplayer");
					FF1PR.SessionManager.SetValue("port", "51186");
					FF1PR.SessionManager.SetValue("server", "archipelago.gg");
					FF1PR.SessionManager.SetValue("password", "");
					FF1PR.SessionManager.SetValue("itemindex", 0);
					Archipelago.instance.Connect();*/
				}
				else
				{
					var seed = (uint)System.DateTime.Now.Ticks;
					FF1PR.SessionManager.SetValue("seed", seed.ToString());
					FF1PR.PlacedItems = Randomizer.DoItemPlacement(seed);
					FF1PR.SessionManager.SetPlacedItems(FF1PR.PlacedItems);
					Initialization.InitializeRandoItems();
				}
			}
			else if (FF1PR.SessionManager.GameState == GameStates.LoadGame)
			{
				FF1PR.SessionManager.GameState = GameStates.WaitingForStart;

				if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
				{

					Archipelago.instance.RestoreState();
					//Archipelago.instance.Disconnect();
					//Archipelago.instance.Connect();
				}
				else
				{
					FF1PR.PlacedItems = FF1PR.SessionManager.GetPlacedItems();
					Initialization.InitializeRandoItems();
				}
			}
			else if (FF1PR.StateTracker.CurrentState == Last.Management.GameStates.InGame && FF1PR.SessionManager.GameState == GameStates.WaitingForStart)
			{
				FF1PR.SessionManager.GameState = GameStates.InGame;
			}
		}
	}

	public class Monitor : MonoBehaviour
	{
		public static Monitor instance { get; set; }

		public MonitorTool tool;

        public void Start() {
			InternalLogger.LogInfo($"Monitor tool started.");
			tool = new MonitorTool();
        }

        public void Update() {
            tool.Update();
        }

        public void OnDestroy() {

        }
    }
}
