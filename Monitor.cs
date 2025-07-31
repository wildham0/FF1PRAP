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
	public class MonitorTool
	{
		public MonitorTool()
		{
			counter = 0;
			FF1PR.SessionManager.GameMode = GameModes.Archipelago;
		}

		private Dictionary<string, float> timers;

		private int counter;

		private bool windowOpen = false;
		//private DateTime timeWindowOpened;

		public GameStates GameState = GameStates.Title;
		public ProcessStates ProcessState = ProcessStates.InitGame;
		public SystemIndicator.Mode LoadingState = SystemIndicator.Mode.kNone;
		private bool newGameProcessed = false;
		public void Update()
		{
			ProcessGameState();
			//ProcessWindow();

			//InternalLogger.LogInfo($"Loading... {SystemIndicator.instance.isActiveAndEnabled}, {SystemIndicator.instance.name}, {SystemIndicator.instance.hideFlags}, {SystemIndicator.instance.text}");

			//InternalLogger.LogInfo($"State: {FF1PR.StateTracker.CurrentState} - {FF1PR.StateTracker.CurrentSubState}");
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
						var seed = (uint)System.DateTime.Now.Ticks;
						FF1PR.SessionManager.SetValue("seed", seed.ToString());
						FF1PR.PlacedItems = Randomizer.DoItemPlacement(seed);
						FF1PR.SessionManager.SetPlacedItems(FF1PR.PlacedItems);
						Initialization.InitializeRandoItems();
					}
				}
			}
			else if (ProcessState == ProcessStates.LoadGame)
			{
				ProcessState = ProcessStates.None;
				if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
				{
					Archipelago.instance.RestoreState();
				}
				else
				{
					FF1PR.PlacedItems = FF1PR.SessionManager.GetPlacedItems();
					Initialization.InitializeRandoItems();
				}
			}
		}

		private void ProcessWindow()
		{
			if (windowOpen)
			{
				/*
				if (DateTime.Now > timeWindowOpened + TimeSpan.FromSeconds(2f))
				{
					MessageWindowManager.instance.Close();
					windowOpen = false;
				}*/
			}
		}

		public void OpenItemWindow(string text)
		{
			Il2CppSystem.Collections.Generic.List<Last.Systems.Message.BaseContent> parsedText = Last.Systems.Message.MessageParser.Parse(text);
			MessageWindowManager.instance.SetContent(parsedText);
			MessageWindowManager.instance.SetAnchor(Last.Management.WindowType.Battle, Last.Management.WindowAnchor.Under);
			MessageWindowManager.instance.ShowWindow(true);
			MessageWindowManager.instance.Play();

			windowOpen = true;
			//timeWindowOpened = DateTime.Now;
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
			InternalLogger.LogInfo($"Monitor tool started.");
			tool = new MonitorTool();
        }
        public void Update() {
            tool.Update();
        }
        public void OnDestroy() {

        }
		public void SetProcess(ProcessStates process)
		{
			tool.ProcessState = process;
		}
		public void SetLoadingState(SystemIndicator.Mode mode)
		{
			tool.LoadingState = mode;
		}
		public GameStates GetGameState()
		{
			return tool.GameState;
		}

		public void OpenItemWindow(string text)
		{
			tool.OpenItemWindow(text);
		}
    }

	public class ApItemWindowManager
	{
		private List<string> queuedMessage;

		private bool windowOpen = false;
		private System.DateTime timeWindowOpened;
		private System.DateTime timeWindowClosed;
		public MessageWindowController exclusiveController;
		public ApItemWindowManager()
		{
			queuedMessage = new();
			//exclusiveController = MessageWindowManager.instance.currentWindowController;
		}
		public void Update()
		{
			SimpleProcess();
		}

		public void QueueMessage(string text)
		{
			queuedMessage.Add(text);
		}
		private void ProcessWindow()
		{
			
			if (MessageWindowManager.instance == null)
			{
				return;
			}
			/*else if (exclusiveController == null)
			{
				exclusiveController = MessageWindowManager.instance.battleWindowController;
			}
			else if (exclusiveController != null)
			{
				MessageWindowManager.instance.currentWindowController = exclusiveController;
			}

			var backupController = MessageWindowManager.instance.currentWindowController;*/

			if (windowOpen)
			{
				// Interrupted window
				if (!MessageWindowManager.instance.IsOpen())
				{
					windowOpen = false;
					timeWindowClosed = System.DateTime.Now;
				}
				else if (System.DateTime.Now > timeWindowOpened + System.TimeSpan.FromSeconds(2f))
				{/*
					bool isFieldWindow = MessageWindowManager.instance.isFieldWindow;

					if (isFieldWindow)
					{
						MessageWindowManager.instance.SetWindowType(Last.Management.WindowType.Battle);
					}
					*/
					MessageWindowManager.instance.Close();
					//MessageWindowManager.instance.EndWaitExit();
					windowOpen = false;
					timeWindowClosed = System.DateTime.Now;
					/*
					if (isFieldWindow)
					{
						MessageWindowManager.instance.SetWindowType(Last.Management.WindowType.Field);
					}*/
				}
			}
			else if (queuedMessage.Any() && !MessageWindowManager.instance.IsOpen() && System.DateTime.Now > timeWindowClosed + System.TimeSpan.FromSeconds(1f))
			{
				string nextMessage = queuedMessage.First();
				queuedMessage.RemoveAt(0);

				OpenItemWindow(nextMessage);
			}
			



			//MessageWindowManager.instance.currentWindowController = backupController;
		}

		private void SimpleProcess()
		{

			if (queuedMessage.Any() && !MessageWindowManager.instance.IsOpen())
			{
				string nextMessage = queuedMessage.First();
				queuedMessage.RemoveAt(0);

				OpenItemWindow(nextMessage);
			}
		}
		public void OpenItemWindow(string text)
		{
			//var backupController = MessageWindowManager.instance.currentWindowController;
			//MessageWindowManager.instance.currentWindowController = exclusiveController;

			//Il2CppSystem.Collections.Generic.List<Last.Systems.Message.BaseContent> parsedText = Last.Systems.Message.MessageParser.Parse(text);
			var parsedText = Last.Systems.Message.MessageParser.Parse(text);

			//MessageWindowManager.instance.
			//MessageWindowManager.instance.Initialize();
			//MessageWindowManager.instance.currentWindowController.Reset();

			//MessageWindowManager.instance.currentWindowController = exclusiveController;
			//bool isFieldWindow = MessageWindowManager.instance.isFieldWindow;

			InternalLogger.LogInfo($"Window inst: {MessageWindowManager.instance.currentWindowController.GetInstanceID()}");
			
			MessageWindowManager.instance.SetContent(parsedText);
			//MessageWindowManager.instance.ShowWindow(false);
			MessageWindowManager.instance.SetAnchor(Last.Management.WindowType.Battle, Last.Management.WindowAnchor.Under);
			MessageWindowManager.instance.SetWindowType(Last.Management.WindowType.Battle);
			//MessageWindowManager.instance.Set(Last.Management.WindowType.Battle);
			MessageWindowManager.instance.ShowWindow(true);
			MessageWindowManager.instance.Create();

			MessageWindowManager.instance.Play();
			/*
			if (isFieldWindow)
			{
				MessageWindowManager.instance.SetWindowType(Last.Management.WindowType.Field);
			}*/

			/*parsedText = Last.Systems.Message.MessageParser.Parse(text + "2");
			MessageWindowManager.instance.SetContent(parsedText);
			MessageWindowManager.instance.Play();*/
			//parsedText = Last.Systems.Message.MessageParser.Parse(text + "3");
			//MessageWindowManager.instance.SetContent(parsedText);

			windowOpen = true;
			timeWindowOpened = System.DateTime.Now;

			//MessageWindowManager.instance.currentWindowController = backupController;
		}
	}
	public class ApItemWindow : MonoBehaviour
	{
		public static ApItemWindow instance { get; set; }
		//public static MessageWindowManager windowManager { get; set; }

		public ApItemWindowManager windowManager;
		public void Start()
		{
			InternalLogger.LogInfo($"Ap Item Window started.");
			//GameObject apItemWindowObject = new GameObject("apItemWindow");
			windowManager = new ApItemWindowManager();
			/*
			var windowController = GameObject.FindObjectOfType<MessageWindowController>();
			windowManager.fieldWindowController = windowController;*/
		}
		public void Update()
		{
			windowManager.Update();

			//var backedInstance = MessageWindowManager.instance;
			//MessageWindowManager.instance = windowManager;
			//ProcessWindow();
			//windowMan.
			//MessageWindowManager.instance.Update();
			//MessageWindowManager.instance = backedInstance;
		}
		public void QueueMessage(string text)
		{
			windowManager.QueueMessage(text);
		}
		public void OnDestroy()
		{

		}


		/*
		public void OpenItemWindow(string text)
		{
			//Il2CppSystem.Collections.Generic.List<Last.Systems.Message.BaseContent> parsedText = Last.Systems.Message.MessageParser.Parse(text);
			var parsedText = Last.Systems.Message.MessageParser.Parse(text);
			
			if (windowManager != null)
			{
				InternalLogger.LogInfo($"Manager {windowManager.currentWindowController.Pointer} - {MessageWindowManager.instance.currentWindowController.Pointer}");
			}
			else
			{
				InternalLogger.LogInfo($"ParsedText was null");
			}

			//var backedInstance = MessageWindowManager.instance;
			//MessageWindowManager.instance = windowManager;

			//parsedText.Pointer

			//Il2CppSystem.Collections.Generic.List<Last.Systems.Message.BaseContent>
			//List<Last.Systems.Message.BaseContent> parsedText = new();
			//parsedText.Add(new Last.Systems.Message.TextContent(text));
			//windowManager.Create();
			//windowManager.CreateState();
			//windowManager.Create();
			//windowManager.PlayInit();
			//windowManager.Initialize();
			//windowManager.NewPageInit();
			//windowManager.PlayingInit();
			//windowManager.instan
			
			windowManager.SetContent(parsedText);
			windowManager.SetAnchor(Last.Management.WindowType.Battle, Last.Management.WindowAnchor.Under);
			windowManager.ShowWindow(true);
			windowManager.Play();

			windowOpen = true;
			timeWindowOpened = System.DateTime.Now;

			//MessageWindowManager.instance = backedInstance;
		}*/
	}
}
