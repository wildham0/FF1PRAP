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
			new LocationData() { Content = (int)Items.BottledFaerie, Qty = 1, Id = 12, Flag = (int)TreasureFlags.Caravan, Type = LocationType.GameObject, Name = "Caravan", Script = 0, Map = "Map_20160", Region = Regions.Caravan, Access = new() { } },
			//new LocationData() { Type = LocationType.Event, Name = "Caravan", Region = Regions.Caravan, Access = new() { }, Trigger = new() { AccessRequirements.BottledFaerie } },

			// Fairy
			new LocationData() { Content = (int)Items.Oxyale, Qty = 1, Id = 10, Flag = (int)TreasureFlags.Fairy, Type = LocationType.GameObject, Name = "Fairy", Script = 0, Map = "Map_20160", Region = Regions.Gaia, Access = new() { new() { AccessRequirements.BottledFaerie } } },

			// Bahamut
			new LocationData() { Content = (int)Items.JobAll, Qty = 1, Id = 10, Flag = (int)TreasureFlags.Bahamut, Type = LocationType.GameObject, Name = "Bahamut", Script = 0, Map = "Map_20160", Region = Regions.CardiaTop, Access = new() { new() { AccessRequirements.RatsTail } } },

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
			new LocationData() { Content = (int)Items.Chime, Qty = 1, Id = 8, Flag = 255, Type = LocationType.GameObject, Name = "Lefeinman", Script = 0, Map = "Map_20160", Region = Regions.Lefein, Access = new() { new() { AccessRequirements.LeifenishLearned } } },

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

	}
}
