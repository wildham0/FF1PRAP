using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using System.Text.Json.Serialization;
//using System.Reflection;
using LibCpp2IL.NintendoSwitch;
using static FF1PRAP.Patches;
using System.Text.Json;
using Last.Systems.Indicator;
using LibCpp2IL.Wasm;
using RomUtilities;
using static UnityEngine.ParticleSystem.PlaybackState;
using System.Security.Cryptography;
using Last.Data.Master;

namespace FF1PRAP
{
	public enum GameModes
	{ 
		Vanilla,
		Randomizer,
		Archipelago
	}
	public class SessionManager
    {
		public static Dictionary<string, string> Slot = new();
		public static Dictionary<string, string> Global = new();
		public static Dictionary<int, string> SlotInfo = new();

		public int currentSlot;
		public string folderPath;
		public bool RandomizerInitialized = false;
		public GameModes GameMode { get => GetGameMode(); set => SetGameMode(value); }
		//public GameStates GameState = GameStates.Title;
		//public SystemIndicator.Mode LoadingState = SystemIndicator.Mode.kNone;
		public SessionManager()
		{
			if (!Directory.Exists(Application.persistentDataPath + "/Randomizer/"))
			{
				Directory.CreateDirectory(Application.persistentDataPath + "/Randomizer/");
			}

			folderPath = Application.persistentDataPath + "/Randomizer/";

			LoadGlobalData();
			LoadSaveSlotInfoData();
		}

		public void SetGlobal(string key, string value)
		{
			Global[key] = value;
		}
		public void SetGlobal(string key, int value)
		{

			SetGlobal(key, value.ToString());
		}
		public void SetGlobal(string key, bool value)
		{
			SetGlobal(key, value ? "true" : "false");
		}

		public T GetGlobal<T>(string key)
		{
			if (Global.TryGetValue(key, out string value))
			{

				return (T)Convert.ChangeType(value, typeof(T));
			}
			else
			{
				return (T)Convert.ChangeType("", typeof(T));
			}
		}

		public bool TryGetGlobal<T>(string key, out T result)
		{
			if (Global.TryGetValue(key, out string value))
			{

				result = (T)Convert.ChangeType(value, typeof(T));
				return true;
			}
			else
			{
				result = default;
				return false;
			}
		}

		public void SetValue(string key, string value)
		{
			Slot[key] = value;
		}
		public void SetValue(string key, int value)
		{

			SetValue(key, value.ToString());
		}
		public void SetValue(string key, bool value)
		{
			SetValue(key, value ? "true" : "false");
		}
		public T GetValue<T>(string key)
		{
			if (Slot.TryGetValue(key, out string value))
			{

				return (T)Convert.ChangeType(value, typeof(T));
			}
			else
			{
				return default;
				//return (T)Convert.ChangeType("null", typeof(T));
			}
		}

		public bool TryGetValue<T>(string key, out T result)
		{
			if (Slot.TryGetValue(key, out string value))
			{

				result = (T)Convert.ChangeType(value, typeof(T));
				return true;
			}
			else
			{
				result = default;
				return false;
			}
		}

		public void SetSlot(int slot)
		{
			currentSlot = slot;
		}

