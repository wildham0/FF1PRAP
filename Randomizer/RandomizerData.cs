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

namespace FF1PRAP
{
	class RandomizerData
    {
		public List<Product> GearShops { get; set; }
		public Dictionary<int, ItemData> PlacedItems { get; set; }

		public RandomizerData()
		{
			GearShops = new();
			PlacedItems = new();
		}

		public void Serialize(string folderPath, string filedata)
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
						var randoString = JsonSerializer.Serialize<RandomizerData>(this, serializeOptions);
						writer.Write(randoString);
					}
				}
			}
			catch (Exception e)
			{
				InternalLogger.LogInfo(e.Message);
			}
		}

		public bool Load(string folderPath, string filedata)
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

						var randoData = JsonSerializer.Deserialize<RandomizerData>(configdata, serializeOptions);
						GearShops = randoData.GearShops != null ? randoData.GearShops : new();
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
