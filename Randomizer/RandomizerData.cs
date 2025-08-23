using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last.Data.Master;
using UnityEngine.Bindings;
using System.Text.Json;

using System.IO;
using UnityEngine;
using static FF1PRAP.Randomizer;

namespace FF1PRAP
{
	public class RandomizerData
    {
		public List<Product> GearShops { get; set; }
		public List<ShuffledSpell> ShuffledSpells { get; set; }
		public Dictionary<int, ItemData> PlacedItems { get; set; }
		public Dictionary<int, int> OrdealsMaze { get; set; }
		public float DungeonEncounterRate { get; set; }
		public float OverworldEncounterRate { get; set; }
		public float XpBoost { get; set; }
		public float GilBoost { get; set; }
		public bool BoostMenu { get; set; }
		public bool NerfChaos { get; set; }
		public JobPromotionModes JobPromotion { get; set; }
		public EarlyProgressionModes EarlyProgression { get; set; }

		public RandomizerData()
		{
			GearShops = new();
			PlacedItems = new();
			ShuffledSpells = new();
			OrdealsMaze = new();
		}
	}

	public partial class Randomizer
	{ 
		public static void Serialize(string folderPath, string filedata)
		{
			string filepath = folderPath + "ff1pr_" + filedata + ".dat";

			var serializeOptions = new JsonSerializerOptions();
			serializeOptions.Converters.Add(new JsonConverters.ProductConverter());

			try
			{
				using (Stream randoDataFile = new FileStream(filepath, FileMode.Create))
				{
					using (StreamWriter writer = new StreamWriter(randoDataFile))
					{
						var randoString = JsonSerializer.Serialize<RandomizerData>(RandomizerData, serializeOptions);
						writer.Write(randoString);
					}
				}
			}
			catch (Exception e)
			{
				InternalLogger.LogInfo(e.Message);
			}
		}

		public static bool Load(string folderPath, string filedata)
		{
			string filepath = folderPath + "ff1pr_" + filedata + ".dat";
			bool fileexist = true;

			var serializeOptions = new JsonSerializerOptions();
			serializeOptions.Converters.Add(new JsonConverters.ProductConverter());

			try
			{
				using (Stream configfile = new FileStream(filepath, FileMode.Open))
				{
					using (StreamReader reader = new StreamReader(configfile))
					{
						string configdata = reader.ReadToEnd();

						//var options = new JsonSerializerOptions();
						//options.Converters.Add(new ValueToStringConverter());

						RandomizerData = JsonSerializer.Deserialize<RandomizerData>(configdata, serializeOptions);
					}
				}
			}
			catch (Exception e)
			{
				InternalLogger.LogError(e.Message);
				fileexist = false;
				return false;
			}

			InternalLogger.LogInfo($"Previously randomized data {filedata} loaded successfully.");
			//SetGlobal("mode", GetValue<string>("mode"));
			return fileexist;
		}
	}
}
