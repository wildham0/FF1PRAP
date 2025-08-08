using Last.Management;
using Last.UI.KeyInput;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace FF1PRAP
{
	public partial class SaveInfoWindow : MonoBehaviour
	{
		public static Font PixelRemasterFont;
		private static float guiScale = 1f;
		private static int screenWidth = 0;
		private static int screenHeight = 0;
		private static float standardFontSize = 15f;

		private static int currentCursor = 0;
		private static int previousCursor = 1;
		private static int currentIndex = 0;
		private static string currentInfo = "";

		private static GUISkin windowSkin;
		private void OnGUI()
		{
			bool titleLoadScreen = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Title" &&
				FF1PR.TitleWindowController != null &&
				FF1PR.LoadGameWindowController != null &&
				FF1PR.SaveListController != null &&
				FF1PR.TitleWindowController.stateMachine.Current == Last.UI.KeyInput.TitleWindowController.State.LoadGame &&
				FF1PR.LoadGameWindowController.stateMachine.Current == Last.UI.KeyInput.LoadGameWindowController.State.Select;

			bool menuLoadScreen = FF1PR.StateTracker != null &&
				FF1PR.SaveListController != null &&
				FF1PR.MainMenuController != null &&
				FF1PR.LoadWindowController != null &&
				FF1PR.StateTracker.CurrentState == Last.Management.GameStates.InGame &&
				FF1PR.MainMenuController.stateMachine.Current == Last.Defaine.MenuCommandId.Load &&
				FF1PR.LoadWindowController.stateMachine.Current == LoadWindowController.State.Select;

			bool menuSaveScreen = FF1PR.StateTracker != null &&
				FF1PR.SaveListController != null &&
				FF1PR.MainMenuController != null &&
				FF1PR.SaveWindowController != null &&
				FF1PR.StateTracker.CurrentState == Last.Management.GameStates.InGame &&
				FF1PR.MainMenuController.stateMachine.Current == Last.Defaine.MenuCommandId.Save &&
				FF1PR.SaveWindowController.stateMachine.Current == Last.UI.KeyInput.SaveWindowController.State.Select;

			if (titleLoadScreen || menuLoadScreen || menuSaveScreen)
			{
				// initial draw or screen was resized, redraw texture
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

					windowSkin = GUI.skin;

					if (PixelRemasterFont == null)
					{
						PixelRemasterFont = Resources.FindObjectsOfTypeAll<Font>().Where(f => f.name == "FOT-NewCezannePro-B").ToList()[0];
					}

					GUI.skin = windowSkin;
					GUI.skin.font = PixelRemasterFont;
					GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				}

				GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 0f);
				GUI.color = new Color(1.0f, 1.0f, 1.0f, 0.98f);

				previousCursor = currentCursor;
				currentCursor = FF1PR.SaveListController.selectCursor.Index;

				if (previousCursor != currentCursor)
				{
					switch (FF1PR.SaveListController.selectCursor.Index)
					{
						case 0:
							currentIndex = 21; // autosave 21
							break;
						case 1:
							currentIndex = 22; // quicksave is 22
							break;
						default:
							currentIndex = FF1PR.SaveListController.selectCursor.Index + (menuSaveScreen ? 1 : - 1);
							break;
					}

					currentInfo =  FF1PR.SessionManager.GetSlotInfo(currentIndex);
				}

				var cursor = FF1PR.SaveListController.selectCursor;
				var worldpos = RectTransformUtility.WorldToScreenPoint(Camera.current, cursor.rect.position);
				var followRect = new Rect(Screen.width * 0.45f, (Screen.height - worldpos.y) - Screen.height * 0.035f, 300f * guiScale, 300f * guiScale);

				if (currentInfo != "" && FF1PR.SaveListController.watcher.IsInRange())
				{
					GUI.Window(400, followRect, new Action<int>(SaveInfowindow), "", GUI.skin.window);
				}
			}
		}
		private void Update() { }

		public static Rect ScaledRect(float x, float y, float width, float height)
		{
			return new Rect(x * guiScale, y * guiScale, width * guiScale, height * guiScale);
		}

		private static void SaveInfowindow(int windowID)
		{
			GUI.skin.label.fontSize = (int)(standardFontSize * 1.2 * guiScale);
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.clipping = TextClipping.Overflow;
			GUI.skin.label.richText = true;
			GUI.color = new Color(1f, 1f, 1f, 1.0f);
			GUI.contentColor = new Color(1f, 1f, 1f, 1.0f);

			GUI.Label(ScaledRect(0f, 0f, 260f, 120f), currentInfo, GUI.skin.label);
		}
	}
}
