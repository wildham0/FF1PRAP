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

namespace FF1PRAP
{
	partial class Patches
	{
		public static void GetScript_Postfix(string scriptName, ref TextAsset __result, ref MapAssetData __instance)
		{
			InternalLogger.LogInfo($"Running {scriptName} on {FF1PR.CurrentMap}");

			if (scriptReplacements.TryGetValue(scriptName, out var script))
			{
				if (Randomizer.ScriptToItemFlag.TryGetValue(scriptName, out var locationflag))
				{
					if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
					{
						if (Randomizer.FlagToDialogue.TryGetValue(locationflag, out var dialogue))
						{

							var location = Randomizer.ApLocations[locationflag];

							FF1PR.MessageManager.GetMessageDictionary()[dialogue] = $"You obtained {location.Content}.";

							script = script.Replace("RANDOITEM", $"{43}");
							script = script.Replace("RANDOQTY", $"{0}");
							script = script.Replace("CHESTFLAG", $"{locationflag}");
						}
					}
					else if (FF1PR.PlacedItems.TryGetValue(locationflag, out var item))
					{
						//InternalLogger.LogInfo($"{item.Id} - {item.Qty}");
						//InternalLogger.LogInfo($"{script}");
						script = script.Replace("RANDOITEM", $"{item.Id}");
						script = script.Replace("RANDOQTY", $"{item.Qty}");
						script = script.Replace("CHESTFLAG", $"{locationflag}");
					}
					else
					{
						throw new Exception("Error. No item placed at this game object.");
					}
				}

				//InternalLogger.LogInfo($"{script}");
				TextAsset scriptAsset = new TextAsset(UnityEngine.TextAsset.CreateOptions.CreateNativeObject, script);
				//scriptAsset.name = scriptName;
				//__instance.scriptList.Remove(scriptName);
				//__instance.SetScript(scriptName, scriptAsset, "map_10010", "Map_10010/sc_e_0001");
				__result = scriptAsset;

				

				
				//__instance.SetScript(scriptName, scriptAsset, scriptName, scriptName);
			}

			//InternalLogger.LogInfo($"{__result.text}");
		}

		public static void GetScript_Prefix(string scriptName, ref MapAssetData __instance)
		{
			//InternalLogger.LogInfo($"Asset Name: {asset.AssetName} in {asset.AssetGroup}");
			if (scriptName == "sc_map_30011_1")
			{
				var entitylist = __instance.eventEntityInfo;
				foreach (var entity in entitylist)
				{
					InternalLogger.LogInfo($"Key:{entity.Key}");
				}

				//__instance.GetEventEntityList[]
				if (entitylist.TryGetValue("ev_e_0003", out var entity3))
				{
					InternalLogger.LogInfo($"Entity step1 success.");

					var garland = entity3.ObjectMap.layers[0].objects.ToList().Find(o => o.id == 173);
					garland.GetPropertyByName("direction").value = "4";
					InternalLogger.LogInfo($"Entity step2 success.");
					garland.GetPropertyByName("turn_around").value = "true";
					InternalLogger.LogInfo($"Entity step3 success.");
				}
				else
				{
					InternalLogger.LogInfo($"Couldn't get entity script here.");
				}



			}

		}


		// we'll need to dynamically replace entities files so we can have chests for what, adamant, floater...

