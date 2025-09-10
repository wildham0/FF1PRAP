using Last.Data.Master;
using Last.Interpreter;
using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Bindings;
using UnityEngine;

namespace FF1PRAP
{
	partial class Randomizer
    {
		public static Dictionary<string, string> ScriptToReplace = new()
		{
			{ "sc_e_0001", ScriptBuilder.FromScript(Scripts.Intro, "sc_e_0001") }, // Intro script
			{ "sc_map_10010", ScriptBuilder.FromScript(Scripts.Overworld, "sc_overworld_01") }, // Overworld
			// { "sc_map_20020", new ScriptBuilder("sc_empty") }, // Coneria Town
			 { "sc_map_20011_1", ScriptBuilder.FromScript(Scripts.ConeriaCastle, "sc_map_20011_1") }, // Coneria Castle
			// { "sc_e_0002_2", new ScriptBuilder("sc_empty") }, // Go see the king
			// { "sc_e_0002_1", new ScriptBuilder("sc_empty") }, // Talk King
			{ "sc_map_30011_1", ScriptBuilder.FromScript(Scripts.TempleOfFiends, "sc_templeoffiends") }, // ToF Map
			{ "sc_e_0003", ScriptBuilder.FromScript(Scripts.Garland, "sc_garland_01") }, // Garland
			//{ "sc_e_0003", ScriptBuilder.FromJson("pasta") }, // Garland
			{ "sc_garlandpostbattle", ScriptBuilder.FromScript(Scripts.GarlandPostBattle, "sc_garlandpostbattle") }, // Post-fight, pick which script to use
			{ "sc_e_0003_2", ScriptBuilder.FromJson("sc_garland_02") }, // Pos-fight + Princess Warp
			{ "sc_princessnowarp", ScriptBuilder.FromJson("sc_garland_02_no_warp") }, // Post-fight + Princess No Warp
			{ "sc_e_0003_3", ScriptBuilder.FromJson("sc_garland_03") }, // At the castle?
			{ "sc_e_0004_1", ScriptBuilder.FromScript(Scripts.Princess, "sc_princess_01") }, // Talk Princess
			//{ "sc_e_0004_2", ScriptBuilder.FromJson("sc_empty") }, // Exit trigger Princess
			//{ "sc_e_0005", ScriptBuilder.FromJson("sc_empty") }, // Bridge building
			//{ "sc_e_0006", ScriptBuilder.FromJson("sc_empty") }, // Bridge Intro
			{ "sc_map_20040", ScriptBuilder.FromScript(Scripts.Pravoka, "sc_pravokamap") }, // Pravoka
			{ "sc_e_0009", ScriptBuilder.FromJson("sc_bikke_01") }, // Bikke
			{ "sc_e_0009_2", ScriptBuilder.FromJson("sc_bikke_02") }, // Bikke post fight
			// { "sc_e_0010", ScriptBuilder.FromScript(Scripts.CrownScript, "sc_crownchest_01") }, // Crown Chest, Crown map checks correctly for key
			{ "sc_map_30021_3", ScriptBuilder.FromJson("sc_marsh_bottom") }, // Marsh Bottom
			{ "sc_e_0010_1", ScriptBuilder.FromScript(Scripts.MarshChest, "sc_crownchest_01") }, // Crown post fight
			{ "sc_map_20081_1", ScriptBuilder.FromScript(Scripts.NWCastle, "sc_nwcastle_01") }, // NW Castle
			{ "sc_e_0011", ScriptBuilder.FromJson("sc_astos_01") }, // Astos
			{ "sc_e_0011_2", ScriptBuilder.FromJson("sc_astos_02") }, // Astos post fight
			//{ "sc_e_0007", ScriptBuilder.FromJson("sc_empty") }, // Matoya Intro
			{ "sc_map_20031_1", ScriptBuilder.FromScript(Scripts.MatoyasCave, "sc_matoyascave") }, // Matoya
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
			{ "sc_map_20090", ScriptBuilder.FromScript(Scripts.Melmond, "sc_melmond_01") }, // Melmond
			{ "sc_map_30031_3", ScriptBuilder.FromJson("sc_earth_b3") }, // Earth B3
			//{ "sc_e_0016", new ScriptBuilder("sc_empty") }, // Vampire
			//{ "sc_e_0016_2", new ScriptBuilder("sc_empty") }, // Vampire Post fight
			{ "sc_e_0017", ScriptBuilder.FromScript(Scripts.VampireChest, "sc_vampirechest_01") }, // Ruby Chest
			//{ "sc_e_0017", new ScriptBuilder("sc_empty") }, // Slab
			{ "sc_e_0018", ScriptBuilder.FromJson("sc_titan") }, // Titan
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
			{ "sc_ordealsman", ScriptBuilder.FromScript(Scripts.OrdealsMan, "sc_ordealsman") }, // Ordeals dude / sc_e_0046
			// Add pillars script so they can be individually shuffled
			{ "sc_ordeals_1010", ScriptBuilder.FromJson("sc_ordeals_1010") },
			{ "sc_ordeals_1011", ScriptBuilder.FromJson("sc_ordeals_1011") },
			{ "sc_ordeals_1012", ScriptBuilder.FromJson("sc_ordeals_1012") },
			{ "sc_ordeals_1013", ScriptBuilder.FromJson("sc_ordeals_1013") },
			{ "sc_ordeals_1014", ScriptBuilder.FromJson("sc_ordeals_1014") },
			{ "sc_war_30071_12", ScriptBuilder.FromJson("sc_ordeals_last_warp") }, // Prevent poisoning the teleporter cache
			{ "sc_e_0047", ScriptBuilder.FromScript(Scripts.OrdealsChest, "sc_ordealschest") }, // Rat Tail chest
			{ "sc_bahamut", ScriptBuilder.FromScript(Scripts.Bahamut, "sc_bahamut") }, // Bahamut
			//{ "sc_e_0023", new ScriptBuilder("sc_empty") }, // Kary
			//{ "sc_e_0023_2", new ScriptBuilder("sc_empty") }, // Kary Post fight
			{ "sc_e_0027", ScriptBuilder.FromScript(Scripts.Caravan, "sc_caravan") }, // Caravan
			//{ "sc_map_20150", new ScriptBuilder("sc_empty") }, // Gaia
			//{ "sc_e_0028", new ScriptBuilder("sc_empty") }, // Fairy release
			{ "sc_e_0029", ScriptBuilder.FromJson("sc_fairy") }, // Fairy
			{ "sc_map_30091_1", ScriptBuilder.FromScript(Scripts.Waterfall, "sc_waterfall") }, // Waterfall
			{ "sc_e_0026", ScriptBuilder.FromJson("sc_cubebot") }, // CubeBot
			{ "sc_map_20130", ScriptBuilder.FromJson("sc_onrac") }, // Onrac
			{ "sc_subeng", ScriptBuilder.FromScript(Scripts.SubEngineer, "sc_subeng") }, // SubEngineer
			//{ "sc_e_0031", new ScriptBuilder("sc_empty") }, // SubStuff
			//{ "sc_e_0031_1", new ScriptBuilder("sc_empty") }, // SubStuff
			{ "sc_e_0032_1", ScriptBuilder.FromJson("sc_onrac_sub_back") }, // SubStuff
			{ "sc_e_0033", ScriptBuilder.FromScript(Scripts.MermaidsChest, "sc_mermaidschest") }, // Slab Chest
			//{ "sc_e_0036", new ScriptBuilder("sc_empty") }, // Kraken
			//{ "sc_e_0036_2", new ScriptBuilder("sc_empty") }, // Kraken Post-fight
			//{ "sc_e_0034", new ScriptBuilder("sc_empty") }, // Dr.Unne
			{ "sc_map_20160", ScriptBuilder.FromScript(Scripts.Lefein, "sc_lefeinmap") }, // Lefeinman
			{ "sc_e_0035", ScriptBuilder.FromJson("sc_lefeinman") }, // Lefeinman
			{ "sc_map_30101_3", ScriptBuilder.FromJson("sc_cubewarp") }, // Cube Warp
			{ "sc_map_30111_2", ScriptBuilder.FromScript(Scripts.Sky2F, "sc_sky2fmap") }, // Sky 2F
			{ "sc_e_0051", ScriptBuilder.FromJson("sc_sky_chest") }, // SkyChest
			//{ "sc_e_0037", new ScriptBuilder("sc_empty") }, // Tiamat
			//{ "sc_e_0037_2", new ScriptBuilder("sc_empty") }, // Tiamat Post-fight
			//{ "sc_e_0038", new ScriptBuilder("sc_empty") }, // Black Orb
			{ "sc_luteslab", ScriptBuilder.FromScript(Scripts.LuteSlab, "sc_luteslab") }, // Lute Slab
			{ "sc_e_0044", ScriptBuilder.FromJson("sc_chaos_fight") }, // Chaos
			{ "sc_chaosdefeated", ScriptBuilder.FromScript(Scripts.ChaosDefeated, "sc_chaosdefeated") }, // Chaos Post fight
			//{ "sc_e_0044_1", new ScriptBuilder("sc_empty") }, // Chaos Post fight

			// Inn Scripts
			{ "sc_sty_20021_1", ScriptBuilder.FromJson("sc_inn_cornelia") },
			{ "sc_sty_20041_1", ScriptBuilder.FromJson("sc_inn_pravoka") },
			{ "sc_sty_20061_7", ScriptBuilder.FromJson("sc_inn_elfheim") },
			{ "sc_sty_20091_2", ScriptBuilder.FromJson("sc_inn_melmond") },
			{ "sc_sty_20111_2", ScriptBuilder.FromJson("sc_inn_crescent") },
			{ "sc_sty_20131_3", ScriptBuilder.FromJson("sc_inn_onrac") },
			{ "sc_sty_20151_2", ScriptBuilder.FromJson("sc_inn_gaia") },

			// Fiend Warp Script
			{ "sc_war_30031_1", ScriptBuilder.FromJson("sc_warp_earth") },
			{ "sc_war_30051_1", ScriptBuilder.FromJson("sc_warp_gulg") },
			{ "sc_war_30081_1", ScriptBuilder.FromJson("sc_warp_sea") },
			{ "sc_war_30111_10", ScriptBuilder.FromJson("sc_warp_flying") },

			// Ice Backdoor Warp
			{ "sc_icecavernwap", ScriptBuilder.FromScript(Scripts.IceWarp, "sc_icecavernwap")}

		};
	}
}
