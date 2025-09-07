using Last.Data.Master;
using Last.Interpreter;
using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Bindings;
using UnityEngine;

namespace FF1PRAP
{
	partial class Randomizer
    {
		public static Dictionary<string, int> ScriptToItemFlag = new()
		{
			{ "sc_e_0004_1", (int)TreasureFlags.Princess }, // Princess
			{ "sc_e_0009_2", (int)TreasureFlags.Bikke }, // Bikke
			{ "sc_e_0011_2", (int)TreasureFlags.Astos }, // Astos
			{ "sc_e_0012", (int)TreasureFlags.Matoya }, // Matoya
			{ "sc_e_0013", (int)TreasureFlags.ElfPrince }, // ElfPrince
			{ "sc_e_0019", (int)TreasureFlags.Sarda }, // Sarda
			{ "sc_e_0022", (int)TreasureFlags.CanoeSage }, // CanoeSage
			{ "sc_e_0024_2", (int)TreasureFlags.EyeChest }, // Levistone
			
			{ "sc_e_0026", (int)TreasureFlags.CubeBot }, // Levistone
			{ "sc_e_0052", (int)TreasureFlags.Smitt }, // Levistone
			{ "sc_e_0051", (int)TreasureFlags.SkyChest }, // Levistone
			{ "sc_e_0029", (int)TreasureFlags.Fairy }, // Levistone
			{ "sc_e_0035", (int)TreasureFlags.Lefeinman }, // Levistone
			{ "sc_bahamut", (int)TreasureFlags.Bahamut }, // Levistone
			//{ "sc_e_0035", (int)TreasureFlags.Caravan }, // Caravan
		};
	}
}
