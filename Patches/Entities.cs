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
using Last.Map;
using static Last.Interpreter.Instructions.External;
using Last.Data.Master;
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
		public struct Condition
		{
			public EntityCondition Value;
			public DataStorage.Category FlagCategory;
			public int Flag;
		}

		public struct EntityData
		{
			public int EntityId;
			public List<Condition> Conditions;
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

		private static string GetFile(string name, string ext = "json")
		{
			string scriptfile = "";
			var assembly = Assembly.GetExecutingAssembly();
			string filepath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name + "." + ext));
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
			/*
			if (GameData.CurrentMap == "Map_20031_1")
			{
				InternalLogger.LogInfo($"Moving treasure");

				var tresurebox = GameData.FieldController.GetFieldEntity(5);

				//tresurebox.Property.Pos = new Vector3(160, 192, tresurebox.Property.Pos.z);
				tresurebox.Init(new Vector3(160, 192, tresurebox.Property.Pos.z), LayerSetting.Layers.BottomLayer, 15);
				//tresurebox.Show();


			}*/

			
			if (entititesToUpdate.TryGetValue(GameData.CurrentMap, out var entitiesSet))
			{
				InternalLogger.LogInfo($"Updating entities on {GameData.CurrentMap}");

				foreach (var entity in entitiesSet)
				{
					var fieldentity = GameData.FieldController.GetFieldEntity(entity.EntityId);

					if (fieldentity != null)
					{
						bool met = true;

						foreach (var condition in entity.Conditions)
						{
							//InternalLogger.LogInfo($"Evaluation condition: {condition.Flag} - {condition.Value}.");
							var flag = GameData.DataStorage.Get(condition.FlagCategory, condition.Flag);
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
							GameData.FieldController.GetFieldEntity(entity.EntityId).Show();
						}
						else
						{
							InternalLogger.LogInfo($"Updating entity {entity.EntityId}: Hide.");
							GameData.FieldController.GetFieldEntity(entity.EntityId).Hide();
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
