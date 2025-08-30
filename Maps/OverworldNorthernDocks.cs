using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SocialPlatforms;
using System.Xml.Linq;
using System.Net.NetworkInformation;
using Last.Data.Master;
using Last.Interpreter;
using static FF1PRAP.Patches;
//using Il2CppSystem.Collections.Generic;

namespace FF1PRAP
{
	public static class MapPatchesNorthernDocks
	{
		public static PatchOpGroup TilemapTiles = new(0, new()
			{
				new RandoCondition(ConditionState.On, "ScenarioFlag1", (int) ScenarioFlags.NorthernDocks, FlagMode.Gameflag),
			}, new()
			{
				// Onrac Continent
				// Fix Forest
				new PatchOp(51,68,782),

				new PatchOp(49,69,386),
				new PatchOp(50,69,386),
				new PatchOp(51,69,387),
				new PatchOp(52,69,391),

				// Dock 
				new PatchOp(49,70,1031),
				new PatchOp(50,70,1032),
				new PatchOp(51,70,1032),
				new PatchOp(52,70,1033),

				// Sea
				new PatchOp(50,71,0),
				new PatchOp(51,71,0),

				// Mirage Continent
				new PatchOp(201,82,1031),
				new PatchOp(202,82,1032),
				new PatchOp(203,82,1032),
				new PatchOp(204,82,1033),

				new PatchOp(202,83,0),
				new PatchOp(203,83,0),

			});
		public static PatchOpGroup TilemapBottom = new(1, new()
			{
				new RandoCondition(ConditionState.On, "ScenarioFlag1", (int)ScenarioFlags.NorthernDocks, FlagMode.Gameflag),
			}, new()
			{
				// Onrac Continent
				// Fix Forest
				new PatchOp(51,68,798),

				new PatchOp(49,69,402),
				new PatchOp(50,69,402),
				new PatchOp(51,69,403),
				new PatchOp(52,69,407),

				// Dock
				new PatchOp(49,70,1047),
				new PatchOp(50,70,1048),
				new PatchOp(51,70,1048),
				new PatchOp(52,70,1049),

				// Sea
				new PatchOp(50,71,0),
				new PatchOp(51,71,0),

				// Mirage Continent
				new PatchOp(201,82,1047),
				new PatchOp(202,82,1048),
				new PatchOp(203,82,1048),
				new PatchOp(204,82,1049),

				new PatchOp(202,83,0),
				new PatchOp(203,83,0),

			});
		public static PatchOpGroup TilemapGround = new(2, new()
			{
				new RandoCondition(ConditionState.On, "ScenarioFlag1", (int)ScenarioFlags.NorthernDocks, FlagMode.Gameflag),
			}, new()
			{
				// Onrac Continent
				// Fix Forest
				new PatchOp(51,69,0),

				// Dock
				new PatchOp(49,70,0),
				new PatchOp(50,70,0),
				new PatchOp(51,70,0),
				new PatchOp(52,70,0),
				
				// Sea
				new PatchOp(50,71,0),
				new PatchOp(51,71,0),

				// Mirage Continent
				new PatchOp(201,82,0),
				new PatchOp(202,82,0),
				new PatchOp(203,82,0),
				new PatchOp(204,82,0),

				new PatchOp(202,83,0),
				new PatchOp(203,83,0),
			});
		public static PatchOpGroup Attributes = new(0, new()
		{
			new RandoCondition(ConditionState.On, "ScenarioFlag1", (int)ScenarioFlags.NorthernDocks, FlagMode.Gameflag),
		}, new()
		{
			// Allow docking
			new PatchOp(49,70,1),
			new PatchOp(50,70,1),
			new PatchOp(51,70,1),
			new PatchOp(52,70,1),

			new PatchOp(201,82,1),
			new PatchOp(202,82,1),
			new PatchOp(203,82,1),
			new PatchOp(204,82,1),
		});
	}
}
