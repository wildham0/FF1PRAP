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
			{ "sc_e_0004_1", (int)TreasureFlags.Princess },
			{ "sc_e_0009_2", (int)TreasureFlags.Bikke },
			{ "sc_e_0011_2", (int)TreasureFlags.Astos },
			{ "sc_e_0012", (int)TreasureFlags.Matoya },
			{ "sc_e_0013", (int)TreasureFlags.ElfPrince },
			{ "sc_e_0019", (int)TreasureFlags.Sarda },
			{ "sc_e_0022", (int)TreasureFlags.CanoeSage },
			{ "sc_e_0024_2", (int)TreasureFlags.EyeChest },
			{ "sc_e_0026", (int)TreasureFlags.CubeBot },
			{ "sc_e_0052", (int)TreasureFlags.Smitt },
			{ "sc_e_0051", (int)TreasureFlags.SkyChest },
			{ "sc_e_0029", (int)TreasureFlags.Fairy },
			{ "sc_e_0035", (int)TreasureFlags.Lefeinman },
			{ "sc_bahamut", (int)TreasureFlags.Bahamut },
		};
	}
}
