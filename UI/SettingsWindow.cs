//using Il2CppSystem;
using BepInEx.Core.Logging.Interpolation;
using FF1PRAP.UI;
using Last.Battle;
using Last.UI.KeyInput;
using MonoMod.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem.PlaybackState;


namespace FF1PRAP
{
	public partial class SettingsWindow : MonoBehaviour
	{
		// Assets
		public static Font PixelRemasterFont;
		public static Texture2D windowTexture;

		// State
		private static bool ShowAPSettingsWindow = false;
		private static bool showPort = false;
		private static bool showPassword = false;
		private static string stringToEdit = "";
		private static int stringCursorPosition = 0;
		private static string currentOptionShowing = "";
		private static Option currentToolTip;
		private static bool generationReady = false;
		public static Vector2 scrollPosition = Vector2.zero;

		// Parameters
		private static float guiScale = 1f;
		private static float apHeight = 0f;
		private static float lastApHeight = 0f;
		private static float apMargin = 20f;
		private static int screenWidth = 0;
		private static int screenHeight = 0;
		private static float standardFontSize = 15f;
		public static GUISkin windowSkin;
		public static Rect standardWindowRect;

		// Gamedata
		public static string CustomSeed = "";
		private static byte[] seed = new byte[4];

		private static Dictionary<GameModes, string> gameModeOption = new()
		{
			{ GameModes.Archipelago, "Archipelago" },
			{ GameModes.Randomizer, "Single Player" },
		};
		private static Dictionary<string, bool> editingFlags = new Dictionary<string, bool>() {
			{"Player", false},
			{"Hostname", false},
			{"Port", false},
			{"Password", false},
			{"Seed", false},
		};

		//Get a conenction setting value by fieldname
		private static string getConnectionSetting(string fieldName)
		{
			switch(fieldName)
			{
				case "Player":
					return FF1PR.SessionManager.Data.Player;
				case "Hostname":
					return FF1PR.SessionManager.Data.Host;
				case "Port":
					return FF1PR.SessionManager.Data.Port;
				case "Password":
					return FF1PR.SessionManager.Data.Password;
				case "Seed":
					return FF1PR.SessionManager.Data.Seed;
				default:
					return "";
			}
		}

		//Set a conenction setting value by fieldname
		private static void setConnectionSetting(string fieldName, string value)
		{
			switch (fieldName)
			{
				case "Player":
					FF1PR.SessionManager.Data.Player = value;
					return;
				case "Hostname":
					FF1PR.SessionManager.Data.Host = value;
					return;
				case "Port":
					FF1PR.SessionManager.Data.Port = value;
					return;
				case "Password":
					FF1PR.SessionManager.Data.Password = value;
					return;
				case "Seed":
					FF1PR.SessionManager.Data.Seed = value;
					try
					{
						seed = Convert.FromHexString(value);
					}
					catch { }
					
					return;
				default:
					return;
			}
		}

		//Place a visible cursor in a text label when editing the field
		private static string textWithCursor(string text, bool isEditing, bool showText)
		{
			string baseText = showText ? text : new string('*', text.Length);
			if (!isEditing) return baseText;
			if (stringCursorPosition > baseText.Length) stringCursorPosition = baseText.Length;
			return baseText.Insert(stringCursorPosition, "<color=#EAA614>|</color>");
		}

		//Clear all field editing flags (since we do this in a few places)
		private static void clearAllEditingFlags()
		{

			List<string> fieldKeys = new List<string>(editingFlags.Keys);
			foreach (string fieldKey in fieldKeys)
			{
				editingFlags[fieldKey] = false;
			}
		}

		//Initialize a text field for editing
		private static void beginEditingTextField(string fieldName)
		{
			if (editingFlags[fieldName]) return; //can't begin if we're already editing this field

			//check and finalize if another field was mid-edit
			List<string> fieldKeys = new List<string>(editingFlags.Keys);
			foreach (string fieldKey in fieldKeys)
			{
				if (editingFlags[fieldKey]) finishEditingTextField(fieldKey);
			}

			stringToEdit = getConnectionSetting(fieldName);
			stringCursorPosition = stringToEdit.Length;
			editingFlags[fieldName] = true;
		}

