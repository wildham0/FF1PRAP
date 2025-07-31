using Il2CppSystem.Common;
using Last.Data.Master;
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
using UnityEngine;

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

				InternalLogger.LogInfo($"Flag {flag} set by item");

				// Update transport
				for (int i = 0; i < FF1PR.UserData.OwnedTransportationList.Count; i++)
				{
					if (targetData.Id == (int)Items.Ship && FF1PR.UserData.OwnedTransportationList[i].flagNumber == 517)
					{
						// Coneria dock is 145, 162
						// Pravoka dock is 203, 146
						FF1PR.UserData.OwnedTransportationList[i].Position = new UnityEngine.Vector3(203, 146, 149);
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
			//InternalLogger.LogInfo($"Add Owned Content: {targetData.Id} - {targetData.MesIdName}, {count}");
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


				//var test = new Il2CppSystem.Collections.Generic.List<Last.Systems.Message.BaseContent>();

				/*
				if (MessageWindowManager.instance != null)
				{
					Il2CppSystem.Collections.Generic.List<Last.Systems.Message.BaseContent> test = Last.Systems.Message.MessageParser.Parse($"Hello, hello! {itemdata.Id}");
					//MessageWindowManager.instance.EndWaitExit();
					MessageWindowManager.instance.Close();
					//MessageWindowManager.instance.SetNextState(MessageWindowManager.State.None);
					if (itemdata.Id == 4)
					{
						//MessageWindowManager.instance.SetWindowType(Last.Management.WindowType.Field);
						MessageWindowManager.instance.SetContent(test);
						MessageWindowManager.instance.SetAnchor(Last.Management.WindowType.Battle, Last.Management.WindowAnchor.Under);
						MessageWindowManager.instance.ShowWindow(true);
						MessageWindowManager.instance.Play();
					}



				}
				*/

				//var test = new Last.Interpreter.Instructions.Message.MessageWindowParams(1, 2);
				//Message.PlayMessageCommon("MSG_WND_DAN_04", test, true);
				return ItemResults.Success;
			}
			else
			{
				//var test = FF1PR.UserData.
				
				return ItemResults.Busy;
			}
		}
	}
}
