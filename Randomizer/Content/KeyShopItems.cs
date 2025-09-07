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
		public static Dictionary<int, int> KeyShopItems = new()
		{
			{ 141, (int)TreasureFlags.Caravan }
		};
	}
}
