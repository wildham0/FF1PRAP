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
	public partial class Patches
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
			if (Randomizer.EntititesToUpdate.TryGetValue(GameData.CurrentMap, out var entitiesSet))
			{
				InternalLogger.LogTesting($"Updating entities on {GameData.CurrentMap}");

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
							InternalLogger.LogTesting($"Updating entity {entity.EntityId}: Show.");
							GameData.FieldController.GetFieldEntity(entity.EntityId).Show();
						}
						else
						{
							InternalLogger.LogTesting($"Updating entity {entity.EntityId}: Hide.");
							GameData.FieldController.GetFieldEntity(entity.EntityId).Hide();
						}
					}
					else
					{
						InternalLogger.LogTesting($"Entity {entity.EntityId} was null.");
					}
				}
			}
		}
	}
}
