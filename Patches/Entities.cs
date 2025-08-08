using Last.Entity.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Last.Interpreter;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;
using static Disarm.Disassembler;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using UnityEngine;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using static FF1PRAP.Patches;
//using Il2CppSystem.Collections.Generic;

namespace FF1PRAP
{
	partial class Patches
	{
		public enum EntityAction
		{ 
			Replace,
			Remove,
			Titan,
			ElfPrince,
			Show,
			Hide,
		}
		public enum EntityCondition
		{ 
			Always,
			On,
			Off
		}
		public struct EntityData
		{
			public int Layer;
			public int ObjectId;
			public string EntitiesFile;
			public int EntityId;
			//Dictionary<string>
			public string PropFile;
			public EntityAction Action;
			public EntityCondition Condition;
			public List<Condition> Conditions;
			public DataStorage.Category FlagCategory;
			public int Flag;
		}
		public struct Condition
		{
			public EntityCondition Value;
			public DataStorage.Category FlagCategory;
			public int Flag;
		}

		/*
		public class ValueToStringConverter : JsonConverter<object>
		{
			public override bool CanConvert(Type typeToConvert)
			{
				return typeof(string) == typeToConvert;
			}
			public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				if (reader.TokenType == JsonTokenType.Number)
				{
					return reader.TryGetInt64(out long l) ?
						l.ToString() :
						reader.GetDouble().ToString();
				}

				if (reader.TokenType == JsonTokenType.True)
				{
					return "true";
				}

				if (reader.TokenType == JsonTokenType.False)
				{
					return "false";
				}

				if (reader.TokenType == JsonTokenType.String)
				{
					return reader.GetString();
				}
				using (JsonDocument document = JsonDocument.ParseValue(ref reader))
				{
					return document.RootElement.Clone().ToString();
				}
			}

			public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
			{
				writer.WriteStringValue(value.ToString());
			}
		}
		*/
		private static TileMapCustomPropertyData[] GetMapObjecData(string name)
		{
			string scriptfile = "";
			var assembly = Assembly.GetExecutingAssembly();
			string filepath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name + ".json"));
			using (Stream logicfile = assembly.GetManifestResourceStream(filepath))
			{
				using (StreamReader reader = new StreamReader(logicfile))
				{
					scriptfile = reader.ReadToEnd();
				}
			}

			/*
			var options = new JsonSerializerOptions();
			options.Converters.Add(new ValueToStringConverter());
			

			return JsonSerializer.Deserialize<List<TileMapCustomPropertyData>>(scriptfile, options).ToArray();*/

