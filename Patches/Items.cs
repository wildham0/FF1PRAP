using Il2CppSystem.Common;
using Last.Data;
using Last.Data.Master;
using Last.Data.User;
using Last.Entity.Field;
using Last.Interpreter;
using Last.Message;
using Last.Systems.EndRoll;
using Last.Systems.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine;
using static Serial.FF1.Management.StatusUpProvider;
using Last.Management;
using Il2CppSystem.Linq;

namespace FF1PRAP
{
	partial class Patches
	{
		public enum ItemResults
		{ 
			Success,
			Busy,
			Invalid
		}
		public static void Items_Postfix(Content targetData, int count)
		{
			if (Randomizer.ItemsToIgnore.Contains(targetData.Id))
			{
				return;
			}

			if (Randomizer.ItemIdToFlag.TryGetValue(targetData.Id, out var flag))
			{
				GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, flag, 1);

				InternalLogger.LogTesting($"Flag {flag} set by {(Items)targetData.Id}");

				UpdateEntities();
			}

			Randomizer.ProcessSpecialItems(targetData.Id);

			//InternalLogger.LogInfo($"Add Owned Content: {targetData.Id} - {targetData.MesIdName}, {count}");
		}

		private static void BuyItemProduct_Post(ShopProductData data, int count, bool __result)
		{
			if (SessionManager.GameMode == GameModes.Randomizer)
			{
				//InternalLogger.LogInfo($"Buy Item: {__result}");

				// 141 is the caravan shop product id
				if (Randomizer.KeyShopItems.TryGetValue(data.ProductId, out var itemflag) && __result)
				{
					//InternalLogger.LogInfo($"Buy Item: {__result} - {data.ProductId} - {itemflag}");
					GameData.DataStorage.Set(Last.Interpreter.DataStorage.Category.kTreasureFlag1, itemflag, 1);
				}
			}
			else if (SessionManager.GameMode == GameModes.Archipelago)
			{
				if (Randomizer.KeyShopItems.TryGetValue(data.ProductId, out var itemflag) && __result)
				{
					GameData.DataStorage.Set(Last.Interpreter.DataStorage.Category.kTreasureFlag1, itemflag, 1);
					Archipelago.instance.ActivateCheck(Randomizer.FlagToLocationName[itemflag]);
				}
			}
		}
		public static ItemResults GiveItem(string itemName, bool showMessage)
		{
			bool validItem = Randomizer.ItemNameToData.TryGetValue(itemName, out var itemdata);

			if (!validItem)
			{
				return ItemResults.Invalid;
			}

			// check if in state to give item
			if (GameData.OwnedItemsClient != null && GameData.GameState == GameStates.InGame)
			{
				if (!Randomizer.ItemsToIgnore.Contains(itemdata.Id))
				{
					GameData.OwnedItemsClient.AddOwnedItem(itemdata.Id, itemdata.Qty);
				}

				if (showMessage)
				{
					string itemMessage = $"You received {itemName}.";
					ApItemWindow.instance.QueueMessage(itemMessage);
				}

				return ItemResults.Success;
			}
			else
			{
				return ItemResults.Busy;
			}
		}
	}
}
