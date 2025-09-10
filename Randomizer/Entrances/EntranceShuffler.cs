using Last.Data.Master;
using Last.Interpreter;
using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Bindings;
using static FF1PRAP.Randomizer;

namespace FF1PRAP
{

	public class EntranceShufflingData
	{
		public string name;
		public List<EntranceData> origin_entrances;
		public List<EntranceData> entrances;
		public List<EntranceData> stored_deadends;
		public bool overworld_shuffle;
		public bool allow_fill;

		public EntranceShufflingData(string name, List<EntranceData> origin_entrances, bool overworld_shuffle = true, bool allow_fill = true)
		{
			this.name = name;
			this.origin_entrances = origin_entrances;
			this.entrances = new();
			this.stored_deadends = new();
			this.overworld_shuffle = overworld_shuffle;
			this.allow_fill = allow_fill;
		}

		public bool early_candidate()
		{
			return overworld_shuffle && allow_fill && (entrances.Count == 0);
		}

		public bool deadend_candidate()
		{
			return allow_fill && (origin_entrances.Count - stored_deadends.Count > 0);
		}

	}
	partial class Logic
	{
		public static Dictionary<string, string> result_entrances = new();
		public static List<EntranceData> all_entrances = new();
		public static List<EntranceData> new_entrances = new();
		public static Dictionary<string, Dictionary<string, string>> region_dict = new();

		private static EntranceData random_pop(List<EntranceData> entrances, MT19337 rng)
		{
			return rng.TakeFrom(entrances);
		}

		private static void connect_entrance(EntranceShufflingData connect_pool, EntranceData target_entrance, MT19337 rng)
		{
			var origin_entrance = rng.TakeFrom(connect_pool.origin_entrances);
			EntranceData new_entrance = new(origin_entrance.name, origin_entrance.region, target_entrance.target_point, target_entrance.target_region, target_entrance.type, target_entrance.group, target_entrance.deadend, target_entrance.access_req);
			new_entrances.Add(new_entrance);

			if(origin_entrance.target_point != target_entrance.target_point)
			{
				result_entrances.Add(origin_entrance.name, target_entrance.target_point);
			}

			if (!target_entrance.deadend)
			{ 
				connect_pool.origin_entrances.AddRange(global_entrances.Where(e => e.region == target_entrance.region));
			}
		}

		private static void place_entrance_rng(List<EntranceShufflingData> pools, EntranceData entrance, MT19337 rng, bool disable_fill = false)
		{
			var place_pool = rng.PickFrom(pools);
			place_pool.entrances.Add(entrance);
			if (all_entrances.Contains(entrance))
			{ 
				all_entrances.Remove(entrance);
			}

			if (disable_fill)
			{
				place_pool.allow_fill = false;
			}

			connect_entrance(place_pool, entrance, rng);
		}

		private static void place_entrance(EntranceShufflingData single_pool, EntranceData entrance, MT19337 rng, bool disable_fill = false)
		{
			var place_pool = single_pool;
			place_pool.entrances.Add(entrance);
			if (all_entrances.Contains(entrance))
			{
				all_entrances.Remove(entrance);
			}

			if (disable_fill)
			{
				place_pool.allow_fill = false;
			}

			connect_entrance(place_pool, entrance, rng);
		}