		public static Dictionary<string, string> scriptReplacements = new()
		{
			{ "sc_e_0001", ScriptBuilder.FromScript(Scripts.Intro, "sc_e_0001") }, // Intro script
			{ "sc_map_10010", ScriptBuilder.FromJson("sc_overworld") }, // Intro script
			// { "sc_map_20020", new ScriptBuilder("sc_empty") }, // Coneria Town
			 { "sc_map_20011_1", ScriptBuilder.FromJson("sc_coneriacastle") }, // Coneria Castle
			// { "sc_e_0002_2", new ScriptBuilder("sc_empty") }, // Go see the king
			// { "sc_e_0002_1", new ScriptBuilder("sc_empty") }, // Talk King
			{ "sc_map_30011_1", ScriptBuilder.FromScript(Scripts.TempleOfFiends, "sc_templeoffiends") }, // ToF Map
			{ "sc_e_0003", ScriptBuilder.FromScript(Scripts.Garland, "sc_garland_01") }, // Garland
			{ "sc_e_0003_2", ScriptBuilder.FromJson("sc_garland_02") }, // Post-fight
			{ "sc_e_0003_3", ScriptBuilder.FromJson("sc_garland_03") }, // At the castle?
			{ "sc_e_0004_1", ScriptBuilder.FromScript(Scripts.Princess, "sc_princess_01") }, // Talk Princess
			//{ "sc_e_0004_2", ScriptBuilder.FromJson("sc_empty") }, // Exit trigger Princess
			//{ "sc_e_0005", ScriptBuilder.FromJson("sc_empty") }, // Bridge building
			//{ "sc_e_0006", ScriptBuilder.FromJson("sc_empty") }, // Bridge Intro
			{ "sc_map_20040", ScriptBuilder.FromScript(Scripts.Pravoka, "sc_pravokamap") }, // Pravoka
			{ "sc_e_0009", ScriptBuilder.FromJson("sc_bikke_01") }, // Bikke
			{ "sc_e_0009_2", ScriptBuilder.FromJson("sc_bikke_02") }, // Bikke post fight
			// { "sc_e_0010", ScriptBuilder.FromScript(Scripts.CrownScript, "sc_crownchest_01") }, // Crown Chest, Crown map checks correctly for key
			{ "sc_e_0010_1", ScriptBuilder.FromScript(Scripts.MarshChest, "sc_crownchest_01") }, // Crown post fight
			{ "sc_map_20081_1", ScriptBuilder.FromScript(Scripts.NWCastle, "sc_nwcastle_01") }, // NW Castle
			{ "sc_e_0011", ScriptBuilder.FromJson("sc_astos_01") }, // Astos
			{ "sc_e_0011_2", ScriptBuilder.FromJson("sc_astos_02") }, // Astos post fight
			//{ "sc_e_0007", ScriptBuilder.FromJson("sc_empty") }, // Matoya Intro
			{ "sc_e_0012", ScriptBuilder.FromScript(Scripts.Matoya, "sc_matoya_01") }, // Matoya
			{ "sc_map_20071_1", ScriptBuilder.FromJson("sc_elflandcastle") }, // Elfland Castle
			{ "sc_e_0013", ScriptBuilder.FromJson("sc_elfprince") }, // Elf Doctor
			//{ "sc_map_20011_1", new ScriptBuilder("sc_empty") }, // Coneria Castle
			{ "sc_e_0014", ScriptBuilder.FromScript(Scripts.ConeriaChest, "sc_coneriachest_01") }, // Nitro Treasure
			//{ "sc_map_30011_1", new ScriptBuilder("sc_empty") }, // ToF Map
			/***
			 * We started figuring it out here (load single entity script for map, then replace/remove entities by flags)
			 * Come back to fix the stuff up before this point
			***/
			{ "sc_map_20051_1", ScriptBuilder.FromScript(Scripts.DwarfMap, "sc_dwarfmap_01") }, // Dwarf Map
			{ "sc_e_0015", ScriptBuilder.FromJson("sc_nerrick_01") }, // Nerrick
			{ "sc_e_0015_2", ScriptBuilder.FromJson("sc_nerrick_02") }, // Nerrick post canal
			{ "sc_e_0052", ScriptBuilder.FromJson("sc_smitt") }, // Smitt
			{ "sc_map_30031_3", ScriptBuilder.FromScript(Scripts.EarthB3, "sc_earthb3fmap_01") }, // Earth B3
			//{ "sc_e_0016", new ScriptBuilder("sc_empty") }, // Vampire
			//{ "sc_e_0016_2", new ScriptBuilder("sc_empty") }, // Vampire Post fight
			{ "sc_e_0017", ScriptBuilder.FromScript(Scripts.VampireChest, "sc_vampirechest_01") }, // Ruby Chest
			//{ "sc_e_0017", new ScriptBuilder("sc_empty") }, // Slab
			//{ "sc_e_0018", new ScriptBuilder("sc_empty") }, // Titan
			{ "sc_map_20101_1", ScriptBuilder.FromScript(Scripts.SardaCave, "sc_sardacavemap_01") }, // Sarda Cave
			{ "sc_e_0019", ScriptBuilder.FromScript(Scripts.Sarda, "sc_sarda_01") }, // Sarda
			//{ "sc_e_0020", new ScriptBuilder("sc_empty") }, // Earth Rod
			//{ "sc_e_0021", new ScriptBuilder("sc_empty") }, // Lich
			//{ "sc_e_0021_2", new ScriptBuilder("sc_empty") }, // Lich Post fight
			{ "sc_map_20110", ScriptBuilder.FromScript(Scripts.CrescentLake, "sc_crescentmap_01") }, // Crescent
			{ "sc_e_0022", ScriptBuilder.FromScript(Scripts.CanoeSage, "sc_canoesage_01") }, // Canoe Sage
			{ "sc_map_30061_4", ScriptBuilder.FromScript(Scripts.FloaterRoom, "sc_floatermap_01") }, // Floater Floor
			//{ "sc_e_0024", new ScriptBuilder("sc_empty") }, // Floater
			{ "sc_e_0024_2", ScriptBuilder.FromJson("sc_eye_chest") }, // Floater Post Fight
			//{ "sc_e_0025", new ScriptBuilder("sc_empty") }, // Airship Rise
			{ "sc_ordealsman", ScriptBuilder.FromScript(Scripts.OrdealsMan, "sc_ordealsman") }, // Ordeals dude
			//{ "sc_e_0046", new ScriptBuilder("sc_empty") }, // Ordeals dude
			// we can shuffle ordeals by just rerouting hhere
			// no wait, we'll do it in prefix
			{ "sc_e_0047", ScriptBuilder.FromScript(Scripts.OrdealsChest, "sc_ordealschest") }, // Rat Tail chest
			//{ "sc_e_0048", new ScriptBuilder("sc_empty") }, // Bahamut
			//{ "sc_e_0023", new ScriptBuilder("sc_empty") }, // Kary
			//{ "sc_e_0023_2", new ScriptBuilder("sc_empty") }, // Kary Post fight
			//{ "sc_e_0027", new ScriptBuilder("sc_empty") }, // Caravan
			//{ "sc_map_20150", new ScriptBuilder("sc_empty") }, // Gaia
			//{ "sc_e_0028", new ScriptBuilder("sc_empty") }, // Fairy release
			{ "sc_e_0029", ScriptBuilder.FromJson("sc_fairy") }, // Fairy
			{ "sc_map_30091", ScriptBuilder.FromScript(Scripts.Waterfall, "sc_waterfall") }, // Waterfall
			{ "sc_e_0026", ScriptBuilder.FromJson("sc_cubebot") }, // CubeBot
			//{ "sc_e_0030", new ScriptBuilder("sc_empty") }, // SubEngineer
			//{ "sc_e_0031", new ScriptBuilder("sc_empty") }, // SubStuff
			//{ "sc_e_0031_1", new ScriptBuilder("sc_empty") }, // SubStuff
			{ "sc_e_0033", ScriptBuilder.FromScript(Scripts.MermaidsChest, "sc_mermaidschest") }, // Slab Chest
			//{ "sc_e_0036", new ScriptBuilder("sc_empty") }, // Kraken
			//{ "sc_e_0036_2", new ScriptBuilder("sc_empty") }, // Kraken Post-fight
			//{ "sc_e_0034", new ScriptBuilder("sc_empty") }, // Dr.Unne
			{ "sc_map_20160", ScriptBuilder.FromScript(Scripts.Lefein, "sc_lefeinmap") }, // Lefeinman
			{ "sc_e_0035", ScriptBuilder.FromJson("sc_lefeinman") }, // Lefeinman
			//{ "sc_war_30101_1", new ScriptBuilder("sc_empty") }, // Cube Warp
			{ "sc_map_30111_2", ScriptBuilder.FromScript(Scripts.Sky2F, "sc_sky2fmap") }, // Sky 2F
			{ "sc_e_0051", ScriptBuilder.FromJson("sc_sky_chest") }, // SkyChest
			//{ "sc_e_0037", new ScriptBuilder("sc_empty") }, // Tiamat
			//{ "sc_e_0037_2", new ScriptBuilder("sc_empty") }, // Tiamat Post-fight
			//{ "sc_e_0038", new ScriptBuilder("sc_empty") }, // Black Orb
			{ "sc_e_0039", ScriptBuilder.FromScript(Scripts.LuteSlab, "sc_luteslab") }, // Lute Slab
			{ "sc_e_0044", ScriptBuilder.FromJson("sc_chaos_fight") }, // Chaos
			{ "sc_chaosdefeated", ScriptBuilder.FromScript(Scripts.ChaosDefeated, "sc_chaosdefeated") }, // Chaos Post fight
			//{ "sc_e_0044_1", new ScriptBuilder("sc_empty") }, // Chaos Post fight
		};
	}



