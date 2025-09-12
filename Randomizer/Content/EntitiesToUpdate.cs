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
using Last.Interpreter;
using static FF1PRAP.Patches;
//using Il2CppSystem.Collections.Generic;

namespace FF1PRAP
{
	public static partial class Randomizer
	{
		public static Dictionary<string, List<EntityData>> EntititesToUpdate = new()
		{
			// Coneria Castle
			{ "Map_20011_1", new() {
				new EntityData() { EntityId = 88, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 89, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 72, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 75, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			} },
			// Coneria Castle Second Floor
			{ "Map_20011_2", new() {
				new EntityData() { EntityId = 42, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.IntroDone, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			} },
			// Temple of Fiend
			{ "Map_30011_1", new() {
				new EntityData() { EntityId = 32, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.PrincessSaved, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 33, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.PrincessSaved, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 88, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 89, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 80, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 83, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 70, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 73, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 111, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 112, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			} },
			// North West Castle
			{ "Map_20081_1", new() {
				new EntityData() { EntityId = 141, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.Crown, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 105, Conditions = new() {
					new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.Crown, FlagCategory = DataStorage.Category.kScenarioFlag1 },
					new Condition() { Value = EntityCondition.Off, Flag = (int)TreasureFlags.Astos, FlagCategory = DataStorage.Category.kTreasureFlag1 },} },
				new EntityData() { EntityId = 134, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 132, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			} },
			// Marsh Cave B3
			{ "Map_30021_3", new() {
				new EntityData() { EntityId = 126, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 129, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 132, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 135, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 138, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 140, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 141, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 142, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 143, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 144, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			} },
			// Matoya's Cave
			{ "Map_20031_1", new() {
				new EntityData() { EntityId = 16, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.CrystalEye, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 15, Conditions = new() {
					new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.CrystalEye, FlagCategory = DataStorage.Category.kScenarioFlag1 },
					new Condition() { Value = EntityCondition.Off, Flag = (int)TreasureFlags.Matoya, FlagCategory = DataStorage.Category.kTreasureFlag1 },} },
				new EntityData() { EntityId = 14, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)TreasureFlags.Matoya, FlagCategory = DataStorage.Category.kTreasureFlag1 } } },
			} },
			// Elfland Castle
			{ "Map_20071_1", new() {
				new EntityData() { EntityId = 118, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.JoltTonic, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 116, Conditions = new() {
					new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.JoltTonic, FlagCategory = DataStorage.Category.kScenarioFlag1 },
					new Condition() { Value = EntityCondition.Off, Flag = (int)TreasureFlags.ElfPrince, FlagCategory = DataStorage.Category.kTreasureFlag1 },} },
				new EntityData() { EntityId = 115, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)TreasureFlags.ElfPrince, FlagCategory = DataStorage.Category.kTreasureFlag1 } } },
				new EntityData() { EntityId = 133, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)TreasureFlags.ElfPrince, FlagCategory = DataStorage.Category.kTreasureFlag1 } } },
				new EntityData() { EntityId = 114, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)TreasureFlags.ElfPrince, FlagCategory = DataStorage.Category.kTreasureFlag1 } } },
				new EntityData() { EntityId = 140, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 258, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			} },
			// Dwarf Cave
			{ "Map_20051_1", new() {
				new EntityData() { EntityId = 127, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.Adamant, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 40, Conditions = new() {
					new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.Adamant, FlagCategory = DataStorage.Category.kScenarioFlag1 },
					new Condition() { Value = EntityCondition.Off, Flag = (int)TreasureFlags.Smitt, FlagCategory = DataStorage.Category.kTreasureFlag1 },} },
				new EntityData() { EntityId = 39, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)TreasureFlags.Smitt, FlagCategory = DataStorage.Category.kTreasureFlag1 } } },
				new EntityData() { EntityId = 52, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.NitroPowder, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 45, Conditions = new() {
					new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.NitroPowder, FlagCategory = DataStorage.Category.kScenarioFlag1 },
					new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.Canal, FlagCategory = DataStorage.Category.kScenarioFlag1 },} },
				new EntityData() { EntityId = 90, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 82, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			} },
			// Melmond
			{ "Map_20090", new() {
				new EntityData() { EntityId = 127, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.RosettaStone, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 126, Conditions = new() {
					new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.RosettaStone, FlagCategory = DataStorage.Category.kScenarioFlag1 },
					new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.StoneTranslated, FlagCategory = DataStorage.Category.kScenarioFlag1 },} },
				new EntityData() { EntityId = 125, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.StoneTranslated, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			} },
			// Earth B3
			{ "Map_30031_3", new() {
				new EntityData() { EntityId = 54, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.VampireDefeated, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 55, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.EarthRod, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 52, Conditions = new() {
					new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.EarthRod, FlagCategory = DataStorage.Category.kScenarioFlag1 },
					new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.SlabLifted, FlagCategory = DataStorage.Category.kScenarioFlag1 },} },
			} },
			// Cube Warp
			{ "Map_30101_3", new() {
				new EntityData() { EntityId = 14, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.WarpCube, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 21, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.WarpCube, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			} },
			// Overworld
			{ "Map_10010", new() {
				new EntityData() { EntityId = 82, Conditions = new() {
					new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.Levistone, FlagCategory = DataStorage.Category.kScenarioFlag1 },
					new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.Airship, FlagCategory = DataStorage.Category.kScenarioFlag1 },} },
				new EntityData() { EntityId = 145, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.Chime, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				//new EntityData() { EntityId = 205, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.IntroDone, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				/*new EntityData() { EntityId = 197, Conditions = new() {
					new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.Canal, FlagCategory = DataStorage.Category.kScenarioFlag1 },
					new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.WestwardProgressionMode, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 500, Conditions = new() {
					new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.Canal, FlagCategory = DataStorage.Category.kScenarioFlag1 },
					new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.WestwardProgressionMode, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },*/
				
				//new EntityData() { EntityId = 205, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.Canal, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				//new EntityData() { EntityId = 101, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.Canal, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			} },
		};
	}
}
