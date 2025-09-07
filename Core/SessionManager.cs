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

	public class SessionInfo
	{
		public GameModes Mode { get; set; }
		public string Seed { get; set; }
		public byte[] Hash { get; set; }
		public string Hashstring { get; set; }
		public string Player { get; set; }
		public string Host { get; set; }
		public string Port { get; set; }
		[JsonIgnore]
		public string Password { get; set; }
		public string StoredPassword { get; set; }
		public bool RememberPassword { get; set; }
		public string WorldSeed { get; set; }
		public int LocationCount { get; set; }
		public List<string> LocationsToSend { get; set; }
		public int ItemIndex { get; set; }
		public int Slot { get; set; }
		public Dictionary<string, int> Options { get; set; }
		public SessionInfo()
		{
			LocationsToSend = new();
			Hash = new byte[4];
			Options = new();
		}
	}
	public static class SessionManager
    {
		public static Dictionary<string, string> Slot = new();
		public static Dictionary<int, string> SlotInfo = new();

		private static SessionInfo Info { get; set; } = new();
		public static int CurrentSlot { get => Info.Slot; set => Info.Slot = value; }
		public static GameModes GameMode { get => Info.Mode; set => Info.Mode = value; }
		public static SessionInfo Data { get => Info; }
		public static Dictionary<string, int> Options { get => Info.Options; }

		public static string FolderPath;
		public static bool RandomizerInitialized = false;
		public static void Create()
		{
			if (!Directory.Exists(Application.persistentDataPath + "/Randomizer/"))
			{
				Directory.CreateDirectory(Application.persistentDataPath + "/Randomizer/");
			}

			FolderPath = Application.persistentDataPath + "/Randomizer/";

			if (!LoadSessionInfo(0))
			{
				Info = new();
				Info.Mode = GameModes.Archipelago;
				Info.Slot = 0;
				Info.WorldSeed = "";
				Info.Seed = "";
				Info.RememberPassword = false;
			}
			else
			{
				if (Info.Player is null) Info.Player = "";
				if (Info.Port is null) Info.Port = "";
				if (Info.Host is null) Info.Host = "archipelago.gg";
				if (Info.Password is null) Info.Password = "";
			}

			LoadSaveSlotInfoData();
		}

		public static bool LoadSessionInfo(int slot)
		{
			string filepath = FolderPath + "ff1pr_rando_data_" + slot + ".dat";
			bool fileexist = true;

			try
			{
				using (Stream configfile = new FileStream(filepath, FileMode.Open))
				{
					using (StreamReader reader = new StreamReader(configfile))
					{
						string configdata = reader.ReadToEnd();
						Info = JsonSerializer.Deserialize<SessionInfo>(configdata);
					}
				}
			}
			catch (Exception e)
			{
				fileexist = false;
				InternalLogger.LogWarning($"Unsucessful attempt at loading data at Slot {slot}: " + e);
				return false;
			}
			
			Info.Password = Info.RememberPassword ? Info.StoredPassword : "";

			InternalLogger.LogInfo($"Successfully loaded Session Info for Slot {slot}");
			//InternalLogger.LogInfo($"Info: {Info.Mode} - {Info.Seed} - {Info.Hashstring} - {Info.Player}");

			return fileexist;
		}
		public static void WriteSessionInfo()
		{
			string filepath = FolderPath + "ff1pr_rando_data_" + CurrentSlot + ".dat";

			Info.StoredPassword = Info.RememberPassword ? Info.Password : "";

			try
			{
				using (Stream configfile = new FileStream(filepath, FileMode.Create))
				{
					using (StreamWriter writer = new StreamWriter(configfile))
					{
						var configdata = JsonSerializer.Serialize<SessionInfo>(Info);
						writer.Write(configdata);
					}
				}
			}
			catch (Exception e)
			{
				InternalLogger.LogError(e.Message);
			}
		}
		public static void LoadSaveSlotInfoData()
		{

			for (int i = 1; i <= 22; i++)
			{
				string filepath = FolderPath + "ff1pr_rando_data_" + i + ".dat";
				bool fileexist = true;
				SessionInfo slotdata = new();
				try
				{
					using (Stream configfile = new FileStream(filepath, FileMode.Open))
					{
						using (StreamReader reader = new StreamReader(configfile))
						{
							string configdata = reader.ReadToEnd();

							//var options = new JsonSerializerOptions();
							//options.Converters.Add(new ValueToStringConverter());

							slotdata = JsonSerializer.Deserialize<SessionInfo>(configdata);
						}
					}
				}
				catch
				{
					fileexist = false;
				}

				string content = "";
				if (fileexist)
				{
					GameModes mode = slotdata.Mode;
					switch (mode)
					{
						case GameModes.Archipelago:
							content = $"Archipelago ({slotdata.Player})";
							break;
						case GameModes.Randomizer:
							content = $"Solo Randomizer ({slotdata.Hashstring})";
							break;
						default:
							break;
					}
				}

				SlotInfo[i] = content;
			}
		}
		public static string GetSlotInfo(int slot)
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
		public static uint CreateHash()
		{
			string settings = "";
			foreach (var option in FF1PRAP.Options.Dict.Values)
			{
				if (Info.Options.TryGetValue(option.Key, out var setting))
				{ 
					settings += setting;
				}
				else
				{
					settings += option.Default;
				}
			}

			settings += Info.Seed;
			var encodedsettings = Encoding.UTF8.GetBytes(settings);
			uint finalhash;
			string hashString;
			using (SHA256 hasher = SHA256.Create())
			{
				Blob hash = hasher.ComputeHash(encodedsettings);
				hashString = EncodeTo32(hash).Substring(0,8);
				finalhash = (uint)hash.ToUInts().Sum(x => x);
				Info.Hash = hash;
				Info.Hashstring = hashString;
			}

			return finalhash;
		}
		public static uint CreateApHash(string hashseed)
		{
			var encodedseed = Encoding.UTF8.GetBytes(hashseed);
			uint finalhash;
			string hashString;
			using (SHA256 hasher = SHA256.Create())
			{
				Blob hash = hasher.ComputeHash(encodedseed);
				hashString = EncodeTo32(hash).Substring(0, 8);
				finalhash = (uint)hash.ToUInts().Sum(x => x);
				Info.Hash = hash;
				Info.Hashstring = hashString;
			}

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
		public static void SaveLocationsToSend(List<string> locationsToSend)
		{
			Info.LocationsToSend = new(locationsToSend);
			Info.LocationCount = Info.LocationsToSend.Count;
		}
		public static List<string> LoadLocationsToSend()
		{
			return Info.LocationsToSend;
		}
	}
}
