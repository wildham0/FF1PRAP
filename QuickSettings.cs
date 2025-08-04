//using Il2CppSystem;
using BepInEx.Core.Logging.Interpolation;
using Last.UI.KeyInput;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace FF1PRAP {
	public class QuickSettings : MonoBehaviour {

		public static string CustomSeed = "";
		public static Font PixelRemasterFont;
		public static List<string> FoolChoices = new List<string>() { "Off", "Normal", "Double", "Onslaught" };
		public static List<string> FoolColors = new List<string>() { "white", "#4FF5D4", "#E3D457", "#FF3333" };
		private static bool ShowAdvancedSinglePlayerOptions = false;
		private static bool ShowAPSettingsWindow = false;
		private static bool ShowMysterySeedWindow = false;
		private static int MysteryWindowPage = 0;
		private static string stringToEdit = "";
		private static int stringCursorPosition = 0;
		private static bool showPort = false;
		private static bool showPassword = false;
		private static float guiScale = 1f;
		private static float apHeight = 0f;
		private static float lastApHeight = 0f;
		private static float apMargin = 20f;
		private static float advHeight = 0f;
		private static float mystHeight = 0f;
		private static int screenWidth = 0;
		private static int screenHeight = 0;
		private static float standardFontSize = 15f;
		private static Texture2D windowTexture;
		private static bool showGameOption = false;
		private static float gameOptionHeight = 0f;
		private static string currentOptionShowing = "";
		private static string currentToolTip = "";
		public static Vector2 scrollPosition = Vector2.zero;
		private static Dictionary<GameModes, string> gameModeOption = new()
		{
			{ GameModes.Archipelago, "Archipelago" },
			{ GameModes.Randomizer, "Single Player" },
		};

		//private static Texture2D 
		private static GUISkin windowSkin;
		private static Rect standardWindowRect;
		private static Dictionary<string, bool> editingFlags = new Dictionary<string, bool>() {
			{"Player", false},
			{"Hostname", false},
			{"Port", false},
			{"Password", false},
		};

		// For showing option tooltips
		private static string hoveredOption = "";
		private static Dictionary<string, Rect> windowRects = new Dictionary<string, Rect>() {
			{ "singlePlayer", new Rect() },
			{ "advancedSinglePlayer", new Rect() },
			{ "mysterySeed", new Rect() },
			{ "archipelago", new Rect() },
			{ "archipelagoConfig", new Rect() },
		};

		//Get a conenction setting value by fieldname
		private static string getConnectionSetting(string fieldName)
		{
			switch(fieldName)
			{
				case "Player":
					return FF1PR.SessionManager.GetGlobal<string>("player");
				case "Hostname":
					return FF1PR.SessionManager.GetGlobal<string>("server");
				case "Port":
					return FF1PR.SessionManager.GetGlobal<string>("port");
				case "Password":
					return FF1PR.SessionManager.GetGlobal<string>("password");
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
					FF1PR.SessionManager.SetGlobal("player", value);
					return;
				case "Hostname":
					FF1PR.SessionManager.SetGlobal("server", value);
					return;
				case "Port":
					FF1PR.SessionManager.SetGlobal("port", value);
					return;
				case "Password":
					FF1PR.SessionManager.SetGlobal("password", value);
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
			FF1PR.SessionManager.WriteGlobalData();
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
			FF1PR.SessionManager.WriteGlobalData();
		}

		private static void handleClearButton(string fieldName)
		{
			setConnectionSetting(fieldName, "");
			if (editingFlags[fieldName]) stringToEdit = "";
			FF1PR.SessionManager.WriteGlobalData();
		}

		private static void DrawBorders(ref int[] colorArray, int width, int height, int borderwidth, int setindent, int offset, int color)
		{
			//int borderwidth = 6;
			//int setindent = 4;

			var indent = setindent + 1;
			for (int y = 0 + offset; y < borderwidth + offset; y++)
			{
				if (indent > 0) indent--;
				for (int x = indent + offset; x < width - indent - offset; x++)
				{
					colorArray[x + (y * width)] = color;
				}
			}

			indent = setindent + 1;
			for (int y = height - offset - 1; y > height - offset - borderwidth; y--)
			{
				if (indent > 0) indent--;
				for (int x = indent + offset; x < width - indent - offset; x++)
				{
					colorArray[x + (y * width)] = color;
				}
			}

			indent = setindent + 1;
			for (int x = 0 + offset; x < borderwidth + offset; x++)
			{
				if (indent > 0) indent--;
				for (int y = indent + offset; y < height - indent - offset; y++)
				{
					colorArray[x + (y * width)] = color;
				}
			}

			indent = setindent + 1;
			for (int x = width - offset - 1; x > width - borderwidth - offset; x--)
			{
				if (indent > 0) indent--;
				for (int y = indent + offset; y < height - indent - offset; y++)
				{
					colorArray[x + (y * width)] = color;
				}
			}


		}
		private void GenerateSkin()
		{
			InternalLogger.LogInfo("Generating Skin.");
			
			var skin = GUI.skin;
			//GUISkin test = new();
			var width = (int)standardWindowRect.width;
			var height = (int)standardWindowRect.height;

			var texture = new Texture2D((int)standardWindowRect.width, (int)standardWindowRect.height);

			var colorArray = new Color[width * height];
			var intArray = new int[width * height];
			var selectColors = new Color[]
			{
					new Color(0.05f, 0.05f, 0.55f, 1.0f),
					new Color(0.09f, 0.09f, 0.60f, 1.0f),
					new Color(0.14f, 0.14f, 0.66f, 1.0f),
					new Color(0.18f, 0.18f, 0.72f, 1.0f),
					new Color(0.23f, 0.23f, 0.78f, 1.0f),
					new Color(0.25f, 0.25f, 0.80f, 1.0f),
			};

			float maxrg = 0.25f;
			float minrg = 0.05f;
			float maxb = 0.80f;
			float minb = 0.55f;


			Color currentColor;
			Color whiteColor = new Color(0.96f, 0.96f, 0.96f, 1.0f);
			Color blackColor = new Color(0.04f, 0.04f, 0.04f, 1.0f);
			int borderwidth = 7;
			int setindent = 4;

			DrawBorders(ref intArray, width, height, borderwidth, 4, 0, 1);
			DrawBorders(ref intArray, width, height, borderwidth - 1, 4, 1, 2);

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (intArray[x + (y * width)] == 1)
					{
						colorArray[x + (y * width)] = blackColor;
					}
					else if(intArray[x + (y * width)] == 2)
					{
						colorArray[x + (y * width)] = whiteColor;
					}
				}
			}

			/*
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{

					colorArray[x + (y * width)] = blackColor;
				}
			}

			for (int y = 1; y < height - 1; y++)
			{
				for (int x = 1; x < width -1; x++)
				{

					colorArray[x + (y * width)] = whiteColor;
				}
			}

			for (int y = 4; y < height - 4; y++)
			{
				for (int x = 4; x < width - 4; x++)
				{

					colorArray[x + (y * width)] = blackColor;
				}
			}
			*/

			for (int y = borderwidth; y < height - borderwidth + 1; y++)
			{

				float currentrg = ((maxrg - minrg) * ((float)y / (float)height)) + minrg;
				float currentb = ((maxb - minb) * ((float)y / (float)height)) + minb;
				//InternalLogger.LogInfo($"Blue pixel: {currentb}");
				currentColor = new Color(currentrg, currentrg, currentb, 1.0f);
				for (int x = borderwidth; x < width - borderwidth + 1; x++)
				{
					colorArray[x + (y * width)] = currentColor;
				}
			}



			//texture.SetPixel(0, 0, new Color(0.25f, 0.25f, 0.80f, 1.0f));
			texture.SetPixels(colorArray);
			texture.Apply();

			//texture.draw
			//skin.window = new GUIStyle();
			windowTexture = texture;
			skin.window.normal.background = windowTexture;
			skin.window.normal.textColor = new Color(0.95f, 0.95f, 0.95f, 1.0f);
			skin.window.active.background = windowTexture;
			skin.window.active.textColor = new Color(0.95f, 0.95f, 0.95f, 1.0f);
			skin.window.hover.background = windowTexture;
			skin.window.hover.textColor = new Color(0.95f, 0.95f, 0.95f, 1.0f);
			skin.window.focused.background = windowTexture;
			skin.window.focused.textColor = new Color(0.95f, 0.95f, 0.95f, 1.0f);

			skin.window.onNormal.background = windowTexture;
			skin.window.onNormal.textColor = new Color(0.95f, 0.95f, 0.95f, 1.0f);
			skin.window.onActive.background = windowTexture;
			skin.window.onActive.textColor = new Color(0.95f, 0.95f, 0.95f, 1.0f);
			skin.window.onHover.background = windowTexture;
			skin.window.onHover.textColor = new Color(0.95f, 0.95f, 0.95f, 1.0f);
			skin.window.onFocused.background = windowTexture;
			skin.window.onFocused.textColor = new Color(0.95f, 0.95f, 0.95f, 1.0f);


			skin.window.border = new RectOffset(8, 8, 8, 8);
			skin.window.alignment = TextAnchor.UpperCenter;
			skin.window.fontStyle = FontStyle.Normal;
			skin.window.margin = new RectOffset(8, 8, 8, 8);
			skin.window.padding = new RectOffset(16, 16, 32, 16);

			skin.window.stretchHeight = true;
			skin.window.stretchWidth = true;

			//skin.window.

			windowSkin = skin;
		}
		private void OnGUI() {
			//InternalLogger.LogInfo($"GUI called: {SceneManager.GetActiveScene().name} > controller null? {GameObject.FindObjectOfType<Last.UI.KeyInput.TitleWindowController>() == null}");
			if (SceneManager.GetActiveScene().name == "Title" && GameObject.FindObjectOfType<Last.UI.KeyInput.TitleWindowController>() != null && FF1PR.StateTracker.CurrentSubState == Last.Management.GameSubStates.Title_Main)
			{
				if (screenWidth != Screen.width || screenHeight != Screen.height)
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

					standardWindowRect = new Rect(20f, (float)Screen.height * 0.12f, 430f * guiScale, 600f * guiScale);
					windowSkin = GUI.skin;
					GenerateSkin();

					if (PixelRemasterFont == null)
					{
						PixelRemasterFont = Resources.FindObjectsOfTypeAll<Font>().Where(f => f.name == "FOT-NewCezannePro-B").ToList()[0];
					}

					GUI.skin = windowSkin;
					GUI.skin.font = PixelRemasterFont;
					GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

					var alltexture = Resources.FindObjectsOfTypeAll<Texture2D>().ToList();
					if (alltexture != null)
					{
						foreach (var texture in alltexture)
						{
							InternalLogger.LogInfo($"Texture: {texture.name}");
						}

					}
					
				}
				

				//var stylestate = new GUIStyleState();

				//stylestate.
				


				

				//skin.window.
				//GUI.skin = windowSkin;

				//GUI.color = new Color(0, 0, 128, 255);
				//GUI.
				//GUIStyle test = new();


				Cursor.visible = true;

				switch (FF1PR.SessionManager.GameMode)
				{
					/*case GameModes.Randomizer:
						windowRects["singlePlayer"] = new Rect(20f, (float)Screen.height * 0.12f, 430f * guiScale, 630f * guiScale);
						GUI.Window(101, windowRects["singlePlayer"], new Action<int>(SinglePlayerQuickSettingsWindow), "Single Player Settings");
						ShowAPSettingsWindow = false;
						clearAllEditingFlags();
						break;*/
					case GameModes.Archipelago:
						//windowRects["archipelago"] = new Rect(20f, (float)Screen.height * 0.12f, 430f * guiScale, apHeight);
						GUI.Window(101, standardWindowRect, new Action<int>(ArchipelagoQuickSettingsWindow), "Archipelago Settings", GUI.skin.window);
						break;
				}
				/*
				if (showGameOption)
				{
					Rect gameOptionWindow = new Rect(((apMargin + 20f) * guiScale), gameOptionHeight + (30f * guiScale), 300f * guiScale, 100f * guiScale);
					GUI.Window(200, gameOptionWindow, new Action<int>(createDropdown), "t");
				}*/

				if (ShowAPSettingsWindow && FF1PR.SessionManager.GameMode == GameModes.Archipelago)
				{
					Rect sideSettingsWindow = new Rect(460f * guiScale, standardWindowRect.y, standardWindowRect.width, standardWindowRect.height);
					//windowRects["archipelagoConfig"] = new Rect(460f * guiScale, (float)Screen.height * 0.12f, 350f * guiScale, 490f * guiScale);
					GUI.Window(103, sideSettingsWindow, new Action<int>(ArchipelagoConfigEditorWindow), "Archipelago Connection");
				}

				InternalLogger.LogInfo(GUI.tooltip);

				if (currentToolTip != "")
				{
					Rect sideSettingsWindow = new Rect(460f * guiScale, standardWindowRect.y, standardWindowRect.width, standardWindowRect.height);
					GUI.Window(104, sideSettingsWindow, new Action<int>(ArchipelagoConfigEditorWindow), currentToolTip);

				}
				/*
				if (ShowAdvancedSinglePlayerOptions && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER && !TunicRandomizer.Settings.MysterySeed) {
					windowRects["advancedSinglePlayer"] = new Rect(460f * guiScale, (float)Screen.height * 0.12f, 405f * guiScale, advHeight);
					GUI.Window(105, windowRects["advancedSinglePlayer"], new Action<int>(AdvancedLogicOptionsWindow), "Advanced Logic Options");
				}
				if (ShowMysterySeedWindow && TunicRandomizer.Settings.Mode == RandomizerSettings.RandomizerType.SINGLEPLAYER && TunicRandomizer.Settings.MysterySeed) {
					windowRects["mysterySeed"] = new Rect(460f * guiScale, (float)Screen.height * 0.12f, 405f * guiScale, mystHeight);
					GUI.Window(106, windowRects["mysterySeed"], new Action<int>(MysterySeedWeightsWindow), "Mystery Seed Settings");
				}*/
				/*
				if (hoveredOption != "")
				{
					GUI.Window(107, new Rect(20f, (float)Screen.height - (110f * guiScale), 1000f * guiScale, 110f * guiScale), new Action<int>(TooltipWindow), hoveredOption);
				}*/
				//GameObject.Find("elderfox_sword graphic").GetComponent<Renderer>().enabled = !ShowAdvancedSinglePlayerOptions && !ShowAPSettingsWindow && !ShowMysterySeedWindow;

				//GUI.Label(new Rect(460f * guiScale, standardWindowRect.y, 100, 40), GUI.tooltip);
			}
		}

		private void Update()
		{
			if ((FF1PR.SessionManager.GameMode == GameModes.Archipelago && ShowAPSettingsWindow) && SceneManager.GetActiveScene().name == "Title")
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

					if(inputValid)
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
					if (!editingFlags["Player"] && !editingFlags["Hostname"] && !editingFlags["Port"] && !editingFlags["Password"])
					{
						CloseAPSettingsWindow();
					}

					submitKeyPressed = true;
				}

				//update the relevant connection setting field
				Dictionary<string, bool> originalEditingFlags = new Dictionary<string, bool>(editingFlags);
				foreach(KeyValuePair<string,bool> editingFlag in originalEditingFlags)
				{
					if (!editingFlag.Value) continue;
					setConnectionSetting(editingFlag.Key, stringToEdit);
					if (submitKeyPressed) finishEditingTextField(editingFlag.Key);
				}
			}
		}
		/*
		private static Rect ShowTooltip(Rect rect, string window, string option) {
			bool hovered = CombineRects(windowRects[window], rect).Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y));
			if (hovered && Tooltips.OptionTooltips.ContainsKey(option)) {
				hoveredOption = option;
			}
			if (hoveredOption == option && !hovered) {
				hoveredOption = "";
			}
			return rect;
		}
		*/
		/*
		private static void TooltipWindow(int windowID) {
			GUI.skin.label.fontSize = (int)(22f * guiScale);
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.clipping = TextClipping.Overflow;
			GUI.color = Color.white;
			GUI.DragWindow(new Rect(0, Screen.height - 100, 1000f * guiScale, 80 * guiScale));
			GUI.Label(new Rect(10f * guiScale, 20f * guiScale, 980f * guiScale, 100 * guiScale), Tooltips.OptionTooltips.ContainsKey(hoveredOption) ? Tooltips.OptionTooltips[hoveredOption] : hoveredOption);
		}
		*/
		private static Rect CombineRects(Rect r1, Rect r2) {
			Rect r3 = r1;
			r3.x += r2.x;
			r3.y += r2.y;
			r3.height = r2.height;
			r3.width = r2.width;
			return r3;
		}

		private static float GetApHeight(float increment)
		{
			var oldApHeight = apHeight;
			apHeight += increment * guiScale;
			return oldApHeight;
		}
		private static void CreateDropdown(string label, Option option)
		{

			//GUI.tooltip = "Yoyoyo!";

			GUI.Label(new Rect(apMargin * guiScale, apHeight, 300f * guiScale, 30f * guiScale), label);

			var backcolor = GUI.backgroundColor;
			GUI.backgroundColor = new Color(0.80f, 0.80f, 0.80f, 0.2f);
			if (GUI.Button(new Rect(0, GetApHeight(30f), 25f * guiScale, 25f * guiScale), "?"))
			{
				currentToolTip = "Yoyoyoy!";
			}
			GUI.backgroundColor = backcolor;

			bool showOptions = currentOptionShowing == label;

			string currentSelection = option.Choices[option.Default].display;
			if (FF1PR.SessionManager.TryGetGlobal<string>(option.Key, out var select))
			{
				if (option.Choices.TryFind(o => o.key == select, out var foundchoice))
				{
					currentSelection = foundchoice.display;
				}
			}

			if (showOptions)
			{
				GUI.color = new Color(0.40f, 0.40f, 0.40f, 1.0f);
			}
			//new GUIContent()

			//bool shopOption 
			if (GUI.Button(new Rect(apMargin * guiScale, GetApHeight(30f), 300f * guiScale, 30f * guiScale), currentSelection))
			{
				currentOptionShowing = label;
			}

			GUI.color = new Color(0.95f, 0.95f, 0.95f, 1.0f);

			if (currentOptionShowing == label)
			{
				foreach (var choice in option.Choices)
				{
					if (GUI.Button(new Rect(apMargin * guiScale, GetApHeight(30f), 300f * guiScale, 30f * guiScale), choice.display))
					{
						FF1PR.SessionManager.SetGlobal(option.Key, choice.key);
						currentOptionShowing = "";
					}
				}
			}

			apHeight += 10f * guiScale;
		}

		private static void CreateToggle(int windowID)
		{
			//int currentOption = 0;



			//GUI.

			//if(GUI)
			//GUI.skin.button.b

		}
		private static void ArchipelagoQuickSettingsWindow(int windowID) {
			GUI.skin.label.fontSize = (int)(standardFontSize * 1.3 * guiScale);
			GUI.skin.button.fontSize = (int)(standardFontSize * guiScale);
			GUI.skin.toggle.fontSize = (int)(standardFontSize * guiScale);
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.clipping = TextClipping.Overflow;
			GUI.color = new Color(0.96f, 0.96f, 0.96f, 1.0f);
			
			GUI.contentColor = new Color(0.96f, 0.96f, 0.96f, 1.0f);

			//GUI.DragWindow(new Rect(500f * guiScale, 50f * guiScale, 500f * guiScale, 30f * guiScale));

			apHeight = 40f * guiScale;
			scrollPosition = GUI.BeginScrollView(new Rect(apMargin * guiScale, apHeight, standardWindowRect.width - (apMargin * 2f * guiScale), standardWindowRect.height - (apMargin * 3f * guiScale)), scrollPosition, new Rect(0, 0, standardWindowRect.width - (apMargin * 3f * guiScale), lastApHeight));

			GUI.skin.toggle.fontSize = (int)(standardFontSize * 0.8 * guiScale); 
			GUI.skin.button.fontSize = (int)(standardFontSize * 0.8 * guiScale); 
			GUI.skin.label.fontSize = (int)(standardFontSize * 0.8 * guiScale);
			/*
			if (TunicRandomizer.Settings.RaceMode) {
				TunicRandomizer.Settings.RaceMode = GUI.Toggle(new Rect(330f * guiScale, apHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.RaceMode, "Race Mode");
			} else {
				if (Archipelago.instance.integration.disableSpoilerLog) {
					GUI.Label(new Rect(240f * guiScale, apHeight, 200f * guiScale, 30f * guiScale), "Spoiler Log Disabled by Host");
				} else {
					bool ToggleSpoilerLog = GUI.Toggle(new Rect(TunicRandomizer.Settings.CreateSpoilerLog ? 280f * guiScale : 330f * guiScale, apHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.CreateSpoilerLog, "Spoiler Log");
					TunicRandomizer.Settings.CreateSpoilerLog = ToggleSpoilerLog;
					if (ToggleSpoilerLog) {
						GUI.skin.button.fontSize = (int)(15 * guiScale);
						bool OpenSpoilerLog = GUI.Button(new Rect(370f * guiScale, apHeight, 50f * guiScale, 25f * guiScale), "Open");
						if (OpenSpoilerLog) {
							if (File.Exists(TunicRandomizer.SpoilerLogPath)) {
								System.Diagnostics.Process.Start(TunicRandomizer.SpoilerLogPath);
							}
						}
					}
				}
			}*/

			GUI.skin.label.fontSize = (int)(standardFontSize * 1.3 * guiScale);
			GUI.skin.toggle.fontSize = (int)(standardFontSize * guiScale);
			GUI.skin.button.fontSize = (int)(standardFontSize * guiScale);
			//GUI.skin


			GUI.Label(new Rect(apMargin * guiScale, apHeight, 300f * guiScale, 30f * guiScale), "Randomizer Mode");
			apHeight += 40f * guiScale;

			//GUI.pop

			if(GUI.Button(new Rect(apMargin * guiScale, apHeight, 300f * guiScale, 30f * guiScale), gameModeOption[FF1PR.SessionManager.GameMode]))
			{
				showGameOption = true;
			}
			apHeight += 40f * guiScale;

			if (showGameOption)
			{
				if (GUI.Button(new Rect((apMargin + 20f) * guiScale, apHeight, 200f * guiScale, 30f * guiScale), gameModeOption[GameModes.Randomizer]))
				{
					FF1PR.SessionManager.GameMode = GameModes.Randomizer;
					showGameOption = false;
				}
				apHeight += 40f * guiScale;


				if (GUI.Button(new Rect((apMargin + 20f) * guiScale, apHeight, 200f * guiScale, 30f * guiScale), gameModeOption[GameModes.Archipelago]))
				{
					FF1PR.SessionManager.GameMode = GameModes.Archipelago;
					showGameOption = false;
				}
				apHeight += 40f * guiScale;
			}
			foreach (var option in Options.Dict)
			{
				CreateDropdown(option.Value.Display, option.Value);
			}

			
			//apHeight += 40f * guiScale;
			/*
			bool ToggleSinglePlayer = GUI.Toggle(new Rect(apMargin * guiScale, apHeight, 130f * guiScale, 30f * guiScale), FF1PR.SessionManager.GameMode == GameModes.Randomizer, "Single Player");
			if (ToggleSinglePlayer && FF1PR.SessionManager.GameMode == GameModes.Archipelago)
			{
				FF1PR.SessionManager.GameMode = GameModes.Randomizer;
				CloseAPSettingsWindow();
			}
			bool ToggleArchipelago = GUI.Toggle(new Rect(150f * guiScale, apHeight, 150f * guiScale, 30f * guiScale), FF1PR.SessionManager.GameMode == GameModes.Archipelago, "Archipelago");
			if (ToggleArchipelago && FF1PR.SessionManager.GameMode == GameModes.Randomizer)
			{
				FF1PR.SessionManager.GameMode = GameModes.Archipelago;
			}*/
			//apHeight += 40f * guiScale;
			GUI.Label(new Rect(10f * guiScale, apHeight, 500f * guiScale, 30f * guiScale), $"Player: {(FF1PR.SessionManager.GetGlobal<string>("player"))}");
			apHeight += 40f * guiScale;
			GUI.Label(new Rect(10f * guiScale, apHeight, 80f * guiScale, 30f * guiScale), $"Status:");
			if (Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
				GUI.color = Color.green;
				GUI.Label(new Rect(95f * guiScale, apHeight, 150f * guiScale, 30f * guiScale), $"Connected!");
				GUI.color = Color.white;
				int playerCount = 0;
				foreach (var player in Archipelago.instance.integration.session.Players.AllPlayers) {
					if (player.Slot > 0 && player.GetGroupMembers(Archipelago.instance.integration.session.Players) == null) {
						playerCount++;
					}
				}
				GUI.Label(new Rect(250f * guiScale, apHeight, 300f * guiScale, 30f * guiScale), $"(world {Archipelago.instance.integration.session.ConnectionInfo.Slot} of {playerCount})");
			} else {
				GUI.color = Color.red;
				GUI.Label(new Rect(95f * guiScale, apHeight, 300f * guiScale, 30f * guiScale), $"Disconnected");
			}
			GUI.color = Color.white;
			apHeight += 40f * guiScale;
			bool Connection = GUI.Button(new Rect(10f * guiScale, apHeight, 160f * guiScale, 30f * guiScale), Archipelago.instance.IsConnected() ? "Disconnect" : "Connect");
			if (Connection) {
				if (Archipelago.instance.IsConnected()) {
					Archipelago.instance.Disconnect();
				} else {
					Archipelago.instance.Connect();
				}
			}

			
			bool OpenAPSettings = GUI.Button(new Rect(180f * guiScale, apHeight, 240f * guiScale, 30f * guiScale), ShowAPSettingsWindow ? "Close Connection Info" : "Edit Connection Info");
			if (OpenAPSettings) {
				if (ShowAPSettingsWindow) {
					CloseAPSettingsWindow();
					Archipelago.instance.Disconnect();
					Archipelago.instance.Connect();
				} else {
					ShowAPSettingsWindow = true;
					Last.UI.KeyInput.TitleWindowController titleScreen = GameObject.FindObjectOfType<Last.UI.KeyInput.TitleWindowController>();
					if (titleScreen != null) {
						titleScreen.SetEnableMenu(false);
						//titleScreen.commandController.
						//titleScreen.lockout = true;
					}
				}
			}
			apHeight += 40f * guiScale;
			GUI.Label(new Rect(10f * guiScale, apHeight, 200f * guiScale, 30f * guiScale), $"World Settings");

			GUI.skin.button.fontSize = (int)(15 * guiScale);

			GUI.EndScrollView();

			lastApHeight = apHeight + 40f * guiScale;
			/*
			bool ToggleSettings = GUI.Button(new Rect(180f * guiScale, apHeight + 7.5f, 50f * guiScale, 25f * guiScale), TunicRandomizer.Settings.ShowSlotSettings ? "Hide" : "Show");
			if (ToggleSettings) {
				TunicRandomizer.Settings.ShowSlotSettings = !TunicRandomizer.Settings.ShowSlotSettings;
				RandomizerSettings.SaveSettings();
			}
			if (TunicRandomizer.Settings.ShowSlotSettings) {
				if (Archipelago.instance.integration != null && Archipelago.instance.integration.connected) {
					Dictionary<string, object> slotData = Archipelago.instance.GetPlayerSlotData();
					apHeight += 40f * guiScale;
					GUI.Toggle(new Rect(10f * guiScale, apHeight, 180f * guiScale, 30f * guiScale), slotData["keys_behind_bosses"].ToString() == "1", "Keys Behind Bosses");
					GUI.Toggle(new Rect(220f * guiScale, apHeight, 210f * guiScale, 30f * guiScale), slotData["sword_progression"].ToString() == "1", "Sword Progression");
					apHeight += 40f * guiScale;
					GUI.Toggle(new Rect(10f * guiScale, apHeight, 175f * guiScale, 30f * guiScale), slotData["start_with_sword"].ToString() == "1", "Start With Sword");
					GUI.Toggle(new Rect(220f * guiScale, apHeight, 175f * guiScale, 30f * guiScale), slotData["ability_shuffling"].ToString() == "1", "Shuffled Abilities");
					apHeight += 40f * guiScale;
					GUI.Toggle(new Rect(10f * guiScale, apHeight, 185f * guiScale, 30f * guiScale), slotData["hexagon_quest"].ToString() == "1", slotData["hexagon_quest"].ToString() == "1" ? 
						$"Hexagon Quest (<color=#E3D457>{slotData["Hexagon Quest Goal"].ToString()}</color>)" : $"Hexagon Quest");
					int FoolIndex = int.Parse(slotData["fool_traps"].ToString());
					GUI.Toggle(new Rect(220f * guiScale, apHeight, 195f * guiScale, 60f * guiScale), FoolIndex != 0, $"Fool Traps: {(FoolIndex == 0 ? "Off" : $"<color={FoolColors[FoolIndex]}>{FoolChoices[FoolIndex]}</color>")}");

					apHeight += 40f * guiScale;
				
					if (slotData.ContainsKey("entrance_rando")) {
						GUI.Toggle(new Rect(10f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), slotData["entrance_rando"].ToString() == "1", $"Entrance Randomizer");
					} else {
						GUI.Toggle(new Rect(10f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), false, $"Entrance Randomizer");
					}
					if (slotData.ContainsKey("shuffle_ladders")) {
						GUI.Toggle(new Rect(220f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), slotData["shuffle_ladders"].ToString() == "1", $"Shuffled Ladders");
					} else {
						GUI.Toggle(new Rect(220f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), false, $"Shuffled Ladders");
					}

					apHeight += 40f * guiScale;
			 
					if (slotData.ContainsKey("grass_randomizer")) {
						GUI.Toggle(new Rect(10f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), slotData["grass_randomizer"].ToString() == "1", $"Grass Randomizer");
					} else {
						GUI.Toggle(new Rect(10f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), false, $"Grass Randomizer");
					}
					//breakable_shuffle
					if (slotData.ContainsKey("breakable_shuffle")) {
						GUI.Toggle(new Rect(220f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), slotData["breakable_shuffle"].ToString() == "1", $"Shuffled Breakables");
					} else {
						GUI.Toggle(new Rect(220f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), false, $"Shuffled Breakables");
					}

					apHeight += 40f * guiScale;

					if (slotData.ContainsKey("shuffle_fuses")) {
						GUI.Toggle(new Rect(10f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), slotData["shuffle_fuses"].ToString() == "1", $"Shuffled Fuses");
					} else {
						GUI.Toggle(new Rect(10f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), false, $"Shuffled Fuses");
					}
					//breakable_shuffle
					if (slotData.ContainsKey("shuffle_bells")) {
						GUI.Toggle(new Rect(220f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), slotData["shuffle_bells"].ToString() == "1", $"Shuffled Bells");
					} else {
						GUI.Toggle(new Rect(220f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), false, $"Shuffled Bells");
					}
				} else {
					apHeight += 40f * guiScale;
					GUI.Toggle(new Rect(10f * guiScale, apHeight, 180f * guiScale, 30f * guiScale), false, "Keys Behind Bosses");
					GUI.Toggle(new Rect(220f * guiScale, apHeight, 210f * guiScale, 30f * guiScale), false, "Sword Progression");
					apHeight += 40f * guiScale;
					GUI.Toggle(new Rect(10f * guiScale, apHeight, 175f * guiScale, 30f * guiScale), false, "Start With Sword");
					GUI.Toggle(new Rect(220f * guiScale, apHeight, 175f * guiScale, 30f * guiScale), false, "Shuffled Abilities");
					apHeight += 40f * guiScale;
					GUI.Toggle(new Rect(10f * guiScale, apHeight, 175f * guiScale, 30f * guiScale), false, "Hexagon Quest");
					GUI.Toggle(new Rect(220f * guiScale, apHeight, 175f * guiScale, 30f * guiScale), false, "Fool Traps: Off");
					apHeight += 40f * guiScale;
					GUI.Toggle(new Rect(10f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), false, $"Entrance Randomizer");
					GUI.Toggle(new Rect(220f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), false, $"Shuffled Ladders");
					apHeight += 40f * guiScale;
					GUI.Toggle(new Rect(10f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), false, $"Grass Randomizer");
					GUI.Toggle(new Rect(220f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), false, $"Shuffled Breakables");
					apHeight += 40f * guiScale;
					GUI.Toggle(new Rect(10f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), false, $"Shuffled Fuses");
					GUI.Toggle(new Rect(220f * guiScale, apHeight, 195f * guiScale, 30f * guiScale), false, $"Shuffled Bells");
				}
			}
			apHeight += 40f * guiScale;
			GUI.Label(new Rect(10f * guiScale, apHeight, 400f * guiScale, 30f * guiScale), $"Other Settings <size={(int)(18 * guiScale)}>(see in-game options menu!)</size>");
			apHeight += 40f * guiScale;
			TunicRandomizer.Settings.DeathLinkEnabled = GUI.Toggle(ShowTooltip(new Rect(10f * guiScale, apHeight, 130f * guiScale, 30f * guiScale), "archipelago", "Death Link"), TunicRandomizer.Settings.DeathLinkEnabled, "Death Link");
			TunicRandomizer.Settings.TrapLinkEnabled = GUI.Toggle(ShowTooltip(new Rect(220f * guiScale, apHeight, 130f * guiScale, 30f * guiScale), "archipelago", "Trap Link"), TunicRandomizer.Settings.TrapLinkEnabled, "Trap Link");
			apHeight += 40f * guiScale;
			TunicRandomizer.Settings.EnemyRandomizerEnabled = GUI.Toggle(ShowTooltip(new Rect(10f * guiScale, apHeight, 170f * guiScale, 30f * guiScale), "archipelago", "Enemy Randomizer"), TunicRandomizer.Settings.EnemyRandomizerEnabled, "Enemy Randomizer");
			TunicRandomizer.Settings.MusicShuffle = GUI.Toggle(ShowTooltip(new Rect(220f * guiScale, apHeight, 130f * guiScale, 30f * guiScale), "archipelago", "Music Shuffle"), TunicRandomizer.Settings.MusicShuffle, "Music Shuffle");
			apHeight += 40f * guiScale;
			GUI.skin.label.fontSize = (int)(20 * guiScale);*/
		}

		private static void ArchipelagoConfigEditorWindow(int windowID) {
			GUI.skin.label.fontSize = (int)(25 * guiScale);
			GUI.skin.button.fontSize = (int)(17 * guiScale);
			
			//Player name
			GUI.Label(new Rect(10f * guiScale, 20f * guiScale, 300f * guiScale, 30f * guiScale), $"Player: {textWithCursor(getConnectionSetting("Player"), editingFlags["Player"], true)}");
			
			bool EditPlayer = GUI.Button(new Rect(10f * guiScale, 70f * guiScale, 75f * guiScale, 30f * guiScale), editingFlags["Player"] ? "Save" : "Edit");
			if (EditPlayer) handleEditButton("Player");
			
			bool PastePlayer = GUI.Button(new Rect(100f * guiScale, 70f * guiScale, 75f * guiScale, 30f * guiScale), "Paste");
			if (PastePlayer) handlePasteButton("Player");
			
			bool ClearPlayer = GUI.Button(new Rect(190f * guiScale, 70f * guiScale, 75f * guiScale, 30f * guiScale), "Clear");
			if (ClearPlayer) handleClearButton("Player");

			//Hostname
			GUI.Label(new Rect(10f * guiScale, 120f * guiScale, 300f * guiScale, 30f * guiScale), $"Host: {textWithCursor(getConnectionSetting("Hostname"), editingFlags["Hostname"], true)}");
			
			bool setLocalhost = GUI.Toggle(new Rect(160f * guiScale, 160f * guiScale, 90f * guiScale, 30f * guiScale), FF1PR.SessionManager.GetGlobal	<string>("server") == "localhost", "localhost");
			if (setLocalhost && FF1PR.SessionManager.GetGlobal<string>("server") != "localhost") {
				setConnectionSetting("Hostname", "localhost");
				FF1PR.SessionManager.WriteGlobalData();

			}
			
			bool setArchipelagoHost = GUI.Toggle(new Rect(10f * guiScale, 160f * guiScale, 140f * guiScale, 30f * guiScale), FF1PR.SessionManager.GetGlobal<string>("server") == "archipelago.gg", "archipelago.gg");
			if (setArchipelagoHost && FF1PR.SessionManager.GetGlobal<string>("server") != "archipelago.gg") {
				setConnectionSetting("Hostname", "archipelago.gg");
				FF1PR.SessionManager.WriteGlobalData();
			}
			
			bool EditHostname = GUI.Button(new Rect(10f * guiScale, 200f * guiScale, 75f * guiScale, 30f * guiScale), editingFlags["Hostname"] ? "Save" : "Edit");
			if (EditHostname) handleEditButton("Hostname");
			
			bool PasteHostname = GUI.Button(new Rect(100f * guiScale, 200f * guiScale, 75f * guiScale, 30f * guiScale), "Paste");
			if (PasteHostname) handlePasteButton("Hostname");
			
			bool ClearHostname = GUI.Button(new Rect(190f * guiScale, 200f * guiScale, 75f * guiScale, 30f * guiScale), "Clear");
			if (ClearHostname) handleClearButton("Hostname");

			//Port
			GUI.Label(new Rect(10f * guiScale, 250f * guiScale, 300f * guiScale, 30f * guiScale), $"Port: {textWithCursor(getConnectionSetting("Port"), editingFlags["Port"], showPort)}");
			
			showPort = GUI.Toggle(new Rect(270f * guiScale, 260f * guiScale, 75f * guiScale, 30f * guiScale), showPort, "Show");
			
			bool EditPort = GUI.Button(new Rect(10f * guiScale, 300f * guiScale, 75f * guiScale, 30f * guiScale), editingFlags["Port"] ? "Save" : "Edit");
			if (EditPort) handleEditButton("Port");
			
			bool PastePort = GUI.Button(new Rect(100f * guiScale, 300f * guiScale, 75f * guiScale, 30f * guiScale), "Paste");
			if (PastePort) handlePasteButton("Port");
			
			bool ClearPort = GUI.Button(new Rect(190f * guiScale, 300f * guiScale, 75f * guiScale, 30f * guiScale), "Clear");
			if (ClearPort) handleClearButton("Port");

			//Password
			GUI.Label(new Rect(10f * guiScale, 350f * guiScale, 300f * guiScale, 30f * guiScale), $"Password: {textWithCursor(getConnectionSetting("Password"), editingFlags["Password"], showPassword)}");
			
			showPassword = GUI.Toggle(new Rect(270f * guiScale, 360f * guiScale, 75f * guiScale, 30f * guiScale), showPassword, "Show");
			bool EditPassword = GUI.Button(new Rect(10f * guiScale, 400f * guiScale, 75f * guiScale, 30f * guiScale), editingFlags["Password"] ? "Save" : "Edit");
			if (EditPassword) handleEditButton("Password");

			bool PastePassword = GUI.Button(new Rect(100f * guiScale, 400f * guiScale, 75f * guiScale, 30f * guiScale), "Paste");
			if (PastePassword) handlePasteButton("Password");
			
			bool ClearPassword = GUI.Button(new Rect(190f * guiScale, 400f * guiScale, 75f * guiScale, 30f * guiScale), "Clear");
			if (ClearPassword) handleClearButton("Password");
			
			//Close button
			bool Close = GUI.Button(new Rect(10f * guiScale, 450f * guiScale, 165f * guiScale, 30f * guiScale), "Close");
			if (Close) {
				CloseAPSettingsWindow();
				Archipelago.instance.Disconnect();
				Archipelago.instance.Connect();
			}

		}
		/*
		private static void MysterySeedWeightsWindow(int windowID) {
			GUI.skin.label.fontSize = (int)(20 * guiScale);
			GUI.skin.button.fontSize = (int)(17 * guiScale);
			mystHeight = 25f * guiScale;
			if (MysteryWindowPage == 0) {
				ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Sword Progression");
				ShowTooltip(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Hexagon Quest");
				GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Sword Progression");
				GUI.Label(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Hexagon Quest");
				mystHeight += 25f * guiScale;
				GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.SwordProgression}%");
				TunicRandomizer.Settings.MysterySeedWeights.SwordProgression = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.SwordProgression, 0, 100);

				GUI.Label(new Rect(360f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.HexagonQuest}%");
				TunicRandomizer.Settings.MysterySeedWeights.HexagonQuest = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexagonQuest, 0, 100);

				mystHeight += 40f * guiScale;
				ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Keys Behind Bosses");
				ShowTooltip(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Shuffle Abilities");
				GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Keys Behind Bosses");
				GUI.Label(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Shuffle Abilities");
				mystHeight += 25f * guiScale;
				GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.KeysBehindBosses}%");
				TunicRandomizer.Settings.MysterySeedWeights.KeysBehindBosses = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.KeysBehindBosses, 0, 100);
				GUI.Label(new Rect(360f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleAbilities}%");
				TunicRandomizer.Settings.MysterySeedWeights.ShuffleAbilities = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ShuffleAbilities, 0, 100);

				mystHeight += 40f * guiScale;
				ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Entrance Randomizer");
				ShowTooltip(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Decoupled Entrances");
				GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Entrance Randomizer");
				GUI.skin.label.fontSize = (int)(18 * guiScale);
				GUI.Label(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"ER: Decoupled Entrances");
				GUI.skin.label.fontSize = (int)(20 * guiScale);
				mystHeight += 25f * guiScale;
				GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.EntranceRando}%");
				TunicRandomizer.Settings.MysterySeedWeights.EntranceRando = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.EntranceRando, 0, 100);
				GUI.Label(new Rect(360f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ERDecoupled}%");
				TunicRandomizer.Settings.MysterySeedWeights.ERDecoupled = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ERDecoupled, 0, 100);
				
				mystHeight += 40f * guiScale;
				ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Fewer Shops");
				ShowTooltip(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Matching Directions");
				GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"ER: Fewer Shops");
				GUI.Label(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"ER: Matching Directions");
				mystHeight += 25f * guiScale;
				GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ERFixedShop}%");
				TunicRandomizer.Settings.MysterySeedWeights.ERFixedShop = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ERFixedShop, 0, 100);
				GUI.Label(new Rect(360f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ERDirectionPairs}%");
				TunicRandomizer.Settings.MysterySeedWeights.ERDirectionPairs = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ERDirectionPairs, 0, 100);

				mystHeight += 40f * guiScale;
				ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Shuffle Ladders");
				ShowTooltip(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Grass Randomizer");
				GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Shuffle Ladders");
				GUI.Label(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Grass Randomizer");
				mystHeight += 25f * guiScale;
				GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders}%");
				TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ShuffleLadders, 0, 100);
				// Todo add grass randomizer
				GUI.Label(new Rect(360f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.GrassRando}%");
				TunicRandomizer.Settings.MysterySeedWeights.GrassRando = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.GrassRando, 0, 100);

				mystHeight += 40f * guiScale;
				ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Shuffle Breakable Objects");
				ShowTooltip(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Shuffle Fuses");
				GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Shuffle Breakables");
				GUI.Label(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Shuffle Fuses");
				mystHeight += 25f * guiScale; 
				GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleBreakables}%");
				TunicRandomizer.Settings.MysterySeedWeights.ShuffleBreakables = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ShuffleBreakables, 0, 100);
				GUI.Label(new Rect(360f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleFuses}%");
				TunicRandomizer.Settings.MysterySeedWeights.ShuffleFuses = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ShuffleFuses, 0, 100);

				mystHeight += 40f * guiScale;
				ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Shuffle Bells");
				GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Shuffle Bells");
				mystHeight += 25f * guiScale;
				GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.ShuffleBells}%");
				TunicRandomizer.Settings.MysterySeedWeights.ShuffleBells = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.ShuffleBells, 0, 100);
			} else if (MysteryWindowPage == 1) {

				GUI.Label(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 30f * guiScale), $"Hexagon Quest - Goal Amount");
				ShowTooltip(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 125f * guiScale), "mysterySeed", "Randomize Hexagon Quest Amounts");
				mystHeight += 30f * guiScale;
				TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalRandom = GUI.Toggle(new Rect(10f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalRandom, "Random");
				TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalLow = GUI.Toggle(new Rect(110f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalLow, " Low");
				TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalMedium = GUI.Toggle(new Rect(210f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalMedium, "Medium");
				TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalHigh = GUI.Toggle(new Rect(310f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestGoalHigh, "High");

				mystHeight += 35f * guiScale;
				GUI.Label(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 30f * guiScale), $"Hexagon Quest - # of Extra Hexagons");
				mystHeight += 30f * guiScale;
				TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasRandom = GUI.Toggle(new Rect(10f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasRandom, "Random");
				TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasLow = GUI.Toggle(new Rect(110f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasLow, "Low");
				TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasMedium = GUI.Toggle(new Rect(210f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasMedium, "Medium");
				TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasHigh = GUI.Toggle(new Rect(310f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestExtrasHigh, "High");

				mystHeight += 35f * guiScale;
				GUI.Label(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 30f * guiScale), $"Hexagon Quest - Shuffled Abilities Unlock By");
				ShowTooltip(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 60f * guiScale), "mysterySeed", "Hexagon Quest Ability Shuffle Mode");
				mystHeight += 35f * guiScale;
				GUI.skin.label.fontSize = (int)(16 * guiScale); 
				GUI.Label(new Rect(10f * guiScale, mystHeight, 120f * guiScale, 30f * guiScale), $"Hexagons: {100 - TunicRandomizer.Settings.MysterySeedWeights.HexQuestAbilityShufflePages}%");
				GUI.Label(new Rect(320f * guiScale, mystHeight, 100f * guiScale, 30f * guiScale), $"Pages: {TunicRandomizer.Settings.MysterySeedWeights.HexQuestAbilityShufflePages}%");
				TunicRandomizer.Settings.MysterySeedWeights.HexQuestAbilityShufflePages = (int)GUI.HorizontalSlider(new Rect(120f * guiScale, mystHeight + 5f, 190f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.HexQuestAbilityShufflePages, 0, 100);
				GUI.skin.label.fontSize = (int)(20 * guiScale);

				mystHeight += 30f * guiScale;
				GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Fool Traps");
				ShowTooltip(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 60f * guiScale), "mysterySeed", "Fool Traps");
				mystHeight += 30f * guiScale;
				TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNone = GUI.Toggle(new Rect(10f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNone, $"None");
				TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNormal = GUI.Toggle(new Rect(110f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapNormal, $"<color={FoolColors[1]}>{FoolChoices[1]}</color>");
				TunicRandomizer.Settings.MysterySeedWeights.FoolTrapDouble = GUI.Toggle(new Rect(210f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapDouble, $"<color={FoolColors[2]}>{FoolChoices[2]}</color>");
				TunicRandomizer.Settings.MysterySeedWeights.FoolTrapOnslaught = GUI.Toggle(new Rect(300f * guiScale, mystHeight, 100f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.FoolTrapOnslaught, $"<color={FoolColors[3]}>{FoolChoices[3]}</color>");

				mystHeight += 35f * guiScale;
				GUI.Label(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 30f * guiScale), $"Hero's Laurels Location");
				ShowTooltip(new Rect(10f * guiScale, mystHeight, 400f * guiScale, 60f * guiScale), "mysterySeed", "Hero's Laurels Location");
				mystHeight += 30f * guiScale;
				TunicRandomizer.Settings.MysterySeedWeights.LaurelsRandom = GUI.Toggle(new Rect(10f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.LaurelsRandom, $"Random");
				TunicRandomizer.Settings.MysterySeedWeights.LaurelsSixCoins = GUI.Toggle(new Rect(110f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.LaurelsSixCoins, $"6 Coins");
				TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenCoins = GUI.Toggle(new Rect(210f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenCoins, $"10 Coins");
				TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenFairies = GUI.Toggle(new Rect(310f * guiScale, mystHeight, 90f * guiScale, 30f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.LaurelsTenFairies, $"10 Fairies");

				mystHeight += 40f * guiScale;
				ShowTooltip(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Lanternless Logic");
				ShowTooltip(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 75f * guiScale), "mysterySeed", "Maskless Logic");
				GUI.Label(new Rect(10f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Lanternless Logic");
				GUI.Label(new Rect(210f * guiScale, mystHeight, 300f * guiScale, 30f * guiScale), $"Maskless Logic");
				mystHeight += 25f * guiScale;
				GUI.Label(new Rect(160f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.Lanternless}%");
				TunicRandomizer.Settings.MysterySeedWeights.Lanternless = (int)GUI.HorizontalSlider(new Rect(10f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.Lanternless, 0, 100);
				GUI.Label(new Rect(360f * guiScale, mystHeight + 5, 45f * guiScale, 30f * guiScale), $"{TunicRandomizer.Settings.MysterySeedWeights.Maskless}%");
				TunicRandomizer.Settings.MysterySeedWeights.Maskless = (int)GUI.HorizontalSlider(new Rect(210f * guiScale, mystHeight + 15, 140f * guiScale, 20f * guiScale), TunicRandomizer.Settings.MysterySeedWeights.Maskless, 0, 100);
			}
			GUI.skin.button.fontSize = (int)(20 * guiScale);
			mystHeight += 40f * guiScale;
			bool RandomizeAll = GUI.Button(new Rect(10f * guiScale, mystHeight, 187.5f * guiScale, 30f * guiScale), "Randomize All");
			if (RandomizeAll) {
				TunicRandomizer.Settings.MysterySeedWeights.Randomize();
				RandomizerSettings.SaveSettings();
			}
			bool ResetToDefault = GUI.Button(new Rect(207.5f * guiScale, mystHeight, 187.5f * guiScale, 30f * guiScale), "Reset To Defaults");
			if (ResetToDefault) {
				TunicRandomizer.Settings.MysterySeedWeights = new MysterySeedWeights();
				RandomizerSettings.SaveSettings();
			}
			mystHeight += 40f * guiScale;
			bool Close = GUI.Button(new Rect(10f * guiScale, mystHeight, 187.5f * guiScale, 30f * guiScale), "Close");
			if (Close) {
				ShowMysterySeedWindow = false;
				MysteryWindowPage = 0;
				RandomizerSettings.SaveSettings();
			}
			bool SwitchPage = GUI.Button(new Rect(207.5f * guiScale, mystHeight, 187.5f * guiScale, 30f * guiScale), MysteryWindowPage == 0 ? "Next Page >" : "< Prev Page");
			if (SwitchPage) {
				MysteryWindowPage = MysteryWindowPage == 0 ? 1 : 0;
				RandomizerSettings.SaveSettings();
			}
			mystHeight += 40f * guiScale;
		}
		*/
		private static void CloseAPSettingsWindow() {
			ShowAPSettingsWindow = false;
			stringToEdit = "";
			clearAllEditingFlags();
			FF1PR.SessionManager.WriteGlobalData();
			Last.UI.KeyInput.TitleWindowController titleScreen = GameObject.FindObjectOfType<Last.UI.KeyInput.TitleWindowController>();
			if (titleScreen != null) {
				titleScreen.SetEnableMenu(true);
			}
		}

		public static bool TitleScreen___NewGame_PrefixPatch(Last.UI.KeyInput.TitleWindowController __instance) {
			//CloseAPSettingsWindow();
			/*
			RecentItemsDisplay.instance.ResetQueue();
			if (SaveFlags.IsArchipelago()) {
				Archipelago.instance.integration.ItemIndex = 0;
				Archipelago.instance.integration.ClearQueue();
			}*/
			return true;
		}
		/*
		public static bool FileManagement_LoadFileAndStart_PrefixPatch(FileManagementGUI __instance, string filename) {
			CloseAPSettingsWindow();
			SaveFile.LoadFromFile(filename);
			if (SaveFile.GetInt("archipelago") == 0 && SaveFile.GetInt("randomizer") == 0) {
				TunicLogger.LogInfo("Non-Randomizer file selected!");
				GenericMessage.ShowMessage("<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Non-Randomizer file selected.\"\n\"Returning to menu.\"");
				return false;
			}
			string errorMessage = "";
			if (SaveFile.GetInt("archipelago") == 1 && SaveFile.GetString("archipelago player name") != "") {
				if (!Archipelago.instance.IsConnected() || (Archipelago.instance.integration.connected && (SaveFile.GetString("archipelago player name") != Archipelago.instance.GetPlayerName(Archipelago.instance.GetPlayerSlot()) || int.Parse(Archipelago.instance.integration.slotData["seed"].ToString()) != SaveFile.GetInt("seed")))) {
					if (SaveFile.GetString(SaveFlags.ArchipelagoHostname) != "" && SaveFile.GetInt(SaveFlags.ArchipelagoPort) != 0) { 
						TunicRandomizer.Settings.ReadConnectionSettingsFromSaveFile();
					}
					Archipelago.instance.Disconnect();
					errorMessage = Archipelago.instance.Connect();
				}
				if (!Archipelago.instance.IsConnected()) {
					GenericMessage.ShowMessage($"<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Failed to connect to Archipelago:\"\n{errorMessage}\n\"Returning to title screen.\"");
					return false;
				} else if (SaveFlags.IsArchipelago()) {
					if (SaveFile.GetString("archipelago player name") != Archipelago.instance.GetPlayerName(Archipelago.instance.GetPlayerSlot()) || int.Parse(Archipelago.instance.integration.slotData["seed"].ToString()) != SaveFile.GetInt("seed")) {
						TunicLogger.LogInfo("Save does not match connected slot! Connected to " + TunicRandomizer.Settings.ConnectionSettings.Player + " [seed " + Archipelago.instance.integration.slotData["seed"].ToString() + "] but slot name in save file is " + SaveFile.GetString("archipelago player name") + " [seed " + SaveFile.GetInt("seed") + "]");
						GenericMessage.ShowMessage("<#FF0000>[death] \"<#FF0000>warning!\" <#FF0000>[death]\n\"Save does not match connected slot.\"\n\"Returning to menu.\"");
						return false;
					}
					PlayerCharacterSpawn.OnArrivalCallback += (Action)(() => {
						List<long> locationsInLimbo = new List<long>();
						foreach (KeyValuePair<string, long> pair in Locations.LocationIdToArchipelagoId) {
							if (SaveFile.GetInt("randomizer picked up " + pair.Key) == 1 && !Archipelago.instance.integration.session.Locations.AllLocationsChecked.Contains(pair.Value) && Archipelago.instance.integration.session.Locations.AllMissingLocations.Contains(pair.Value)) {
								locationsInLimbo.Add(pair.Value);
							}
						}
						if (locationsInLimbo.Count > 0) {
							LanguageLine line = ScriptableObject.CreateInstance<LanguageLine>();
							line.text = $"<#FFFF00>[death] \"<#FFFF00>attention!\" <#FFFF00>[death]\n" +
								$"\"Found {locationsInLimbo.Count} location{(locationsInLimbo.Count != 1 ? "s": "")} in the save file\"\n\"that {(locationsInLimbo.Count != 1 ? "were" : "was")} not sent to Archipelago.\"\n" +
								$"\"Send {(locationsInLimbo.Count != 1 ? "these" : "this")} location{(locationsInLimbo.Count != 1 ? "s" : "")} now?\"";
							if (TunicRandomizer.Settings.UseTrunicTranslations) {
								line.text = $"<#FFFF00>[death] <#FFFF00>uhtehn$uhn! <#FFFF00>[death]\nfownd \"{locationsInLimbo.Count}\" lOkA$uhn{(locationsInLimbo.Count != 1 ? "z" : "")} in #uh sAv fIl #aht\n{(locationsInLimbo.Count != 1 ? "wur" : "wawz")} nawt sehnt too RkipehluhgO.\nsehnd {(locationsInLimbo.Count != 1 ? "#Ez" : "#is")} lOkA$uhn{(locationsInLimbo.Count != 1 ? "z" : "")} now?";
							}
							GenericPrompt.ShowPrompt(line, (Action)(() => { Archipelago.instance.integration.session.Locations.CompleteLocationChecks(locationsInLimbo.ToArray()); }), (Action)(() => { }));
						}
					});
				}
			}

			return true;
		}*/

	}

	public class GUItest : MonoBehaviour
	{
		public string player = "";
		public string server = "";
		public string port = "";
		public string password = "";
		private void OnGUI()
		{


			
			GUI.BeginGroup(new Rect(20, 20, 300, 300));
			GUI.Label(new Rect(10, 10, 280, 20), "Archipelago Settings");
			GUI.Label(new Rect(10, 40, 50, 20), "Player:");
			GUI.Label(new Rect(10, 70, 50, 20), "Host:");
			GUI.Label(new Rect(10, 100, 50, 20), "Port:");
			GUI.Label(new Rect(10, 130, 50, 20), "Server:");

			/*
			player = GUI.TextField(new Rect(60, 40, 50, 20), player, 16);
			server = GUI.TextField(new Rect(60, 70, 50, 20), server);
			port = GUI.TextField(new Rect(60, 100, 50, 20), port);
			password = GUI.TextField(new Rect(60, 130, 50, 20), password);
			*/
			//GUI.Button(new Rect(10, 180, 100, 40), "Connect");
			//GUI.Button(new Rect(10, 250, 100, 40), "Disconnect");

			GUI.EndGroup();

			/*
			if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button"))
			{
				print("You clicked the button!");
			}*/
		}
	}
}
