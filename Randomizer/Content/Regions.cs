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

		static public List<RegionData> FixedRegionsWest = new()
		{
			new RegionData() { Region = Regions.MatoyaCave, Access = new() { new() { AccessRequirements.Ship }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.Pravoka, Access = new() { new() { AccessRequirements.Ship }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.DwarfCave, Access = new() { } },
			new RegionData() { Region = Regions.Elfland, Access = new() { } },
			new RegionData() { Region = Regions.ElflandCastle, Access = new() { } },
			new RegionData() { Region = Regions.NorthWestCastle, Access = new() { } },
			new RegionData() { Region = Regions.MarshCave, Access = new() { } },
			new RegionData() { Region = Regions.CrescentLake, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canal }, new() { AccessRequirements.Canoe }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.RyukhanDesert, Access = new() { new() { AccessRequirements.Canoe }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.Volcano, Access = new() { new() { AccessRequirements.Canoe }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.IceCave, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canoe }, new() { AccessRequirements.Airship }, } },
		};

		static public List<RegionData> FixedRegionsNorthernDocks = new()
		{
			new RegionData() { Region = Regions.Onrac, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canal }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.Caravan, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canal }, new() { AccessRequirements.Airship }, } },
			new RegionData() { Region = Regions.Waterfall, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canal, AccessRequirements.Canoe }, new() { AccessRequirements.Airship, AccessRequirements.Canoe }, } },

			new RegionData() { Region = Regions.MirageTower, Access = new() { new() { AccessRequirements.Ship, AccessRequirements.Canal, AccessRequirements.Bell }, new() { AccessRequirements.Airship, AccessRequirements.Bell } } },
		};
	}
}
