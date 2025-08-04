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

namespace FF1PRAP
{

	public class Option
	{
		public string Key;
		public string Display;
		public List<(string key, string display)> Choices;
		public int Default;

		public Option(string key, string display, List<(string key, string display)> choices, int defaultchoice)
		{
			Key = key;
			Display = display;
			Choices = choices;
			Default = defaultchoice;
		}
	}

	public static class Options
	{
		private static List<Option> optionlist = new()
		{
			new("npcs_priority", "NPCs", new() { ("prioritize", "Prioritize"), ("include", "Include"), ("exclude", "Exclude") }, 0),
			new("keychests_priority", "Key Chests", new() { ("prioritize", "Prioritize"), ("include", "Include"), ("exclude", "Exclude") }, 0),
			new("trapped_priority", "Trapped Chests", new() { ("prioritize", "Prioritize"), ("include", "Include"), ("exclude", "Exclude") }, 1),
		};

		public static Dictionary<string, Option> Dict = optionlist.ToDictionary(o => o.Key, o => o);
		/*
		public static Dictionary<string, List<string>> Flags = new()
		{
			{ "prioritize npcs", new() { "prioritize", "include", "exclude" } },
			{ "prioritize key chests", new() { "prioritize", "include", "exclude" } },
			{ "prioritize trapped chests", new() { "prioritize", "include", "exclude" } },
		};
	*/
	
	}
	
}