		//finalize editing a text field and save the changes
		private static void finishEditingTextField(string fieldName)
		{
			if (!editingFlags[fieldName]) return; //can't finish if we're not editing this field

			stringToEdit = "";
			stringCursorPosition = 0;
			FF1PR.SessionManager.WriteSessionInfo();
			editingFlags[fieldName] = false;
		}

		private static void handleEditButton(string fieldName)
		{
			if (editingFlags[fieldName])
			{
				finishEditingTextField(fieldName);
			}
			else
			{
				beginEditingTextField(fieldName);
			}
		}

		private static void handlePasteButton(string fieldName)
		{
			
			setConnectionSetting(fieldName, GUIUtility.systemCopyBuffer);
			if (editingFlags[fieldName])
			{
				stringToEdit = GUIUtility.systemCopyBuffer;
				finishEditingTextField(fieldName);
			}
			FF1PR.SessionManager.WriteSessionInfo();
		}

		private static void handleClearButton(string fieldName)
		{
			setConnectionSetting(fieldName, "");
			if (editingFlags[fieldName]) stringToEdit = "";
			FF1PR.SessionManager.WriteSessionInfo();
		}

		private static void handleRollButton(string fieldName)
		{
			var rng = new System.Random();
			rng.NextBytes(seed);
			var seedString = seed.ToHexadecimalString().PadLeft(8, '0');
			if (seedString == "00000000")
			{
				rng.NextBytes(seed);
				seedString = seed.ToHexadecimalString().PadLeft(8, '0');
			}
			setConnectionSetting(fieldName, seedString);
			if (editingFlags[fieldName]) stringToEdit = "";
			FF1PR.SessionManager.WriteSessionInfo();
		}

		private static void CalcGuiScale()
		{
			screenWidth = Screen.width;
			screenHeight = Screen.height;

			if (Screen.width == 3840 && Screen.height == 2160)
			{
				guiScale = 1.25f;
			}
			else if (Screen.width == 1280 && Screen.height <= 800)
			{
				guiScale = 0.75f;
			}
			else
			{
				guiScale = 1f;
			}
		}

		private void OnGUI()
		{
			bool showSettings = SceneManager.GetActiveScene().name == "Title" &&
				FF1PR.TitleWindowController != null &&
				(FF1PR.TitleWindowController.stateMachine.Current == TitleWindowController.State.None ||
				FF1PR.TitleWindowController.stateMachine.Current == TitleWindowController.State.Select);

			if (showSettings)
			{
				FF1PR.SessionManager.CurrentSlot = 0;

				//InternalLogger.LogInfo($"Title Window is here.");
				// initial draw or screen was resized, redraw texture
				if (screenWidth != Screen.width || screenHeight != Screen.height || windowTexture == null)
				{
					CalcGuiScale();
					standardWindowRect = new Rect(20f, (float)Screen.height * 0.12f, 430f * guiScale, 600f * guiScale);
					windowTexture = WindowTexture.GenerateWindowTexture(standardWindowRect);
					windowSkin = WindowTexture.CreateWindowSyle(windowTexture);

					if (PixelRemasterFont == null)
					{
						PixelRemasterFont = Resources.FindObjectsOfTypeAll<Font>().Where(f => f.name == "FOT-NewCezannePro-B").ToList()[0];
					}

					GUI.skin = windowSkin;
					GUI.skin.font = PixelRemasterFont;
				}

				Cursor.visible = true;
				GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.98f);
				GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

				// Show Main window
				switch (FF1PR.SessionManager.GameMode)
				{
					case GameModes.Randomizer:
						GUI.Window(101, standardWindowRect, new Action<int>(RandomizerSettingsWindow), "Single Player Randomizer Settings", GUI.skin.window);
						ShowAPSettingsWindow = false;
						break;
					case GameModes.Archipelago:
						GUI.Window(101, standardWindowRect, new Action<int>(ArchipelagoQuickSettingsWindow), "Archipelago Settings", GUI.skin.window);
						break;
				}

				// Show ToolTips
				if (currentToolTip != null)
				{
					Rect sideSettingsWindow = new Rect(460f * guiScale, standardWindowRect.y, standardWindowRect.width, standardWindowRect.height);
					GUI.Window(104, sideSettingsWindow, new Action<int>(ToolTipWindow), currentToolTip.Display);

					if (FF1PR.TitleWindowController != null)
					{
						FF1PR.TitleWindowController.SetEnableMenu(false);
					}
				}

				var versionStyle = new GUIStyle();
				GUI.backgroundColor = new Color(1f, 1f, 1f, 0f);

				versionStyle.alignment = TextAnchor.MiddleLeft;
				versionStyle.font = PixelRemasterFont;
				var blackColor = new Color(0f, 0f, 0f, 1.0f);
				versionStyle.normal.textColor = blackColor;
				versionStyle.active.textColor = blackColor;
				versionStyle.hover.textColor = blackColor;
				versionStyle.focused.textColor = blackColor;
				versionStyle.onActive.textColor = blackColor;
				versionStyle.onNormal.textColor = blackColor;
				versionStyle.onHover.textColor = blackColor;
				versionStyle.onFocused.textColor = blackColor;
				
				GUI.Window(105, new Rect(standardWindowRect.x + apMargin, standardWindowRect.y + standardWindowRect.height, 100f, 30f), new Action<int>(VersionWindow), "", versionStyle);
			}
		}

