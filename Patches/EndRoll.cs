using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.Text.RegularExpressions;
using Last.Interpreter;
using Last.Systems.EndRoll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FF1PRAP
{
	partial class Patches
	{
		public static void CheckGroupLoadAssetCompleted_Post(string groupName)
		{
			if (groupName == "key_endroll")
			{
				var assetName = "stuff_ja";

				if (GameData.ResourceManager.completeAssetDic.ContainsKey("Assets/GameAssets/Serial/Res/EndRoll/" + assetName))
				{
					var text = GameData.ResourceManager.completeAssetDic["Assets/GameAssets/Serial/Res/EndRoll/" + assetName].TryCast<TextAsset>().text;

					text = text.Replace("STUFF_SQDEV_01_02\tYoshinori Kitase", "STUFF_SQDEV_01_02\t<color=\"#43deff\">SUPERVISOR</color>\nYoshinori Kitase");
					text = text.Replace("STUFF_SQDEV_01_01\t<color=\"#43deff\">SUPERVISOR</color>", "STUFF_SQDEV_01_01\t" + CreditAsset.CreditNames);

					var textasset = new TextAsset(UnityEngine.TextAsset.CreateOptions.CreateNativeObject, text);
					GameData.ResourceManager.completeAssetDic["Assets/GameAssets/Serial/Res/EndRoll/" + assetName] = textasset;
				}
				else
				{
					InternalLogger.LogInfo($"End Roll: No {assetName} found.");
				}

				assetName = "01_FF1_sqex_dev";

				if (GameData.ResourceManager.completeAssetDic.ContainsKey("Assets/GameAssets/Serial/Res/EndRoll/" + assetName))
				{
					var text = GameData.ResourceManager.completeAssetDic["Assets/GameAssets/Serial/Res/EndRoll/" + assetName].TryCast<TextAsset>().text;

					text = text.Replace("\n      \"STUFF_SQDEV_01_01\",", "");
					var index = text.IndexOf("      \"STUFF_TITLE_00_01\",");
					text = text.Insert(index, "      \"STUFF_SQDEV_01_01\",\n");

					var textasset = new TextAsset(UnityEngine.TextAsset.CreateOptions.CreateNativeObject, text);
					GameData.ResourceManager.completeAssetDic["Assets/GameAssets/Serial/Res/EndRoll/" + assetName] = textasset;
				}
				else
				{
					InternalLogger.LogInfo($"End Roll: No {assetName} found.");
				}
			}

		}
	}

	public static class CreditAsset
	{
		public static string CreditNames = "<color=\"#ffffcc\"><b>FF1\\nPIXEL REMASTER\\nArchipelago</b></color>\n\n\n\n" +
			"<color=\"#43deff\">MAIN DEVELOPER</color>\n" +
			"wildham\n\n\n" +
			"<color=\"#43deff\">PLAYTESTERS</color>\n" +
			"Prime\n\n\n" +
			"<color=\"#43deff\">SPECIAL THANKS</color>\n" +
			"silentdestroyer\n" +
			"Scipio\n" +
			"Silvris\n" +
			"FFR Dev Team\n" +
			"FFR Community\n" +
			"FFMQR Community\n" +
			"\n\n\n\n\n";

	}
}