		public string GetSlotInfo(int slot)
		{
			if (SlotInfo.TryGetValue(slot, out var info))
			{
				return info;
			}
			else
			{
				return "";
			}
		}
		public void LoadSaveSlotInfoData()
		{

			for (int i = 1; i <= 22; i++)
			{
				string filepath = folderPath + "ff1pr_rando_data_" + i + ".dat";
				bool fileexist = true;
				Dictionary<string, string> slotdata = new();
				try
				{
					using (Stream configfile = new FileStream(filepath, FileMode.Open))
					{
						using (StreamReader reader = new StreamReader(configfile))
						{
							string configdata = reader.ReadToEnd();

							//var options = new JsonSerializerOptions();
							//options.Converters.Add(new ValueToStringConverter());

							slotdata = JsonSerializer.Deserialize<Dictionary<string, string>>(configdata);
						}
					}
				}
				catch (Exception e)
				{
					fileexist = false;
				}

				string content = "";
				if (fileexist)
				{
					string mode = slotdata.TryGetValue("mode", out var mode_result) ? mode_result : "";
					switch (mode)
					{
						case "archipelago":
							string player = slotdata.TryGetValue("player", out var player_result) ? player_result : "";
							string port = slotdata.TryGetValue("port", out var port_result) ? port_result : "";
							string itemindex = slotdata.TryGetValue("itemindex", out var itemindex_result) ? itemindex_result : "";
							content = $"Archipelago\n{player} / {port}";
							break;
						case "randomizer":
							string hashstring = slotdata.TryGetValue("hashstring", out var hashstring_result) ? hashstring_result : "";
							string seed = slotdata.TryGetValue("seed", out var seed_result) ? seed_result : "";
							content = $"Solo Randomizer\n{hashstring} / {seed}";
							break;
						default:
							break;
					}
				}

				SlotInfo[i] = content;
			}
		}

		public bool LoadGlobalData()
		{
			string filepath = folderPath + "ff1pr_rando_data_global.dat";
			bool fileexist = true;

			try
			{
				using (Stream configfile = new FileStream(filepath, FileMode.Open))
				{
					using (StreamReader reader = new StreamReader(configfile))
					{
						string configdata = reader.ReadToEnd();

						//var options = new JsonSerializerOptions();
						//options.Converters.Add(new ValueToStringConverter());

						Global = JsonSerializer.Deserialize<Dictionary<string, string>>(configdata);
					}
				}
			}
			catch (Exception e)
			{
				fileexist = false;
			}

			if (!Global.ContainsKey("mode"))
			{
				SetGlobal("mode", "randomizer");
			}

			SetValue("mode", GetGlobal<string>("mode"));

			return fileexist;
		}

		public void WriteGlobalData()
		{
			string filepath = folderPath + "ff1pr_rando_data_global.dat";
			SetGlobal("mode", GetValue<string>("mode"));

			try
			{
				using (Stream configfile = new FileStream(filepath, FileMode.Create))
				{
					using (StreamWriter writer = new StreamWriter(configfile))
					{
						var configdata = JsonSerializer.Serialize<Dictionary<string, string>>(Global);
						writer.Write(configdata);
					}
				}
			}
			catch (Exception e) { }
		}

		public bool LoadSlotData()
		{
			string filepath = folderPath + "ff1pr_rando_data_" + currentSlot + ".dat";
			bool fileexist = true;

			try
			{
				using (Stream configfile = new FileStream(filepath, FileMode.Open))
				{
					using (StreamReader reader = new StreamReader(configfile))
					{
						string configdata = reader.ReadToEnd();

						//var options = new JsonSerializerOptions();
						//options.Converters.Add(new ValueToStringConverter());

						Slot = JsonSerializer.Deserialize<Dictionary<string, string>>(configdata);
					}
				}
			}
			catch(Exception e)
			{
				fileexist = false;
			}

			//SetGlobal("mode", GetValue<string>("mode"));
			return fileexist;
		}
		public void WriteSlotData()
		{
			string filepath = folderPath + "ff1pr_rando_data_" + currentSlot + ".dat";

			try
			{
				using (Stream configfile = new FileStream(filepath, FileMode.Create))
				{
					using (StreamWriter writer = new StreamWriter(configfile))
					{
						var configdata = JsonSerializer.Serialize<Dictionary<string, string>>(Slot);
						writer.Write(configdata);
					}
				}
			}
			catch (Exception e) { }

			//SetGlobal("mode", GetValue<string>("mode"));
		}

		public void SetPlacedItems(Dictionary<int, ItemData> placedItems)
		{
			foreach (var item in placedItems)
			{
				InternalLogger.LogInfo("flag_" + item.Key + "_id");
				SetValue("flag_" + item.Key + "_id", item.Value.Id);
				SetValue("flag_" + item.Key + "_qty", item.Value.Qty);
			}
		}

