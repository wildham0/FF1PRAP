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
		public static List<string> Overworld = new()
		{
			"Sub Main:",
			"Nop",
			"SysCall MapEntryRoofControl",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.WestwardProgressionMode} [Westward]",
			"Call [Puppet_Bridge]",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.Canal} [CanalDug]",
			"SetEntities ev_e_0025",
			"Exit",
			"CanalDug:",
			"Nop",
			"Call [Puppet_Canal]",
			"SetEntities ev_e_0025",
			"Exit",
			"Westward:",
			"Nop",
			"SetEntities ev_e_0007",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.Canal} [CanalDug2]",
			"Exit",
			"CanalDug2:",
			"Nop",
			"Call [Puppet_Canal]",
			"Exit",
			"Sub Puppet_Bridge:",
			"Nop",
			"SetPuppet 0 112 [BridgeAnim]",
			"ExecPuppet",
			"Return",
			"BridgeAnim:",
			"Nop",
			"CueAnim 1",
			"Exit",
			"Sub Puppet_Canal:",
			"Nop",
			"SetPuppet 0 111 [CanalAnim]",
			"ExecPuppet",
			"Return",
			"CanalAnim:",
			"Nop",
			"CueAnim 1",
			"Exit"
		};
		public static List<string> ConeriaCastle = new()
		{
			"Sub Main:",
			"Nop",
			"SetFlag ScenarioFlag4 143",
			"SysCall MapEntryRoofControl",
			"SetEntities ev_e_0014",
			"Exit"
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
			// Will probably need to change has new conditions are added?
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
			"SetEntities ev_e_0010",
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
		public static List<string> MatoyasCave = new()
		{
			"Sub Main:",
			"Nop",
			"SetFlag ScenarioFlag4 155",
			"SysCall MapEntryRoofControl",
			"SetEntities ev_e_0012",
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
		public static List<string> Melmond = new()
		{
			"Sub Main:",
			"Nop",
			"SetFlag ScenarioFlag4 157",
			"SetEntities ev_e_0034",
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
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.VampireDefeated} [VampireDefeated]",
			"Msg MSG_SAG_CAV_02",
			"Exit",
			"VampireDefeated:",
			"Nop",
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
		public static List<string> Caravan = new()
		{
			"MainSub:",
			"Nop",
			$"Branch TreasureFlag1 {(int)TreasureFlags.Caravan} [CaravanItemBought]",
			"Msg MSG_SYSTEM_134",
			"SysCall 妖精の瓶ショップ",
			"Msg MSG_SYSTEM_143",
			"Exit",
			"CaravanItemBought:",
			"Nop",
			"Msg MSG_SYSTEM_134",
			"SysCall キャラバンショップ",
			"Msg MSG_SYSTEM_143",
			"Exit",
		};
		public static List<string> SubEngineer = new()
		{
			"Sub Main:",
			"Nop",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.Oxyale} [HasOxyale]",
			"Msg MSG_ORK_CTY_14",
			"Exit",
			"HasOxyale:",
			"Nop",
			"ChangeScript sc_e_0030",
			"Exit",
		};
		public static List<string> Bahamut = new()
		{
			"Sub Main:",
			"Nop",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.BahamutGivesItem} [BahamutItem]",
			"ChangeScript sc_e_0048",
			"BahamutItem:",
			"Nop",
			$"Branch TreasureFlag1 {(int)TreasureFlags.Bahamut} [ItemGiven]",
			$"Branch ScenarioFlag1 {(int)ScenarioFlags.RatTail} [GiveItem]",
			"Msg MSG_DRG_CAV_01",
			"Exit",
			"GiveItem:",
			"Nop",
			"Msg MSG_CLASS_CHG_01",
			"MsgFunfare MSG_CLASS_CHG_04",
			"GetItem RANDOITEM RANDOQTY",
			$"SetFlag TreasureFlag1 {(int)TreasureFlags.Bahamut}",
			"Exit",
			"ItemGiven:",
			"Nop",
			"Msg MSG_DRG_CAV_02",
			"Exit",
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
		public static List<string> OrdealsMaze = new()
		{
			"Sub Main:",
			"Nop",
			"SysCall MapEntryRoofControl",
			"SetEntities ev_e_0026",
			"Exit"
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
