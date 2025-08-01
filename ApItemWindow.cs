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
	public enum WindowStates
	{ 
		Closed,
		Open,
		Suspended,
	}
	public enum ForeignWindowStates
	{
		Closed,
		Open,
		InTransit
	}
	public class ApItemWindowManager
	{
		private List<string> queuedMessage;

		private WindowStates windowState = WindowStates.Closed;
		private ForeignWindowStates foreignWindow = ForeignWindowStates.Closed;
		private bool suspendedFromState = false;
		private string currentMessage = "";
		private System.TimeSpan timeLeft;
		private System.DateTime timeWindowOpened;
		private System.DateTime timeWindowClosed;
		private System.DateTime timeForeignWindowClosed;

		private static List<Last.Management.GameSubStates> validSubstates = new()
		{
			Last.Management.GameSubStates.InGame_Field,
		};
		public ApItemWindowManager()
		{
			queuedMessage = new();
		}
		public void Update()
		{
			ProcessWindow();
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

			// Process Foreign Window State
			bool notBattleWindow = MessageWindowManager.instance.currentWindowController != MessageWindowManager.instance.battleWindowController;
			if (notBattleWindow && MessageWindowManager.instance.IsOpen())
			{
				foreignWindow = ForeignWindowStates.Open;
			}
			else if (!MessageWindowManager.instance.IsOpen() && foreignWindow == ForeignWindowStates.Open)
			{
				timeForeignWindowClosed = System.DateTime.Now;
				foreignWindow = ForeignWindowStates.InTransit;
			}
			else if (foreignWindow == ForeignWindowStates.InTransit && (System.DateTime.Now > timeForeignWindowClosed + System.TimeSpan.FromSeconds(0.3f)))
			{
				foreignWindow = ForeignWindowStates.Closed;
			}

			// Process Item Window
			if (windowState == WindowStates.Open)
			{
				// Interrupted window
				if (foreignWindow == ForeignWindowStates.Open || !validSubstates.Contains(FF1PR.StateTracker.CurrentSubState))
				{
					MessageWindowManager.instance.battleWindowController.Close();
					timeLeft = (timeWindowOpened + System.TimeSpan.FromSeconds(3f)) - System.DateTime.Now;

					windowState = WindowStates.Suspended;
					suspendedFromState = !validSubstates.Contains(FF1PR.StateTracker.CurrentSubState);
					//InternalLogger.LogInfo($"TimeLefT: {timeLeft.TotalSeconds}");

				}
				else if (System.DateTime.Now > timeWindowOpened + timeLeft)
				{
					//InternalLogger.LogInfo($"Current inst: {MessageWindowManager.instance.currentWindowController.GetInstanceID()}");

					MessageWindowManager.instance.Close();
					windowState = WindowStates.Closed;
					timeWindowClosed = System.DateTime.Now;
				}
			}
			else if (windowState == WindowStates.Suspended)
			{
				if (foreignWindow == ForeignWindowStates.Closed && validSubstates.Contains(FF1PR.StateTracker.CurrentSubState))
				{
					//InternalLogger.LogInfo($"Unsuspendending: {timeLeft.TotalSeconds}");
					windowState = WindowStates.Open;
					timeWindowOpened = System.DateTime.Now;

					if (suspendedFromState)
					{
						OpenItemWindow(currentMessage);
					}
					else
					{
						//MessageWindowManager.instance.currentWindowController.GetInstanceID();
						//InternalLogger.LogInfo($"From talk - Cur {MessageWindowManager.instance.currentWindowController.GetInstanceID()} vs Bat {MessageWindowManager.instance.battleWindowController.GetInstanceID()}");
						MessageWindowManager.instance.SetAnchor(Last.Management.WindowType.Battle, Last.Management.WindowAnchor.Under);
						MessageWindowManager.instance.SetWindowType(Last.Management.WindowType.Battle);
						MessageWindowManager.instance.ShowWindow(true);
						MessageWindowManager.instance.Play();
						//InternalLogger.LogInfo($"Cuurrent inst: {MessageWindowManager.instance.currentWindowController.GetInstanceID()}");
					}
				}
			}
			else if (windowState == WindowStates.Closed)
			{
				if (validSubstates.Contains(FF1PR.StateTracker.CurrentSubState) && foreignWindow == ForeignWindowStates.Closed)
				{
					if (queuedMessage.Any() && System.DateTime.Now > timeWindowClosed + System.TimeSpan.FromSeconds(1.5f))
					{
						timeLeft = System.TimeSpan.FromSeconds(3f);
						windowState = WindowStates.Open;

						currentMessage = queuedMessage.First();
						queuedMessage.RemoveAt(0);

						OpenItemWindow(currentMessage);

						windowState = windowState = WindowStates.Open;
						timeWindowOpened = System.DateTime.Now;
					}
				}
			}
		}
		public void OpenItemWindow(string text)
		{
			var parsedText = Last.Systems.Message.MessageParser.Parse(text);

			InternalLogger.LogInfo($"Opening Ap Item Window for: {text}");
			
			MessageWindowManager.instance.SetContent(parsedText);
			MessageWindowManager.instance.SetAnchor(Last.Management.WindowType.Battle, Last.Management.WindowAnchor.Under);
			MessageWindowManager.instance.SetWindowType(Last.Management.WindowType.Battle);
			MessageWindowManager.instance.ShowWindow(true);
			MessageWindowManager.instance.Play();
		}
	}
	public class ApItemWindow : MonoBehaviour
	{
		public static ApItemWindow instance { get; set; }

		public ApItemWindowManager windowManager;
		public void Start()
		{
			InternalLogger.LogInfo($"Ap Item Window started.");
			windowManager = new ApItemWindowManager();
		}
		public void Update()
		{
			windowManager.Update();
		}
		public void QueueMessage(string text)
		{
			windowManager.QueueMessage(text);
		}
		public void OnDestroy()
		{

		}
	}
}
