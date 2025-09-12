using Last.Data.Master;
using Last.Data.User;
using Last.Entity.Field;
using Last.Interpreter;
using Last.Management;
using RomUtilities;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FF1PRAP
{
	public static class Initialization
	{
		// This is always applied, this is structural change that creates the basis for the randomizer
		public static void ApplyBaseGameModifications()
		{
			if (SessionManager.RandomizerInitialized)
			{
				return;
			}
			else
			{
				SessionManager.RandomizerInitialized = true;
			}

			InternalLogger.LogInfo($"Applying base game modifications.");

			// Create Ship Item
			GameData.MessageManager.AddMessage("MSG_KEY_NAME_18", "<IC_IOBJ>Ship");
			GameData.MessageManager.AddMessage("MSG_KEY_INF_18", "It's a ship, waddaya want me to say?");
			GameData.MasterManager.GetList<Item>().Add(43, new Item("43,43,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0"));
			GameData.MasterManager.GetList<Content>().Add(44, new Content("44,MSG_KEY_NAME_18,None,MSG_KEY_INF_18,0,1,43"));

			// Create AP Item
			GameData.MessageManager.AddMessage("MSG_KEY_NAME_19", "<IC_IOBJ>AP Item");
			GameData.MessageManager.AddMessage("MSG_KEY_INF_19", "An Archipelago Item. What are these anyway?");
			GameData.MasterManager.GetList<Item>().Add(42, new Item("42,42,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0"));
			GameData.MasterManager.GetList<Content>().Add(43, new Content("43,MSG_KEY_NAME_19,None,MSG_KEY_INF_19,0,1,42"));

			// Create Lute Tablature
			GameData.MessageManager.AddMessage("MSG_TABLATURE_NAME", "<IC_IOBJ>Lute Tablature");
			GameData.MessageManager.AddMessage("MSG_TABLATURE_INF", "Some musical notation for the Lute. The melody seems beautiful, but the song is incomplete...");
			GameData.MasterManager.GetList<Item>().Add((int)Items.LuteTablature, new Item($"{(int)Items.LuteTablature},{(int)Items.LuteTablature},2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0"));
			GameData.MasterManager.GetList<Content>().Add((int)Items.LuteTablature, new Content($"{(int)Items.LuteTablature},MSG_TABLATURE_NAME,None,MSG_TABLATURE_INF,0,1,{(int)Items.LuteTablature}"));

			Randomizer.GeneratePromoItems();

			// Update dialogues
			GameData.MessageManager.GetMessageDictionary()["MSG_NPC_GARLAND"] = "You really think you have what it takes to cross swords with ME? Very well... I, Garland, will knock you all down!!!";
			GameData.MessageManager.GetMessageDictionary()["MSG_NPC_SARASAVE"] = "I am Sarah, princess of Cornelia. You must allow me to show my gratitude. Please, accompany me to Castle Cornelia.";
			GameData.MessageManager.GetMessageDictionary()["MSG_NPC_KINGSARA"] = "Thank you for returning my daughter to my side. There can be no doubt that you are the Warriors of Light from Lukahn's prophecy! May you succeed in restoring the Crystals.";
			GameData.MessageManager.GetMessageDictionary()["MSG_SHIP_03"] = "I be most sorry, young masters. I'll be makin' no more fuss, I swear. Can ye find it in yer heart to fergive an old pirate?";
			GameData.MessageManager.GetMessageDictionary()["MSG_ASTOS_01"] = "Muwahaha! You fools fell right into my trap! I AM Astos, king of the dark elves!";
			GameData.MessageManager.GetMessageDictionary()["MSG_AWAKEPOT_01"] = "Oh, what's this? My crystal eye? Give it here! Don't worry, I have something to give you in exchange. Ahh! I can see! I can see again!";
			GameData.MessageManager.GetMessageDictionary()["MSG_AWAKEELF_01"] = "This jolt tonic... may be just what we need to break the curse and awaken the prince!";
			GameData.MessageManager.GetMessageDictionary()["MSG_AWAKEELF_03"] = "Am I still...dreaming? You...you're the legendary warriors! I shall heed the legend as it was told to me and my forefathers.";
			GameData.MessageManager.GetMessageDictionary()["MSG_CANAL_01"] = "Nitro powder! The explosive force in this powder will hae ma canal open in no time flat! Noo we can blast this rock tae smithereens!";
			GameData.MessageManager.GetMessageDictionary()["MSG_SAG_CAV_02"] = "I shall help only the true LIGHT WARRIORS. Prove yourself by defeating the Vampire.";
			GameData.MessageManager.GetMessageDictionary()["MSG_GET_STICK_01"] = "So you are the ones who defeated the vampire, eh? He was but a servant... The beast corrupting the Earth Crystal lurks much deeper.";
			GameData.MessageManager.GetMessageDictionary()["MSG_GET_CANOE_01"] = "Well done, Warriors of Light. You have defeated the Fiend of Earth and restored the Crystal's light. Take this, and go face the Fiend in Mount Gulg!";
			GameData.MessageManager.GetMessageDictionary()["MSG_TRIAL_CAS_03"] = "Only those of royalty, bearing a crown, are worthy of undergoing the trials. Go away, peasant!";
			GameData.MessageManager.GetMessageDictionary()["MSG_LUTESLAB"] = "There is a stone plate on the floor... You sense something... Evil?";
			GameData.MessageManager.GetMessageDictionary()["MSG_GET_EXCALIBAR_02"] = "Hoots! I'll use this tae make ye the finest craft ye'll ever see!";
			GameData.MessageManager.GetMessageDictionary()["MSG_GET_EXCALIBAR_03"] = $"Done! In all ma years I've never crafted a finer object!";
			GameData.MessageManager.GetMessageDictionary()["MSG_OXYALE_01"] = "You're the ones who rescued me from that bottle!\nI'll get you somethiing from the bottom of the spring to thank you, okay?";
			GameData.MessageManager.GetMessageDictionary()["MSG_OXYALE_02"] = "I hope it will bring you luck as long as you have it!";
			GameData.MessageManager.GetMessageDictionary()["MSG_GAI_CTY_13"] = "If you wanna party with me\nBaby, there's a price to pay\nI'm a fairy in a bottle\nYou gotta release me right away<P>... How long have you been there?\nOh no, this is embarrassing...";
			GameData.MessageManager.GetMessageDictionary()["MSG_GET_WARPCUBE_01"] = "...I have BEEN waiting...\n...take this... ...TIAmat...flying fortress... ...please...";
			GameData.MessageManager.GetMessageDictionary()["MSG_GET_CHIME_01"] = "Before you leave, legendary warriors, take this with you.\nIt will help you in your fight to restore the Crystals.";
			GameData.MessageManager.GetMessageDictionary()["MSG_NPC_SARALUTE_01"] = $"This heirloom has been entrusted to the princesses of Cornelia for many generations. I want you to have it. It may aid you on your journey.";

			//InternalLogger.LogInfo(GameData.MessageManager.GetMessageDictionary()["STUFF_SQDEV_01_07"]);

			GameData.MasterManager.GetList<Script>().Add(1000, new Script() { Id = 1000, ScriptName = "sc_ordealsman" });		// ordeal man script
			GameData.MasterManager.GetList<Script>().Add(1001, new Script() { Id = 1001, ScriptName = "sc_luteslab" });		// lute slab script
			GameData.MasterManager.GetList<Script>().Add(1002, new Script() { Id = 1002, ScriptName = "sc_chaosdefeated" });	// chaos defeated script
			GameData.MasterManager.GetList<Script>().Add(1003, new Script() { Id = 1003, ScriptName = "sc_subeng" });			// sub engineer
			GameData.MasterManager.GetList<Script>().Add(1004, new Script() { Id = 1004, ScriptName = "sc_bahamut" });          // bahamut
			GameData.MasterManager.GetList<Script>().Add(1005, new Script() { Id = 1005, ScriptName = "sc_garlandpostbattle" });   // 
			GameData.MasterManager.GetList<Script>().Add(1006, new Script() { Id = 1006, ScriptName = "sc_princessnowarp" });   // 
			GameData.MasterManager.GetList<Script>().Add(1007, new Script() { Id = 1007, ScriptName = "sc_icecavernwap" });   // 

			// Trials Maze scripts
			GameData.MasterManager.GetList<Script>().Add(1010, new Script() { Id = 1010, ScriptName = "sc_ordeals_1010" });
			GameData.MasterManager.GetList<Script>().Add(1011, new Script() { Id = 1011, ScriptName = "sc_ordeals_1011" });
			GameData.MasterManager.GetList<Script>().Add(1012, new Script() { Id = 1012, ScriptName = "sc_ordeals_1012" });
			GameData.MasterManager.GetList<Script>().Add(1013, new Script() { Id = 1013, ScriptName = "sc_ordeals_1013" });
			GameData.MasterManager.GetList<Script>().Add(1014, new Script() { Id = 1014, ScriptName = "sc_ordeals_1014" });

			// give buy price to ribbon
			var ribbon = GameData.MasterManager.GetData<Armor>(54).Buy = 65535;

			// formations
			/*
			var garlandform = GameData.MasterManager.GetData<MonsterParty>(350);

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

			Randomizer.ApplyBoost();
			Randomizer.ApplyChaos();
			Randomizer.ApplyLute();
			Randomizer.ApplyCrystals();

			GameData.MessageManager.GetMessageDictionary()["MSG_GET_EXCALIBAR_03"] = $"Done! In all ma years I've never crafted a finer {(randoData.SmittThingy == "" ? "object" : randoData.SmittThingy)}!";
			CreateCaravanItem();
		}
		public static void CreateCaravanItem()
		{
			if (SessionManager.GameMode == GameModes.Randomizer)
			{
				// 141 is the bottle product
				var caravanitem = Randomizer.Data.PlacedItems[(int)TreasureFlags.Caravan].Id;
				var caravanproduct = GameData.MasterManager.GetData<Product>(141);
				caravanproduct.ContentId = caravanitem;

				if (caravanitem >= (int)Items.CaravanItem && caravanitem <= (int)Items.Canoe)
				{
					var keyitemid = GameData.MasterManager.GetData<Content>(caravanitem).TypeValue;
					GameData.MasterManager.GetData<Item>(keyitemid).Buy = 40000;
				}
			}
			else if (SessionManager.GameMode == GameModes.Archipelago)
			{
				ApLocationData caravanlocation;
				if (!Randomizer.ApLocations.TryGetValue((int)TreasureFlags.Caravan, out caravanlocation))
				{
					// jank backward compatibility, remove down the road
					return;	
				}

				GameData.MessageManager.GetMessageDictionary()["MSG_KEY_NAME_19"] = "<IC_IOBJ>" + caravanlocation.Content;
				var caravanproduct = GameData.MasterManager.GetData<Product>(141);
				caravanproduct.ContentId = (int)Items.CaravanItem;

				var keyitemid = GameData.MasterManager.GetData<Content>((int)Items.CaravanItem).TypeValue;
				GameData.MasterManager.GetData<Item>(keyitemid).Buy = 40000;
			}
		}
		public static void InitializeNewGame()
		{
			InternalLogger.LogInfo($"Initialize New Game.");
			
			//DevHacks();

			// Set Flags
			GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 1, 1); // Force visit King in Coneria
			GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 4, 1); // Bridge Building Cutscene
			GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 7, 1); // Bridge Intro
			GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, 8, 1); // Matoya Cutscene

			// Process Randomizer Options
			Randomizer.InitializeEarlyProgression();
			Randomizer.InitializeNorthernDocks();
			Randomizer.InitializeJobPromotions();
			Randomizer.InitializeBoost();
			Randomizer.InitializeTransportation();
			Randomizer.InitializeLute();
			Randomizer.InitializeCrystals();
			Randomizer.InitializeEntrances();
		}
		public static void DevHacks()
		{
			// Start with airship
			// So 517 is ship 100%, 516 is canoe, what's 519 for???
			for (int i = 0; i < GameData.UserData.OwnedTransportationList.Count; i++)
			{
				if (GameData.UserData.OwnedTransportationList[i].flagNumber == 518)
				{
					GameData.UserData.OwnedTransportationList[i].Position = new Vector3(144, 159, 149);
					GameData.UserData.OwnedTransportationList[i].MapId = 1;
					GameData.UserData.OwnedTransportationList[i].Direction = 2;
					GameData.UserData.OwnedTransportationList[i].SetDataStorageFlag(true);
				}
				else if (GameData.UserData.OwnedTransportationList[i].flagNumber == 516)
				{
					GameData.UserData.OwnedTransportationList[i].Position = new UnityEngine.Vector3(1000, 1000, 0);
					GameData.UserData.OwnedTransportationList[i].MapId = 1;
					GameData.UserData.OwnedTransportationList[i].Direction = 2;
					GameData.UserData.OwnedTransportationList[i].SetDataStorageFlag(true);
				}
				else if (GameData.UserData.OwnedTransportationList[i].flagNumber == 517)
				{
					// Coneria dock is 145, 162
					// Pravoka dock is 203, 146
					GameData.UserData.OwnedTransportationList[i].Position = new UnityEngine.Vector3(145, 162, 149);
					GameData.UserData.OwnedTransportationList[i].MapId = 1;
					GameData.UserData.OwnedTransportationList[i].Direction = 2;
					GameData.UserData.OwnedTransportationList[i].SetDataStorageFlag(true);
				}
			}

			GameData.OwnedItemsClient.AddOwnedItem((int)Items.Masamune, 4);
			GameData.OwnedItemsClient.AddOwnedItem((int)Items.IceArmor, 4);
			GameData.OwnedItemsClient.AddOwnedItem((int)Items.Ribbon, 4);
			GameData.OwnedItemsClient.AddOwnedItem((int)Items.IceShield, 4);
			GameData.OwnedItemsClient.AddOwnedItem((int)Items.Elixir, 99);
			GameData.OwnedItemsClient.AddOwnedItem((int)Items.PhoenixDown, 99);
			GameData.OwnedItemsClient.AddOwnedItem((int)Items.RatsTail, 1);
			GameData.OwnedItemsClient.AddOwnedItem((int)Items.Canoe, 1);
			GameData.OwnedItemsClient.AddOwnedItem((int)Items.RosettaStone, 1);
			//GameData.OwnedItemsClient.AddOwnedItem((int)Items.JoltTonic, 1);
			GameData.OwnedItemsClient.AddOwnedItem((int)Items.Oxyale, 1);
			GameData.OwnedItemsClient.AddOwnedItem((int)Items.BottledFaerie, 1);
			GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.Canal, 1); // Force visit King in Coneria
		}
	}
}
