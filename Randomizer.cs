using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF1PRAP
{

	public enum LocationType
	{ 
		Treasure,
		GameObject,
		Event,
	}
	public enum Regions
	{ 
		TempleOfFiends,
		Coneria,
		ConeriaCastle,
		MatoyaCave,
		Pravoka,
		DwarfCave,
		Elfland,
		ElflandCastle,
		NorthWestCastle,
		MarshCave,
		Melmond,
		EarthCave,
		TitanTunnelE,
		TitanTunnelW,
		SardaCave,
		CrescentLake,
		RyukhanDesert,
		IceCave,
		Volcano,
		Caravan,
		Waterfall,
		Onrac,
		SeaShrine,
		CardiaPlains,
		CardiaForest,
		CardiaMarsh,
		CardiaSmall,
		CardiaTop,
		BahamutCave,
		CasteOfOrdeals,
		MirageTower,
		SkyCastle,
		Gaia,
		Lefein
	}
	public enum AccessRequirements
	{ 
		Lute,
		Bridge,
		Ship,
		Crown,
		CrystalEye,
		JoltTonic,
		MysticKey,
		NitroPowder,
		StarRuby,
		EarthRod,
		Canoe,
		Levistone,
		RatsTail,
		BottledFaerie,
		Oxyale,
		RosettaStone,
		Bell,
		WarpCube,
		Adamantite,

		Canal,
		Airship,

		TranslatedRosettaStone,
		EarthCrystal,
		FireCrystal,
		WaterCrystal,
		AirCrystal,

		GarlandDefeated,
		VampireDefeated,
		SlabLifted,
		TitanFed,
		Submarine,
		LeifenishLearned,
		MirageAccess,
		BlackOrbDestroyed,

		None,
	}
	public enum Items
	{ 
		Gil = 1,
		Potion = 2,
		HiPotion = 3,
		XPotion = 4,
		Ether = 5,
		DryEther = 7,
		Elixir = 8,
		PhoenixDown = 10,
		Antidote = 11,
		EyeDrops = 12,
		EchoGrass = 13,
		GoldNeedle = 14,
		Remedy = 15,
		SleepingBag = 17,
		Tent = 18,
		Cottage = 19,
		GiantsTonic = 32,
		StrengthTonic = 34,
		ProtectDrink = 35,
		SpeedDrink = 36,


		Ship = 44, // ?
		Lute = 45,
		Crown = 46,
		CrystalEye = 47,
		JoltTonic = 48,
		MysticKey = 49,
		NitroPowder = 50,
		Adamantite = 51,
		RosettaStone = 52,
		StarRuby = 53,
		EarthRod = 54,
		Levistone = 55,
		Bell = 56,
		RatsTail = 57,
		WarpCube = 58,
		BottledFaerie = 59,
		Oxyale = 60,
		Canoe = 61,

		
		Empty = 62,

		Knife = 63,
		Dagger,
		MythrilKnife,
		CatClaws,
		Rapier,

		Masamune = 103,



	}
	public struct ItemData
	{
		public int Id;
		public int Qty;
		//public int Flag;
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

	public struct EventData
	{
		public List<AccessRequirements> Trigger;
		public Regions Region;
		public string Name;
		public int Script;
		public string Map;
		//public string Message;
		public List<List<AccessRequirements>> Access;
	}

	public struct RegionData
	{
		public Regions Region;
		public List<List<AccessRequirements>> Access;
	}



	partial class Randomizer
    {
		public Dictionary<int, LocationData> Locations;
		public Dictionary<int, ItemData> PlacedItems;

		public static Dictionary<string, string> LocationIdToDescription = new Dictionary<string, string>();
		public static Dictionary<string, string> LocationDescriptionToId = new Dictionary<string, string>();
		public static Dictionary<string, long> LocationIdToArchipelagoId = new Dictionary<string, long>();
		public static Dictionary<string, bool> CheckedLocations = new Dictionary<string, bool>();
		public static Dictionary<int, ApLocationData> ApLocations = new Dictionary<int, ApLocationData>();
		public Randomizer()
		{
			Locations = new();
			PlacedItems = new();
			/*
			List<ItemData> items = FixedLocations.Select(l => new ItemData() { Id = l.Content, Qty = l.Qty }).ToList();
			//items = Enumerable.Repeat((int)Items.Ship, items.Count).Select(s => new ItemData() { Id = s, Qty = 1 }).ToList();
			//List<int> flags = FixedLocations.Select(l => l.Flag).ToList();
			
			Dictionary<ItemData, int> mergedItems = new();

			foreach (var item in items)
			{
				if (mergedItems.ContainsKey(item))
				{
					mergedItems[item]++;
				}
				else
				{
					mergedItems[item] = 1;
				}
			}*/

			/*
			InternalLogger.LogInfo("--- Item List ---");
			foreach (var item in mergedItems)
			{
				InternalLogger.LogInfo($"\"name\": FF1PRItemData(IC.filler, {item.Value}, {item.Key.Id}, {item.Key.Qty}),");
			}

			*/
			
			

			//items.Shuffle(rng);
			//PlacedItems = items.Select((x, i) => (flags[i], x)).ToDictionary(y => y.Item1, y => y.x);
		}
		public static Dictionary<int, ItemData> DoItemPlacement(uint seed)
		{
			//MT19337 rng = new((uint)System.DateTime.Now.Ticks);
			MT19337 rng = new(seed);
			return ItemPlacement(rng);
		}

		static public List<RegionData> FixedRegions = new()
		{ 
			new RegionData() { Region = Regions.TempleOfFiends, Access = new() { } },
			new RegionData() { Region = Regions.Coneria, Access = new() { } },
			new RegionData() { Region = Regions.ConeriaCastle, Access = new() { } },
			new RegionData() { Region = Regions.MatoyaCave, Access = new() { } },
			new RegionData() { Region = Regions.Pravoka, Access = new() { } },
			new RegionData() { Region = Regions.DwarfCave, Access = new() { new() { AccessRequirements.Ship }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.Elfland, Access = new() { new() { AccessRequirements.Ship }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.ElflandCastle, Access = new() { new() { AccessRequirements.Ship }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.NorthWestCastle, Access = new() { new() { AccessRequirements.Ship }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.MarshCave, Access = new() { new() { AccessRequirements.Ship }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.Melmond, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canal }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.EarthCave, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canal }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.TitanTunnelE, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canal }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.TitanTunnelW, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canal, AccessRequirements.TitanFed }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.SardaCave, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canal, AccessRequirements.TitanFed }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.CrescentLake, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canal }, new() { AccessRequirements.Ship, AccessRequirements.Canoe }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.RyukhanDesert, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canoe }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.Volcano, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canal, AccessRequirements.Canoe }, new() { AccessRequirements.Ship, AccessRequirements.Canoe }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.IceCave, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canal, AccessRequirements.Canoe }, new() { AccessRequirements.Canoe }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.CasteOfOrdeals, Access = new() { new() { AccessRequirements.Airship, AccessRequirements.Canoe }, new() { AccessRequirements.Ship, AccessRequirements.Canal, AccessRequirements.Canoe } } },
			new RegionData() { Region = Regions.Caravan, Access = new() { new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.Waterfall, Access = new() { new() { AccessRequirements.Airship, AccessRequirements.Canoe } } },
			new RegionData() { Region = Regions.Onrac, Access = new() { new() { AccessRequirements.Airship } } },
			new RegionData() { Region = Regions.SeaShrine, Access = new() { new() { AccessRequirements.Submarine } } },
			new RegionData() { Region = Regions.CardiaTop, Access = new() { new() { AccessRequirements.Airship } } },
			new RegionData() { Region = Regions.CardiaForest, Access = new() { new() { AccessRequirements.Airship } } },
			new RegionData() { Region = Regions.CardiaMarsh, Access = new() { new() { AccessRequirements.Airship } } },
			new RegionData() { Region = Regions.CardiaPlains, Access = new() { new() { AccessRequirements.Airship } } },
			new RegionData() { Region = Regions.CardiaSmall, Access = new() { new() { AccessRequirements.Airship } } },
			new RegionData() { Region = Regions.BahamutCave, Access = new() { new() { AccessRequirements.Airship } } },
			new RegionData() { Region = Regions.NorthWestCastle, Access = new() { new() { AccessRequirements.Airship, AccessRequirements.Canoe }, new() { AccessRequirements.Ship, AccessRequirements.Canal, AccessRequirements.Canoe } } },
			new RegionData() { Region = Regions.MirageTower, Access = new() { new() { AccessRequirements.Airship, AccessRequirements.Bell } } },
			new RegionData() { Region = Regions.SkyCastle, Access = new() { new() { AccessRequirements.MirageAccess, AccessRequirements.WarpCube } } },
			new RegionData() { Region = Regions.Gaia, Access = new() { new() { AccessRequirements.Airship } } },
			new RegionData() { Region = Regions.Lefein, Access = new() { new() { AccessRequirements.Airship } } },
		};

		static public List<LocationData> FixedLocations = new()
		{
			// ToF
			new LocationData() { Content = 175, Qty = 1, Id = 6, Flag = 5, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30011_1", Region = Regions.TempleOfFiends, Access = new() { } },
			new LocationData() { Content = 2, Qty = 1, Id = 4, Flag = 6, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30011_1", Region = Regions.TempleOfFiends, Access = new() { } },
			new LocationData() { Content = 18, Qty = 1, Id = 5, Flag = 7, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30011_1", Region = Regions.TempleOfFiends, Access = new() { } },

			// ToF Locked
			new LocationData() { Content = 75, Qty = 1, Id = 9, Flag = 10, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30011_1", Region = Regions.TempleOfFiends, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 74, Qty = 1, Id = 7, Flag = 8, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30011_1", Region = Regions.TempleOfFiends, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 14, Qty = 1, Id = 8, Flag = 9, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30011_1", Region = Regions.TempleOfFiends, Access = new() { new() { AccessRequirements.MysticKey } } },

			// Garland
			new LocationData() { Type = LocationType.Event, Name = "Garland", Region = Regions.TempleOfFiends, Access = new() { }, Trigger = new() { AccessRequirements.GarlandDefeated } },

			// Coneria
			new LocationData() { Content = (int)Items.Lute, Qty = 1, Id = 1, Flag = (int)TreasureFlags.Princess, Type = LocationType.GameObject, Name = "Princess", Script = 0, Map = "Map_20011_2", Region = Regions.ConeriaCastle, Access = new() { new() { AccessRequirements.GarlandDefeated } } },

			// Matoya
			new LocationData() { Content = 2, Qty = 1, Id = 5, Flag = 11, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20031_1", Region = Regions.MatoyaCave, Access = new() { } },
			new LocationData() { Content = 2, Qty = 1, Id = 6, Flag = 12, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20031_1", Region = Regions.MatoyaCave, Access = new() { } },
			new LocationData() { Content = 11, Qty = 1, Id = 7, Flag = 13, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20031_1", Region = Regions.MatoyaCave, Access = new() { } },
			new LocationData() { Content = (int)Items.JoltTonic, Qty = 1, Id = 4, Flag = (int)TreasureFlags.Matoya, Type = LocationType.GameObject, Name = "Matoya", Script = 0, Map = "Map_20031", Region = Regions.MatoyaCave, Access = new() { new() { AccessRequirements.CrystalEye } } },

			// Pravoka
			new LocationData() { Content = (int)Items.Ship, Qty = 1, Id = 2, Flag = (int)TreasureFlags.Bikke, Type = LocationType.GameObject, Name = "Bikke", Script = 0, Map = "Map_20040", Region = Regions.Pravoka, Access = new() { } },
			//new LocationData() { Type = LocationType.Event, Name = "Bikke", Script = 0, Map = "Map_20040", Region = Regions.Pravoka, Access = new() { }, Trigger = new() { AccessRequirements.Ship } },

			// Marsh Cave
			// Top
			new LocationData() { Content = 64, Qty = 1, Id = 26, Flag = 22, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_2", Region = Regions.MarshCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1180, Id = 27, Flag = 21, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_2", Region = Regions.MarshCave, Access = new() { } },
			new LocationData() { Content = 2, Qty = 1, Id = 28, Flag = 232, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_2", Region = Regions.MarshCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1120, Id = 29, Flag = 24, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_2", Region = Regions.MarshCave, Access = new() { } },

			// B2
			new LocationData() { Content = 2, Qty = 1, Id = 30, Flag = 231, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_2", Region = Regions.MarshCave, Access = new() { } },
			new LocationData() { Content = 73, Qty = 1, Id = 31, Flag = 23, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_2", Region = Regions.MarshCave, Access = new() { } },
			new LocationData() { Content = 2, Qty = 1, Id = 32, Flag = 233, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_2", Region = Regions.MarshCave, Access = new() { } },

			// Bottom
			new LocationData() { Content = 10, Qty = 1, Id = 34, Flag = 25, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_3", Region = Regions.MarshCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1045, Id = 37, Flag = 26, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_3", Region = Regions.MarshCave, Access = new() { } },
			new LocationData() { Content = 19, Qty = 1, Id = 39, Flag = 27, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_3", Region = Regions.MarshCave, Access = new() { } },
			new LocationData() { Content = 2, Qty = 1, Id = 35, Flag = 234, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_3", Region = Regions.MarshCave, Access = new() { } },
			new LocationData() { Content = 156, Qty = 1, Id = 38, Flag = 28, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_3", Region = Regions.MarshCave, Access = new() { } },
			// Crown is in a chest??
			new LocationData() { Content = (int)Items.Crown, Qty = 1, Id = 40, Flag = (int)TreasureFlags.MarshChest, Type = LocationType.Treasure, Name = "", Script = 24, Map = "Map_30021_3", Region = Regions.MarshCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1135, Id = 41, Flag = 29, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_3", Region = Regions.MarshCave, Access = new() { } },
			new LocationData() { Content = 2, Qty = 1, Id = 36, Flag = 235, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30021_3", Region = Regions.MarshCave, Access = new() { } },

			// Locked
			new LocationData() { Content = 157, Qty = 1, Id = 42, Flag = 30, Type = LocationType.Treasure, Name = "", Script = 500, Map = "Map_30021_3", Region = Regions.MarshCave, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 11, Qty = 1, Id = 43, Flag = 31, Type = LocationType.Treasure, Name = "", Script = 501, Map = "Map_30021_3", Region = Regions.MarshCave, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 1, Qty = 1020, Id = 44, Flag = 32, Type = LocationType.Treasure, Name = "", Script = 502, Map = "Map_30021_3", Region = Regions.MarshCave, Access = new() { new() { AccessRequirements.MysticKey } } },

			// Astos
			new LocationData() { Content = (int)Items.CrystalEye, Qty = 1, Id = 3, Flag = (int)TreasureFlags.Astos, Type = LocationType.GameObject, Name = "Astos", Script = 0, Map = "Map_20081", Region = Regions.NorthWestCastle, Access = new() { new() { AccessRequirements.Crown } } },

			// Elfland Castle
			new LocationData() { Content = 115, Qty = 1, Id = 248, Flag = 14, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20071_1", Region = Regions.ElflandCastle, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 1, Qty = 800, Id = 249, Flag = 15, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20071_1", Region = Regions.ElflandCastle, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 1, Qty = 700, Id = 250, Flag = 16, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20071_1", Region = Regions.ElflandCastle, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 192, Qty = 1, Id = 251, Flag = 17, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20071_1", Region = Regions.ElflandCastle, Access = new() { new() { AccessRequirements.MysticKey } } },

			new LocationData() { Content = (int)Items.MysticKey, Qty = 1, Id = 5, Flag = (int)TreasureFlags.ElfPrince, Type = LocationType.GameObject, Name = "ElfPrince", Script = 0, Map = "Map_20071", Region = Regions.ElflandCastle, Access = new() { new() { AccessRequirements.JoltTonic } } },

			// NW Castle
			new LocationData() { Content = 97, Qty = 1, Id = 5, Flag = 19, Type = LocationType.Treasure, Name = "", Script = 540, Map = "Map_20081_1", Region = Regions.NorthWestCastle, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 193, Qty = 1, Id = 6, Flag = 20, Type = LocationType.Treasure, Name = "", Script = 541, Map = "Map_20081_1", Region = Regions.NorthWestCastle, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 120, Qty = 1, Id = 4, Flag = 18, Type = LocationType.Treasure, Name = "", Script = 539, Map = "Map_20081_1", Region = Regions.NorthWestCastle, Access = new() { new() { AccessRequirements.MysticKey } } },

			// Coneria Treasury
			new LocationData() { Content = 72, Qty = 1, Id = 8, Flag = 3, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20011_1", Region = Regions.ConeriaCastle, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 19, Qty = 1, Id = 7, Flag = 2, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20011_1", Region = Regions.ConeriaCastle, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 65, Qty = 1, Id = 9, Flag = 4, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20011_1", Region = Regions.ConeriaCastle, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 148, Qty = 1, Id = 5, Flag = 0, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20011_1", Region = Regions.ConeriaCastle, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 18, Qty = 1, Id = 6, Flag = 1, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20011_1", Region = Regions.ConeriaCastle, Access = new() { new() { AccessRequirements.MysticKey } } },
			// TNT is a chest
			new LocationData() { Content = (int)Items.NitroPowder, Qty = 1, Id = 10, Flag = (int)TreasureFlags.ConeriaChest, Type = LocationType.Treasure, Name = "", Region = Regions.ConeriaCastle, Script = 29, Map = "Map_20011_1", Access = new() { new() { AccessRequirements.MysticKey } } },

			// Dwarf Cave
			// Entrance
			new LocationData() { Content = 1, Qty = 575, Id = 12, Flag = 34, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20051_1", Region = Regions.DwarfCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 450, Id = 11, Flag = 33, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20051_1", Region = Regions.DwarfCave, Access = new() { } },

			// Smitt
			new LocationData() { Content = 92, Qty = 1, Id = 5, Flag = (int)TreasureFlags.Smitt, Type = LocationType.GameObject, Name = "Smitt", Script = 0, Map = "Map_20051", Region = Regions.DwarfCave, Access = new() { new() { AccessRequirements.Adamantite } } },

			// Nerrick
			new LocationData() { Type = LocationType.Event, Name = "Nerrick", Region = Regions.DwarfCave, Access = new() { new() { AccessRequirements.NitroPowder } }, Trigger = new() { AccessRequirements.Canal } },

			// Locked
			new LocationData() { Content = 18, Qty = 1, Id = 13, Flag = 35, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20051_1", Region = Regions.DwarfCave, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 185, Qty = 1, Id = 14, Flag = 36, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20051_1", Region = Regions.DwarfCave, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 76, Qty = 1, Id = 15, Flag = 37, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20051_1", Region = Regions.DwarfCave, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 5, Qty = 1, Id = 16, Flag = 38, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20051_1", Region = Regions.DwarfCave, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 2, Qty = 1, Id = 17, Flag = 39, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20051_1", Region = Regions.DwarfCave, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 147, Qty = 1, Id = 18, Flag = 40, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20051_1", Region = Regions.DwarfCave, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 1, Qty = 575, Id = 20, Flag = 41, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20051_1", Region = Regions.DwarfCave, Access = new() { new() { AccessRequirements.MysticKey } } },
			new LocationData() { Content = 19, Qty = 1, Id = 19, Flag = 42, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20051_1", Region = Regions.DwarfCave, Access = new() { new() { AccessRequirements.MysticKey } } },

			// Earth Cave
			// B1
			new LocationData() { Content = 1, Qty = 1975, Id = 10, Flag = 43, Type = LocationType.Treasure, Name = "", Script = 503, Map = "Map_30031_1", Region = Regions.EarthCave, Access = new() { } },
			new LocationData() { Content = 11, Qty = 1, Id = 6, Flag = 46, Type = LocationType.Treasure, Name = "", Script = 505, Map = "Map_30031_1", Region = Regions.EarthCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 795, Id = 7, Flag = 47, Type = LocationType.Treasure, Name = "", Script = 506, Map = "Map_30031_1", Region = Regions.EarthCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 880, Id = 8, Flag = 44, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30031_1", Region = Regions.EarthCave, Access = new() { } },
			new LocationData() { Content = 2, Qty = 1, Id = 9, Flag = 45, Type = LocationType.Treasure, Name = "", Script = 504, Map = "Map_30031_1", Region = Regions.EarthCave, Access = new() { } },

			// B2
			new LocationData() { Content = 1, Qty = 575, Id = 8, Flag = 53, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30031_2", Region = Regions.EarthCave, Access = new() { } },
			new LocationData() { Content = 161, Qty = 1, Id = 7, Flag = 52, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30031_2", Region = Regions.EarthCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 5000, Id = 6, Flag = 51, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30031_2", Region = Regions.EarthCave, Access = new() { } },
			new LocationData() { Content = 77, Qty = 1, Id = 9, Flag = 48, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30031_2", Region = Regions.EarthCave, Access = new() { } },
			new LocationData() { Content = 18, Qty = 1, Id = 10, Flag = 49, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30031_2", Region = Regions.EarthCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 330, Id = 11, Flag = 50, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30031_2", Region = Regions.EarthCave, Access = new() { } },

			// B3
			new LocationData() { Content = 17, Qty = 1, Id = 18, Flag = 54, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30031_3", Region = Regions.EarthCave, Access = new() { } },
			new LocationData() { Content = 14, Qty = 1, Id = 19, Flag = 55, Type = LocationType.Treasure, Name = "", Script = 507, Map = "Map_30031_3", Region = Regions.EarthCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 3400, Id = 20, Flag = 56, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30031_3", Region = Regions.EarthCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1020, Id = 21, Flag = 57, Type = LocationType.Treasure, Name = "", Script = 508, Map = "Map_30031_3", Region = Regions.EarthCave, Access = new() { } },
			new LocationData() { Content = (int)Items.StarRuby, Qty = 1, Id = 22, Flag = (int)TreasureFlags.VampireChest, Type = LocationType.Treasure, Name = "", Script = 35, Map = "Map_30031_3", Region = Regions.EarthCave, Access = new() { } },

			// Vampire
			new LocationData() { Type = LocationType.Event, Name = "Vampire", Region = Regions.EarthCave, Access = new() { }, Trigger = new() { AccessRequirements.VampireDefeated } },

			// B4
			new LocationData() { Content = 118, Qty = 1, Id = 12, Flag = 60, Type = LocationType.Treasure, Name = "", Script = 510, Map = "Map_30031_4", Region = Regions.EarthCave, Access = new() { new() { AccessRequirements.EarthRod } } },
			new LocationData() { Content = 1, Qty = 3400, Id = 13, Flag = 61, Type = LocationType.Treasure, Name = "", Script = 509, Map = "Map_30031_4", Region = Regions.EarthCave, Access = new() { new() { AccessRequirements.EarthRod } } },
			new LocationData() { Content = 1, Qty = 1520, Id = 11, Flag = 59, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30031_4", Region = Regions.EarthCave, Access = new() { new() { AccessRequirements.EarthRod } } },
			new LocationData() { Content = 1, Qty = 5450, Id = 9, Flag = 58, Type = LocationType.Treasure, Name = "", Script = 511, Map = "Map_30031_4", Region = Regions.EarthCave, Access = new() { new() { AccessRequirements.EarthRod } } },
			new LocationData() { Content = 1, Qty = 1455, Id = 10, Flag = 62, Type = LocationType.Treasure, Name = "", Script = 512, Map = "Map_30031_4", Region = Regions.EarthCave, Access = new() { new() { AccessRequirements.EarthRod } } },
			new LocationData() { Content = 18, Qty = 1, Id = 14, Flag = 65, Type = LocationType.Treasure, Name = "", Script = 513, Map = "Map_30031_4", Region = Regions.EarthCave, Access = new() { new() { AccessRequirements.EarthRod } } },
			new LocationData() { Content = 164, Qty = 1, Id = 15, Flag = 64, Type = LocationType.Treasure, Name = "", Script = 515, Map = "Map_30031_4", Region = Regions.EarthCave, Access = new() { new() { AccessRequirements.EarthRod } } },
			new LocationData() { Content = 1, Qty = 1250, Id = 16, Flag = 63, Type = LocationType.Treasure, Name = "", Script = 514, Map = "Map_30031_4", Region = Regions.EarthCave, Access = new() { new() { AccessRequirements.EarthRod } } },

			// Lich
			new LocationData() { Type = LocationType.Event, Name = "Lich", Region = Regions.EarthCave, Access = new() { new() { AccessRequirements.EarthRod } }, Trigger = new() { AccessRequirements.EarthCrystal } },

			// Titan
			new LocationData() { Content = 107, Qty = 1, Id = 5, Flag = 244, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30041_1", Region = Regions.TitanTunnelE, Access = new() { new() { AccessRequirements.TitanFed } } },
			new LocationData() { Content = 1, Qty = 620, Id = 4, Flag = 243, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30041_1", Region = Regions.TitanTunnelE, Access = new() { new() { AccessRequirements.TitanFed } } },
			new LocationData() { Content = 1, Qty = 450, Id = 3, Flag = 242, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30041_1", Region = Regions.TitanTunnelE, Access = new() { new() { AccessRequirements.TitanFed } } },
			new LocationData() { Content = 187, Qty = 1, Id = 2, Flag = 241, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30041_1", Region = Regions.TitanTunnelE, Access = new() { new() { AccessRequirements.TitanFed } } },

			// Titan
			new LocationData() { Type = LocationType.Event, Name = "Titan", Region = Regions.TitanTunnelE, Access = new() { new() { AccessRequirements.StarRuby } }, Trigger = new() { AccessRequirements.TitanFed } },

			// Sarda
			new LocationData() { Content = (int)Items.EarthRod, Qty = 1, Id = 5, Flag = (int)TreasureFlags.Sarda, Type = LocationType.GameObject, Name = "Sarda", Script = 0, Map = "Map_20081", Region = Regions.SardaCave, Access = new() { new() { AccessRequirements.VampireDefeated } } },

			// Crescent CanoeSage
			new LocationData() { Content = (int)Items.Canoe, Qty = 1, Id = 6, Flag = (int)TreasureFlags.CanoeSage, Type = LocationType.GameObject, Name = "CanoeSage", Script = 0, Map = "Map_20110", Region = Regions.CrescentLake, Access = new() { new() { AccessRequirements.EarthCrystal } } },

			// Ice Cave
			// Floater Room
			new LocationData() { Content = 134, Qty = 1, Id = 4, Flag = 105, Type = LocationType.Treasure, Name = "", Script = 530, Map = "Map_30061_4", Region = Regions.IceCave, Access = new() { } },
			new LocationData() { Content = 83, Qty = 1, Id = 3, Flag = 104, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30061_4", Region = Regions.IceCave, Access = new() { } },

			// Levistone
			new LocationData() { Content = (int)Items.Levistone, Qty = 1, Id = 7, Flag = (int)TreasureFlags.EyeChest, Type = LocationType.GameObject, Name = "EyeChest", Script = 0, Map = "Map_20081_4", Region = Regions.IceCave, Access = new() { } },

			// B3
			new LocationData() { Content = 149, Qty = 1, Id = 8, Flag = 107, Type = LocationType.Treasure, Name = "", Script = 531, Map = "Map_30061_5", Region = Regions.IceCave, Access = new() { } },
			new LocationData() { Content = 197, Qty = 1, Id = 7, Flag = 106, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30061_5", Region = Regions.IceCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 7900, Id = 12, Flag = 108, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30061_5", Region = Regions.IceCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 9900, Id = 9, Flag = 111, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30061_5", Region = Regions.IceCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 5454, Id = 13, Flag = 109, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30061_5", Region = Regions.IceCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 5000, Id = 10, Flag = 112, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30061_5", Region = Regions.IceCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1180, Id = 14, Flag = 110, Type = LocationType.Treasure, Name = "", Script = 533, Map = "Map_30061_5", Region = Regions.IceCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 12350, Id = 11, Flag = 113, Type = LocationType.Treasure, Name = "", Script = 532, Map = "Map_30061_5", Region = Regions.IceCave, Access = new() { } },

			// B1
			new LocationData() { Content = 5, Qty = 1, Id = 8, Flag = 99, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30061_1", Region = Regions.IceCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 10000, Id = 9, Flag = 100, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30061_1", Region = Regions.IceCave, Access = new() { } },
			new LocationData() { Content = 1, Qty = 9500, Id = 10, Flag = 101, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30061_1", Region = Regions.IceCave, Access = new() { } },
			new LocationData() { Content = 17, Qty = 1, Id = 11, Flag = 102, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30061_1", Region = Regions.IceCave, Access = new() { } },
			new LocationData() { Content = 165, Qty = 1, Id = 12, Flag = 103, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30061_1", Region = Regions.IceCave, Access = new() { } },

			// Airship
			new LocationData() { Type = LocationType.Event, Name = "Airship", Region = Regions.RyukhanDesert, Access = new() { new() { AccessRequirements.Levistone } }, Trigger = new() { AccessRequirements.Airship } },

			// Ordeals
			// Maze
			new LocationData() { Content = 2, Qty = 1, Id = 3, Flag = 240, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30071_2", Region = Regions.CasteOfOrdeals, Access = new() { new() { AccessRequirements.Crown } } },

			// 3F
			new LocationData() { Content = 119, Qty = 1, Id = 1, Flag = 210, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30071_3", Region = Regions.CasteOfOrdeals, Access = new() { new() { AccessRequirements.Crown } } },
			new LocationData() { Content = 158, Qty = 1, Id = 2, Flag = 212, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30071_3", Region = Regions.CasteOfOrdeals, Access = new() { new() { AccessRequirements.Crown } } },
			new LocationData() { Content = 86, Qty = 1, Id = 3, Flag = 214, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30071_3", Region = Regions.CasteOfOrdeals, Access = new() { new() { AccessRequirements.Crown } } },
			new LocationData() { Content = 193, Qty = 1, Id = 4, Flag = 216, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30071_3", Region = Regions.CasteOfOrdeals, Access = new() { new() { AccessRequirements.Crown } } },
			new LocationData() { Content = 1, Qty = 7340, Id = 8, Flag = 215, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30071_3", Region = Regions.CasteOfOrdeals, Access = new() { new() { AccessRequirements.Crown } } },
			new LocationData() { Content = 1, Qty = 1455, Id = 7, Flag = 213, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30071_3", Region = Regions.CasteOfOrdeals, Access = new() { new() { AccessRequirements.Crown } } },
			new LocationData() { Content = 19, Qty = 1, Id = 6, Flag = 211, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30071_3", Region = Regions.CasteOfOrdeals, Access = new() { new() { AccessRequirements.Crown } } },
			new LocationData() { Content = 194, Qty = 1, Id = 5, Flag = 217, Type = LocationType.Treasure, Name = "", Script = 534, Map = "Map_30071_3", Region = Regions.CasteOfOrdeals, Access = new() { new() { AccessRequirements.Crown } } },
			new LocationData() { Content = (int)Items.RatsTail, Qty = 1, Id = 9, Flag = (int)TreasureFlags.MouseChest, Type = LocationType.Treasure, Name = "", Script = 50, Map = "Map_30071_3", Region = Regions.CasteOfOrdeals, Access = new() { new() { AccessRequirements.Crown } } },

			// Cardia Forest
			new LocationData() { Content = 1, Qty = 2750, Id = 24, Flag = 230, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20121_1", Region = Regions.CardiaForest, Access = new() { } },
			new LocationData() { Content = 1, Qty = 2000, Id = 25, Flag = 229, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20121_1", Region = Regions.CardiaForest, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1455, Id = 26, Flag = 228, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20121_1", Region = Regions.CardiaForest, Access = new() { } },
			new LocationData() { Content = 1, Qty = 2750, Id = 22, Flag = 226, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20121_1", Region = Regions.CardiaForest, Access = new() { } },
			new LocationData() { Content = 1, Qty = 9500, Id = 21, Flag = 225, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20121_1", Region = Regions.CardiaForest, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1520, Id = 23, Flag = 227, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20121_1", Region = Regions.CardiaForest, Access = new() { } },
			new LocationData() { Content = 4, Qty = 1, Id = 20, Flag = 224, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20121_1", Access = new() { } },

			// Cardia Marsh
			new LocationData() { Content = 8, Qty = 1, Id = 17, Flag = 221, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20121_1", Region = Regions.CardiaMarsh, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1000, Id = 19, Flag = 223, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20121_1", Region = Regions.CardiaMarsh, Access = new() { } },
			new LocationData() { Content = 19, Qty = 1, Id = 18, Flag = 222, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20121_1", Region = Regions.CardiaMarsh, Access = new() { } },

			// Cardia Plains
			new LocationData() { Content = 18, Qty = 1, Id = 16, Flag = 218, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20121_1", Region = Regions.CardiaPlains, Access = new() { } },
			new LocationData() { Content = 7, Qty = 1, Id = 14, Flag = 219, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20121_1", Region = Regions.CardiaPlains, Access = new() { } },
			new LocationData() { Content = 14, Qty = 1, Id = 15, Flag = 220, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_20121_1", Region = Regions.CardiaPlains, Access = new() { } },

			// Volcano
			// B2
			new LocationData() { Content = 187, Qty = 1, Id = 10, Flag = 66, Type = LocationType.Treasure, Name = "", Script = 516, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1520, Id = 9, Flag = 67, Type = LocationType.Treasure, Name = "", Script = 518, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 79, Qty = 1, Id = 11, Flag = 69, Type = LocationType.Treasure, Name = "", Script = 517, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 4150, Id = 8, Flag = 68, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1250, Id = 24, Flag = 71, Type = LocationType.Treasure, Name = "", Script = 519, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1295, Id = 25, Flag = 70, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 187, Qty = 1, Id = 22, Flag = 73, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 197, Qty = 1, Id = 23, Flag = 72, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 3, Qty = 1, Id = 21, Flag = 74, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 18, Qty = 1, Id = 20, Flag = 75, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1975, Id = 19, Flag = 81, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1520, Id = 17, Flag = 77, Type = LocationType.Treasure, Name = "", Script = 522, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 11, Qty = 1, Id = 18, Flag = 82, Type = LocationType.Treasure, Name = "", Script = 520, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1455, Id = 14, Flag = 79, Type = LocationType.Treasure, Name = "", Script = 521, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1760, Id = 16, Flag = 76, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 164, Qty = 1, Id = 15, Flag = 78, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 109, Qty = 1, Id = 12, Flag = 80, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_2", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 10, Qty = 1, Id = 13, Flag = 83, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_2", Access = new() { } },

			// Agama Floor
			new LocationData() { Content = 1, Qty = 2750, Id = 12, Flag = 84, Type = LocationType.Treasure, Name = "", Script = 524, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 14, Qty = 1, Id = 24, Flag = 85, Type = LocationType.Treasure, Name = "", Script = 523, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1760, Id = 13, Flag = 86, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 7340, Id = 14, Flag = 88, Type = LocationType.Treasure, Name = "", Script = 525, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 11, Qty = 1, Id = 25, Flag = 87, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1380, Id = 15, Flag = 91, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 86, Qty = 1, Id = 20, Flag = 90, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 166, Qty = 1, Id = 26, Flag = 89, Type = LocationType.Treasure, Name = "", Script = 526, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1150, Id = 16, Flag = 92, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1155, Id = 17, Flag = 93, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 2000, Id = 18, Flag = 97, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 19, Qty = 1, Id = 21, Flag = 96, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 2, Qty = 1, Id = 22, Flag = 236, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 118, Qty = 1, Id = 23, Flag = 94, Type = LocationType.Treasure, Name = "", Script = 528, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1250, Id = 19, Flag = 95, Type = LocationType.Treasure, Name = "", Script = 527, Map = "Map_30051_5", Region = Regions.Volcano, Access = new() { } },

			// Kary Floor
			new LocationData() { Content = 150, Qty = 1, Id = 19, Flag = 98, Type = LocationType.Treasure, Name = "", Script = 529, Map = "Map_30051_6", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 2, Qty = 1, Id = 16, Flag = 239, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_6", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 2, Qty = 1, Id = 17, Flag = 238, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_6", Region = Regions.Volcano, Access = new() { } },
			new LocationData() { Content = 2, Qty = 1, Id = 18, Flag = 237, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30051_6", Region = Regions.Volcano, Access = new() { } },

			// Kary
			new LocationData() { Type = LocationType.Event, Name = "Kary", Region = Regions.Volcano, Access = new() { }, Trigger = new() { AccessRequirements.FireCrystal } },


			// Waterfall
			new LocationData() { Content = 123, Qty = 1, Id = 2, Flag = 114, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30091_1", Region = Regions.Waterfall, Access = new() { } },
			new LocationData() { Content = 183, Qty = 1, Id = 3, Flag = 116, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30091_1", Region = Regions.Waterfall, Access = new() { } },
			new LocationData() { Content = 1, Qty = 6400, Id = 5, Flag = 117, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30091_1", Region = Regions.Waterfall, Access = new() { } },
			new LocationData() { Content = 1, Qty = 13450, Id = 4, Flag = 115, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30091_1", Region = Regions.Waterfall, Access = new() { } },
			new LocationData() { Content = 1, Qty = 5000, Id = 6, Flag = 118, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30091_1", Region = Regions.Waterfall, Access = new() { } },
			new LocationData() { Content = 87, Qty = 1, Id = 7, Flag = 119, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30091_1", Region = Regions.Waterfall, Access = new() { } },

			// CubeBot
			new LocationData() { Content = (int)Items.WarpCube, Qty = 1, Id = 11, Flag = (int)TreasureFlags.CubeBot, Type = LocationType.GameObject, Name = "CubeBot", Script = 0, Map = "Map_20160", Region = Regions.Waterfall, Access = new() { } },

			// Caravan
			//new LocationData() { Content = (int)Items.BottledFaerie, Qty = 1, Id = 12, Flag = (int)TreasureFlags.Caravan, Type = LocationType.GameObject, Name = "Caravan", Script = 0, Map = "Map_20160", Region = Regions.Caravan, Access = new() { } },
			new LocationData() { Type = LocationType.Event, Name = "Caravan", Region = Regions.Caravan, Access = new() { }, Trigger = new() { AccessRequirements.BottledFaerie } },

			// Fairy
			new LocationData() { Content = (int)Items.Oxyale, Qty = 1, Id = 10, Flag = (int)TreasureFlags.Fairy, Type = LocationType.GameObject, Name = "Fairy", Script = 0, Map = "Map_20160", Region = Regions.Gaia, Access = new() { new() { AccessRequirements.BottledFaerie } } },

			// Onrac
			new LocationData() { Type = LocationType.Event, Name = "SubEngineer", Region = Regions.Onrac, Access = new() { new() { AccessRequirements.Oxyale } }, Trigger = new() { AccessRequirements.Submarine } },

			// Sea Shrine
			// B3
			new LocationData() { Content = 1, Qty = 2000, Id = 9, Flag = 130, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_3", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 9900, Id = 8, Flag = 131, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_3", Region = Regions.SeaShrine, Access = new() { } },

			// B2
			new LocationData() { Content = 153, Qty = 1, Id = 14, Flag = 134, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_6", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1300, Id = 13, Flag = 135, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_6", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 110, Qty = 1, Id = 12, Flag = 136, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_6", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 121, Qty = 1, Id = 15, Flag = 137, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_6", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 12350, Id = 16, Flag = 138, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_6", Region = Regions.SeaShrine, Access = new() { } },

			// Mermaids
			new LocationData() { Content = 1, Qty = 1760, Id = 33, Flag = 142, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_8", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 9000, Id = 32, Flag = 141, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_8", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 160, Qty = 1, Id = 34, Flag = 143, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_8", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 2750, Id = 37, Flag = 146, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_8", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 4150, Id = 35, Flag = 144, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_8", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 10000, Id = 38, Flag = 147, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_8", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1150, Id = 39, Flag = 148, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_8", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 5000, Id = 36, Flag = 145, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_8", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 11, Qty = 1, Id = 30, Flag = 139, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_8", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 168, Qty = 1, Id = 31, Flag = 140, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_8", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 198, Qty = 1, Id = 41, Flag = 150, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_8", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 188, Qty = 1, Id = 40, Flag = 149, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_8", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = (int)Items.RosettaStone, Qty = 1, Id = 42, Flag = (int)TreasureFlags.MermaidsChest, Type = LocationType.Treasure, Name = "", Script = 59, Map = "Map_30081_8", Region = Regions.SeaShrine, Access = new() { } },

			// B4
			new LocationData() { Content = 1, Qty = 1110, Id = 5, Flag = 133, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_5", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1450, Id = 4, Flag = 132, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_5", Region = Regions.SeaShrine, Access = new() { } },

			// Sharknado
			new LocationData() { Content = 1, Qty = 8135, Id = 14, Flag = 121, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_2", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 7690, Id = 13, Flag = 120, Type = LocationType.Treasure, Name = "", Script = 535, Map = "Map_30081_2", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 5450, Id = 15, Flag = 122, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_2", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 195, Qty = 1, Id = 17, Flag = 124, Type = LocationType.Treasure, Name = "", Script = 537, Map = "Map_30081_2", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1385, Id = 16, Flag = 123, Type = LocationType.Treasure, Name = "", Script = 536, Map = "Map_30081_2", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 110, Qty = 1, Id = 8, Flag = 126, Type = LocationType.Treasure, Name = "", Script = 538, Map = "Map_30081_2", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 7340, Id = 12, Flag = 129, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_2", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 9900, Id = 10, Flag = 125, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_2", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 183, Qty = 1, Id = 9, Flag = 128, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_2", Region = Regions.SeaShrine, Access = new() { } },
			new LocationData() { Content = 1, Qty = 2750, Id = 11, Flag = 127, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30081_2", Region = Regions.SeaShrine, Access = new() { } },

			// Kraken
			new LocationData() { Type = LocationType.Event, Name = "Kraken", Region = Regions.SeaShrine, Access = new() { }, Trigger = new() { AccessRequirements.WaterCrystal } },

			// Melmond
			new LocationData() { Type = LocationType.Event, Name = "Dr. Unne", Region = Regions.Melmond, Access = new() { new() { AccessRequirements.RosettaStone } }, Trigger = new() { AccessRequirements.LeifenishLearned } },

			// Lefeinman
			new LocationData() { Content = (int)Items.Bell, Qty = 1, Id = 8, Flag = 255, Type = LocationType.GameObject, Name = "Lefeinman", Script = 0, Map = "Map_20160", Region = Regions.Lefein, Access = new() { new() { AccessRequirements.LeifenishLearned } } },

			// Mirage
			new LocationData() { Type = LocationType.Event, Name = "Mirage", Region = Regions.MirageTower, Access = new() { }, Trigger = new() { AccessRequirements.MirageAccess } },

			// Mirage
			// 1F
			new LocationData() { Content = 1, Qty = 1300, Id = 3, Flag = 158, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_1", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 186, Qty = 1, Id = 10, Flag = 156, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_1", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 1, Qty = 18010, Id = 7, Flag = 157, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_1", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 1, Qty = 3400, Id = 8, Flag = 154, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_1", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 82, Qty = 1, Id = 9, Flag = 152, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_1", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 1, Qty = 2750, Id = 6, Flag = 155, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_1", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 170, Qty = 1, Id = 5, Flag = 153, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_1", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 18, Qty = 1, Id = 4, Flag = 151, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_1", Region = Regions.MirageTower, Access = new() { } },

			// 2F
			new LocationData() { Content = 1, Qty = 8135, Id = 2, Flag = 159, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_2", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 1, Qty = 7900, Id = 3, Flag = 163, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_2", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 116, Qty = 1, Id = 9, Flag = 167, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_2", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 1, Qty = 12350, Id = 5, Flag = 162, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_2", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 1, Qty = 13000, Id = 7, Flag = 166, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_2", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 19, Qty = 1, Id = 11, Flag = 168, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_2", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 1, Qty = 7600, Id = 6, Flag = 164, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_2", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 89, Qty = 1, Id = 10, Flag = 160, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_2", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 1, Qty = 10000, Id = 4, Flag = 165, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_2", Region = Regions.MirageTower, Access = new() { } },
			new LocationData() { Content = 154, Qty = 1, Id = 8, Flag = 161, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30101_2", Region = Regions.MirageTower, Access = new() { } },



			// Sky
			// 1F
			new LocationData() { Content = 80, Qty = 1, Id = 18, Flag = 178, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_1", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 2, Qty = 1, Id = 10, Flag = 171, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_1", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 9900, Id = 9, Flag = 169, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_1", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 4150, Id = 11, Flag = 173, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_1", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 7900, Id = 12, Flag = 175, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_1", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1180, Id = 17, Flag = 176, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_1", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 186, Qty = 1, Id = 16, Flag = 174, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_1", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 6720, Id = 15, Flag = 172, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_1", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 202, Qty = 1, Id = 14, Flag = 170, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_1", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 5000, Id = 13, Flag = 177, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_1", Region = Regions.SkyCastle, Access = new() { } },

			// 2F
			new LocationData() { Content = 19, Qty = 1, Id = 9, Flag = 185, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_2", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 187, Qty = 1, Id = 8, Flag = 183, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_2", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 1380, Id = 6, Flag = 179, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_2", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 13000, Id = 7, Flag = 181, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_2", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 137, Qty = 1, Id = 14, Flag = 187, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_2", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 136, Qty = 1, Id = 13, Flag = 186, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_2", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 168, Qty = 1, Id = 12, Flag = 184, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_2", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 183, Qty = 1, Id = 11, Flag = 182, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_2", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 198, Qty = 1, Id = 10, Flag = 180, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_2", Region = Regions.SkyCastle, Access = new() { } },

			// Sky Chest
			new LocationData() { Content = (int)Items.Adamantite, Qty = 1, Id = 20, Flag = 250, Type = LocationType.GameObject, Name = "SkyChest", Script = 0, Map = "Map_30111_2", Region = Regions.SkyCastle, Access = new() { } },

			// 3F
			new LocationData() { Content = 1, Qty = 5450, Id = 11, Flag = 200, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 2, Qty = 1, Id = 9, Flag = 196, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 9000, Id = 10, Flag = 198, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 202, Qty = 1, Id = 12, Flag = 189, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 8135, Id = 16, Flag = 197, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 173, Qty = 1, Id = 13, Flag = 191, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 134, Qty = 1, Id = 14, Flag = 193, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 9500, Id = 17, Flag = 199, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 14, Qty = 1, Id = 18, Flag = 201, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 6400, Id = 15, Flag = 195, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 4150, Id = 6, Flag = 190, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 14, Qty = 1, Id = 5, Flag = 188, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 1, Qty = 3400, Id = 7, Flag = 192, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },
			new LocationData() { Content = 100, Qty = 1, Id = 8, Flag = 194, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30111_3", Region = Regions.SkyCastle, Access = new() { } },

			// Tiamat
			new LocationData() { Type = LocationType.Event, Name = "Tiamat", Region = Regions.SkyCastle, Access = new() { }, Trigger = new() { AccessRequirements.AirCrystal } },

			// ToFR
			// Black Orb
			new LocationData() { Type = LocationType.Event, Name = "BlackOrb", Region = Regions.TempleOfFiends, Access = new() { new() { AccessRequirements.EarthCrystal, AccessRequirements.FireCrystal, AccessRequirements.WaterCrystal, AccessRequirements.AirCrystal } }, Trigger = new() { AccessRequirements.BlackOrbDestroyed } },

			// B3
			new LocationData() { Content = 7, Qty = 1, Id = 3, Flag = 202, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30121_3", Region = Regions.TempleOfFiends, Access = new() { new() { AccessRequirements.BlackOrbDestroyed } } },
			new LocationData() { Content = 8, Qty = 1, Id = 4, Flag = 203, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30121_3", Region = Regions.TempleOfFiends, Access = new() { new() { AccessRequirements.BlackOrbDestroyed } } }, 

			// Fire
			new LocationData() { Content = 202, Qty = 1, Id = 8, Flag = 204, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30121_5", Region = Regions.TempleOfFiends, Access = new() { new() { AccessRequirements.BlackOrbDestroyed, AccessRequirements.Lute } } },
			new LocationData() { Content = 100, Qty = 1, Id = 10, Flag = 206, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30121_5", Region = Regions.TempleOfFiends, Access = new() { new() { AccessRequirements.BlackOrbDestroyed, AccessRequirements.Lute } } },
			new LocationData() { Content = 173, Qty = 1, Id = 9, Flag = 205, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30121_5", Region = Regions.TempleOfFiends, Access = new() { new() { AccessRequirements.BlackOrbDestroyed, AccessRequirements.Lute } } },
			new LocationData() { Content = 8, Qty = 1, Id = 11, Flag = 207, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30121_5", Region = Regions.TempleOfFiends, Access = new() { new() { AccessRequirements.BlackOrbDestroyed, AccessRequirements.Lute } } },

			// Air
			new LocationData() { Content = 103, Qty = 1, Id = 1, Flag = 208, Type = LocationType.Treasure, Name = "", Script = 0, Map = "Map_30121_7", Region = Regions.TempleOfFiends, Access = new() { new() { AccessRequirements.BlackOrbDestroyed, AccessRequirements.Lute } } },

			};



		public static Dictionary<int, int> ItemIdToFlag = new()
		{
			{ (int)Items.Lute, (int)ScenarioFlags.Lute },
			{ (int)Items.Crown, (int)ScenarioFlags.Crown },
			{ (int)Items.CrystalEye, (int)ScenarioFlags.CrystalEye },
			{ (int)Items.JoltTonic, (int)ScenarioFlags.JoltTonic },
			{ (int)Items.MysticKey, (int)ScenarioFlags.MysticKey },
			{ (int)Items.NitroPowder, (int)ScenarioFlags.NitroPowder },
			{ (int)Items.StarRuby, (int)ScenarioFlags.StarRuby },
			{ (int)Items.EarthRod, (int)ScenarioFlags.EarthRod },
			{ (int)Items.Canoe, (int)ScenarioFlags.Canoe },
			{ (int)Items.Levistone, (int)ScenarioFlags.Levistone },
			{ (int)Items.WarpCube, (int)ScenarioFlags.WarpCube },
			{ (int)Items.Oxyale, (int)ScenarioFlags.Oxyale },
			{ (int)Items.RosettaStone, (int)ScenarioFlags.RosettaStone },
			{ (int)Items.Bell, (int)ScenarioFlags.Chime },
			{ (int)Items.RatsTail, (int)ScenarioFlags.RatTail },
			{ (int)Items.Adamantite, (int)ScenarioFlags.Adamant },
			{ (int)Items.BottledFaerie, (int)ScenarioFlags.BottledFaerie },
			{ (int)Items.Ship, (int)ScenarioFlags.Ship },
		};

		public static Dictionary<string, int> ScriptToItemFlag = new()
		{
			{ "sc_e_0004_1", (int)TreasureFlags.Princess }, // Princess
			{ "sc_e_0009_2", (int)TreasureFlags.Bikke }, // Bikke
			{ "sc_e_0011_2", (int)TreasureFlags.Astos }, // Astos
			{ "sc_e_0012", (int)TreasureFlags.Matoya }, // Matoya
			{ "sc_e_0013", (int)TreasureFlags.ElfPrince }, // ElfPrince
			{ "sc_e_0019", (int)TreasureFlags.Sarda }, // Sarda
			{ "sc_e_0022", (int)TreasureFlags.CanoeSage	}, // CanoeSage
			{ "sc_e_0024_2", (int)TreasureFlags.EyeChest }, // Levistone
			
			{ "sc_e_0026", (int)TreasureFlags.CubeBot }, // Levistone
			{ "sc_e_0052", (int)TreasureFlags.Smitt }, // Levistone
			{ "sc_e_0051", (int)TreasureFlags.SkyChest }, // Levistone
			{ "sc_e_0029", (int)TreasureFlags.Fairy }, // Levistone
			{ "sc_e_0035", (int)TreasureFlags.Lefeinman }, // Levistone
			//{ "sc_e_0035", (int)TreasureFlags.Caravan }, // Caravan
		};

		public static Dictionary<int, string> FlagToDialogue = new()
		{
			{ (int)TreasureFlags.Princess, "MSG_NPC_SARALUTE_02" }, // Princess
			{ (int)TreasureFlags.Bikke, "MSG_SHIP_04" }, // Bikke
			{ (int)TreasureFlags.Astos, "MSG_ASTOS_04" }, // Astos
			{ (int)TreasureFlags.Matoya, "MSG_AWAKEPOT_03" }, // Matoya
			{ (int)TreasureFlags.ElfPrince, "MSG_AWAKEELF_05" }, // ElfPrince
			{ (int)TreasureFlags.Sarda, "MSG_GET_STICK_02" }, // Sarda
			{ (int)TreasureFlags.CanoeSage, "MSG_GET_CANOE_02" }, // CanoeSage
			{ (int)TreasureFlags.EyeChest, "MSG_GET_FLOAT_01" }, // Levistone
			{ (int)TreasureFlags.CubeBot, "MSG_GET_WARPCUBE_03" }, // Levistone
			{ (int)TreasureFlags.Smitt, "MSG_GET_EXCALIBAR_05" }, // Levistone
			{ (int)TreasureFlags.SkyChest, "MSG_WND_DAN_04" }, // Levistone
			{ (int)TreasureFlags.Fairy, "MSG_OXYALE_03" }, // Levistone
			{ (int)TreasureFlags.Lefeinman, "MSG_GET_CHIME_02" }, // Levistone
			//{ "sc_e_0035", (int)TreasureFlags.Caravan }, // Caravan
		};


		public static Dictionary<int, string> FlagToLocationName = new()
		{
			{ 5, "Chaos Shrine - Unlocked Single" },
			{ 6, "Chaos Shrine - Unlocked Duo 1" },
			{ 7, "Chaos Shrine - Unlocked Duo 2" },
			{ 10, "Chaos Shrine - Locked Single" },
			{ 8, "Chaos Shrine - Locked Duo 1" },
			{ 9, "Chaos Shrine - Locked Duo 2" },
			{ 400, "Castle Cornelia - Princess" },
			{ 0, "Castle Cornelia - Treasury 1" },
			{ 1, "Castle Cornelia - Treasury 2" },
			{ 2, "Castle Cornelia - Treasury 3" },
			{ 3, "Castle Cornelia - Treasury 4" },
			{ 4, "Castle Cornelia - Treasury 5" },
			{ 505, "Castle Cornelia - Treasury Major" },
			{ 11, "Matoya's Cave - Chest 1" },
			{ 12, "Matoya's Cave - Chest 2" },
			{ 13, "Matoya's Cave - Chest 3" },
			{ 247, "Matoya's Cave - Matoya" },
			{ 401, "Pravoka - Bikke" },
			{ 24, "Marsh Cave B2 (Top) - Single" },
			{ 232, "Marsh Cave B2 (Top) - Corner" },
			{ 21, "Marsh Cave B2 (Top) - Duo 1" },
			{ 22, "Marsh Cave B2 (Top) - Duo 2" },
			{ 231, "Marsh Cave B2 (Bottom) - Leftmost Chest" },
			{ 23, "Marsh Cave B2 (Bottom) - Center Chest" },
			{ 233, "Marsh Cave B2 (Bottom) - Rightmost Chest" },
			{ 25, "Marsh Cave B3 (Bottom) - Distant Chest" },
			{ 26, "Marsh Cave B3 (Bottom) - Tetris-Z First" },
			{ 27, "Marsh Cave B3 (Bottom) - Tetris-Z Middle 1" },
			{ 234, "Marsh Cave B3 (Bottom) - Tetris-Z Middle 2" },
			{ 501, "Marsh Cave B3 (Bottom) - Tetris-Z Major" },
			{ 28, "Marsh Cave B3 (Bottom) - Tetris-Z Last" },
			{ 29, "Marsh Cave B3 (Bottom) - First Room Chest" },
			{ 235, "Marsh Cave B3 (Bottom) - Dunno Chest" },
			{ 30, "Marsh Cave B3 (Bottom) - Locked Corner" },
			{ 31, "Marsh Cave B3 (Bottom) - Locked Middle" },
			{ 32, "Marsh Cave B3 (Bottom) - Locked Cross" },
			{ 246, "Western Keep - Astos" },
			{ 18, "Western Keep - Treasury 1" },
			{ 19, "Western Keep - Treasury 2" },
			{ 20, "Western Keep - Treasury 3" },
			{ 248, "Elven Castle - Elf Prince" },
			{ 14, "Elven Castle - Treasury 1" },
			{ 15, "Elven Castle - Treasury 2" },
			{ 16, "Elven Castle - Treasury 3" },
			{ 17, "Elven Castle - Treasury 4" },
			{ 33, "Mount Duergar - Entrance 1" },
			{ 34, "Mount Duergar - Entrance 2" },
			{ 35, "Mount Duergar - Treasury 1" },
			{ 36, "Mount Duergar - Treasury 2" },
			{ 37, "Mount Duergar - Treasury 3" },
			{ 38, "Mount Duergar - Treasury 4" },
			{ 39, "Mount Duergar - Treasury 5" },
			{ 40, "Mount Duergar - Treasury 6" },
			{ 41, "Mount Duergar - Treasury 7" },
			{ 42, "Mount Duergar - Treasury 8" },
			{ 405, "Mount Duergar - Smitt" },
			{ 43, "Cavern of Earth B1 - Single" },
			{ 44, "Cavern of Earth B1 - Appendix 1" },
			{ 45, "Cavern of Earth B1 - Appendix 2" },
			{ 46, "Cavern of Earth B1 - Side Path 1" },
			{ 47, "Cavern of Earth B1 - Side Path 2" },
			{ 48, "Cavern of Earth B2 - Guarded 1" },
			{ 49, "Cavern of Earth B2 - Guarded 2" },
			{ 50, "Cavern of Earth B2 - Guarded 3" },
			{ 51, "Cavern of Earth B2 - Side Room 1" },
			{ 52, "Cavern of Earth B2 - Side Room 2" },
			{ 53, "Cavern of Earth B2 - Side Room 3" },
			{ 54, "Cavern of Earth B3 - Side Room" },
			{ 55, "Cavern of Earth B3 - TFC" },
			{ 56, "Cavern of Earth B3 - Asher Trunk" },
			{ 57, "Cavern of Earth B3 - Vampire's Closet" },
			{ 508, "Cavern of Earth B3 - Vampire's Chest" },
			{ 58, "Cavern of Earth B4 - Armory 1" },
			{ 59, "Cavern of Earth B4 - Armory 2" },
			{ 60, "Cavern of Earth B4 - Armory 3" },
			{ 61, "Cavern of Earth B4 - Armory 4" },
			{ 62, "Cavern of Earth B4 - Armory 5" },
			{ 63, "Cavern of Earth B4 - Lich's Closet 1" },
			{ 64, "Cavern of Earth B4 - Lich's Closet 2" },
			{ 65, "Cavern of Earth B4 - Lich's Closet 3" },
			{ 241, "Giant's Cave - Chest 1" },
			{ 242, "Giant's Cave - Chest 2" },
			{ 243, "Giant's Cave - Chest 3" },
			{ 244, "Giant's Cave - Chest 4" },
			{ 253, "Sage's Cave - Sarda" },
			{ 402, "Crescent Lake - Canoe Sage" },
			{ 66, "Mount Gulg B2 - Guarded" },
			{ 67, "Mount Gulg B2 - Center" },
			{ 68, "Mount Gulg B2 - Hairpins" },
			{ 69, "Mount Gulg B2 - Shortpins" },
			{ 70, "Mount Gulg B2 - Vertpins 1" },
			{ 71, "Mount Gulg B2 - Vertpins 2" },
			{ 72, "Mount Gulg B2 - Armory 1" },
			{ 73, "Mount Gulg B2 - Armory 2" },
			{ 74, "Mount Gulg B2 - Armory 3" },
			{ 75, "Mount Gulg B2 - Armory 4" },
			{ 76, "Mount Gulg B2 - Armory 5" },
			{ 77, "Mount Gulg B2 - Armory 6" },
			{ 78, "Mount Gulg B2 - Armory 7" },
			{ 79, "Mount Gulg B2 - Armory 8" },
			{ 80, "Mount Gulg B2 - Armory 9" },
			{ 81, "Mount Gulg B2 - Armory 10" },
			{ 82, "Mount Gulg B2 - Armory 11" },
			{ 83, "Mount Gulg B2 - Armory 12" },
			{ 84, "Mount Gulg B4 - Entrance 1" },
			{ 85, "Mount Gulg B4 - Entrance 2" },
			{ 86, "Mount Gulg B4 - First Greed" },
			{ 87, "Mount Gulg B4 - Worm Room 1" },
			{ 88, "Mount Gulg B4 - Worm Room 2" },
			{ 89, "Mount Gulg B4 - Worm Room 3" },
			{ 90, "Mount Gulg B4 - Worm Room 4" },
			{ 91, "Mount Gulg B4 - Worm Room 5" },
			{ 92, "Mount Gulg B4 - Second Greed 1" },
			{ 93, "Mount Gulg B4 - Second Greed 2" },
			{ 94, "Mount Gulg B4 - Side Room 1" },
			{ 95, "Mount Gulg B4 - Side Room 2" },
			{ 236, "Mount Gulg B4 - Side Room 3" },
			{ 96, "Mount Gulg B4 - Last Room 1" },
			{ 97, "Mount Gulg B4 - Last Room 2" },
			{ 98, "Mount Gulg B5 - West Room" },
			{ 237, "Mount Gulg B5 - Northeast Room" },
			{ 238, "Mount Gulg B5 - East Room" },
			{ 239, "Mount Gulg B5 - Southeast Room" },
			{ 104, "Cavern of Ice B2 - Chest 1" },
			{ 105, "Cavern of Ice B2 - Chest 2" },
			{ 254, "Cavern of Ice B2 - Eye's Treasure" },
			{ 106, "Cavern of Ice B3 - Ice Dragon Room 1" },
			{ 107, "Cavern of Ice B3 - Ice Dragon Room 2" },
			{ 108, "Cavern of Ice B3 - Six-Pack 1" },
			{ 109, "Cavern of Ice B3 - Six-Pack 2" },
			{ 110, "Cavern of Ice B3 - Six-Pack 3" },
			{ 111, "Cavern of Ice B3 - Six-Pack 4" },
			{ 112, "Cavern of Ice B3 - Six-Pack 5" },
			{ 113, "Cavern of Ice B3 - Six-Pack 6" },
			{ 99, "Cavern of Ice B1 - Greed 1" },
			{ 100, "Cavern of Ice B1 - Greed 2" },
			{ 101, "Cavern of Ice B1 - Drop Room 1" },
			{ 102, "Cavern of Ice B1 - Drop Room 2" },
			{ 103, "Cavern of Ice B1 - Drop Room 3" },
			{ 240, "Citadel of Trials 2F - Side Chest" },
			{ 210, "Citadel of Trials 3F - Single" },
			{ 211, "Citadel of Trials 3F - Three-Pack 1" },
			{ 212, "Citadel of Trials 3F - Three-Pack 2" },
			{ 213, "Citadel of Trials 3F - Three-Pack 3" },
			{ 214, "Citadel of Trials 3F - Four-Pack 1" },
			{ 215, "Citadel of Trials 3F - Four-Pack 2" },
			{ 216, "Citadel of Trials 3F - Four-Pack 3" },
			{ 217, "Citadel of Trials 3F - Four-Pack 4" },
			{ 512, "Citadel of Trials 3F - Mouse Chest" },
			{ 224, "Dragon Caves (Forest) - Entrance 1" },
			{ 225, "Dragon Caves (Forest) - Entrance 2" },
			{ 226, "Dragon Caves (Forest) - Entrance 3" },
			{ 227, "Dragon Caves (Forest) - West Room 1" },
			{ 228, "Dragon Caves (Forest) - West Room 2" },
			{ 229, "Dragon Caves (Forest) - West Room 3" },
			{ 230, "Dragon Caves (Forest) - West Room 4" },
			{ 221, "Dragon Caves (Marsh) - Chest 1" },
			{ 222, "Dragon Caves (Marsh) - Chest 2" },
			{ 223, "Dragon Caves (Marsh) - Chest 3" },
			{ 218, "Dragon Caves (Plains) - Entrance" },
			{ 219, "Dragon Caves (Plains) - Duo 1" },
			{ 220, "Dragon Caves (Plains) - Duooo 2" },
			{ 114, "Waterfall Caverns - Chest 1" },
			{ 115, "Waterfall Caverns - Chest 2" },
			{ 116, "Waterfall Caverns - Chest 3" },
			{ 117, "Waterfall Caverns - Chest 4" },
			{ 118, "Waterfall Caverns - Chest 5" },
			{ 119, "Waterfall Caverns - Chest 6" },
			{ 404, "Waterfall Caverns - CubeBot" },
			{ 403, "Gaia - Fairy" },
			{ 130, "Sunken Shrine 3F (Split) - Kraken Side Chest" },
			{ 131, "Sunken Shrine 3F (Split) - Mermaids Side Chest" },
			{ 134, "Sunken Shrine 4F - TFC" },
			{ 135, "Sunken Shrine 4F - TFC North" },
			{ 136, "Sunken Shrine 4F - Side Corner" },
			{ 137, "Sunken Shrine 4F - First Greed" },
			{ 138, "Sunken Shrine 4F - Second Greed" },
			{ 139, "Sunken Shrine 5F - Passby" },
			{ 140, "Sunken Shrine 5F - Bubbles 1" },
			{ 141, "Sunken Shrine 5F - Bubbles 2" },
			{ 142, "Sunken Shrine 5F - Far Room 1" },
			{ 143, "Sunken Shrine 5F - Far Room 2" },
			{ 507, "Sunken Shrine 5F - Far Major" },
			{ 144, "Sunken Shrine 5F - Entrance 1" },
			{ 145, "Sunken Shrine 5F - Entrance 2" },
			{ 146, "Sunken Shrine 5F - Entrance 3" },
			{ 147, "Sunken Shrine 5F - Four-Corner 1" },
			{ 148, "Sunken Shrine 5F - Four-Corner 2" },
			{ 149, "Sunken Shrine 5F - Four-Corner 3" },
			{ 150, "Sunken Shrine 5F - Four-Corner 4" },
			{ 132, "Sunken Shrine 3F (Corridor) - Top Chest" },
			{ 133, "Sunken Shrine 3F (Corridor) - Bottom Chest" },
			{ 120, "Sunken Shrine 2F - Dengbait 1" },
			{ 121, "Sunken Shrine 2F - Dengbait 2" },
			{ 122, "Sunken Shrine 2F - Side Corner 1" },
			{ 123, "Sunken Shrine 2F - Side Corner 2" },
			{ 124, "Sunken Shrine 2F - Side Corner 3" },
			{ 125, "Sunken Shrine 2F - Exit" },
			{ 126, "Sunken Shrine 2F - Shark 1" },
			{ 127, "Sunken Shrine 2F - Shark 2" },
			{ 128, "Sunken Shrine 2F - Shark 3" },
			{ 129, "Sunken Shrine 2F - Shark 4" },
			{ 255, "Lufenia - Lufenian Man" },
			{ 151, "Mirage Tower 1F - Chest 1" },
			{ 152, "Mirage Tower 1F - Chest 2" },
			{ 153, "Mirage Tower 1F - Chest 3" },
			{ 154, "Mirage Tower 1F - Chest 4" },
			{ 155, "Mirage Tower 1F - Chest 5" },
			{ 156, "Mirage Tower 1F - Chest 6" },
			{ 157, "Mirage Tower 1F - Chest 7" },
			{ 158, "Mirage Tower 1F - Chest 8" },
			{ 159, "Mirage Tower 2F - Lesser 1" },
			{ 160, "Mirage Tower 2F - Lesser 2" },
			{ 161, "Mirage Tower 2F - Lesser 3" },
			{ 162, "Mirage Tower 2F - Lesser 4" },
			{ 163, "Mirage Tower 2F - Lesser 5" },
			{ 164, "Mirage Tower 2F - Greater 1" },
			{ 165, "Mirage Tower 2F - Greater 2" },
			{ 166, "Mirage Tower 2F - Greater 3" },
			{ 167, "Mirage Tower 2F - Greater 4" },
			{ 168, "Mirage Tower 2F - Greater 5" },
			{ 178, "Flying Fortress 1F - Solo" },
			{ 169, "Flying Fortress 1F - West Room 1" },
			{ 170, "Flying Fortress 1F - West Room 2" },
			{ 171, "Flying Fortress 1F - West Room 3" },
			{ 172, "Flying Fortress 1F - West Room 4" },
			{ 173, "Flying Fortress 1F - East Room 1" },
			{ 174, "Flying Fortress 1F - East Room 2" },
			{ 175, "Flying Fortress 1F - East Room 3" },
			{ 176, "Flying Fortress 1F - East Room 4" },
			{ 177, "Flying Fortress 1F - East Room 5" },
			{ 179, "Flying Fortress 2F - Cheap Room 1" },
			{ 180, "Flying Fortress 2F - Cheap Room 2" },
			{ 181, "Flying Fortress 2F - Vault 1" },
			{ 182, "Flying Fortress 2F - Vault 2" },
			{ 250, "Flying Fortress 2F - Major" },
			{ 183, "Flying Fortress 2F - Gauntlet Room" },
			{ 184, "Flying Fortress 2F - Ribbon Room 1" },
			{ 185, "Flying Fortress 2F - Ribbon Room 2" },
			{ 186, "Flying Fortress 2F - Wardrobe 1" },
			{ 187, "Flying Fortress 2F - Wardrobe 2" },
			{ 188, "Flying Fortress 3F - CC's Gambit 1" },
			{ 189, "Flying Fortress 3F - CC's Gambit 2" },
			{ 190, "Flying Fortress 3F - CC's Gambit 3" },
			{ 191, "Flying Fortress 3F - CC's Gambit 4" },
			{ 192, "Flying Fortress 3F - Six-Pack 1" },
			{ 193, "Flying Fortress 3F - Six-Pack 2" },
			{ 194, "Flying Fortress 3F - Six-Pack 3" },
			{ 195, "Flying Fortress 3F - Six-Pack 4" },
			{ 196, "Flying Fortress 3F - Six-Pack 5" },
			{ 197, "Flying Fortress 3F - Six-Pack 6" },
			{ 198, "Flying Fortress 3F - Exit 1" },
			{ 199, "Flying Fortress 3F - Exit 2" },
			{ 200, "Flying Fortress 3F - Exit 3" },
			{ 201, "Flying Fortress 3F - Exit 4" },
			{ 202, "Chaos Shrine 3F - Validation 1" },
			{ 203, "Chaos Shrine 3F - Validation 2" },
			{ 204, "Chaos Shrine B2 (Fire) - Right Chest" },
			{ 205, "Chaos Shrine B2 (Fire) - Left Chest" },
			{ 206, "Chaos Shrine B2 (Fire) - Eastern Vault" },
			{ 207, "Chaos Shrine B2 (Fire) - Southern Vault" },
			{ 208, "Chaos Shrine B4 (Air) - Legendary Chest" },
		};

		public static Dictionary<string, ItemData> ItemNameToData = new()
		{
			{ "Leather Cap", new ItemData() { Id = 175, Qty = 1 } },
			{ "Potion", new ItemData() { Id = 2, Qty = 1 } },
			{ "Tent", new ItemData() { Id = 18, Qty = 1 } },
			{ "Rune Blade", new ItemData() { Id = 75, Qty = 1 } },
			{ "Werebuster", new ItemData() { Id = 74, Qty = 1 } },
			{ "Gold Needle", new ItemData() { Id = 14, Qty = 1 } },
			{ "Lute", new ItemData() { Id = 45, Qty = 1 } },
			{ "Antidote", new ItemData() { Id = 11, Qty = 1 } },
			{ "Jolt Tonic", new ItemData() { Id = 48, Qty = 1 } },
			{ "Ship", new ItemData() { Id = 44, Qty = 1 } },
			{ "Dagger", new ItemData() { Id = 64, Qty = 1 } },
			{ "1180 Gil", new ItemData() { Id = 1, Qty = 1180 } },
			{ "1120 Gil", new ItemData() { Id = 1, Qty = 1120 } },
			{ "Broadsword", new ItemData() { Id = 73, Qty = 1 } },
			{ "Phoenix Down", new ItemData() { Id = 10, Qty = 1 } },
			{ "1045 Gil", new ItemData() { Id = 1, Qty = 1045 } },
			{ "Cottage", new ItemData() { Id = 19, Qty = 1 } },
			{ "Copper Armlet", new ItemData() { Id = 156, Qty = 1 } },
			{ "Crown", new ItemData() { Id = 46, Qty = 1 } },
			{ "1135 Gil", new ItemData() { Id = 1, Qty = 1135 } },
			{ "Silver Armlet", new ItemData() { Id = 157, Qty = 1 } },
			{ "1020 Gil", new ItemData() { Id = 1, Qty = 1020 } },
			{ "Crystal Eye", new ItemData() { Id = 47, Qty = 1 } },
			{ "Mythril Hammer", new ItemData() { Id = 115, Qty = 1 } },
			{ "800 Gil", new ItemData() { Id = 1, Qty = 800 } },
			{ "700 Gil", new ItemData() { Id = 1, Qty = 700 } },
			{ "Bronze Gloves", new ItemData() { Id = 192, Qty = 1 } },
			{ "Mystic Key", new ItemData() { Id = 49, Qty = 1 } },
			{ "Falchion", new ItemData() { Id = 97, Qty = 1 } },
			{ "Steel Gloves", new ItemData() { Id = 193, Qty = 1 } },
			{ "Power Staff", new ItemData() { Id = 120, Qty = 1 } },
			{ "Saber", new ItemData() { Id = 72, Qty = 1 } },
			{ "Mythril Knife", new ItemData() { Id = 65, Qty = 1 } },
			{ "Iron Armor", new ItemData() { Id = 148, Qty = 1 } },
			{ "Nitro Powder", new ItemData() { Id = 50, Qty = 1 } },
			{ "575 Gil", new ItemData() { Id = 1, Qty = 575 } },
			{ "450 Gil", new ItemData() { Id = 1, Qty = 450 } },
			{ "Excalibur", new ItemData() { Id = 92, Qty = 1 } },
			{ "Great Helm", new ItemData() { Id = 185, Qty = 1 } },
			{ "Wyrmkiller", new ItemData() { Id = 76, Qty = 1 } },
			{ "Ether", new ItemData() { Id = 5, Qty = 1 } },
			{ "Mythril Mail", new ItemData() { Id = 147, Qty = 1 } },
			{ "1975 Gil", new ItemData() { Id = 1, Qty = 1975 } },
			{ "795 Gil", new ItemData() { Id = 1, Qty = 795 } },
			{ "880 Gil", new ItemData() { Id = 1, Qty = 880 } },
			{ "Leather Shield", new ItemData() { Id = 161, Qty = 1 } },
			{ "5000 Gil", new ItemData() { Id = 1, Qty = 5000 } },
			{ "Coral Sword", new ItemData() { Id = 77, Qty = 1 } },
			{ "330 Gil", new ItemData() { Id = 1, Qty = 330 } },
			{ "Sleeping Bag", new ItemData() { Id = 17, Qty = 1 } },
			{ "3400 Gil", new ItemData() { Id = 1, Qty = 3400 } },
			{ "Star Ruby", new ItemData() { Id = 53, Qty = 1 } },
			{ "Staff", new ItemData() { Id = 118, Qty = 1 } },
			{ "1520 Gil", new ItemData() { Id = 1, Qty = 1520 } },
			{ "5450 Gil", new ItemData() { Id = 1, Qty = 5450 } },
			{ "1455 Gil", new ItemData() { Id = 1, Qty = 1455 } },
			{ "Mythril Shield", new ItemData() { Id = 164, Qty = 1 } },
			{ "1250 Gil", new ItemData() { Id = 1, Qty = 1250 } },
			{ "Great Axe", new ItemData() { Id = 107, Qty = 1 } },
			{ "620 Gil", new ItemData() { Id = 1, Qty = 620 } },
			{ "Mythril Helm", new ItemData() { Id = 187, Qty = 1 } },
			{ "Earth Rod", new ItemData() { Id = 54, Qty = 1 } },
			{ "Canoe", new ItemData() { Id = 61, Qty = 1 } },
			{ "Clothes", new ItemData() { Id = 134, Qty = 1 } },
			{ "Flame Sword", new ItemData() { Id = 83, Qty = 1 } },
			{ "Levistone", new ItemData() { Id = 55, Qty = 1 } },
			{ "Ice Armor", new ItemData() { Id = 149, Qty = 1 } },
			{ "Mythril Gloves", new ItemData() { Id = 197, Qty = 1 } },
			{ "7900 Gil", new ItemData() { Id = 1, Qty = 7900 } },
			{ "9900 Gil", new ItemData() { Id = 1, Qty = 9900 } },
			{ "5454 Gil", new ItemData() { Id = 1, Qty = 5454 } },
			{ "12350 Gil", new ItemData() { Id = 1, Qty = 12350 } },
			{ "10000 Gil", new ItemData() { Id = 1, Qty = 10000 } },
			{ "9500 Gil", new ItemData() { Id = 1, Qty = 9500 } },
			{ "Ice Shield", new ItemData() { Id = 165, Qty = 1 } },
			{ "Healing Staff", new ItemData() { Id = 119, Qty = 1 } },
			{ "Ruby Armlet", new ItemData() { Id = 158, Qty = 1 } },
			{ "Ice Brand", new ItemData() { Id = 86, Qty = 1 } },
			{ "7340 Gil", new ItemData() { Id = 1, Qty = 7340 } },
			{ "Gauntlets", new ItemData() { Id = 194, Qty = 1 } },
			{ "Rat's Tail", new ItemData() { Id = 57, Qty = 1 } },
			{ "2750 Gil", new ItemData() { Id = 1, Qty = 2750 } },
			{ "2000 Gil", new ItemData() { Id = 1, Qty = 2000 } },
			{ "X-Potion", new ItemData() { Id = 4, Qty = 1 } },
			{ "Elixir", new ItemData() { Id = 8, Qty = 1 } },
			{ "1000 Gil", new ItemData() { Id = 1, Qty = 1000 } },
			{ "Dry Ether", new ItemData() { Id = 7, Qty = 1 } },
			{ "Great Sword", new ItemData() { Id = 79, Qty = 1 } },
			{ "4150 Gil", new ItemData() { Id = 1, Qty = 4150 } },
			{ "1295 Gil", new ItemData() { Id = 1, Qty = 1295 } },
			{ "Hi-Potion", new ItemData() { Id = 3, Qty = 1 } },
			{ "1760 Gil", new ItemData() { Id = 1, Qty = 1760 } },
			{ "Mythril Axe", new ItemData() { Id = 109, Qty = 1 } },
			{ "1380 Gil", new ItemData() { Id = 1, Qty = 1380 } },
			{ "Flame Shield", new ItemData() { Id = 166, Qty = 1 } },
			{ "1150 Gil", new ItemData() { Id = 1, Qty = 1150 } },
			{ "1155 Gil", new ItemData() { Id = 1, Qty = 1155 } },
			{ "Flame Mail", new ItemData() { Id = 150, Qty = 1 } },
			{ "Wizard's Staff", new ItemData() { Id = 123, Qty = 1 } },
			{ "Ribbon", new ItemData() { Id = 183, Qty = 1 } },
			{ "6400 Gil", new ItemData() { Id = 1, Qty = 6400 } },
			{ "13450 Gil", new ItemData() { Id = 1, Qty = 13450 } },
			{ "Defender", new ItemData() { Id = 87, Qty = 1 } },
			{ "Warp Cube", new ItemData() { Id = 58, Qty = 1 } },
			{ "Oxyale", new ItemData() { Id = 60, Qty = 1 } },
			{ "Diamond Armor", new ItemData() { Id = 153, Qty = 1 } },
			{ "1300 Gil", new ItemData() { Id = 1, Qty = 1300 } },
			{ "Light Axe", new ItemData() { Id = 110, Qty = 1 } },
			{ "Mage's Staff", new ItemData() { Id = 121, Qty = 1 } },
			{ "9000 Gil", new ItemData() { Id = 1, Qty = 9000 } },
			{ "Diamond Armlet", new ItemData() { Id = 160, Qty = 1 } },
			{ "Diamond Shield", new ItemData() { Id = 168, Qty = 1 } },
			{ "Diamond Gloves", new ItemData() { Id = 198, Qty = 1 } },
			{ "Diamond Helm", new ItemData() { Id = 188, Qty = 1 } },
			{ "Rosetta Stone", new ItemData() { Id = 52, Qty = 1 } },
			{ "1110 Gil", new ItemData() { Id = 1, Qty = 1110 } },
			{ "1450 Gil", new ItemData() { Id = 1, Qty = 1450 } },
			{ "8135 Gil", new ItemData() { Id = 1, Qty = 8135 } },
			{ "7690 Gil", new ItemData() { Id = 1, Qty = 7690 } },
			{ "Giant's Gloves", new ItemData() { Id = 195, Qty = 1 } },
			{ "1385 Gil", new ItemData() { Id = 1, Qty = 1385 } },
			{ "Chime", new ItemData() { Id = 56, Qty = 1 } },
			{ "Healing Helm", new ItemData() { Id = 186, Qty = 1 } },
			{ "18010 Gil", new ItemData() { Id = 1, Qty = 18010 } },
			{ "Vorpal Sword", new ItemData() { Id = 82, Qty = 1 } },
			{ "Aegis Shield", new ItemData() { Id = 170, Qty = 1 } },
			{ "Thor's Hammer", new ItemData() { Id = 116, Qty = 1 } },
			{ "13000 Gil", new ItemData() { Id = 1, Qty = 13000 } },
			{ "7600 Gil", new ItemData() { Id = 1, Qty = 7600 } },
			{ "Sun Blade", new ItemData() { Id = 89, Qty = 1 } },
			{ "Dragon Mail", new ItemData() { Id = 154, Qty = 1 } },
			{ "Razer", new ItemData() { Id = 80, Qty = 1 } },
			{ "6720 Gil", new ItemData() { Id = 1, Qty = 6720 } },
			{ "Protect Ring", new ItemData() { Id = 202, Qty = 1 } },
			{ "Black Robe", new ItemData() { Id = 137, Qty = 1 } },
			{ "White Robe", new ItemData() { Id = 136, Qty = 1 } },
			{ "Adamantite", new ItemData() { Id = 51, Qty = 1 } },
			{ "Protect Cloak", new ItemData() { Id = 173, Qty = 1 } },
			{ "Sasuke's Blade", new ItemData() { Id = 100, Qty = 1 } },
			{ "Masamune", new ItemData() { Id = 103, Qty = 1 } },


		};
	}




}
