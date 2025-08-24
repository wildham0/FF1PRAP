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
	/***
	Steps to add Options
	1. Write feature (duh); there's several parts
		a. Patching (code executed in-game):
			i. Create hook in Plugins.cs
			ii. Add patching code in Patches
		b. Data Randomizer (generated data that is injected and used by the patches/the game):
			i. Create randomizing code in Randomizer, the data randomizers don't write to game's memory, they generate minimal data that can be injected
			ii. Create data injection code in Randomizer (usually same file)
			iii. Add Randomized Data container to RandomizerData
			iv. Add the Data Randomizer call to Randomizer > Randomize()
			v. Add the Data Injection call to Initialization > ApplyRandomizedFeatures()
	2. Add Options parameters and description in Options
	3. Add Option in SettingsWindow.cs > AddRandoOptions()
	4. Add Option to APWorld > options.py
	***/

	public class Option
	{
		public string Key;
		public string Display;
		public Dictionary<string, string> Choices;
		public string Default;
		public string Description;
		public Option(string key, string display, Dictionary<string, string> choices, string defaultchoice, string description)
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
		// Generic choices for easy reference
		public static string Enable = "1";
		public static string Disable = "0";
		public static string Include = "include";
		public static string Exclude = "exclude";
		public static string Prioritize = "prioritize";
		public static string BikkeShip = "0";
		public static string MarshPath = "1";

		private static List<Option> optionlist = new()
		{
			new("npcs_priority", "NPCs", new() { { "prioritize", "Prioritize" }, {"include", "Include"}, {"exclude", "Exclude"} }, "prioritize",
				"When placing Key Items, set if NPCs are prioritized (if possible, a Key Item is always placed there), are included (a Key Item may be placed there) or are excluded (a Key Item is never placed there)."),
			new("keychests_priority", "Key Chests", new() {  { "prioritize", "Prioritize" }, {"include", "Include"}, {"exclude", "Exclude"} }, "prioritize",
				"When placing Key Items, set if Chests containing Key Items in the Vanilla Game are prioritized (if possible, a Key Item is always placed there), are included (a Key Item may be placed there) or are excluded (a Key Item is never placed there)."),
			new("trapped_priority", "Trapped Chests", new() {  { "prioritize", "Prioritize" }, {"include", "Include"}, {"exclude", "Exclude"} }, "include",
				"When placing Key Items, set if Trapped Chests are prioritized (if possible, a Key Item is always placed there), are included (a Key Item may be placed there) or are excluded (a Key Item is never placed there)."),
			new("adamantite_craft", "Adamantite Craft", new() {  { "92", "Excalibur" }, {"103", "Masamune"}, {"183", "Ribbon"}, {"121", "Mage's Staff"}, {"160", "Diamond Armlet"}, { "-1", "Random (Good Gear)" }, { "-2", "Random (Anything)" } }, "92",
				"When using Prioritized Locations, select what will replace the Excalibur as a potential Prioritized Item."),

			new("shuffle_gear_shops", "Shuffle Gear Shops", new() {  { "1", "Enable" }, {"0", "Disable"} }, "0",
				"Shuffle the content of all Weapon Shops together, and do the same for Armor Shops."),
			new("shuffle_spells", "Shuffle Spells", new() {  { "1", "Enable" }, {"0", "Disable"} }, "0",
				"Shuffle Spells amongst their own School."),
			new("job_promotion", "Job Promotion", new() {  { "0", "Bahamut" }, {"1", "Promote All Item"}, {"2", "Job Item"} }, "0",
				"Set how Promotion Jobs are handled.\n\nBahamut: Giving the Rat's Tail to Bahamut promote all Characters.\n\nPromote All Item: A Promote All Item is added to the Item Pool, when found all Characters promote. Bahamut becomes a Location.\n\nJob Item: All six Promotion Jobs become an individual Item added to the Item Pool. When acquired, all characters of the corresponding base Job promote. Bahamut becomes a Location."),

			new("shuffle_trials_maze", "Shuffle Trials' Maze", new() {  { "1", "Enable" }, { "0", "Disable" } }, "0", "Shuffle the Pillars Maze on floor 2F of the Citadel of Trials."),
			new("early_progression", "Early Progression", new() {  { "0", "Bikke's Ship" }, { "1", "Marsh Cave Path" } }, "0", "Set how the world is opened at the start of the game.\n\nBikke's Ship: The Bridge is built from the start and Bikke will always give the Ship.\n\nMarsh Cave Path: Open a path to the West of Coneria that allows you to reach the Marsh Cave area by foot. The Bridge is never built. The Ship is shuffled with other items and Bikke is a Location."),

			new("nerf_chaos", "Nerf Chaos", new() {  { "1", "Enable" }, { "0", "Disable" } }, "0", "Halve Chaos' HP and reduce his Intelligence and Attack Power by 25%."),
			new("monster_parties", "Monster Parties", new() {  { "0", "Standard" }, { "1", "Random No Variance" }, { "2", "Random Low Variance" }, { "3", "Random High Variance" } }, "0", "Randomize Monster Parties.\n\nStandard: Original Parties are maintained.\n\nNo Variance: Monsters will be replaced by Monsters of roughly the same power.\n\nLow Variance: Monsters can be replaced by slightly weaker or slightly stronger Monsters.\n\nHigh Variance: Monsters can be replaced by much weaker or much stronger Monsters."),
			new("monsters_cap", "Variance Cap", new() {  { "0", "None" }, { "1", "Upper Bound" }, { "2", "Lower Bound" } }, "0", "If Monster Parties are randomized, bound Power Variance. This option doesn't do anything for Standard and No Variance choices.\n\nNone: Variance is unbounded, Randomized Monster Parties can be weaker or stronger.\n\nUpper Bound: Randomized Monsters cannot be more powerful than the replaced Monsters, but they can be weaker.\n\nLower Bound: Randomized Monsters cannot be weaker than the replaced Monsters, but they can be more powerful."),

			new("dungeon_encounter_rate", "Dungeon Encounter Rate", new() {  { "0", "0.0x" }, { "1", "0.25x" }, { "2", "0.5x" }, { "3", "0.75x" }, { "4", "1.0x" }, { "5", "1.25x" }, { "6", "1.5x" }}, "4", "Modify the Encounter Rate in dungeons by the multiplier selected.\n\nNOTE: This option doesn't affect the Boost setting to disable/enable encounters."),
			new("overworld_encounter_rate", "Overworld Encounter Rate", new() {  { "0", "0.0x" }, { "1", "0.25x" }, { "2", "0.5x" }, { "3", "0.75x" }, { "4", "1.0x" }, { "5", "1.25x" }, { "6", "1.5x" }}, "4", "Modify the Encounter Rate on the Overworld by the multiplier selected.\n\nNOTE: This option doesn't affect the Boost setting to disable/enable encounters."),
			new("xp_boost", "Experience Boost", new() {  { "0", "0.5x" }, { "1", "1x" }, { "2", "2x" }, { "3", "3x" }, { "4", "4x" }}, "1", "Set the default Experience Boost multiplier. This can still be modified in the Boost menu."),
			new("gil_boost", "Gil Boost", new() {  { "0", "0.5x" }, { "1", "1x" }, { "2", "2x" }, { "3", "3x" }, { "4", "4x" }}, "1", "Set the default Gil Boost multiplier. This can still be modified in the Boost menu."),
			new("boost_menu", "Boost Menu", new() {  { "1", "Enable" }, { "0", "Disable" } }, "1", "Enable/Disable the in-game Boost menu. This will lock you to your current XP, Gil and Encounter Rate options."),
		};

		public static Dictionary<string, Option> Dict = optionlist.ToDictionary(o => o.Key, o => o);
	}
	
}