		public void SetRandomizedGame(Dictionary<int, ItemData> placedItems)
		{
			SetPlacedItems(placedItems);
			foreach (var option in Options.Dict.Values)
			{
				if (TryGetGlobal<string>(option.Key, out var setting))
				{
					SetValue(option.Key, setting);
				}
				else
				{
					SetValue(option.Key, option.Default);
				}
			}
			SetValue("seed", GetGlobal<string>("seed"));
			SetValue("hash", GetGlobal<string>("hash"));
			SetValue("hashstring", GetGlobal<string>("hashstring"));
			//SetValue("mode", GetGlobal<string>("mode"));
		}
		public uint CreateHash()
		{
			string settings = "";
			foreach (var option in Options.Dict.Values)
			{
				if (TryGetGlobal<string>(option.Key, out var setting))
				{
					settings += setting;
				}
				else
				{
					settings += option.Default;
				}
			}

			settings += GetGlobal<string>("seed");
			var encodedsettings = Encoding.UTF8.GetBytes(settings);
			uint finalhash;
			string hashString;
			using (SHA256 hasher = SHA256.Create())
			{
				Blob hash = hasher.ComputeHash(encodedsettings);
				hashString = EncodeTo32(hash);
				finalhash = (uint)hash.ToUInts().Sum(x => x);
			}

			SetGlobal("hash", finalhash.ToString());
			SetGlobal("hashstring", hashString);

			return finalhash;
		}
		public static string EncodeTo32(byte[] bytesToEncode)
		{
			string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

			string encodedString = "";

			foreach (var byteValue in bytesToEncode)
			{
				encodedString += characters[(byteValue / 16)];
				encodedString += characters[(byteValue % 16)];
			}

			return encodedString;
		}
		public Dictionary<int, ItemData> GetPlacedItems()
		{

			Dictionary<int, ItemData> placedItems = new();
			InternalLogger.LogInfo($"GetPlacedItems: mode: {GetValue<string>("mode")}");

			if (GetValue<string>("mode") == "randomizer")
			{
				foreach (var location in Randomizer.FixedLocations.Where(l => l.Type != LocationType.Event).ToList())
				{
					int flag = location.Flag;
					int item = GetValue<int>("flag_" + flag + "_id");
					int qty = GetValue<int>("flag_" + flag + "_qty");

					InternalLogger.LogInfo($"GetPlacedItems: item: {flag} - {item}");
					placedItems.Add(flag, new ItemData() { Id = item, Qty = qty });
				}
			}

			return placedItems;
		}

		public void SaveLocationsToSend(List<string> locationsToSend)
		{
			SetValue("locationsToSendCount", locationsToSend.Count);
			for (int i = 0; i < locationsToSend.Count; i++)
			{
				SetValue("locationsToSend_" + i, locationsToSend[i]);
			}
		}

		public List<string> LoadLocationsToSend()
		{
			int locationCount = 0;
			List<string> locationsToSend = new();
			if (TryGetValue<int>("locationsToSendCount", out locationCount))
			{
				for (int i = 0; i < locationCount; i++)
				{
					if (TryGetValue<string>("locationsToSend_" + i, out var location))
					{
						locationsToSend.Add(location);
					}
				}
			}

			return locationsToSend;
		}

		private GameModes GetGameMode()
		{
			var newmode = FF1PR.SessionManager.GetValue<string>("mode");
			if (newmode == "vanilla")
			{
				return GameModes.Vanilla;
			}
			else if (newmode == "randomizer")
			{
				return GameModes.Randomizer;
			}
			else if (newmode == "archipelago")
			{
				return GameModes.Archipelago;
			}

			return GameModes.Randomizer;
		}
		private void SetGameMode(GameModes newmode)
		{
			string modetype = "vanilla";

			if (newmode == GameModes.Vanilla)
			{
				modetype = "vanilla";
			}
			else if (newmode == GameModes.Randomizer)
			{
				modetype = "randomizer";
			}
			else if (newmode == GameModes.Archipelago)
			{
				modetype = "archipelago";
			}
			FF1PR.SessionManager.SetValue("mode", modetype);
		}


	}
}
