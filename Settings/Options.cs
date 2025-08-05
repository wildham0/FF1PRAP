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
		public string Description;

		public Option(string key, string display, List<(string key, string display)> choices, int defaultchoice, string description)
		{
			Key = key;
			Display = display;
			Choices = choices;
			Default = defaultchoice;
			Description = description;
		}
	}

	public static class Options
	{
		private static List<Option> optionlist = new()
		{
			new("npcs_priority", "NPCs", new() { ("prioritize", "Prioritize"), ("include", "Include"), ("exclude", "Exclude") }, 0,
				"When placing Key Items, set if NPCs are prioritized (if possible, a Key Item is always placed there), are included (a Key Item may be placed there) or are excluded (a Key Item is never placed there)."
				),
			new("keychests_priority", "Key Chests", new() { ("prioritize", "Prioritize"), ("include", "Include"), ("exclude", "Exclude") }, 0,
				"When placing Key Items, set if Chests containing Key Items in the Vanilla Game are prioritized (if possible, a Key Item is always placed there), are included (a Key Item may be placed there) or are excluded (a Key Item is never placed there)."),
			new("trapped_priority", "Trapped Chests", new() { ("prioritize", "Prioritize"), ("include", "Include"), ("exclude", "Exclude") }, 1,
				"hen placing Key Items, set if Trapped Chests are prioritized (if possible, a Key Item is always placed there), are included (a Key Item may be placed there) or are excluded (a Key Item is never placed there)."),
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