		private void Update()
		{
			//if ((FF1PR.SessionManager.GameMode == GameModes.Archipelago && ShowAPSettingsWindow) && SceneManager.GetActiveScene().name == "Title")
			if (editingFlags.Where(f => f.Value).Any() && SceneManager.GetActiveScene().name == "Title")
			{
				bool submitKeyPressed = false;

				//handle text input
				if (Input.anyKeyDown
					&& !Input.GetKeyDown(KeyCode.Return)
					&& !Input.GetKeyDown(KeyCode.Escape)
					&& !Input.GetKeyDown(KeyCode.Tab)
					&& !Input.GetKeyDown(KeyCode.Backspace)
					&& !Input.GetKeyDown(KeyCode.Delete)
					&& !Input.GetKeyDown(KeyCode.LeftArrow)
					&& !Input.GetKeyDown(KeyCode.RightArrow)
					&& Input.inputString != ""
					)
				{

					bool inputValid = true;

					//validation for any fields that require it
					if (editingFlags["Port"] && !int.TryParse(Input.inputString, out int num)) inputValid = false;
					if (editingFlags["Seed"] && !Input.inputString.All("0123456789abcdefABCDEF".Contains)) inputValid = false;
					if (editingFlags["Seed"] && (stringToEdit.Length + Input.inputString.Length) > 8) inputValid = false;

					if (inputValid)
					{
						stringToEdit = stringToEdit.Insert(stringCursorPosition, Input.inputString);
						stringCursorPosition++;
					}
				}

				//handle backspacing
				if (Input.GetKeyDown(KeyCode.Backspace))
				{
					if (stringToEdit.Length > 0 && stringCursorPosition > 0)
					{
						stringToEdit = stringToEdit.Remove(stringCursorPosition - 1, 1);
						stringCursorPosition--;
					}
				}

				//handle delete
				if (Input.GetKeyDown(KeyCode.Delete))
				{
					if (stringToEdit.Length > 0 && stringCursorPosition < stringToEdit.Length)
					{
						stringToEdit = stringToEdit.Remove(stringCursorPosition, 1);
					}
				}

				//handle cursor navigation
				if (Input.GetKeyDown(KeyCode.LeftArrow) && stringCursorPosition > 0)
				{
					stringCursorPosition--;
				}
				if (Input.GetKeyDown(KeyCode.RightArrow) && stringCursorPosition < stringToEdit.Length)
				{
					stringCursorPosition++;
				}

				//handle Enter/Esc
				if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Escape))
				{
					if (!editingFlags["Player"] && !editingFlags["Hostname"] && !editingFlags["Port"] && !editingFlags["Password"] && !editingFlags["Seed"])
					{
						currentToolTip = null;
						if (FF1PR.TitleWindowController != null)
						{
							FF1PR.TitleWindowController.SetEnableMenu(true);
						}
					}

					submitKeyPressed = true;
				}