	class Scripts
	{
		public static List<string> Intro = new()
		{
			"Sub Main:",
			"Nop",
			//"Call [PlaceBridge]",
			"SysCall MapEntryRoofControl",
			//"SetEntities ev_e_0007",
			"SetFlag ScenarioFlag1 0", // Intro done
			"SetFlag ScenarioFlag1 1", // Talked to King
			"PlayBGM 70 1",
			"Wait 3.5",
			"ChangeScript sc_map_10010",
			"Exit",
		};
		public static List<string> Garland = new()
		{
			"Sub Main:",
			"Nop",
			"Msg MSG_NPC_GARLAND",
			"Wait 0.35",
			"EncountBoss 350 38 29 sc_e_0003_2",
			"Wait 1.4",
			"Exit"
		};
		public static List<string> TempleOfFiends = new()
		{
			"Sub Main:",
			"Nop",
			"SetFlag ScenarioFlag4 138",
			"SysCall MapEntryRoofControl",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.PrincessSaved} [PrincessSaved]",
			"SetEntities ev_e_0003",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.MysticKey} [HasMysticKey]",
			"Exit",
			"HasMysticKey:",
			"Nop",
			"SetEntities ev_e_0014",
			"Exit",
			// Will probably need to change has new conditions are added?
			"PrincessSaved:",
			"Nop",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.LichDefeated} [LichDefeated]",
			"SetEntities ev_e_0014",
			"Exit",
			"LichDefeated:",
			"Nop",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.KaryDefeated} [KaryDefeated]",
			"SetEntities ev_e_0014",
			"Exit",
			"KaryDefeated:",
			"Nop",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.KrakenDefeated} [KrakenDefeated]",
			"SetEntities ev_e_0014",
			"Exit",
			"KrakenDefeated:",
			"Nop",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.TiamatDefeated} [TiamatDefeated]",
			"SetEntities ev_e_0014",
			"Exit",
			"TiamatDefeated:",
			"Nop",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.BlackOrbRemoved} [BlacOrbRemoved]",
			"SetEntities ev_e_0038",
			"Exit",
			"BlacOrbRemoved:",
			"Nop",
			"SetEntities ev_e_0039",
			"Exit",
		};
		public static List<string> Princess = new()
		{
			"Sub Main:",
			"Nop",
			$"Branch TreasureFlag1 {(int)TreasureFlags.Princess} [TreasureGiven]",
			"Msg MSG_NPC_SARALUTE_01",
			"MsgFunfare MSG_NPC_SARALUTE_02",
			"GetItem RANDOITEM RANDOQTY",
			$"SetFlag TreasureFlag1 {(int)TreasureFlags.Princess}",
			"Exit",
			"TreasureGiven:",
			"Nop",
			"Msg MSG_KON_CAS_20",
			"Exit"
		};

		public static List<string> Pravoka = new()
		{
			"Sub Main:",
			"Nop",
			"SetFlag ScenarioFlag4 156",
			$"Branch TreasureFlag1 {(int)TreasureFlags.Bikke} [TreasureGiven]",
			"SetEntities ev_e_0009",
			"Exit",
			"TreasureGiven:",
			"Nop",
			"SetEntities ev_e_0010",
			"Exit"
		};

		public static List<string> MarshChest = new()
		{
			"Sub Main:",
			"Nop",
			"FadeIn 0.5 255",
			"TreasureBox 40 1 0",
			"Exit"
		};
		public static List<string> NWCastle = new()
		{
			"Sub Main:",
			"Nop",
			"SetFlag ScenarioFlag4 152",
			"SysCall MapEntryRoofControl",
			$"Branch TreasureFlag1 {(int)TreasureFlags.Astos} [AstosDefeated]",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.Crown} [HasCrown]",
			"SetEntities ev_e_0010",
			"Jump [CheckKey]",
			"HasCrown:",
			"Nop",
			"SetEntities ev_e_0011",
			"Jump [CheckKey]",
			"AstosDefeated:",
			"Nop",
			"SetEntities ev_e_0012",
			"CheckKey:",
			"Nop",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.MysticKey} [HasKey]",
			"Exit",
			"HasKey:",
			"Nop",
			"SetEntities ev_e_0014",
			"Exit"
		};

		public static List<string> Matoya = new()
		{
			"Sub Main:",
			"Nop",
			$"Branch TreasureFlag1 {(int)TreasureFlags.Matoya} [TreasureGiven]",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.CrystalEye} [EyeFound]",
			"Msg MSG_MATOYA_03",
			"Exit",
			"EyeFound:",
			"Nop",
			"Msg MSG_AWAKEPOT_01",
			"MsgFunfare MSG_AWAKEPOT_03",
			"GetItem RANDOITEM RANDOQTY",
			"UseItem 47 1",
			$"SetFlag TreasureFlag1 {(int)TreasureFlags.Matoya}",
			"Exit",
			"TreasureGiven:",
			"Nop",
			"Msg MSG_MAT_02",
			"Exit"
		};
		public static List<string> ConeriaChest = new()
		{
			"Sub Main:",
			"Nop",
			"TreasureBox 10 1 0",
			"Exit"
		};
		public static List<string> DwarfMap = new()
		{
			"Sub Main:",
			"Nop",
			"SetEntities ev_e_0014",
			"Exit"
		};
		public static List<string> EarthB3 = new()
		{
			"Sub Main:",
			"Nop",
			"SysCall MapEntryRoofControl",
			"SetEntities ev_e_0016",
			"Exit"
		};
		public static List<string> VampireChest = new()
		{
			"Sub Main:",
			"Nop",
			"TreasureBox 22 1 0",
			"Exit"
		};
		public static List<string> SardaCave = new()
		{
			"Sub Main:",
			"Nop",
			"SetFlag ScenarioFlag4 142",
			"SysCall MapEntryRoofControl",
			"SetEntities ev_e_0019",
			"Exit"
		};
		public static List<string> Sarda = new()
		{
			"Sub Main:",
			"Nop",
			$"Branch TreasureFlag1 {(int)TreasureFlags.Sarda} [TreasureGiven]",
			"Msg MSG_GET_STICK_01",
			"MsgFunfare MSG_GET_STICK_02",
			"GetItem RANDOITEM RANDOQTY",
			$"SetFlag TreasureFlag1 {(int)TreasureFlags.Sarda}",
			"Exit",
			"TreasureGiven:",
			"Nop",
			"Msg MSG_SAG_CAV_01",
			"Exit"
		};
		public static List<string> CrescentLake = new()
		{
			"Sub Main:",
			"Nop",
			"SetFlag ScenarioFlag4 141",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.LichDefeated} [LichDefeated]",
			"SetEntities ev_e_0015",
			"Exit",
			"LichDefeated:",
			"Nop",
			$"Branch TreasureFlag1 {(int)TreasureFlags.CanoeSage} [CanoeSageGiven]",
			"SetEntities ev_e_0021",
			"Exit",
			"CanoeSageGiven:",
			"Nop",
			"SetEntities ev_e_0023",
			"Exit"
		};
		public static List<string> CanoeSage = new()
		{
			"Sub Main:",
			"Nop",
			$"Branch TreasureFlag1 {(int)TreasureFlags.CanoeSage} [TreasureGiven]",
			"Msg MSG_GET_CANOE_01",
			"MsgFunfare MSG_GET_CANOE_02",
			"GetItem RANDOITEM RANDOQTY",
			$"SetFlag TreasureFlag1 {(int)TreasureFlags.CanoeSage}",
			"Exit",
			"TreasureGiven:",
			"Nop",
			"Msg MSG_CLK_CTY_22",
			"Exit"
		};
		public static List<string> FloaterRoom = new()
		{
			"Sub Main:",
			"Nop",
			"SysCall MapEntryRoofControl",
			$"Branch TreasureFlag1 {(int)TreasureFlags.EyeChest} [TreasureGiven]",
			"SetEntities ev_e_0025",
			"Exit",
			"TreasureGiven:",
			"Nop",
			"SetEntities ev_e_0026",
			"Exit"
		};
		public static List<string> OrdealsMan = new()
		{
			"Sub Main:",
			"Nop",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.Crown} [HasCrown]",
			"Msg MSG_TRIAL_CAS_03",
			"Exit",
			"HasCrown:",
			"Nop",
			"ChangeScript sc_e_0046",
			"Exit",
		};
		public static List<string> OrdealsChest = new()
		{
			"Sub Main:",
			"Nop",
			"TreasureBox 9 1 0",
			"Exit"
		};
		public static List<string> Waterfall = new()
		{
			"Sub Main:",
			"Nop",
			"SetFlag ScenarioFlag4 149",
			"SysCall MapEntryRoofControl",
			$"Branch TreasureFlag1 {(int)TreasureFlags.CubeBot} [TreasureGiven]",
			"SetEntities ev_e_0026",
			"Exit",
			"TreasureGiven:",
			"Nop",
			"SetEntities ev_e_0037",
			"Exit"
		};
		public static List<string> MermaidsChest = new()
		{
			"Sub Main:",
			"Nop",
			"TreasureBox 42 1 0",
			"Exit"
		};
		public static List<string> Lefein = new()
		{
			"Sub Main:",
			"Nop",
			"SetFlag ScenarioFlag4 158",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.StoneTranslated} [StoneTranslated]",
			"SetEntities ev_e_0025",
			"Exit",
			"StoneTranslated:",
			"Nop",
			$"Branch TreasureFlag1 {(int)TreasureFlags.Lefeinman} [TreasureGiven]",
			"SetEntities ev_e_0034",
			"Exit",
			"TreasureGiven:",
			"Nop",
			"SetEntities ev_e_0035",
			"Exit"
		};
		public static List<string> Sky2F = new()
		{
			"Sub Main:",
			"Nop",
			"SysCall MapEntryRoofControl",
			$"Branch TreasureFlag1 {(int)TreasureFlags.SkyChest} [TreasureGiven]",
			"SetEntities ev_e_0051",
			"Exit",
			"TreasureGiven:",
			"Nop",
			"SetEntities ev_e_0052",
			"Exit"
		};
		public static List<string> LuteSlab = new()
		{
			"Sub Main:",
			"Nop",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.Lute} [HasLute]",
			"Msg MSG_LUTESLAB",
			"Exit",
			"HasLute:",
			"Nop",
			"ChangeScript sc_e_0039",
			"Exit",
		};
		public static List<string> ChaosDefeated = new()
		{
			"Sub Main:",
			"Nop",
			$"SetFlag ScenarioFlag1 {(int)ScenarioFlags.ChaosDefeated}",
			"ChangeScript sc_e_0044_1",
			"Exit",
		};

	};
}