		public static void init_regions()
		{
			foreach (var region_name in overworld_regions)
			{
				region_dict[region_name] = new();
			}

			foreach (var region_name in location_regions)
			{
				region_dict[region_name] = new();
			}

		}
		public static void shuffle_entrance(bool shuffle_overworld, ShuffleEntrancesMode shuffle_entrances, ShuffleTownsMode shuffle_towns, EarlyProgressionModes early_progression, MT19337 rng)
		{
			bool ow_is_shuffled = shuffle_overworld || (shuffle_entrances == ShuffleEntrancesMode.All);
			new_entrances = new();
			all_entrances = new();
			result_entrances = new();

			// 1. Process Town options
			var town_option = shuffle_towns;
			if (town_option == ShuffleTownsMode.ShuffleDeep && shuffle_entrances < ShuffleEntrancesMode.All)
			{
				town_option = ShuffleTownsMode.ShuffleShallow;
			}

			if (town_option == ShuffleTownsMode.ShuffleShallow && !shuffle_overworld)
			{
				town_option = ShuffleTownsMode.BetweenTowns;
			}

			// 2. Initial Pools + Add special pools
			var ow_entrances = global_entrances.Where(e => e.type == (int)EntGroup.OverworldDungeon || e.type == (int)EntGroup.OverworldTown).ToList();
			var ow_town_entrances = global_entrances.Where(e => e.type == (int)EntGroup.OverworldTown).ToList();
			var ow_pools = ow_entrances.ToDictionary(e => e.name, e => new EntranceShufflingData(e.name, new() { e }));

			ow_pools["Sunken Shrine"] = new EntranceShufflingData("Sunken Shrine", global_entrances.Where(e => e.region == RegionNames.sunken_shrine_3f_split).ToList(), false, shuffle_entrances > ShuffleEntrancesMode.NoShuffle);
			ow_pools["Chaos Shrine"] = new EntranceShufflingData("Chaos Shrine", global_entrances.Where(e => e.region == RegionNames.chaos_shrine_1f_entrance).ToList(), false, false);

			// 3. Set Entrances to be shuffle
			if (ow_is_shuffled)
			{
				all_entrances = ow_entrances;
			}

			List<string> dungeon_entrances = new();

			if (shuffle_entrances > ShuffleEntrancesMode.NoShuffle && shuffle_entrances < ShuffleEntrancesMode.All)
			{
				if (shuffle_entrances == ShuffleEntrancesMode.DungeonInternal)
				{
					dungeon_entrances = internal_dungeons;
				}
				else
				{
					dungeon_entrances = internal_dungeons_ext;
				}

				foreach (var dungeon in dungeon_entrances)
				{
					all_entrances.AddRange(global_entrances.Where(e => e.group == dungeon && e.type != (int)EntGroup.Fixed && !all_entrances.Contains(e)));
				}
			}
			else if (shuffle_entrances == ShuffleEntrancesMode.All)
			{
				all_entrances = global_entrances.Where(e => e.type == (int)EntGroup.InnerDungeon).ToList();
			}

			var shallow_entrances = global_entrances.Where(e => e.name == EntranceNames.overworld_chaos_shrine).ToList();

			// 4. Process Towns
			if (early_progression == EarlyProgressionModes.BikkesShip)
			{
				if (ow_town_entrances.TryFind(t => t.name == EntranceNames.overworld_pravoka, out var pravoka_town))
				{
					place_entrance(ow_pools[pravoka_town.name], pravoka_town, rng, true);
					ow_town_entrances.Remove(pravoka_town);
				}
			}

			if (town_option == ShuffleTownsMode.NoShuffle)
			{
				foreach (var town in ow_town_entrances)
				{
					all_entrances = all_entrances.Where(e => e != town).ToList();
					ow_pools[town.name].entrances.Add(town);
					ow_pools[town.name].allow_fill = false;
				}
			}
			else if (town_option == ShuffleTownsMode.BetweenTowns)
			{
				var town_pools = ow_pools.Values.Where(p => ow_town_entrances.Where(t => t.name == p.name).Any()).ToList();
				foreach (var pool in town_pools)
				{
					var picked_entrance = rng.TakeFrom(ow_town_entrances);
					place_entrance(pool, picked_entrance, rng, true);
				}
			}
			else if (town_option == ShuffleTownsMode.ShuffleShallow)
			{
				shallow_entrances.AddRange(ow_town_entrances);
			}

			// 5. Process Special Entrances
			// Safe Entrance
			List<string> candidate_safe_entrances = new();
			List<string> safe_ow_entrances = new();
			if (ow_is_shuffled)
			{
				if (shuffle_entrances == ShuffleEntrancesMode.All)
				{
					candidate_safe_entrances = safe_entrances;
				}
				else
				{
					candidate_safe_entrances = safe_entrances_overworld_only;
				}
			
				var safe_entrance = rng.PickFrom(all_entrances.Where(e => candidate_safe_entrances.Contains(e.name)).ToList());
				if (early_progression == EarlyProgressionModes.BikkesShip)
				{
					safe_ow_entrances = safe_overworld_entrances_early;
				}
				else
				{
					safe_ow_entrances = safe_overworld_entrances_west;
				}

				var safe_pools = ow_pools.Values.Where(p => safe_ow_entrances.Contains(p.name) && p.early_candidate()).ToList();
				place_entrance_rng(safe_pools, safe_entrance, rng, true);
			}

			// Titan
			if (ow_is_shuffled)
			{
				var titan_entrances = all_entrances.Where(e => e.target_region == RegionNames.giants_cavern).ToList();

				Dictionary<string, List<EntranceShufflingData>> new_titan_regions = new();

				foreach (var region in titan_regions)
				{
					var valid_entrances = ow_pools.Values.Where(p => !p.origin_entrances.Where(e => e.access_req).Any() && p.early_candidate() && region.Value.Contains(p.name)).ToList();
					if (valid_entrances.Any())
					{
						new_titan_regions.Add(region.Key, valid_entrances);
					}
				}

				var new_titan_regions_list = new_titan_regions.Values.ToList();
				foreach (var titan_entrance in titan_entrances)
				{
					var picked_region = rng.TakeFrom(new_titan_regions_list);
					place_entrance_rng(picked_region, titan_entrance, rng, true);
				}
			}

			// Mount Duergar (prevent Canal softlock)
			if (ow_is_shuffled)
			{
				var dwarf_entrance = all_entrances.Find(e => e.target_region == RegionNames.mount_duergar);
				var dwarf_pools = ow_pools.Values.Where(p => innersea_entrances.Contains(p.name) && p.early_candidate()).ToList();

				if (shuffle_entrances < ShuffleEntrancesMode.All)
				{
					place_entrance_rng(dwarf_pools, dwarf_entrance, rng, true);
				}
				else
				{ 
					var dwarf_pool = rng.PickFrom(dwarf_pools);
					dwarf_pool.stored_deadends.Add(dwarf_entrance);
					dwarf_pool.entrances.Add(dwarf_entrance);
					all_entrances.Remove(dwarf_entrance);
				}
			}

			// Prevent Onrac getting palced in Sunken Shrine
			if (shuffle_entrances == ShuffleEntrancesMode.All && shuffle_towns == ShuffleTownsMode.ShuffleDeep)
			{
				var onrac_town = all_entrances.Find(e => e.target_region == RegionNames.onrac);
				var onrac_pools = ow_pools.Values.Where(p => p.name != "Sunken Shrine" && p.early_candidate()).ToList();

				var onrac_pool = rng.PickFrom(onrac_pools);
				onrac_pool.stored_deadends.Add(onrac_town);
				onrac_pool.entrances.Add(onrac_town);
				all_entrances.Remove(onrac_town);
			}

			// Shallow Entrances
			if (ow_is_shuffled)
			{
				foreach (var e in all_entrances.Intersect(shallow_entrances).ToList())
				{
					var shallow_pools = ow_pools.Values.Where(p => p.early_candidate()).ToList();
					place_entrance_rng(shallow_pools, e, rng, true);
				}
			}

			// Chaos Shrine
			if (shuffle_entrances > ShuffleEntrancesMode.NoShuffle)
			{
				var chaos_entrances = global_entrances.Where(e => e.group == "Chaos Shrine").ToList();
				var chaos_entrances_prog = chaos_entrances.Where(e => !e.deadend).ToList();
				var chaos_entrances_dead = chaos_entrances.Where(e => e.deadend).ToList();

				while (chaos_entrances_prog.Any())
				{
					var chaos_entrance = rng.TakeFrom(chaos_entrances_prog);
					place_entrance(ow_pools["Chaos Shrine"], chaos_entrance, rng);
				}

				while (chaos_entrances_dead.Any())
				{
					var chaos_entrance = rng.TakeFrom(chaos_entrances_dead);
					place_entrance(ow_pools["Chaos Shrine"], chaos_entrance, rng);
				}
			}

			// 6. Place everything else
			var prog_mixed_entrances = all_entrances.Where(e => !e.deadend).ToList();
			var deadend_mixed_entrances = all_entrances.Where(e => !e.deadend).ToList();

			// Process internal dungeon pool
			Dictionary<string, EntranceShufflingData> dungeon_pools = new();

			if (shuffle_entrances > ShuffleEntrancesMode.NoShuffle && shuffle_entrances < ShuffleEntrancesMode.All)
			{
				dungeon_pools.Add("Sunken Shrine", ow_pools["Sunken Shrine"]);

				foreach (var group in dungeon_entrances)
				{
					if (!dungeon_pools.ContainsKey(group))
					{
						EntranceShufflingData select_pool;
						if (shuffle_overworld)
						{
							select_pool = rng.PickFrom(ow_pools.Values.Where(p => p.early_candidate() && !dungeon_pools.ContainsValue(p)).ToList());
						}
						else
						{
							select_pool = ow_pools[group];
						}
						dungeon_pools[group] = select_pool;
					}
				}
			}

			// Place Progression Entrances
			while (prog_mixed_entrances.Any())
			{
				var select_entrance = rng.TakeFrom(prog_mixed_entrances);

				if (shuffle_overworld && shuffle_entrances == ShuffleEntrancesMode.NoShuffle)
				{
					place_entrance_rng(ow_pools.Values.Where(p => p.early_candidate()).ToList(), select_entrance, rng);
				}
				else if (shuffle_entrances == ShuffleEntrancesMode.DungeonInternal)
				{
					if (dungeon_pools.ContainsKey(select_entrance.group))
					{
						place_entrance(dungeon_pools[select_entrance.group], select_entrance, rng);
					}
					else
					{
						place_entrance_rng(ow_pools.Values.Where(p => p.early_candidate() && !dungeon_pools.ContainsValue(p)).ToList(), select_entrance, rng);
					}
				}
				else if (shuffle_entrances == ShuffleEntrancesMode.DungeonMixed)
				{
					if (dungeon_pools.ContainsKey(select_entrance.group))
					{
						place_entrance_rng(dungeon_pools.Values.ToList(), select_entrance, rng);
					}
					else
					{
						place_entrance_rng(ow_pools.Values.Where(p => p.early_candidate() && !dungeon_pools.ContainsValue(p)).ToList(), select_entrance, rng);
					}
				}
				else if (shuffle_entrances == ShuffleEntrancesMode.All)
				{
					place_entrance_rng(ow_pools.Values.Where(p => p.allow_fill).ToList(), select_entrance, rng);
				}
			}

			// Place Deadend Entrances
			while (deadend_mixed_entrances.Any())
			{
				var select_entrance = rng.TakeFrom(deadend_mixed_entrances);

				if (shuffle_overworld && shuffle_entrances == ShuffleEntrancesMode.NoShuffle)
				{
					place_entrance_rng(ow_pools.Values.Where(p => p.early_candidate()).ToList(), select_entrance, rng);
				}
				else if (shuffle_entrances == ShuffleEntrancesMode.DungeonInternal)
				{
					if (dungeon_pools.ContainsKey(select_entrance.group))
					{
						place_entrance(dungeon_pools[select_entrance.group], select_entrance, rng);
					}
					else
					{
						place_entrance_rng(ow_pools.Values.Where(p => p.early_candidate() && !dungeon_pools.ContainsValue(p)).ToList(), select_entrance, rng);
					}
				}
				else if (shuffle_entrances == ShuffleEntrancesMode.DungeonMixed)
				{
					if (dungeon_pools.ContainsKey(select_entrance.group))
					{
						place_entrance_rng(dungeon_pools.Values.Where(p => p.deadend_candidate()).ToList(), select_entrance, rng);
					}
					else
					{
						place_entrance_rng(ow_pools.Values.Where(p => p.early_candidate() && !dungeon_pools.ContainsValue(p)).ToList(), select_entrance, rng);
					}
				}
				else if (shuffle_entrances == ShuffleEntrancesMode.All)
				{
					place_entrance_rng(ow_pools.Values.Where(p => p.deadend_candidate()).ToList(), select_entrance, rng);
				}
			}


			// 8. Process stored deadends and finalize
			foreach (var p in ow_pools.Values)
			{
				foreach (var deadend in p.stored_deadends)
				{ 
					place_entrance(p, deadend, rng);
				}
			}

			var new_entrances_names = new_entrances.Select(e => e.name).ToList();
			var fixed_entrances = global_entrances.Where(e => !new_entrances_names.Contains(e.name));

			foreach (var data in new_entrances)
			{
				region_dict[data.region][data.target_region] = data.name;
			}

			foreach (var data in fixed_entrances)
			{
				region_dict[data.region][data.target_region] = data.name;
			}

			// region appendend to world regions here???
			foreach (var entrance in region_dict)
			{
				foreach (var targetregion in entrance.Value)
				{
					Regions[entrance.Key].Add(targetregion.Key, targetregion.Value);
				}
			}




		}


	}
}