			return JsonConvert.DeserializeObject<System.Collections.Generic.List<TileMapCustomPropertyData>>(scriptfile).ToArray();
		}

		private static string GetFile(string name)
		{
			string scriptfile = "";
			var assembly = Assembly.GetExecutingAssembly();
			string filepath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name + ".json"));
			using (Stream logicfile = assembly.GetManifestResourceStream(filepath))
			{
				using (StreamReader reader = new StreamReader(logicfile))
				{
					scriptfile = reader.ReadToEnd();
				}
			}

			return scriptfile;
		}
		public static void SetupEntities_Post(bool __result)
		{
			UpdateEntities();
			//InternalLogger.LogInfo($"Entites setup 2 done. Result: {__result}");
		}

		public static void UpdateEntities()
		{
			if (entititesToUpdate.TryGetValue(FF1PR.CurrentMap, out var entitiesSet))
			{
				InternalLogger.LogInfo($"Updating entities on {FF1PR.CurrentMap}");

				foreach (var entity in entitiesSet)
				{
					var fieldentity = FF1PR.FieldController.GetFieldEntity(entity.EntityId);

					if (fieldentity != null)
					{
						bool met = true;

						foreach (var condition in entity.Conditions)
						{
							//InternalLogger.LogInfo($"Evaluation condition: {condition.Flag} - {condition.Value}.");
							var flag = FF1PR.DataStorage.Get(condition.FlagCategory, condition.Flag);
							if ((condition.Value == EntityCondition.On && flag == 0) ||
								(condition.Value == EntityCondition.Off && flag != 0))
							{
								//InternalLogger.LogInfo($"Condition not met.");
								met = false;
								break;
							}
							else
							{
								//InternalLogger.LogInfo($"Condition met.");
							}
						}

						if (met)
						{
							InternalLogger.LogInfo($"Updating entity {entity.EntityId}: Show.");
							FF1PR.FieldController.GetFieldEntity(entity.EntityId).Show();
						}
						else
						{
							InternalLogger.LogInfo($"Updating entity {entity.EntityId}: Hide.");
							FF1PR.FieldController.GetFieldEntity(entity.EntityId).Hide();
						}
					}
					else
					{
						InternalLogger.LogInfo($"Entity {entity.EntityId} was null.");
					}
				}
			}
		}

		public static Dictionary<string, List<EntityData>> entititesToUpdate = new()
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
				new EntityData() { EntityId = 235, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)TreasureFlags.ElfPrince, FlagCategory = DataStorage.Category.kTreasureFlag1 } } },
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
				new EntityData() { EntityId = 114, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
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
				/*
				new EntityData() { EntityId = 205, Conditions = new() { new Condition() { Value = EntityCondition.Off, Flag = (int)ScenarioFlags.Canal, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
				new EntityData() { EntityId = 101, Conditions = new() { new Condition() { Value = EntityCondition.On, Flag = (int)ScenarioFlags.Canal, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },*/
			} },

		};


		public static Dictionary<(int, string), List<EntityData>> EntititesToUpdateOld = new()
		{
			 // Overworld
			{ (10, "ev_e_0025"), new() {
				new EntityData() { Layer = 11, ObjectId = 49, Action = EntityAction.Remove, Condition = EntityCondition.Off, Flag = (int)ScenarioFlags.Levistone, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 11, ObjectId = 49, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.Airship, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 11, ObjectId = 156, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.Chime, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 11, ObjectId = 116, PropFile = "en_nocanal", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)ScenarioFlags.Canal, FlagCategory = DataStorage.Category.kScenarioFlag1 },
			} },
			 // ToF - Garland
			{ (17, "ev_e_0003"), new() { 
				new EntityData() { Layer = 19, ObjectId = 173, PropFile = "en_garland", Action = EntityAction.Replace, Condition = EntityCondition.Always, Flag = 0, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 18, ObjectId = 219, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 18, ObjectId = 221, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 19, ObjectId = 299, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 19, ObjectId = 300, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 18, ObjectId = 292, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 18, ObjectId = 293, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 18, ObjectId = 294, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
			} },
			// ToF - Default Locked Door
			{ (14, "ev_e_0014"), new() {
				new EntityData() { Layer = 16, ObjectId = 269, PropFile = "en_lockeddoor", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 16, ObjectId = 277, PropFile = "en_lockeddoor", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
			} },
			// ToF - Fiends Defeated
			{ (20, "ev_e_0038"), new() {
				new EntityData() { Layer = 21, ObjectId = 288, PropFile = "en_lockeddoor", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 21, ObjectId = 290, PropFile = "en_lockeddoor", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
			} },
			// ToF - Black Orb Removed
			{ (29, "ev_e_0039"), new() {
				new EntityData() { Layer = 21, ObjectId = 288, PropFile = "en_lockeddoor", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 21, ObjectId = 290, PropFile = "en_lockeddoor", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
			} },
			 // Coneria Trigger Sara Step
			{ (26, "ev_e_0004"), new() { new EntityData() { Layer = 27, ObjectId = 27, Action = EntityAction.Remove, Condition = EntityCondition.Always, Flag = 0, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			 // NW Castle MK door
			{ (15, "ev_e_0010"), new() { new EntityData() { Layer = 20, ObjectId = 184, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			{ (16, "ev_e_0011"), new() { new EntityData() { Layer = 21, ObjectId = 162, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			{ (17, "ev_e_0012"), new() { new EntityData() { Layer = 22, ObjectId = 163, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 } } },
			 // Elfland Castle
			{ (16, "ev_e_0014"), new() {
				new EntityData() { Layer = 17, ObjectId = 143, PropFile = "en_elfprince", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)TreasureFlags.ElfPrince, FlagCategory = DataStorage.Category.kTreasureFlag1 },
				new EntityData() { Layer = 17, ObjectId = 143, PropFile = "en_elfprince", Action = EntityAction.ElfPrince, Condition = EntityCondition.Off, Flag = (int)TreasureFlags.ElfPrince, FlagCategory = DataStorage.Category.kTreasureFlag1 },
				new EntityData() { Layer = 17, ObjectId = 144, PropFile = "en_elfdoc_script", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)TreasureFlags.ElfPrince, FlagCategory = DataStorage.Category.kTreasureFlag1 },
				new EntityData() { Layer = 17, ObjectId = 144, PropFile = "en_elfdoc_prejolt", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)ScenarioFlags.JoltTonic, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 17, ObjectId = 162, PropFile = "en_lockeddoor", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
			}},
			// Dwarf 
			{ (30, "ev_e_0014"), new() { 
				new EntityData() { Layer = 31, ObjectId = 114, PropFile = "en_lockeddoor", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 32, ObjectId = 117, PropFile = "en_lockeddoor", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)ScenarioFlags.MysticKey, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 31, ObjectId = 119, PropFile = "en_nerrick_script", Action = EntityAction.Replace, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.NitroPowder, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 31, ObjectId = 119, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.Canal, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 32, ObjectId = 147, PropFile = "en_smitt_script", Action = EntityAction.Replace, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.Adamant, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 32, ObjectId = 147, PropFile = "en_smitt_post_adamant", Action = EntityAction.Replace, Condition = EntityCondition.On, Flag = (int)TreasureFlags.Smitt, FlagCategory = DataStorage.Category.kTreasureFlag1 }
			}},
			// Earth B3
			{ (14, "ev_e_0016"), new() {
				new EntityData() { Layer = 15, ObjectId = 66, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.VampireDefeated, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 23, ObjectId = 89, PropFile = "en_earth_slab", Action = EntityAction.Replace, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.EarthRod, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 23, ObjectId = 89, Action = EntityAction.Remove, Condition = EntityCondition.On, Flag = (int)ScenarioFlags.SlabLifted, FlagCategory = DataStorage.Category.kScenarioFlag1 },
			}},
			// SardaCave
			{ (14, "ev_e_0019"), new() {
				new EntityData() { Layer = 17, ObjectId = 15, PropFile = "en_sarda_prevamp", Action = EntityAction.Replace, Condition = EntityCondition.Off, Flag = (int)ScenarioFlags.VampireDefeated, FlagCategory = DataStorage.Category.kScenarioFlag1 },
				new EntityData() { Layer = 17, ObjectId = 15, PropFile = "en_sarda_postvamp", Action = EntityAction.Replace, Condition = EntityCondition.On, Flag = (int)TreasureFlags.Sarda, FlagCategory = DataStorage.Category.kTreasureFlag1 },
			}},
			// Ordeals
			{ (15, "ev_e_0046"), new() {
				new EntityData() { Layer = 14, ObjectId = 14, PropFile = "en_ordealsman", Action = EntityAction.Replace, Condition = EntityCondition.Always, Flag = 0, FlagCategory = DataStorage.Category.kScenarioFlag1 },
			}},
			// Lute Slab
			{ (38, "ev_e_0039"), new() {
				new EntityData() { Layer = 37, ObjectId = 31, PropFile = "en_luteslab", Action = EntityAction.Replace, Condition = EntityCondition.Always, Flag = 0, FlagCategory = DataStorage.Category.kScenarioFlag1 },
			}},
			//{ "ev_e_0018", new() {				new EntityData() { Layer = 5, ObjectId = 0, Action = EntityAction.Titan },			}},
		};
	}
}
