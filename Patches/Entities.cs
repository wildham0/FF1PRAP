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
using System.Text.Json.Serialization;

namespace FF1PRAP
{
	partial class Patches
	{
		public enum EntityAction
		{ 
			Replace,
			Remove,
			Titan,
			ElfPrince
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
			//Dictionary<string>
			public string PropFile;
			public EntityAction Action;
			public EntityCondition Condition;
			public DataStorage.Category FlagCategory;
			public int Flag;
		}

		public class CustomMapObjecData
		{
			public int gid { get; set; }
			public int height { get; set; }
			public int width { get; set; }
			public int id { get; set; }
			public string name { get; set; }
			public int rotation { get; set; }
			public string type { get; set; }
			public bool visible { get; set; }

			public int x { get; set; }
			public int y { get; set; }
			public List<TileMapCustomPropertyData> properties { get; set; }
			public CustomMapObjecData()	{ properties = new(); }
			public TileMapObjectData ToMapObjectData()
			{
				TileMapObjectData newobject = new();
				newobject.id = id;
				newobject.name = name;
				newobject.height = height;
				newobject.width = width;
				// gid, rotation?? type, visi
				newobject.x = x;
				newobject.y = y;
				newobject.properties = properties.ToArray();
				return newobject;
			}
			public void CopyTo(ref TileMapObjectData newobject)
			{
				newobject.id = id;
				newobject.name = name;
				newobject.height = height;
				newobject.width = width;
				// gid, rotation?? type, visi
				newobject.x = x;
				newobject.y = y;
				newobject.properties = properties.ToArray();
			}
		}
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

			
			var options = new JsonSerializerOptions();
			options.Converters.Add(new ValueToStringConverter());

			return JsonSerializer.Deserialize<List<TileMapCustomPropertyData>>(scriptfile, options).ToArray();
		}
		public static void ParseMapObjectGroupData_Prefix(ref TileMapLayerGroupData tileMapLayerGroupData)
		{
			InternalLogger.LogInfo($"Maplayer: {tileMapLayerGroupData.id}, {tileMapLayerGroupData.name}");

			if (EntititesToUpdate.TryGetValue((tileMapLayerGroupData.id, tileMapLayerGroupData.name), out var entitydata))
			{
				foreach (var entity in entitydata)
				{
					if (tileMapLayerGroupData.layers.TryFind(l => l.id == entity.Layer, out var layer))
					{
						if (layer.objects.TryFind(o => o.id == entity.ObjectId, out var oldEntity))
						{
							if (entity.Action == EntityAction.Replace)
							{
								var flag = FF1PR.DataStorage.Get(entity.FlagCategory, entity.Flag);

								if ((entity.Condition == EntityCondition.On && flag != 0) ||
									(entity.Condition == EntityCondition.Off && flag == 0) ||
									(entity.Condition == EntityCondition.Always))
								{
									var newEntityProp = GetMapObjecData(entity.PropFile);
									oldEntity.properties = newEntityProp;
								}
							}
							else if (entity.Action == EntityAction.Remove)
							{
								var flag = FF1PR.DataStorage.Get(entity.FlagCategory, entity.Flag);

								if ((entity.Condition == EntityCondition.On && flag != 0) ||
									(entity.Condition == EntityCondition.Off && flag == 0) ||
									(entity.Condition == EntityCondition.Always))
								{
									layer.objects = layer.objects.Where(o => o.id != entity.ObjectId).ToArray();
								}
							}
							else if (entity.Action == EntityAction.ElfPrince)
							{
								var flag = FF1PR.DataStorage.Get(entity.FlagCategory, entity.Flag);

								if ((entity.Condition == EntityCondition.On && flag != 0) ||
									(entity.Condition == EntityCondition.Off && flag == 0) ||
									(entity.Condition == EntityCondition.Always))
								{
									var newEntityProp = GetMapObjecData(entity.PropFile);
									oldEntity.x += 16;
								}
							}
						}
						else if (entity.Action == EntityAction.Titan)
						{
							layer.data[34 * 15 + 11] = 203;
						}
					}
				}
				//InternalLogger.LogInfo($"garland transfered?");
			}
		}

		public static Dictionary<(int, string), List<EntityData>> EntititesToUpdate = new()
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
