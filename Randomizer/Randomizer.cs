using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF1PRAP
{
	public class ItemData
	{
		public int Id { get; set; }
		public int Qty { get; set; }
		public ItemData() { }
		public ItemData(int id, int qty)
		{
			Id = id;
			Qty = qty;
		}
	}

	public struct LocationData
	{
		public int Content;
		public int Qty;
		public int Id;
		public int Flag;
		public LocationType Type;
		public Regions Region;
		public string Name;
		public int Script;
		public string Map;
		//public string Message;
		public List<List<AccessRequirements>> Access;
		public List<AccessRequirements> Trigger;
	}

	public struct ApLocationData
	{
		public string Content;
		public int Flag;
		public string Name;
		public long Id;
	}

	public struct RegionData
	{
		public Regions Region;
		public List<List<AccessRequirements>> Access;
	}

	static partial class Randomizer
    {
		public static Dictionary<string, string> LocationIdToDescription = new Dictionary<string, string>();
		public static Dictionary<string, string> LocationDescriptionToId = new Dictionary<string, string>();
		public static Dictionary<string, long> LocationIdToArchipelagoId = new Dictionary<string, long>();
		public static Dictionary<string, bool> CheckedLocations = new Dictionary<string, bool>();
		public static Dictionary<int, ApLocationData> ApLocations = new Dictionary<int, ApLocationData>();
		public static List<int> ItemsToIgnore = new();
		public static Dictionary<string, string> NewTeleporters = new();
		public static bool Teleporting = false;

		public static RandomizerData Data { get; set; }

		public static void Randomize()
		{
			bool archipelago = SessionManager.GameMode == GameModes.Archipelago;

			uint hash;
			string filename;
			if (archipelago)
			{
				hash = SessionManager.CreateApHash(SessionManager.Data.Player + SessionManager.Data.WorldSeed);
				filename = "ap_" + SessionManager.Data.Player + "_" + SessionManager.Data.WorldSeed;
			}
			else
			{
				hash = SessionManager.CreateHash();
				filename = SessionManager.Data.Seed + "_" + SessionManager.Data.Hashstring;
			}

			// Create seed
			RandomizerData randoData = new();
			MT19337 rng = new(hash);

			// Make sure all options are set, if not apply default values
			foreach (var option in Options.Dict)
			{
				if (!SessionManager.Options.ContainsKey(option.Key))
				{
					SessionManager.Options[option.Key] = option.Value.Default;
				}
			}

			// Create randomized data
			if (!archipelago)
			{
				bool validplacement = false;

				while (!validplacement)
				{
					LogicData logicdata = Logic.BuildLogic(SessionManager.Options["shuffle_overworld"] == Options.Enable,
						(ShuffleEntrancesMode)SessionManager.Options["shuffle_entrances"],
						(ShuffleTownsMode)SessionManager.Options["shuffle_towns"],
						(EarlyProgressionModes)SessionManager.Options["early_progression"],
						SessionManager.Options["northern_docks"] == Options.Enable,
						rng);

					var placement = ItemPlacement(logicdata.Locations, rng);

					if (placement != null)
					{
						randoData.PlacedItems = placement;
						randoData.Entrances = Randomizer.ProcessEntrances(logicdata.Entrances);
						validplacement = true;
					}
				}
			}
			else
			{
				randoData.Entrances = Randomizer.ProcessEntrances(Randomizer.NewTeleporters);
			}

			randoData.GearShops = ShuffleGearShop(SessionManager.Options["shuffle_gear_shops"] == Options.Enable, rng);
			randoData.ShuffledSpells = ShuffleSpells(SessionManager.Options["shuffle_spells"] == Options.Enable, rng);
			randoData.DungeonEncounterRate = SetEncounterRate(SessionManager.Options["dungeon_encounter_rate"]);
			randoData.OverworldEncounterRate = SetEncounterRate(SessionManager.Options["overworld_encounter_rate"]);
			randoData.XpBoost = SetVictoryBoost(SessionManager.Options["xp_boost"]);
			randoData.GilBoost = SetVictoryBoost(SessionManager.Options["gil_boost"]);
			randoData.BoostMenu = SessionManager.Options["boost_menu"] == Options.Enable;
			randoData.Entrances = randoData.Entrances.Concat(ShuffleOrdealsMaze(SessionManager.Options["shuffle_trials_maze"] == Options.Enable, rng))
				.ToDictionary(x => x.Key, x => x.Value);
			randoData.JobPromotion = (JobPromotionModes)SessionManager.Options["job_promotion"];
			randoData.EarlyProgression = (EarlyProgressionModes)SessionManager.Options["early_progression"];
			randoData.NorthernDocks = SessionManager.Options["northern_docks"] == Options.Enable;
			randoData.RequiredCrystals = ProcessCrystals(SessionManager.Options["crystals_required"], rng);
			randoData.RequiredTablatures = ProcessLute(SessionManager.Options["lute_tablatures"], rng);
			randoData.NerfChaos = SessionManager.Options["nerf_chaos"] == Options.Enable;
			randoData.EntrancesShuffled = ProcessEntrancesOptions(SessionManager.Options["shuffle_overworld"] == Options.Enable, (ShuffleEntrancesMode)SessionManager.Options["shuffle_entrances"]);
			// This is ugly but it'll do for now
			randoData.MonsterParties = 
				RandomizeMonsterParties(SessionManager.Options["monster_parties"] != Options.Disable, (MonsterPartyRangeModes)SessionManager.Options["monster_parties"], (MonsterPartyCapModes)SessionManager.Options["monsters_cap"], rng)
				.Concat(AddBossMinions(SessionManager.Options["boss_minions"] != Options.Disable, (MinionsRangeModes)SessionManager.Options["boss_minions"], rng))
				.ToDictionary(x => x.Key, x => x.Value);

			Data = randoData;
			
			// Write Rando data file
			Serialize(SessionManager.FolderPath, filename);
		}
	}
}
