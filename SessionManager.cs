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

namespace FF1PRAP
{
	public enum GameModes
	{ 
		Vanilla,
		Randomizer,
		Archipelago
	}
	public enum GameStates
	{
		Title,
		LoadGame,
		NewGame,
		WaitingForStart,
		InGame
	}

	public class SessionManager
    {
		public static Dictionary<string, string> Slot = new();
		public static Dictionary<string, string> Global = new();

		public int currentSlot;
		public string folderPath;
		public bool RandomizerInitialized = false;
		public GameModes GameMode { get => GetGameMode(); set => SetGameMode(value); }
		public GameStates GameState = GameStates.Title;
		public SessionManager()
		{
			if (!Directory.Exists(Application.persistentDataPath + "/Randomizer/"))
			{
				Directory.CreateDirectory(Application.persistentDataPath + "/Randomizer/");
			}

			folderPath = Application.persistentDataPath + "/Randomizer/";

			LoadGlobalData();
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
				return (T)Convert.ChangeType("", typeof(T));
			}
		}
		public void SetSlot(int slot)
		{
			currentSlot = slot;
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

			return fileexist;
		}

		public void WriteGlobalData()
		{
			string filepath = folderPath + "ff1pr_rando_data_global.dat";

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
		}

		public void SetPlacedItems(Dictionary<int, ItemData> placedItems)
		{
			foreach (var item in placedItems)
			{
				SetValue("flag_" + item.Key + "_id", item.Value.Id);
				SetValue("flag_" + item.Key + "_qty", item.Value.Qty);
			}
		}

		public Dictionary<int, ItemData> GetPlacedItems()
		{

			Dictionary<int, ItemData> placedItems = new();
			if (GetValue<string>("mode") == "randomizer")
			{
				foreach (var location in Randomizer.FixedLocations)
				{
					int flag = location.Flag;
					int item = GetValue<int>("flag_" + flag + "_id");
					int qty = GetValue<int>("flag_" + flag + "_qty");

					placedItems.Add(flag, new ItemData() { Id = item, Qty = qty });
				}
			}

			return placedItems;
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

			return GameModes.Vanilla;
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



		public string PlayerName = "testplayer";
		public string Port = "51186";
		public string Server = "archipelago.gg";
		public string Password = "";
		public string GameName = "FF1 Pixel Remaster";
		public bool IsArchipelago = true;
	}
}