				//update the relevant connection setting field
				Dictionary<string, bool> originalEditingFlags = new Dictionary<string, bool>(editingFlags);
				foreach (KeyValuePair<string, bool> editingFlag in originalEditingFlags)
				{
					if (!editingFlag.Value) continue;
					setConnectionSetting(editingFlag.Key, stringToEdit);
					if (submitKeyPressed) finishEditingTextField(editingFlag.Key);
				}
			}
			else if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "Title")
			{
				currentToolTip = null;
				if (FF1PR.TitleWindowController != null)
				{
					FF1PR.TitleWindowController.SetEnableMenu(true);
				}
			}
		}
		private static float GetApHeight(float increment)
		{
			var oldApHeight = apHeight;
			apHeight += increment * guiScale;
			return oldApHeight;
		}
		public static Rect ScaledRect(float x, float y, float width, float height)
		{
			return new Rect(x * guiScale, y * guiScale, width * guiScale, height * guiScale);
		}
		private static void CreateDropdown(string label, Option option)
		{
			var backcolor = GUI.backgroundColor;
			GUI.backgroundColor = new Color(0.80f, 0.80f, 0.80f, 0.2f);
			if (GUI.Button(ScaledRect(330f, apHeight, 30f, 30f), "?"))
			{
				currentToolTip = currentToolTip == option ? null : option;
				if (currentToolTip == null)
				{
					if (FF1PR.TitleWindowController != null)
					{
						FF1PR.TitleWindowController.SetEnableMenu(true);
					}
				}
			}

			bool showOptions = currentOptionShowing == label;

			string currentSelection = option.Choices[option.Default];
			if (FF1PR.SessionManager.Data.Options.TryGetValue(option.Key, out var select))
			{
				if (option.Choices.TryGetValue(select, out var foundchoice))
				{
					currentSelection = foundchoice;
				}
			}

			GUI.skin.button.alignment = TextAnchor.MiddleLeft;

			if (showOptions)
			{
				GUI.color = new Color(0.80f, 0.80f, 0.80f, 1.0f);
			}

			string pointer = showOptions ? "▼" : "▶";

			if (GUI.Button(new Rect(apMargin, GetApHeight(30f), 300f, 30f), pointer + " " + label + ": " + currentSelection))
			{
				currentOptionShowing = showOptions ? "" : label;
			}

			GUI.color = new Color(0.95f, 0.95f, 0.95f, 1.0f);

			if (currentOptionShowing == label)
			{
				foreach (var choice in option.Choices)
				{
					if (GUI.Button(new Rect((apMargin + 20f), GetApHeight(30f), 280f, 30f), choice.Value))
					{
						FF1PR.SessionManager.Options[option.Key] = choice.Key;
						currentOptionShowing = "";
					}
				}
			}

			GUI.skin.button.alignment = TextAnchor.MiddleCenter;
			GUI.backgroundColor = backcolor;

			apHeight += 10f * guiScale;
		}

		private static string CreateGenericDropdown(string label, List<string> options, string defaultchoice)
		{
			var backcolor = GUI.backgroundColor;
			GUI.backgroundColor = new Color(0.80f, 0.80f, 0.80f, 0.2f);

			bool showOptions = currentOptionShowing == label;

			string currentSelection = defaultchoice;
			if (!options.Contains(defaultchoice))
			{
				currentSelection = options[0];
			}

			GUI.skin.button.alignment = TextAnchor.MiddleLeft;

			if (showOptions)
			{
				GUI.color = new Color(0.80f, 0.80f, 0.80f, 1.0f);
			}

			string pointer = showOptions ? "▼" : "▶";

			if (GUI.Button(new Rect(apMargin, GetApHeight(30f), 300f, 30f), pointer + " " + currentSelection))
			{
				currentOptionShowing = showOptions ? "" : label;
			}

			GUI.color = new Color(0.95f, 0.95f, 0.95f, 1.0f);

			string choicepicked = "";
			if (currentOptionShowing == label)
			{
				foreach (var choice in options)
				{
					if (GUI.Button(new Rect((apMargin + 20f), GetApHeight(30f), 280f, 30f), choice))
					{
						choicepicked = choice;
						currentOptionShowing = "";
					}
				}
			}

			GUI.skin.button.alignment = TextAnchor.MiddleCenter;
			GUI.backgroundColor = backcolor;

			apHeight += 10f * guiScale;

			return choicepicked;
		}
		private static void CreateGameModeDropdown(string label)
		{
			var backcolor = GUI.backgroundColor;
			GUI.backgroundColor = new Color(0.80f, 0.80f, 0.80f, 0.2f);
			GUI.skin.button.alignment = TextAnchor.MiddleLeft;
			GUI.skin.button.fontSize = (int)(standardFontSize * 1.3 * guiScale);
			/*
			if (GUI.Button(ScaledRect(330f, apHeight, 30f, 30f), "?"))
			{
				currentToolTip = currentToolTip == option ? null : option;
			}*/

			bool showOptions = currentOptionShowing == label;

			string currentSelection = gameModeOption[FF1PR.SessionManager.GameMode];


			if (showOptions)
			{
				GUI.color = new Color(0.80f, 0.80f, 0.80f, 1.0f);
			}

			string pointer = showOptions ? "▼" : "▶";

			if (GUI.Button(new Rect(apMargin, GetApHeight(30f), 340f, 30f), pointer + " " + label + ": " + currentSelection))
			{
				currentOptionShowing = showOptions ? "" : label;
			}

			GUI.color = new Color(0.95f, 0.95f, 0.95f, 1.0f);

			if (currentOptionShowing == label)
			{
				foreach (var choice in gameModeOption)
				{
					if (GUI.Button(new Rect((apMargin + 20f), GetApHeight(30f), 320f, 30f), choice.Value))
					{
						FF1PR.SessionManager.GameMode = choice.Key;
						currentOptionShowing = "";
						FF1PR.SessionManager.WriteSessionInfo();
					}
				}
			}

			GUI.skin.button.alignment = TextAnchor.MiddleCenter;
			GUI.backgroundColor = backcolor;
			
			GUI.skin.button.fontSize = (int)(standardFontSize * guiScale);
			apHeight += 10f * guiScale;
		}
		private static void ToolTipWindow(int windowID)
		{
			GUI.skin.label.fontSize = (int)(standardFontSize * guiScale);
			GUI.skin.button.fontSize = (int)(standardFontSize * guiScale);
			GUI.skin.toggle.fontSize = (int)(standardFontSize * guiScale);
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.clipping = TextClipping.Overflow;
			GUI.color = new Color(0.96f, 0.96f, 0.96f, 1.0f);

			GUI.contentColor = new Color(0.96f, 0.96f, 0.96f, 1.0f);

			GUI.skin.window.wordWrap = true;

			GUI.Label(ScaledRect(apMargin, 60f, standardWindowRect.width - (apMargin * 3), (standardWindowRect.height - 200f) * guiScale), currentToolTip.Description);

			GUI.skin.window.wordWrap = false;

			if (GUI.Button(ScaledRect(apMargin, standardWindowRect.height - 60f, 100f, 30f), "Close"))
			{
				currentToolTip = null;
				if (FF1PR.TitleWindowController != null)
				{
					FF1PR.TitleWindowController.SetEnableMenu(true);
				}
			}
		}
		private static void RandomizerSettingsWindow(int windowID)
		{
			GUI.skin.label.fontSize = (int)(standardFontSize * 1.2 * guiScale);
			GUI.skin.button.fontSize = (int)(standardFontSize * guiScale);
			GUI.skin.toggle.fontSize = (int)(standardFontSize * guiScale);
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.clipping = TextClipping.Overflow;
			GUI.color = new Color(0.96f, 0.96f, 0.96f, 1.0f);

			GUI.contentColor = new Color(0.96f, 0.96f, 0.96f, 1.0f);

			apHeight = 0f;
			scrollPosition = GUI.BeginScrollView(new Rect(apMargin * guiScale, apMargin * 3f * guiScale, standardWindowRect.width - (apMargin * 2f * guiScale), standardWindowRect.height - (apMargin * 4f * guiScale)), scrollPosition, new Rect(0, 0, standardWindowRect.width - (apMargin * 3f * guiScale), lastApHeight));

			CreateGameModeDropdown("Game Mode");
			apHeight += 20f * guiScale;

			GUI.Label(ScaledRect(0, GetApHeight(40f), 300f, 30f), "Generation");
			GUI.skin.label.fontSize = (int)(standardFontSize * 1.3 * guiScale);
			GUI.Label(ScaledRect(apMargin, GetApHeight(30f), 340f, 30f), $"Seed: {textWithCursor(getConnectionSetting("Seed"), editingFlags["Seed"], true)}");

			bool EditSeed = GUI.Button(ScaledRect(apMargin, apHeight, 75f, 30f), editingFlags["Seed"] ? "Save" : "Edit");
			if (EditSeed) handleEditButton("Seed");
			bool PasteSeed = GUI.Button(ScaledRect(apMargin + 90f, apHeight, 75f, 30f), "Paste");
			if (PasteSeed) handlePasteButton("Seed");
			bool RollSeed = GUI.Button(ScaledRect(apMargin + 180f, GetApHeight(40f), 75f, 30f), "Roll");
			if (RollSeed) handleRollButton("Seed");

			bool generate = GUI.Button(ScaledRect(apMargin, GetApHeight(40f), 150f, 30f), "Generate");

			if (generate)
			{
				var hash = FF1PR.SessionManager.CreateHash();
				Randomizer.Randomize(hash);
				FF1PR.SessionManager.WriteSessionInfo();
				generationReady = true;
			}

			if (generationReady)
			{
				GUI.skin.label.fontSize = (int)(standardFontSize * 1 * guiScale);
				GUI.Label(ScaledRect(apMargin, GetApHeight(30f), 200f, 30f), $"Hash: {FF1PR.SessionManager.Data.Hashstring}");
			}

			string genlabel = generationReady ? "Randomization done. You can start a new game to play with these settings." : "Click Generate to randomize a new game or load a save file to continue a previously randomize game.";
			GUI.skin.label.fontSize = (int)(standardFontSize * 0.9 * guiScale);
			GUI.Label(ScaledRect(apMargin, GetApHeight(80f), 360f, 30f), genlabel);

			// Add rando options
			AddRandoOptions();

			GUI.EndScrollView();

			lastApHeight = apHeight + 40f * guiScale;
		}
		
		private static void ArchipelagoQuickSettingsWindow(int windowID)
		{
			GUI.skin.label.fontSize = (int)(standardFontSize * 1.2 * guiScale);
			GUI.skin.button.fontSize = (int)(standardFontSize * guiScale);
			GUI.skin.toggle.fontSize = (int)(standardFontSize * guiScale);
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.clipping = TextClipping.Overflow;
			GUI.color = new Color(0.96f, 0.96f, 0.96f, 1.0f);
			
			GUI.contentColor = new Color(0.96f, 0.96f, 0.96f, 1.0f);

			apHeight = 0f;
			scrollPosition = GUI.BeginScrollView(new Rect(apMargin * guiScale, apMargin * 3f * guiScale, standardWindowRect.width - (apMargin * 2f * guiScale), standardWindowRect.height - (apMargin * 4f * guiScale)), scrollPosition, new Rect(0, 0, standardWindowRect.width - (apMargin * 3f * guiScale), lastApHeight));

			CreateGameModeDropdown("Game Mode");
			apHeight += 20f * guiScale;

			GUI.Label(ScaledRect(apMargin, GetApHeight(30f), 500f, 30f), $"Player: {FF1PR.SessionManager.Data.Player}");
			GUI.Label(ScaledRect(apMargin, apHeight, 70f, 30f), $"Status:");
			if (Archipelago.instance.integration != null && Archipelago.instance.integration.connected)
			{
				GUI.color = Color.green;
				GUI.Label(ScaledRect(apMargin + 85f, apHeight, 150f, 30f), $"Connected!");
				GUI.color = Color.white;
				int playerCount = 0;
				foreach (var player in Archipelago.instance.integration.session.Players.AllPlayers) {
					if (player.Slot > 0 && player.GetGroupMembers(Archipelago.instance.integration.session.Players) == null) {
						playerCount++;
					}
				}
				GUI.Label(ScaledRect(apMargin + 200f, apHeight, 300f, 30f), $"(world {Archipelago.instance.integration.session.ConnectionInfo.Slot} of {playerCount})");
			} else {
				GUI.color = Color.red;
				GUI.Label(ScaledRect(apMargin + 85f, apHeight, 230f, 30f), $"Disconnected");
			}
			GUI.color = Color.white;
			apHeight += 40f * guiScale;
			bool Connection = GUI.Button(ScaledRect(apMargin, GetApHeight(40f), 160f, 30f), Archipelago.instance.IsConnected() ? "Disconnect" : "Connect");
			if (Connection) {
				if (Archipelago.instance.IsConnected()) {
					Archipelago.instance.Disconnect();
				} else {
					Archipelago.instance.Connect();
				}
			}

			GUI.skin.label.fontSize = (int)(standardFontSize * 1.3 * guiScale);
			GUI.skin.button.fontSize = (int)(standardFontSize * guiScale);
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.clipping = TextClipping.Overflow;
			
			var backcolor = GUI.backgroundColor;
			GUI.backgroundColor = new Color(0.80f, 0.80f, 0.80f, 0.2f);
			GUI.skin.button.alignment = TextAnchor.MiddleLeft;
			//GUI.skin.button.fontSize = (int)(standardFontSize * 1.3 * guiScale);

			string pointer = ShowAPSettingsWindow ? "▼ Hide Connection Info" : "▶ Edit Connection Info";

			if (GUI.Button(new Rect(apMargin, GetApHeight(50f), 340f, 30f), pointer))
			{
				ShowAPSettingsWindow = !ShowAPSettingsWindow;
				if (ShowAPSettingsWindow)
				{
					Archipelago.instance.Disconnect();
				}
			}
			
			GUI.backgroundColor = backcolor;
			GUI.skin.button.alignment = TextAnchor.MiddleCenter;

			if (ShowAPSettingsWindow)
			{
				//Player name
				GUI.skin.label.fontSize = (int)(standardFontSize * 1.3 * guiScale);
				GUI.Label(ScaledRect(apMargin, GetApHeight(30f), 340f, 30f), $"Player: {textWithCursor(getConnectionSetting("Player"), editingFlags["Player"], true)}");

				bool EditPlayer = GUI.Button(ScaledRect(apMargin, apHeight, 75f, 30f), editingFlags["Player"] ? "Save" : "Edit");
				if (EditPlayer) handleEditButton("Player");

				bool PastePlayer = GUI.Button(ScaledRect(apMargin + 90f, apHeight, 75f, 30f), "Paste");
				if (PastePlayer) handlePasteButton("Player");

				bool ClearPlayer = GUI.Button(ScaledRect(apMargin + 180f, apHeight, 75f, 30f), "Clear");
				if (ClearPlayer) handleClearButton("Player");

				apHeight += 50f * guiScale;

				//Hostname
				GUI.skin.label.fontSize = (int)(standardFontSize * 1.3 * guiScale);
				GUI.Label(ScaledRect(apMargin, GetApHeight(30f), 340f, 30f), $"Host: {textWithCursor(getConnectionSetting("Hostname"), editingFlags["Hostname"], true)}");

				string setHost = CreateGenericDropdown("Hostname", new List<string>() { "localhost", "archipelago.gg" }, FF1PR.SessionManager.Data.Host);

				if (setHost != "" && FF1PR.SessionManager.Data.Host != setHost)
				{
					setConnectionSetting("Hostname", setHost);
					FF1PR.SessionManager.WriteSessionInfo();
				}

				bool EditHostname = GUI.Button(ScaledRect(apMargin, apHeight, 75f, 30f), editingFlags["Hostname"] ? "Save" : "Edit");
				if (EditHostname) handleEditButton("Hostname");

				bool PasteHostname = GUI.Button(ScaledRect(apMargin + 90f, apHeight, 75f, 30f), "Paste");
				if (PasteHostname) handlePasteButton("Hostname");

				bool ClearHostname = GUI.Button(ScaledRect(apMargin + 180f, apHeight, 75f, 30f), "Clear");
				if (ClearHostname) handleClearButton("Hostname");

				apHeight += 50f * guiScale;

				//Port
				GUI.skin.label.fontSize = (int)(standardFontSize * 1.3 * guiScale);
				GUI.Label(ScaledRect(apMargin, GetApHeight(30f), 340f, 30f), $"Port: {textWithCursor(getConnectionSetting("Port"), editingFlags["Port"], showPort)}");

				string portVisibility = CreateGenericDropdown("Port Visibility", new List<string>() { "Show", "Hide" }, showPort ? "Show" : "Hide");

				if (portVisibility != "")
				{
					showPort = portVisibility == "Show";
				}

				bool EditPort = GUI.Button(ScaledRect(apMargin, apHeight, 75f, 30f), editingFlags["Port"] ? "Save" : "Edit");
				if (EditPort) handleEditButton("Port");

				bool PastePort = GUI.Button(ScaledRect(apMargin + 90f, apHeight, 75f, 30f), "Paste");
				if (PastePort) handlePasteButton("Port");

				bool ClearPort = GUI.Button(ScaledRect(apMargin + 180f, apHeight, 75f, 30f), "Clear");
				if (ClearPort) handleClearButton("Port");
				
				apHeight += 50f * guiScale;

				//Password
				GUI.skin.label.fontSize = (int)(standardFontSize * 1.3 * guiScale);
				GUI.Label(ScaledRect(apMargin, GetApHeight(30f), 340f, 30f), $"Password: {textWithCursor(getConnectionSetting("Password"), editingFlags["Password"], showPassword)}");

				string passwordVisibility = CreateGenericDropdown("Password Visibility", new List<string>() { "Show", "Hide" }, showPassword ? "Show" : "Hide");

				if (passwordVisibility != "")
				{
					showPassword = passwordVisibility == "Show";
				}
				
				bool EditPassword = GUI.Button(ScaledRect(apMargin, apHeight, 75f, 30f), editingFlags["Password"] ? "Save" : "Edit");
				if (EditPassword) handleEditButton("Password");

				bool PastePassword = GUI.Button(ScaledRect(apMargin + 90f, apHeight, 75f, 30f), "Paste");
				if (PastePassword) handlePasteButton("Password");

				bool ClearPassword = GUI.Button(ScaledRect(apMargin + 180f, apHeight, 75f, 30f), "Clear");
				if (ClearPassword) handleClearButton("Password");
			}

			apHeight += 40f * guiScale;

			GUI.EndScrollView();

			lastApHeight = apHeight + 40f * guiScale;
		}

		// Options and version to modify
		public static void VersionWindow(int windowID)
		{
			GUI.skin.label.fontSize = (int)(standardFontSize * 0.9 * guiScale);
			GUI.color = new Color(0f, 0f, 0f, 1f);
			GUI.contentColor = new Color(0f, 0f, 0f, 1f);
			GUI.Label(ScaledRect(0, 0, 300f, 30f), $"v{PluginInfo.VERSION} Alpha");
		}
		private static void AddRandoOptions()
		{
			GUI.skin.label.fontSize = (int)(standardFontSize * 1.3 * guiScale);
			apHeight += 20f * guiScale;
			
			GUI.Label(ScaledRect(0, GetApHeight(40f), 300f, 30f), "Key Items Placement");
			CreateDropdown(Options.Dict["npcs_priority"].Display, Options.Dict["npcs_priority"]);
			CreateDropdown(Options.Dict["keychests_priority"].Display, Options.Dict["keychests_priority"]);
			CreateDropdown(Options.Dict["trapped_priority"].Display, Options.Dict["trapped_priority"]);
			CreateDropdown(Options.Dict["adamantite_craft"].Display, Options.Dict["adamantite_craft"]);
			apHeight += 20f * guiScale;
			
			GUI.Label(ScaledRect(0, GetApHeight(40f), 300f, 30f), "Shops");
			CreateDropdown(Options.Dict["shuffle_gear_shops"].Display, Options.Dict["shuffle_gear_shops"]);
			CreateDropdown(Options.Dict["shuffle_spells"].Display, Options.Dict["shuffle_spells"]);
			apHeight += 20f * guiScale;
		}
	}
}
