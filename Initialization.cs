using Last.Data.Master;
using Last.Data.User;
using Last.Entity.Field;
using Last.Interpreter;
using Last.Management;
using RomUtilities;
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
		// This is always applied, this is structural change that creates the basis for the randomizer
		public static void ApplyBaseGameModifications()
		{
			if (FF1PR.SessionManager.RandomizerInitialized)
			{
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
			FF1PR.MessageManager.AddMessage("MSG_KEY_INF_19", "An Archipelago Item. What are these anyway?");
			FF1PR.MasterManager.GetList<Item>().Add(42, new Item("42,42,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0"));
			FF1PR.MasterManager.GetList<Content>().Add(43, new Content("43,MSG_KEY_NAME_19,None,MSG_KEY_INF_19,0,1,42"));

			Randomizer.GeneratePromoItems();

			// Update dialogues
			FF1PR.MessageManager.GetMessageDictionary()["MSG_NPC_GARLAND"] = "You really think you have what it takes to cross swords with ME? Very well... I, Garland, will knock you all down!!!";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_NPC_SARASAVE"] = "I am Sarah, princess of Cornelia. You must allow me to show my gratitude. Please, accompany me to Castle Cornelia.";
			FF1PR.MessageManager.GetMessageDictionary()["MSG_NPC_KINGSARA"] = "Thank you for returning my daughter to my side. There can be no doubt that you are the Warriors of Light from Lukahn's prophecy! May you succeed in restoring the Crystals.";
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

			//InternalLogger.LogInfo(FF1PR.MessageManager.GetMessageDictionary()["STUFF_SQDEV_01_07"]);
			


			FF1PR.MasterManager.GetList<Script>().Add(1000, new Script() { Id = 1000, ScriptName = "sc_ordealsman" });		// ordeal man script
			FF1PR.MasterManager.GetList<Script>().Add(1001, new Script() { Id = 1001, ScriptName = "sc_luteslab" });		// lute slab script
			FF1PR.MasterManager.GetList<Script>().Add(1002, new Script() { Id = 1002, ScriptName = "sc_chaosdefeated" });	// chaos defeated script
			FF1PR.MasterManager.GetList<Script>().Add(1003, new Script() { Id = 1003, ScriptName = "sc_subeng" });			// sub engineer
			FF1PR.MasterManager.GetList<Script>().Add(1004, new Script() { Id = 1004, ScriptName = "sc_bahamut" });			// bahamut
			// Trials Maze scripts
			FF1PR.MasterManager.GetList<Script>().Add(1010, new Script() { Id = 1010, ScriptName = "sc_ordeals_1010" });
			FF1PR.MasterManager.GetList<Script>().Add(1011, new Script() { Id = 1011, ScriptName = "sc_ordeals_1011" });
			FF1PR.MasterManager.GetList<Script>().Add(1012, new Script() { Id = 1012, ScriptName = "sc_ordeals_1012" });
			FF1PR.MasterManager.GetList<Script>().Add(1013, new Script() { Id = 1013, ScriptName = "sc_ordeals_1013" });
			FF1PR.MasterManager.GetList<Script>().Add(1014, new Script() { Id = 1014, ScriptName = "sc_ordeals_1014" });

			// give buy price to ribbon
			var ribbon = FF1PR.MasterManager.GetData<Armor>(54).Buy = 65535;

			// formations
			/*
			var garlandform = FF1PR.MasterManager.GetData<MonsterParty>(350);

			garlandform.Monster1 = 111;
			garlandform.Monster1XPosition = 25;
			garlandform.Monster1YPosition = -20;
			garlandform.Monster2 = 1;*/
		}
		public static void ApplyRandomizedFeatures(RandomizerData randoData)
		{
			InternalLogger.LogInfo($"Applying randomizer data.");

			// Apply randomized data
			Randomizer.LoadShuffledShops(randoData.GearShops);
			Randomizer.LoadShuffledSpells(randoData.ShuffledSpells);
			Randomizer.LoadMonsterParties(randoData.MonsterParties);

			if (!Randomizer.RandomizerData.BoostMenu)
			{
				FF1PR.MessageManager.GetMessageDictionary()["MSG_SYSTEM_CS_0_006"] = "Boost as been disabled by your settings.";
			}

			if (Randomizer.RandomizerData.NerfChaos)
			{
				var chaos = FF1PR.MasterManager.GetData<Monster>(128);
				chaos.Hp /= 2;
				chaos.Intelligence = (int)(chaos.Intelligence * 0.75);
				chaos.Attack = (int)(chaos.Attack * 0.75);
			}

			CreateCaravanItem();
		}
		public static void CreateCaravanItem()
		{
			if (FF1PR.SessionManager.GameMode == GameModes.Randomizer)
			{
				// 141 is the bottle product
				var caravanitem = FF1PR.PlacedItems[(int)TreasureFlags.Caravan].Id;
				var caravanproduct = FF1PR.MasterManager.GetData<Product>(141);
				caravanproduct.ContentId = caravanitem;

				if (caravanitem >= (int)Items.CaravanItem && caravanitem <= (int)Items.Canoe)
				{
					var keyitemid = FF1PR.MasterManager.GetData<Content>(caravanitem).TypeValue;
					FF1PR.MasterManager.GetData<Item>(keyitemid).Buy = 40000;
				}
			}
			else if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
			{
				ApLocationData caravanlocation;
				if (!Randomizer.ApLocations.TryGetValue((int)TreasureFlags.Caravan, out caravanlocation))
				{
					// jank backward compatibility, remove down the road
					return;	
				}

				FF1PR.MessageManager.GetMessageDictionary()["MSG_KEY_NAME_19"] = "<IC_IOBJ>" + caravanlocation.Content;
				var caravanproduct = FF1PR.MasterManager.GetData<Product>(141);
				caravanproduct.ContentId = (int)Items.CaravanItem;

				var keyitemid = FF1PR.MasterManager.GetData<Content>((int)Items.CaravanItem).TypeValue;
				FF1PR.MasterManager.GetData<Item>(keyitemid).Buy = 40000;
			}
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
				else if (FF1PR.UserData.OwnedTransportationList[i].flagNumber == 516)
				{
					FF1PR.UserData.OwnedTransportationList[i].Position = new UnityEngine.Vector3(1000, 1000, 0);
					FF1PR.UserData.OwnedTransportationList[i].MapId = 1;
					FF1PR.UserData.OwnedTransportationList[i].Direction = 2;
					FF1PR.UserData.OwnedTransportationList[i].SetDataStorageFlag(true);
				}
				else if (FF1PR.UserData.OwnedTransportationList[i].flagNumber == 517)
				{
					// Coneria dock is 145, 162
					// Pravoka dock is 203, 146
					FF1PR.UserData.OwnedTransportationList[i].Position = new UnityEngine.Vector3(145, 162, 149);
					FF1PR.UserData.OwnedTransportationList[i].MapId = 1;
					FF1PR.UserData.OwnedTransportationList[i].Direction = 2;
					FF1PR.UserData.OwnedTransportationList[i].SetDataStorageFlag(true);
				}
			}

			FF1PR.OwnedItemsClient.AddOwnedItem((int)Items.Masamune, 4);
			FF1PR.OwnedItemsClient.AddOwnedItem((int)Items.IceArmor, 4);
			FF1PR.OwnedItemsClient.AddOwnedItem((int)Items.Ribbon, 4);
			FF1PR.OwnedItemsClient.AddOwnedItem((int)Items.IceShield, 4);
			FF1PR.OwnedItemsClient.AddOwnedItem((int)Items.Elixir, 99);
			FF1PR.OwnedItemsClient.AddOwnedItem((int)Items.RatsTail, 1);
			FF1PR.OwnedItemsClient.AddOwnedItem((int)Items.Canoe, 1);
			FF1PR.OwnedItemsClient.AddOwnedItem((int)Items.StarRuby, 1);
			FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.Canal, 1); // Force visit King in Coneria
			*/
			// Set Flags
			FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 1, 1); // Force visit King in Coneria
			FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 4, 1); // Bridge Building Cutscene
			FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 7, 1); // Bridge Intro
			FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 8, 1); // Matoya Cutscene

			// should be RandomizerData
			if (Randomizer.RandomizerData.EarlyProgression == Randomizer.EarlyProgressionModes.MarshPath)
			{
				FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.WestwardProgressionMode, 1);
				FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 6, 0); // Bridge 
			}
			else
			{
				FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.WestwardProgressionMode, 0);
				FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 6, 1); // Bridge 
			}

			if (Randomizer.RandomizerData.NorthernDocks)
			{
				FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.NorthernDocks, 1);
			}
			else
			{
				FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.NorthernDocks, 0);
			}

			if (Randomizer.RandomizerData.JobPromotion != Randomizer.JobPromotionModes.Bahamut)
			{
				FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.BahamutGivesItem, 1);
			}
			else
			{
				FF1PR.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.BahamutGivesItem, 0);
			}

			// Set New Game options only
			FF1PR.UserData.CheatSettingsData.GilRate = Randomizer.RandomizerData.GilBoost;
			FF1PR.UserData.CheatSettingsData.ExpRate = Randomizer.RandomizerData.XpBoost;
		}
	}
}
