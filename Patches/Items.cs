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
			if (Randomizer.ItemIdToFlag.TryGetValue(targetData.Id, out var flag))
			{
				FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, flag, 1);

				InternalLogger.LogInfo($"Flag {flag} set by {(Items)targetData.Id}");

				UpdateEntities();

				// Update transport
				for (int i = 0; i < FF1PR.UserData.OwnedTransportationList.Count; i++)
				{
					if (targetData.Id == (int)Items.Ship && FF1PR.UserData.OwnedTransportationList[i].flagNumber == 517)
					{
						// Coneria dock is 145, 162
						// Pravoka dock is 203, 146

						(int x, int y) shipSpawn = (203, 146);
						if (FF1PR.DataStorage.Get(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.WestwardProgressionMode) == 1)
						{
							shipSpawn = (145, 162);
						}

						FF1PR.UserData.OwnedTransportationList[i].Position = new UnityEngine.Vector3(shipSpawn.x, shipSpawn.y, 149);
						FF1PR.UserData.OwnedTransportationList[i].MapId = 1;
						FF1PR.UserData.OwnedTransportationList[i].Direction = 2;
						FF1PR.UserData.OwnedTransportationList[i].SetDataStorageFlag(true);
					}
					else if (targetData.Id == (int)Items.Canoe && FF1PR.UserData.OwnedTransportationList[i].flagNumber == 516)
					{
						FF1PR.UserData.OwnedTransportationList[i].Position = new UnityEngine.Vector3(1000, 1000, 0);
						FF1PR.UserData.OwnedTransportationList[i].MapId = 1;
						FF1PR.UserData.OwnedTransportationList[i].Direction = 2;
						FF1PR.UserData.OwnedTransportationList[i].SetDataStorageFlag(true);
					}
				}

				// check map
				// run special script for mystic key, ship, canoe
			}

			Randomizer.ProcessJobItem(targetData.Id);
			//InternalLogger.LogInfo($"Add Owned Content: {targetData.Id} - {targetData.MesIdName}, {count}");
		}

		private static void BuyItemProduct_Post(ShopProductData data, int count, bool __result)
		{
			if (FF1PR.SessionManager.GameMode == GameModes.Randomizer)
			{
				// 141 is the caravan shop product id
				if (data.ProductId == 141 && __result)
				{
					FF1PR.DataStorage.Set(Last.Interpreter.DataStorage.Category.kTreasureFlag1, (int)TreasureFlags.Caravan, 1);
				}
			}
			else if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
			{
				if (data.ProductId == 141 && __result)
				{
					FF1PR.DataStorage.Set(Last.Interpreter.DataStorage.Category.kTreasureFlag1, (int)TreasureFlags.Caravan, 1);
					Archipelago.instance.ActivateCheck(Randomizer.FlagToLocationName[(int)TreasureFlags.Caravan]);
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
			if (FF1PR.OwnedItemsClient != null && FF1PR.GameState == GameStates.InGame)
			{
				FF1PR.OwnedItemsClient.AddOwnedItem(itemdata.Id, itemdata.Qty);

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
