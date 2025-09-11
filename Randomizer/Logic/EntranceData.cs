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

	public enum EntGroup : int
	{ 
		Region = 0,
		OverworldDungeon,
		OverworldTown,
		InnerDungeon,
		ChaosShrine,
		Titan,
		Fixed
	}

	public class EntranceData
	{
		public string name;
		public string region;
		public string target_point;
		public string target_region;
		public int type;
		public string group;
		public bool deadend;
		public bool access_req;

		public EntranceData(string name, string region, string target_point, string target_region, int type, string group, bool deadend = false, bool access_req = false)
		{
			this.name = name;
			this.region = region;
			this.target_point = target_point;
			this.target_region = target_region;
			this.type = type;
			this.group = group;
			this.deadend = deadend;
			this.access_req = access_req;
		}
	}
	partial class Logic
    {
		public static Dictionary<string, List<List<AccessRequirements>>> default_overworld_entrances_rules = new()
		{
			{ RegionNames.overworld + " -> " + RegionNames.pravoka_region, new() },
			{ RegionNames.overworld + " -> " + RegionNames.innersea_region, new() { new() { AccessRequirements.Ship }, new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.ice_region, new() { new() { AccessRequirements.Canoe }, new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.crescent_region, new() { new() { AccessRequirements.Ship, AccessRequirements.Canal }, new() { AccessRequirements.Ship, AccessRequirements.Canoe }, new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.gulg_region, new() { new() { AccessRequirements.Ship, AccessRequirements.Canoe }, new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.ryukhan_desert, new() { new() { AccessRequirements.Ship, AccessRequirements.Canoe, AccessRequirements.Canal }, new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.melmond_region, new() { new() { AccessRequirements.Ship, AccessRequirements.Canal }, new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.sage_region, new() { new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.bahamuts_island, new() { new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.dragon_forest_island, new() { new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.dragon_marsh_island, new() { new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.dragon_small_island, new() { new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.dragon_plains_island, new() { new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.onrac_region, new() { new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.trials_region, new() { new() { AccessRequirements.Airship, AccessRequirements.Canoe }, new() { AccessRequirements.Ship, AccessRequirements.Canal, AccessRequirements.Canoe } } },
			{ RegionNames.overworld + " -> " + RegionNames.gaia_region, new() { new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.mirage_desert, new() { new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.lufenia_region, new() { new() { AccessRequirements.Airship } } },
		};

		public static Dictionary<string, List<List<AccessRequirements>>> marshpath_overworld_entrances_rules = new()
		{
			{ RegionNames.overworld + " -> " + RegionNames.pravoka_region, new() { new() { AccessRequirements.Ship }, new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.innersea_region, new() },
			{ RegionNames.overworld + " -> " + RegionNames.ice_region, new() { new() { AccessRequirements.Ship, AccessRequirements.Canoe }, new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.crescent_region, new() { new() { AccessRequirements.Ship, AccessRequirements.Canal }, new() { AccessRequirements.Canoe }, new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.gulg_region, new() { new() {AccessRequirements.Canoe }, new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.ryukhan_desert, new() { new() { AccessRequirements.Ship, AccessRequirements.Canoe, AccessRequirements.Canal }, new() { AccessRequirements.Airship } } },
		};

		public static Dictionary<string, List<List<AccessRequirements>>> northerndocks_overworld_entrances_rules = new()
		{
			{ RegionNames.overworld + " -> " + RegionNames.onrac_region, new() { new() { AccessRequirements.Ship, AccessRequirements.Canal }, new() { AccessRequirements.Airship } } },
			{ RegionNames.overworld + " -> " + RegionNames.mirage_desert, new() { new() { AccessRequirements.Ship, AccessRequirements.Canal }, new() { AccessRequirements.Airship } } },
		};

		public static Dictionary<string, List<List<AccessRequirements>>> entrances_rules = new()
		{
			{ EntranceNames.overworld_waterfall, new() { new() { AccessRequirements.Canoe } } },
			{ EntranceNames.overworld_mirage_tower, new() { new() { AccessRequirements.Bell } } },
			{ EntranceNames.onrac_submarine_dock, new() { new() { AccessRequirements.Submarine } } },
			{ EntranceNames.cavern_of_earth_b3_center_stairs, new() { new() { AccessRequirements.EarthRod } } },
			{ EntranceNames.citadel_of_trials_1f_throne, new() { new() { AccessRequirements.Crown } } },
			{ EntranceNames.mirage_tower_3f_center_warp, new() { new() { AccessRequirements.WarpCube } } },
			{ EntranceNames.chaos_shrine_black_orb_warp, new() { new() { AccessRequirements.BlackOrbDestroyed } } },
			{ EntranceNames.giants_cavern_east_entrance, new() { new() { AccessRequirements.TitanFed } } },
			{ EntranceNames.giants_cavern_west_entrance, new() { new() { AccessRequirements.TitanFed } } },
		};

		public static List<EntranceData> global_entrances = new List<EntranceData>()
		{
			new EntranceData(EntranceNames.overworld_cornelia, RegionNames.cornelia_region, EntranceNames.cornelia_entrance, RegionNames.cornelia, (int)(int)EntGroup.Fixed, EntranceNames.overworld_cornelia, true),
			new EntranceData(EntranceNames.overworld_pravoka, RegionNames.pravoka_region, EntranceNames.pravoka_entrance, RegionNames.pravoka, (int)EntGroup.OverworldTown, EntranceNames.overworld_pravoka, true),
			new EntranceData(EntranceNames.overworld_elfheim, RegionNames.innersea_region, EntranceNames.elfheim_entrance, RegionNames.elfheim, (int)EntGroup.OverworldTown, EntranceNames.overworld_elfheim, true),
			new EntranceData(EntranceNames.overworld_melmond, RegionNames.melmond_region, EntranceNames.melmond_entrance, RegionNames.melmond, (int)EntGroup.OverworldTown, EntranceNames.overworld_melmond, true),
			new EntranceData(EntranceNames.overworld_crescent_lake, RegionNames.crescent_region, EntranceNames.crescent_lake_entrance, RegionNames.crescent_lake, (int)EntGroup.OverworldTown, EntranceNames.overworld_crescent_lake, true),
			new EntranceData(EntranceNames.overworld_onrac, RegionNames.onrac_region, EntranceNames.onrac_entrance, RegionNames.onrac, (int)EntGroup.OverworldTown, EntranceNames.overworld_onrac, true),
			new EntranceData(EntranceNames.overworld_gaia, RegionNames.gaia_region, EntranceNames.gaia_entrance, RegionNames.gaia, (int)EntGroup.OverworldTown, EntranceNames.overworld_gaia, true),
			new EntranceData(EntranceNames.overworld_lufenia, RegionNames.lufenia_region, EntranceNames.lufenia_entrance, RegionNames.lufenia, (int)EntGroup.OverworldTown, EntranceNames.overworld_lufenia, true),

			new EntranceData(EntranceNames.overworld_castle_cornelia, RegionNames.cornelia_region, EntranceNames.castle_cornelia_1f_entrance, RegionNames.castle_cornelia_1f, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_castle_cornelia),
			new EntranceData(EntranceNames.overworld_chaos_shrine, RegionNames.cornelia_region, EntranceNames.chaos_shrine_entrance, RegionNames.chaos_shrine, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_chaos_shrine, true),
			new EntranceData(EntranceNames.overworld_matoyas_cave, RegionNames.pravoka_region, EntranceNames.matoyas_cave_entrance, RegionNames.matoyas_cave, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_matoyas_cave, true),
			new EntranceData(EntranceNames.overworld_mount_duergar, RegionNames.innersea_region, EntranceNames.mount_duergar_entrance, RegionNames.mount_duergar, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_mount_duergar, true),
			new EntranceData(EntranceNames.overworld_elven_castle, RegionNames.innersea_region, EntranceNames.elven_castle_entrance, RegionNames.elven_castle, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_elven_castle, true),
			new EntranceData(EntranceNames.overworld_western_keep, RegionNames.innersea_region, EntranceNames.western_keep_entrance, RegionNames.western_keep, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_western_keep, true),
			new EntranceData(EntranceNames.overworld_marsh_cave, RegionNames.innersea_region, EntranceNames.marsh_cave_b1_entrance, RegionNames.marsh_cave_b1, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_marsh_cave),
			new EntranceData(EntranceNames.overworld_cavern_of_earth, RegionNames.melmond_region, EntranceNames.cavern_of_earth_b1_center_stairs, RegionNames.cavern_of_earth_b1, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_cavern_of_earth),
			new EntranceData(EntranceNames.overworld_giants_cavern_west, RegionNames.melmond_region, EntranceNames.giants_cavern_west_entrance, RegionNames.giants_cavern, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_giants_cavern_west, true),
			new EntranceData(EntranceNames.overworld_giants_cavern_east, RegionNames.sage_region, EntranceNames.giants_cavern_east_entrance, RegionNames.giants_cavern, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_giants_cavern_east, true),
			new EntranceData(EntranceNames.overworld_sages_cave, RegionNames.sage_region, EntranceNames.sages_cave_entrance, RegionNames.sages_cave, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_sages_cave, true),
			new EntranceData(EntranceNames.overworld_mount_gulg, RegionNames.gulg_region, EntranceNames.mount_gulg_b1_right_stairs, RegionNames.mount_gulg_b1, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_mount_gulg),
			new EntranceData(EntranceNames.overworld_cavern_of_ice, RegionNames.ice_region, EntranceNames.cavern_of_ice_b1_entrance_top_stairs, RegionNames.cavern_of_ice_b1_entrance, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_cavern_of_ice),
			new EntranceData(EntranceNames.overworld_dragon_caves_plains, RegionNames.dragon_plains_island, EntranceNames.dragon_caves_plains_entrance, RegionNames.dragon_caves_plains, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_dragon_caves_plains, true),
			new EntranceData(EntranceNames.overworld_dragon_caves_top, RegionNames.bahamuts_island, EntranceNames.dragon_caves_top_entrance, RegionNames.dragon_caves_top, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_dragon_caves_top, true),
			new EntranceData(EntranceNames.overworld_dragon_caves_marsh, RegionNames.dragon_marsh_island, EntranceNames.dragon_caves_marsh_entrance, RegionNames.dragon_caves_marsh, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_dragon_caves_marsh, true),
			new EntranceData(EntranceNames.overworld_dragon_caves_small, RegionNames.dragon_small_island, EntranceNames.dragon_caves_small_entrance, RegionNames.dragon_caves_small, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_dragon_caves_small, true),
			new EntranceData(EntranceNames.overworld_dragon_caves_forest, RegionNames.dragon_forest_island, EntranceNames.dragon_caves_forest_entrance, RegionNames.dragon_caves_forest, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_dragon_caves_forest, true),
			new EntranceData(EntranceNames.overworld_dragon_caves_bahamut, RegionNames.bahamuts_island, EntranceNames.dragon_caves_bahamut_entrance, RegionNames.dragon_caves_bahamut_corridor, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_dragon_caves_bahamut),
			new EntranceData(EntranceNames.overworld_citadel_of_trials, RegionNames.trials_region, EntranceNames.citadel_of_trials_1f_entrance, RegionNames.citadel_of_trials_1f, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_citadel_of_trials, true),
			new EntranceData(EntranceNames.overworld_caravan, RegionNames.onrac_region, EntranceNames.caravan_entrance, RegionNames.caravan_outside, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_caravan),
			new EntranceData(EntranceNames.overworld_waterfall, RegionNames.onrac_region, EntranceNames.waterfall_entrance, RegionNames.waterfall, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_waterfall, true, true),
			new EntranceData(EntranceNames.overworld_mirage_tower, RegionNames.mirage_desert, EntranceNames.mirage_tower_1f_right_stairs, RegionNames.mirage_tower_1f, (int)EntGroup.OverworldDungeon, EntranceNames.overworld_mirage_tower, false, true),

			new EntranceData(EntranceNames.castle_cornelia_1f_stairs, RegionNames.castle_cornelia_1f, EntranceNames.castle_cornelia_2f_stairs, RegionNames.castle_cornelia_2f, (int)EntGroup.InnerDungeon, EntranceNames.overworld_castle_cornelia, true),
			new EntranceData(EntranceNames.dragon_caves_bahamut_bottom_stairs, RegionNames.dragon_caves_bahamut_corridor, EntranceNames.dragon_caves_bahamut_bahamut_hall, RegionNames.dragon_caves_bahamut_hall, (int)EntGroup.InnerDungeon, EntranceNames.overworld_dragon_caves_bahamut, true),
			new EntranceData(EntranceNames.onrac_submarine_dock, RegionNames.onrac, EntranceNames.sunken_shrine_3f_split_bottom_stairs, RegionNames.sunken_shrine_3f_split, (int)EntGroup.Fixed, EntranceNames.overworld_onrac, false, true),
			new EntranceData(EntranceNames.caravan_outside_tent, RegionNames.caravan_outside, EntranceNames.caravan_inside_tent, RegionNames.caravan_tent, (int)EntGroup.InnerDungeon, EntranceNames.overworld_caravan, true),
			new EntranceData(EntranceNames.chaos_shrine_black_orb_warp, RegionNames.chaos_shrine, EntranceNames.chaos_shrine_1f_center_warp, RegionNames.chaos_shrine_1f_entrance, (int)EntGroup.Fixed, EntranceNames.overworld_chaos_shrine, false, true),
			new EntranceData(EntranceNames.marsh_cave_b1_top_stairs, RegionNames.marsh_cave_b1, EntranceNames.marsh_cave_b2_entrance, RegionNames.marsh_cave_b2_top, (int)EntGroup.InnerDungeon, EntranceNames.overworld_marsh_cave, true),
			new EntranceData(EntranceNames.marsh_cave_b1_bottom_stairs, RegionNames.marsh_cave_b1, EntranceNames.marsh_cave_b2_upper_stairs, RegionNames.marsh_cave_b2_bottom, (int)EntGroup.InnerDungeon, EntranceNames.overworld_marsh_cave),
			new EntranceData(EntranceNames.marsh_cave_b2_lower_stairs, RegionNames.marsh_cave_b2_bottom, EntranceNames.marsh_cave_b3_entrance, RegionNames.marsh_cave_b3, (int)EntGroup.InnerDungeon, EntranceNames.overworld_marsh_cave, true),
			new EntranceData(EntranceNames.cavern_of_earth_b1_right_stairs, RegionNames.cavern_of_earth_b1, EntranceNames.cavern_of_earth_b2_left_stairs, RegionNames.cavern_of_earth_b2, (int)EntGroup.InnerDungeon, EntranceNames.overworld_cavern_of_earth),
			new EntranceData(EntranceNames.cavern_of_earth_b2_right_stairs, RegionNames.cavern_of_earth_b2, EntranceNames.cavern_of_earth_b3_bottom_stairs, RegionNames.cavern_of_earth_b3, (int)EntGroup.InnerDungeon, EntranceNames.overworld_cavern_of_earth),
			new EntranceData(EntranceNames.cavern_of_earth_b3_center_stairs, RegionNames.cavern_of_earth_b3, EntranceNames.cavern_of_earth_b4_right_stairs, RegionNames.cavern_of_earth_b4, (int)EntGroup.InnerDungeon, EntranceNames.overworld_cavern_of_earth, false, true),
			new EntranceData(EntranceNames.cavern_of_earth_b4_left_stairs, RegionNames.cavern_of_earth_b4, EntranceNames.cavern_of_earth_b5_bottom_stairs, RegionNames.cavern_of_earth_b5, (int)EntGroup.InnerDungeon, EntranceNames.overworld_cavern_of_earth, true),
			new EntranceData(EntranceNames.mount_gulg_b1_left_stairs, RegionNames.mount_gulg_b1, EntranceNames.mount_gulg_b2_right_stairs, RegionNames.mount_gulg_b2, (int)EntGroup.InnerDungeon, EntranceNames.overworld_mount_gulg),
			new EntranceData(EntranceNames.mount_gulg_b2_left_stairs, RegionNames.mount_gulg_b2, EntranceNames.mount_gulg_b3_corridor_middle_stairs, RegionNames.mount_gulg_b3_corridor, (int)EntGroup.InnerDungeon, EntranceNames.overworld_mount_gulg),
			new EntranceData(EntranceNames.mount_gulg_b3_corridor_right_stairs, RegionNames.mount_gulg_b3_corridor, EntranceNames.mount_gulg_b4_squares_top_stairs, RegionNames.mount_gulg_b4_squares, (int)EntGroup.InnerDungeon, EntranceNames.overworld_mount_gulg),
			new EntranceData(EntranceNames.mount_gulg_b4_squares_bottom_stairs, RegionNames.mount_gulg_b4_squares, EntranceNames.mount_gulg_b3_maze_top_stairs, RegionNames.mount_gulg_b3_maze, (int)EntGroup.InnerDungeon, EntranceNames.overworld_mount_gulg),
			new EntranceData(EntranceNames.mount_gulg_b3_maze_bottom_stairs, RegionNames.mount_gulg_b3_maze, EntranceNames.mount_gulg_b4_agama_top_stairs, RegionNames.mount_gulg_b4_agama, (int)EntGroup.InnerDungeon, EntranceNames.overworld_mount_gulg),
			new EntranceData(EntranceNames.mount_gulg_b4_agama_bottom_stairs, RegionNames.mount_gulg_b4_agama, EntranceNames.mount_gulg_b5_center_stairs, RegionNames.mount_gulg_b5, (int)EntGroup.InnerDungeon, EntranceNames.overworld_mount_gulg, true),
			new EntranceData(EntranceNames.cavern_of_ice_b1_entrance_bottom_stairs, RegionNames.cavern_of_ice_b1_entrance, EntranceNames.cavern_of_ice_b2_square_top_stairs, RegionNames.cavern_of_ice_b2_square, (int)EntGroup.InnerDungeon, EntranceNames.overworld_cavern_of_ice),
			new EntranceData(EntranceNames.cavern_of_ice_b2_square_bottom_stairs, RegionNames.cavern_of_ice_b2_square, EntranceNames.cavern_of_ice_b3_small_top_stairs, RegionNames.cavern_of_ice_b3_small, (int)EntGroup.InnerDungeon, EntranceNames.overworld_cavern_of_ice),
			new EntranceData(EntranceNames.cavern_of_ice_b3_small_bottom_stairs, RegionNames.cavern_of_ice_b3_small, EntranceNames.cavern_of_ice_b2_room_entrance, RegionNames.cavern_of_ice_b2_room, (int)EntGroup.InnerDungeon, EntranceNames.overworld_cavern_of_ice, true),
			new EntranceData(EntranceNames.cavern_of_ice_b2_room_hole, RegionNames.cavern_of_ice_b2_room, EntranceNames.cavern_of_ice_b3_treasury_trap_room, RegionNames.cavern_of_ice_b3_treasury, (int)EntGroup.Fixed, EntranceNames.overworld_cavern_of_ice),
			new EntranceData(EntranceNames.cavern_of_ice_b3_treasury_right_stairs, RegionNames.cavern_of_ice_b3_treasury, EntranceNames.cavern_of_ice_b1_backdoor_left_stairs, RegionNames.cavern_of_ice_b1_backdoor, (int)EntGroup.Fixed, EntranceNames.overworld_cavern_of_ice),
			new EntranceData(EntranceNames.cavern_of_ice_b1_backdoor_hole, RegionNames.cavern_of_ice_b1_backdoor, EntranceNames.cavern_of_ice_b2_room_ledge, RegionNames.cavern_of_ice_b2_ledge, (int)EntGroup.Fixed, EntranceNames.overworld_cavern_of_ice),
			new EntranceData(EntranceNames.citadel_of_trials_1f_throne, RegionNames.citadel_of_trials_1f, EntranceNames.citadel_of_trials_2f_maze, RegionNames.citadel_of_trials_2f, (int)EntGroup.Fixed, EntranceNames.overworld_citadel_of_trials,true, true),
			new EntranceData(EntranceNames.sunken_shrine_3f_split_right_stairs, RegionNames.sunken_shrine_3f_split, EntranceNames.sunken_shrine_4f_tfc_top_stairs, RegionNames.sunken_shrine_4f_tfc, (int)EntGroup.InnerDungeon, "Sunken Shrine"),
			new EntranceData(EntranceNames.sunken_shrine_4f_tfc_bottom_stairs, RegionNames.sunken_shrine_4f_tfc, EntranceNames.sunken_shrine_5f_center_stairs, RegionNames.sunken_shrine_5f, (int)EntGroup.InnerDungeon, "Sunken Shrine", true),
			new EntranceData(EntranceNames.sunken_shrine_3f_split_left_stairs, RegionNames.sunken_shrine_3f_split, EntranceNames.sunken_shrine_2f_sunken_city_bottom_stairs, RegionNames.sunken_shrine_2f_sunken_city, (int)EntGroup.InnerDungeon, "Sunken Shrine"),
			new EntranceData(EntranceNames.sunken_shrine_2f_sunken_city_top_stairs, RegionNames.sunken_shrine_2f_sunken_city, EntranceNames.sunken_shrine_3f_small_bottom_stairs, RegionNames.sunken_shrine_3f_small, (int)EntGroup.InnerDungeon, "Sunken Shrine"),
			new EntranceData(EntranceNames.sunken_shrine_3f_small_top_stairs, RegionNames.sunken_shrine_3f_small, EntranceNames.sunken_shrine_4f_square_top_stairs, RegionNames.sunken_shrine_4f_square, (int)EntGroup.InnerDungeon, "Sunken Shrine"),
			new EntranceData(EntranceNames.sunken_shrine_4f_square_bottom_stairs, RegionNames.sunken_shrine_4f_square, EntranceNames.sunken_shrine_3f_vertical_top_stairs, RegionNames.sunken_shrine_3f_vertical, (int)EntGroup.InnerDungeon, "Sunken Shrine"),
			new EntranceData(EntranceNames.sunken_shrine_3f_vertical_bottom_stairs, RegionNames.sunken_shrine_3f_vertical, EntranceNames.sunken_shrine_2f_sharknado_right_stairs, RegionNames.sunken_shrine_2f_sharknado, (int)EntGroup.InnerDungeon, "Sunken Shrine"),
			new EntranceData(EntranceNames.sunken_shrine_2f_sharknado_left_stairs, RegionNames.sunken_shrine_2f_sharknado, EntranceNames.sunken_shrine_1f_bottom_stairs, RegionNames.sunken_shrine_1f, (int)EntGroup.InnerDungeon, "Sunken Shrine", true),
			new EntranceData(EntranceNames.mirage_tower_1f_left_stairs, RegionNames.mirage_tower_1f, EntranceNames.mirage_tower_2f_bottom_stairs, RegionNames.mirage_tower_2f, (int)EntGroup.InnerDungeon, EntranceNames.overworld_mirage_tower),
			new EntranceData(EntranceNames.mirage_tower_2f_top_stairs, RegionNames.mirage_tower_2f, EntranceNames.mirage_tower_3f_top_stairs, RegionNames.mirage_tower_3f, (int)EntGroup.InnerDungeon, EntranceNames.overworld_mirage_tower),
			new EntranceData(EntranceNames.mirage_tower_3f_center_warp, RegionNames.mirage_tower_3f, EntranceNames.flying_fortress_1f_center_warp, RegionNames.flying_fortress_1f, (int)EntGroup.InnerDungeon, EntranceNames.overworld_mirage_tower, false, true),
			new EntranceData(EntranceNames.flying_fortress_1f_top_warp, RegionNames.flying_fortress_1f, EntranceNames.flying_fortress_2f_top_warp, RegionNames.flying_fortress_2f, (int)EntGroup.InnerDungeon, EntranceNames.overworld_mirage_tower),
			new EntranceData(EntranceNames.flying_fortress_2f_bottom_warp, RegionNames.flying_fortress_2f, EntranceNames.flying_fortress_3f_center_warp, RegionNames.flying_fortress_3f, (int)EntGroup.InnerDungeon, EntranceNames.overworld_mirage_tower),
			new EntranceData(EntranceNames.flying_fortress_3f_left_warp, RegionNames.flying_fortress_3f, EntranceNames.flying_fortress_4f_entrance_warp, RegionNames.flying_fortress_4f, (int)EntGroup.InnerDungeon, EntranceNames.overworld_mirage_tower),
			new EntranceData(EntranceNames.flying_fortress_4f_exit_warp, RegionNames.flying_fortress_4f, EntranceNames.flying_fortress_5f_bottom_warp, RegionNames.flying_fortress_5f, (int)EntGroup.InnerDungeon, EntranceNames.overworld_mirage_tower, true),
			new EntranceData(EntranceNames.chaos_shrine_1f_entrance_left_stairs, RegionNames.chaos_shrine_1f_entrance, EntranceNames.chaos_shrine_b1_deadend_dead_end_stairs, RegionNames.chaos_shrine_b1_deadend, (int)EntGroup.ChaosShrine, "Chaos Shrine", true),
			new EntranceData(EntranceNames.chaos_shrine_1f_entrance_right_stairs, RegionNames.chaos_shrine_1f_entrance, EntranceNames.chaos_shrine_2f_corridor_left_stairs, RegionNames.chaos_shrine_2f_corridor, (int)EntGroup.ChaosShrine, "Chaos Shrine"),
			new EntranceData(EntranceNames.chaos_shrine_2f_corridor_right_stairs, RegionNames.chaos_shrine_2f_corridor, EntranceNames.chaos_shrine_3f_plaza_left_stairs, RegionNames.chaos_shrine_3f_plaza, (int)EntGroup.ChaosShrine, "Chaos Shrine"),
			new EntranceData(EntranceNames.chaos_shrine_3f_plaza_center_stairs, RegionNames.chaos_shrine_3f_plaza, EntranceNames.chaos_shrine_2f_plaza_center_stairs, RegionNames.chaos_shrine_2f_plaza, (int)EntGroup.ChaosShrine, "Chaos Shrine", false, true),
			new EntranceData(EntranceNames.chaos_shrine_2f_plaza_left_stairs, RegionNames.chaos_shrine_2f_plaza, EntranceNames.chaos_shrine_1f_corridor_right_stairs, RegionNames.chaos_shrine_1f_corridor, (int)EntGroup.ChaosShrine, "Chaos Shrine"),
			new EntranceData(EntranceNames.chaos_shrine_1f_corridor_left_stairs, RegionNames.chaos_shrine_1f_corridor, EntranceNames.chaos_shrine_b1_earth_left_stairs, RegionNames.chaos_shrine_b1_earth, (int)EntGroup.ChaosShrine, "Chaos Shrine"),
			new EntranceData(EntranceNames.chaos_shrine_b1_earth_right_stairs, RegionNames.chaos_shrine_b1_earth, EntranceNames.chaos_shrine_b2_left_stairs, RegionNames.chaos_shrine_b2, (int)EntGroup.ChaosShrine, "Chaos Shrine"),
			new EntranceData(EntranceNames.chaos_shrine_b2_right_stairs, RegionNames.chaos_shrine_b2, EntranceNames.chaos_shrine_b3_top_stairs, RegionNames.chaos_shrine_b3, (int)EntGroup.ChaosShrine, "Chaos Shrine"),
			new EntranceData(EntranceNames.chaos_shrine_b3_bottom_stairs, RegionNames.chaos_shrine_b3, EntranceNames.chaos_shrine_b4_left_stairs, RegionNames.chaos_shrine_b4, (int)EntGroup.ChaosShrine, "Chaos Shrine"),
			new EntranceData(EntranceNames.chaos_shrine_b4_right_stairs, RegionNames.chaos_shrine_b4, EntranceNames.chaos_shrine_b5_top_stairs, RegionNames.chaos_shrine_b5, (int)EntGroup.ChaosShrine, "Chaos Shrine", true),

			new EntranceData(EntranceNames.giants_cavern_east_entrance, RegionNames.giants_cavern, EntranceNames.overworld_giants_cavern_east, RegionNames.onrac_region, (int)EntGroup.Titan, "Titan", true, true),
			new EntranceData(EntranceNames.giants_cavern_west_entrance, RegionNames.giants_cavern, EntranceNames.overworld_giants_cavern_west, RegionNames.sage_region, (int)EntGroup.Titan, "Titan", true, true),
		};

		public static List<string> innersea_entrances = new()
		{
			EntranceNames.overworld_cornelia,
			EntranceNames.overworld_castle_cornelia,
			EntranceNames.overworld_chaos_shrine,
			EntranceNames.overworld_matoyas_cave,
			EntranceNames.overworld_pravoka,
			EntranceNames.overworld_mount_duergar,
			EntranceNames.overworld_western_keep,
			EntranceNames.overworld_marsh_cave,
			EntranceNames.overworld_elven_castle,
			EntranceNames.overworld_elfheim,
			EntranceNames.overworld_cavern_of_ice,
			EntranceNames.overworld_mount_gulg,
			EntranceNames.overworld_crescent_lake,
		};

		public static List<string> town_entrances = new()
		{
			EntranceNames.overworld_pravoka,
			EntranceNames.overworld_elfheim,
			EntranceNames.overworld_melmond,
			EntranceNames.overworld_crescent_lake,
			EntranceNames.overworld_onrac,
			EntranceNames.overworld_gaia,
			EntranceNames.overworld_lufenia
		};

		public static List<string> safe_entrances_overworld_only = new()
		{
			EntranceNames.overworld_chaos_shrine,
			EntranceNames.overworld_matoyas_cave,
			EntranceNames.overworld_dragon_caves_marsh,
			EntranceNames.overworld_dragon_caves_plains,
			EntranceNames.overworld_dragon_caves_forest,
		};

		public static List<string> safe_entrances = safe_entrances_overworld_only.Append(EntranceNames.sunken_shrine_4f_tfc_bottom_stairs).ToList();

		public static List<string> safe_overworld_entrances_early = new()
		{
			EntranceNames.overworld_chaos_shrine,
			EntranceNames.overworld_castle_cornelia,
			EntranceNames.overworld_matoyas_cave,
		};

		public static List<string> safe_overworld_entrances_west = new()
		{
			EntranceNames.overworld_chaos_shrine,
			EntranceNames.overworld_castle_cornelia,
			EntranceNames.overworld_mount_duergar
		};

		public static List<string> internal_dungeons = new()
		{
			EntranceNames.overworld_marsh_cave,
			EntranceNames.overworld_cavern_of_earth,
			EntranceNames.overworld_mount_gulg,
			EntranceNames.overworld_cavern_of_ice,
			"Sunken Shrine",
			EntranceNames.overworld_mirage_tower
		};

		public static List<string> internal_dungeons_ext = new List<string>()
		{
			EntranceNames.overworld_waterfall,
			EntranceNames.overworld_citadel_of_trials,
		}.Concat(internal_dungeons).ToList();

		public static List<string> split_regions = new()
		{
			RegionNames.marsh_cave_b1,
			RegionNames.sunken_shrine_3f_split,
			RegionNames.chaos_shrine_1f_entrance
		};

		public static Dictionary<string, List<string>> titan_regions = new()
		{
			{ RegionNames.innersea_region, new() { EntranceNames.overworld_cornelia, EntranceNames.overworld_chaos_shrine, EntranceNames.overworld_matoyas_cave, EntranceNames.overworld_pravoka, EntranceNames.overworld_mount_duergar, EntranceNames.overworld_western_keep, EntranceNames.overworld_marsh_cave, EntranceNames.overworld_elfheim, EntranceNames.overworld_elven_castle, EntranceNames.overworld_crescent_lake, EntranceNames.overworld_mount_gulg, EntranceNames.overworld_cavern_of_ice }},
			{ RegionNames.pravoka_region, new() { EntranceNames.overworld_melmond, EntranceNames.overworld_cavern_of_earth, EntranceNames.overworld_giants_cavern_east }},
			{ RegionNames.sage_region, new() { EntranceNames.overworld_sages_cave, EntranceNames.overworld_giants_cavern_west }},
			{ RegionNames.onrac_region, new() { EntranceNames.overworld_onrac, EntranceNames.overworld_caravan, EntranceNames.overworld_waterfall }},
			{ RegionNames.bahamuts_island, new() { EntranceNames.overworld_dragon_caves_bahamut, EntranceNames.overworld_dragon_caves_top }}
		};

		public static List<string> overworld_regions = new()
		{
			RegionNames.cornelia_region,
			RegionNames.pravoka_region,
			RegionNames.innersea_region,
			RegionNames.ice_region,
			RegionNames.crescent_region,
			RegionNames.gulg_region,
			RegionNames.ryukhan_desert,
			RegionNames.melmond_region,
			RegionNames.sage_region,
			RegionNames.dragon_small_island,
			RegionNames.dragon_marsh_island,
			RegionNames.dragon_forest_island,
			RegionNames.dragon_plains_island,
			RegionNames.bahamuts_island,
			RegionNames.onrac_region,
			RegionNames.trials_region,
			RegionNames.gaia_region,
			RegionNames.mirage_desert,
			RegionNames.lufenia_region
		};

		public static List<string> location_regions = new()
		{
			RegionNames.cornelia,
			RegionNames.pravoka,
			RegionNames.elfheim,
			RegionNames.melmond,
			RegionNames.crescent_lake,
			RegionNames.onrac,
			RegionNames.gaia,
			RegionNames.lufenia,

			RegionNames.castle_cornelia_1f,
			RegionNames.castle_cornelia_2f,
			RegionNames.chaos_shrine,
			RegionNames.matoyas_cave,
			RegionNames.mount_duergar,
			RegionNames.elven_castle,
			RegionNames.western_keep,
			RegionNames.giants_cavern,
			RegionNames.sages_cave,
			RegionNames.caravan_outside,
			RegionNames.caravan_tent,
			RegionNames.waterfall,
			RegionNames.dragon_caves_plains,
			RegionNames.dragon_caves_forest,
			RegionNames.dragon_caves_top,
			RegionNames.dragon_caves_marsh,
			RegionNames.dragon_caves_small,
			RegionNames.dragon_caves_bahamut_corridor,
			RegionNames.dragon_caves_bahamut_hall,
			RegionNames.marsh_cave_b1,
			RegionNames.marsh_cave_b2_top,
			RegionNames.marsh_cave_b2_bottom,
			RegionNames.marsh_cave_b3,
			RegionNames.cavern_of_earth_b1,
			RegionNames.cavern_of_earth_b2,
			RegionNames.cavern_of_earth_b3,
			RegionNames.cavern_of_earth_b4,
			RegionNames.cavern_of_earth_b5,
			RegionNames.mount_gulg_b1,
			RegionNames.mount_gulg_b2,
			RegionNames.mount_gulg_b3_corridor,
			RegionNames.mount_gulg_b4_squares,
			RegionNames.mount_gulg_b3_maze,
			RegionNames.mount_gulg_b4_agama,
			RegionNames.mount_gulg_b5,
			RegionNames.cavern_of_ice_b1_entrance,
			RegionNames.cavern_of_ice_b2_square,
			RegionNames.cavern_of_ice_b3_small,
			RegionNames.cavern_of_ice_b2_room,
			RegionNames.cavern_of_ice_b3_treasury,
			RegionNames.cavern_of_ice_b1_backdoor,
			RegionNames.cavern_of_ice_b2_ledge,
			RegionNames.citadel_of_trials_1f,
			RegionNames.citadel_of_trials_2f,
			RegionNames.sunken_shrine_3f_split,
			RegionNames.sunken_shrine_4f_tfc,
			RegionNames.sunken_shrine_5f,
			RegionNames.sunken_shrine_2f_sunken_city,
			RegionNames.sunken_shrine_3f_small,
			RegionNames.sunken_shrine_4f_square,
			RegionNames.sunken_shrine_3f_vertical,
			RegionNames.sunken_shrine_2f_sharknado,
			RegionNames.sunken_shrine_1f,
			RegionNames.mirage_tower_1f,
			RegionNames.mirage_tower_2f,
			RegionNames.mirage_tower_3f,
			RegionNames.flying_fortress_1f,
			RegionNames.flying_fortress_2f,
			RegionNames.flying_fortress_3f,
			RegionNames.flying_fortress_4f,
			RegionNames.flying_fortress_5f,
			RegionNames.chaos_shrine_1f_entrance,
			RegionNames.chaos_shrine_b1_deadend,
			RegionNames.chaos_shrine_2f_corridor,
			RegionNames.chaos_shrine_3f_plaza,
			RegionNames.chaos_shrine_2f_plaza,
			RegionNames.chaos_shrine_1f_corridor,
			RegionNames.chaos_shrine_b1_earth,
			RegionNames.chaos_shrine_b2,
			RegionNames.chaos_shrine_b3,
			RegionNames.chaos_shrine_b4,
			RegionNames.chaos_shrine_b5
		};
	}
}
