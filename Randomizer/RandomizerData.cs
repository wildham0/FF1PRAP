using FF1PRAP;
using Last.Data.Master;
using RomUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Bindings;
using static FF1PRAP.Randomizer;

namespace FF1PRAP
{
	public class RandomizerData
    {
		public List<Product> GearShops { get; set; }
		public List<ShuffledSpell> ShuffledSpells { get; set; }
		public Dictionary<int, ItemData> PlacedItems { get; set; }
		public Dictionary<int, int> OrdealsMaze { get; set; }
		//public Dictionary<(int, int), Teleporter> Teleporters { get; set; }
		public Dictionary<string, string> Entrances { get; set; }
		public Dictionary<int, Dictionary<int, MonsterIds>> MonsterParties { get; set; }
		public float DungeonEncounterRate { get; set; }
		public float OverworldEncounterRate { get; set; }
		public float XpBoost { get; set; }
		public float GilBoost { get; set; }
		public bool BoostMenu { get; set; }
		public bool NerfChaos { get; set; }
		public JobPromotionModes JobPromotion { get; set; }
		public EarlyProgressionModes EarlyProgression { get; set; }
		public bool NorthernDocks { get; set; }
		public int RequiredTablatures { get; set; }
		public int RequiredCrystals { get; set; }
		public bool EntrancesShuffled { get; set;}
		public string SmittThingy { get; set; }
		public RandomizerData()
		{
			GearShops = new();
			PlacedItems = new();
			ShuffledSpells = new();
			OrdealsMaze = new();
			MonsterParties = new();
			RequiredTablatures = 0;
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
						var randoString = JsonSerializer.Serialize<RandomizerData>(Data, serializeOptions);
						writer.Write(randoString);
					}
				}
			}
			catch (Exception e)
			{
				InternalLogger.LogError(e.Message);
			}
		}

		public static bool Load(string folderPath, string filedata)
		{
			string filepath = folderPath + "ff1pr_" + filedata + ".dat";

			var serializeOptions = new JsonSerializerOptions();
			serializeOptions.Converters.Add(new JsonConverters.ProductConverter());

			try
			{
				using (Stream configfile = new FileStream(filepath, FileMode.Open))
				{
					using (StreamReader reader = new StreamReader(configfile))
					{
						string configdata = reader.ReadToEnd();

						Data = JsonSerializer.Deserialize<RandomizerData>(configdata, serializeOptions);
					}
				}
			}
			catch (Exception e)
			{
				InternalLogger.LogTesting(e.Message);
				return false;
			}

			InternalLogger.LogInfo($"Previously randomized data {filedata} loaded successfully.");
			return true;
		}
	}
}
