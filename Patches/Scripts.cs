using Last.Entity.Field;
using Last.Map;
using LibCpp2IL.NintendoSwitch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Text.Json;
using static Last.Map.Custom.MapCustomProperties;
using Last.Data.Master;
using UnityEngine.Assertions;

namespace FF1PRAP
{
	partial class Patches
	{
		public static void GetScript_Postfix(string scriptName, ref TextAsset __result, ref MapAssetData __instance)
		{
			InternalLogger.LogTesting($"Running {scriptName} on {GameData.CurrentMap}");

			if (Randomizer.ScriptToReplace.TryGetValue(scriptName, out var script))
			{
				if (Randomizer.ScriptToItemFlag.TryGetValue(scriptName, out var locationflag))
				{
					if (SessionManager.GameMode == GameModes.Archipelago)
					{
						if (Randomizer.FlagToDialogue.TryGetValue(locationflag, out var dialogue))
						{
							var location = Randomizer.ApLocations[locationflag];

							GameData.MessageManager.GetMessageDictionary()[dialogue] = $"You obtained {location.Content}.";

							script = script.Replace("RANDOITEM", $"{43}");
							script = script.Replace("RANDOQTY", $"{0}");
							script = script.Replace("CHESTFLAG", $"{locationflag}");
						}
					}
					else if (Randomizer.Data.PlacedItems.TryGetValue(locationflag, out var item))
					{
						if (Randomizer.FlagToDialogue.TryGetValue(locationflag, out var dialogue))
						{
							var itemname = GetPlacedItemName(locationflag);

							GameData.MessageManager.GetMessageDictionary()[dialogue] = $"You obtained {itemname}.";

							script = script.Replace("RANDOITEM", $"{item.Id}");
							script = script.Replace("RANDOQTY", $"{item.Qty}");
							script = script.Replace("CHESTFLAG", $"{locationflag}");
						}
					}
					else
					{
						throw new Exception("Error. No item placed at this game object.");
					}
				}

				//InternalLogger.LogInfo($"{script}");
				TextAsset scriptAsset = new TextAsset(UnityEngine.TextAsset.CreateOptions.CreateNativeObject, script);
				__result = scriptAsset;
			}
		}

		public static string GetPlacedItemName(int flag)
		{
			if (Randomizer.Data.PlacedItems.TryGetValue(flag, out var itemdata))
			{
				if (itemdata.Id == (int)Items.Gil)
				{
					return itemdata.Qty + " Gil";
				}
				else
				{
					var itemNameKey = GameData.MasterManager.GetList<Content>()[itemdata.Id].MesIdName;
					var itemName = GameData.MessageManager.GetMessage(itemNameKey);
					return itemName;
				}
			}
			else
			{
				return "ITEM_ERROR";
			}
		}
	}
}
