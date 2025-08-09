using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FF1PRAP.UI
{
	public static class WindowTexture
	{
		public static Texture2D GenerateWindowTexture(Rect standardWindowRect)
		{
			InternalLogger.LogInfo("Generating Skin.");

			var skin = GUI.skin;
			//GUISkin test = new();
			var width = (int)standardWindowRect.width;
			var height = (int)standardWindowRect.height;

			var texture = new Texture2D((int)standardWindowRect.width, (int)standardWindowRect.height);

			// This is a pretty ugly way to generate the texture, revisit using the game's window texture
			// var alltexture = Resources.FindObjectsOfTypeAll<Texture2D>().ToList();
			// UI_Common-WindowFrame01?

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
			//int setindent = 4;

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
					else if (intArray[x + (y * width)] == 2)
					{
						colorArray[x + (y * width)] = whiteColor;
					}
				}
			}

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

			texture.SetPixels(colorArray);
			texture.Apply();

			return texture;
		}
		private static void DrawBorders(ref int[] colorArray, int width, int height, int borderwidth, int setindent, int offset, int color)
		{
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
		public static GUISkin CreateWindowSyle(Texture2D windowTexture)
		{
			var skin = GUI.skin;

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

			return skin;
		}
		/*
		private void GenerateSkin()
		{
			InternalLogger.LogInfo("Generating Skin.");

			var skin = GUI.skin;
			//GUISkin test = new();
			var width = (int)standardWindowRect.width;
			var height = (int)standardWindowRect.height;

			var texture = new Texture2D((int)standardWindowRect.width, (int)standardWindowRect.height);

			// This is a pretty ugly way to generate the texture, revisit using the game's window texture
			// var alltexture = Resources.FindObjectsOfTypeAll<Texture2D>().ToList();
			// UI_Common-WindowFrame01?

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
			//int setindent = 4;

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
					else if (intArray[x + (y * width)] == 2)
					{
						colorArray[x + (y * width)] = whiteColor;
					}
				}
			}

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

			texture.SetPixels(colorArray);
			texture.Apply();

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

			windowSkin = skin;
		}*/

	}
}
