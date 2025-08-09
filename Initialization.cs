using Last.Data.Master;
using Last.Data.User;
using Last.Entity.Field;
using Last.Interpreter;
using Last.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FF1PRAP
{
	partial class Initialization
	{
		public static void ApplyBaseGameModifications()
		{
			if (FF1PR.SessionManager.RandomizerInitialized)
			{
				//Archipelago.instance.Connect();
				return;
			}
			else
			{
				FF1PR.SessionManager.RandomizerInitialized = true;
			}

			InternalLogger.LogInfo($"Applying base game modifications.");

			// Create Ship Item
			FF1PR.MessageManager.AddMessage("MSG_KEY_NAME_18", "<IC_IOBJ>Ship");
			FF1PR.MessageManager.AddMessage("MSG_KEY_INF_18", "It's a ship, waddaya want me to say?");
			FF1PR.MasterManager.GetList<Item>().Add(43, new Item("43,43,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0"));
			FF1PR.MasterManager.GetList<Content>().Add(44, new Content("44,MSG_KEY_NAME_18,None,MSG_KEY_INF_18,0,1,43"));

			// Create AP Item
			FF1PR.MessageManager.AddMessage("MSG_KEY_NAME_19", "<IC_IOBJ>AP Item");
			FF1PR.MessageManager.AddMessage("MSG_KEY_INF_19", "AP Item, but we shouldn't be seeing this anyway.");
			FF1PR.MasterManager.GetList<Item>().Add(42, new Item("42,42,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0"));
			FF1PR.MasterManager.GetList<Content>().Add(43, new Content("43,MSG_KEY_NAME_19,None,MSG_KEY_INF_19,0,1,42"));

			// Update dialogues
			FF1PR.MessageManager.GetMessageDictionary()["MSG_NPC_GARLAND"] = "You really think you have what it takes to cross swords with ME? Very well... I, Garland, will knock you all down!!!";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_NPC_SARASAVE"] = "I am Sarah, princess of Cornelia. You must allow me to show my gratitude. Please, accompany me to Castle Cornelia.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_NPC_KINGSARA"] = "Thank you for returning my daughter to my side. There can be no doubt that you are the Warriors of Light from Lukahn's prophecy! I pray that you succeed in restoring the Crystals.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_SHIP_03"] = "I be most sorry, young masters. I'll be makin' no more fuss, I swear. Can ye find it in yer heart to fergive an old pirate?";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_ASTOS_01"] = "Muwahaha! You fools fell right into my trap! I AM Astos, king of the dark elves!";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_AWAKEPOT_01"] = "Oh, what's this? My crystal eye? Give it here! Don't worry, I have something to give you in exchange. Ahh! I can see! I can see again!";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_AWAKEELF_01"] = "This jolt tonic... may be just what we need to break the curse and awaken the prince!";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_AWAKEELF_03"] = "Am I still...dreaming? You...you're the legendary warriors! I shall heed the legend as it was told to me and my forefathers.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_CANAL_01"] = "Nitro powder! The explosive force in this powder will hae ma canal open in no time flat! Noo we can blast this rock tae smithereens!";

			FF1PR.MessageManager.GetMessageDictionary()["MSG_SAG_CAV_02"] = "I shall help only the true LIGHT WARRIORS. Prove yourself by defeating the Vampire.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_GET_STICK_01"] = "So you are the ones who defeated the vampire, eh? He was but a servant... The beast corrupting the Earth Crystal lurks much deeper.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_GET_CANOE_01"] = "Well done, Warriors of Light. You have defeated the Fiend of Earth and restored the Crystal's light. Take this, and go face the Fiend in Mount Gulg!";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_TRIAL_CAS_03"] = "Only those of royalty, bearing a crown, are worthy of undergoing the trials. Go away, peasant!";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_LUTESLAB"] = "There is a stone plate on the floor... You sense something... Evil?";

			FF1PR.MessageManager.GetMessageDictionary()["MSG_NPC_SARALUTE_01"] = $"This heirloom has been entrusted to the princesses of Cornelia for many generations. I want you to have it. It may aid you on your journey.";

			// ordeal man script
			var ordealsmanscript = new Script();
			ordealsmanscript.Id = 1000;
			ordealsmanscript.ScriptName = "sc_ordealsman";
			FF1PR.MasterManager.GetList<Script>().Add(1000, ordealsmanscript);

			// sub engineer
			var subengscript = new Script();
			subengscript.Id = 1003;
			subengscript.ScriptName = "sc_subeng";
			FF1PR.MasterManager.GetList<Script>().Add(1003, subengscript);

			// lute slab script
			var luteslabscript = new Script();
			luteslabscript.Id = 1001;
			luteslabscript.ScriptName = "sc_luteslab";
			FF1PR.MasterManager.GetList<Script>().Add(1001, luteslabscript);

			// chaos defeated script
			var chaosdefeated = new Script();
			chaosdefeated.Id = 1002;
			chaosdefeated.ScriptName = "sc_chaosdefeated";
			FF1PR.MasterManager.GetList<Script>().Add(1002, chaosdefeated);
		}

		public static void InitializeRandoItems(RandomizerData randoData)
		{
			InternalLogger.LogInfo($"Initialization Message.");

			var saraitem = GetPlacedItemName((int)TreasureFlags.Princess);
			var bikkeitem = GetPlacedItemName((int)TreasureFlags.Bikke);
			var marshitem = GetPlacedItemName((int)TreasureFlags.MarshChest);
			var astositem = GetPlacedItemName((int)TreasureFlags.Astos);
			var matoyaitem = GetPlacedItemName((int)TreasureFlags.Matoya);
			var elfprinceitem =	GetPlacedItemName((int)TreasureFlags.ElfPrince);
			var sardaitem = GetPlacedItemName((int)TreasureFlags.Sarda);
			var canoesageitem = GetPlacedItemName((int)TreasureFlags.CanoeSage);
			var eyechestitem = GetPlacedItemName((int)TreasureFlags.EyeChest);
			var fairyitem = GetPlacedItemName((int)TreasureFlags.Fairy);
			var cubebotitem = GetPlacedItemName((int)TreasureFlags.CubeBot);
			var lefeinitem = GetPlacedItemName((int)TreasureFlags.Lefeinman);
			var skyitem = GetPlacedItemName((int)TreasureFlags.SkyChest);
			var smittitem = GetPlacedItemName((int)TreasureFlags.Smitt);

			InternalLogger.LogInfo($"Initialization GetItem.");

			FF1PR.MessageManager.GetMessageDictionary()["MSG_NPC_SARALUTE_02"] = $"You obtain {saraitem}.";

			FF1PR.MessageManager.GetMessageDictionary()["MSG_SHIP_04"] = $"You obtain {bikkeitem}.";
			//FF1PR.MessageManager.GetMessageDictionary()["MSG_GET_CROWN_01"] = $"You obtain {marshitem}.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_ASTOS_04"] = $"You obtain {astositem}.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_AWAKEPOT_03"] = $"You obtain {matoyaitem}.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_AWAKEELF_05"] = $"You obtain {elfprinceitem}.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_GET_STICK_02"] = $"You obtain {sardaitem}.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_GET_CANOE_02"] = $"You obtain {canoesageitem}.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_GET_FLOAT_01"] = $"You obtain {eyechestitem}.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_OXYALE_03"] = $"You obtain {fairyitem}.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_GET_WARPCUBE_03"] = $"You obtain {cubebotitem}.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_GET_CHIME_02"] = $"You obtain {lefeinitem}.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_WND_DAN_04"] = $"You obtain {skyitem}.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_GET_EXCALIBAR_05"] = $"You obtain {smittitem}.";

			// Apply randomized data
			Randomizer.LoadShuffledShops(randoData.GearShops);
			Randomizer.LoadShuffledSpells(randoData.ShuffledSpells);
		}

		public static void InitializeNewGame()
		{
			InternalLogger.LogInfo($"Initialize New Game.");

			// Start with airship
			// So 517 is ship 100%, 516 is canoe, what's 519 for???
			/*
			for (int i = 0; i < FF1PR.UserData.OwnedTransportationList.Count; i++)
			{
				if (FF1PR.UserData.OwnedTransportationList[i].flagNumber == 518)
				{
					FF1PR.UserData.OwnedTransportationList[i].Position = new Vector3(144, 159, 149);
					FF1PR.UserData.OwnedTransportationList[i].MapId = 1;
					FF1PR.UserData.OwnedTransportationList[i].Direction = 2;
					FF1PR.UserData.OwnedTransportationList[i].SetDataStorageFlag(true);
				}
			}*/

			//FF1PR.OwnedItemsClient.AddOwnedItem((int)Items.Masamune, 4);
			
			// Set Flags
			FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 1, 1); // Force visit King in Coneria
			FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 4, 1); // Bridge Building Cutscene
			FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 6, 1); // Bridge 
			FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 7, 1); // Bridge Intro
			FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 8, 1); // Matoya Cutscene

		}

		public static string GetPlacedItemName(int flag)
		{
			if (FF1PR.PlacedItems.TryGetValue(flag, out var itemdata))
			{
				if (itemdata.Id == (int)Items.Gil)
				{
					return itemdata.Qty + " Gil";
				}
				else
				{
					var itemNameKey = FF1PR.MasterManager.GetList<Content>()[itemdata.Id].MesIdName;
					var itemName = FF1PR.MessageManager.GetMessage(itemNameKey);
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
