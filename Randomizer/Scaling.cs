using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last.Data.Master;
using UnityEngine.Bindings;

namespace FF1PRAP
{
	partial class Randomizer
    {
		public static float SetEncounterRate(string rateOption)
		{
			switch (rateOption)
			{
				case "0":
					return 5000.0f;
				case "1":
					return 4.0f;
				case "2":
					return 2.0f;
				case "3":
					return 1.5f;
				case "4":
					return 1.0f;
				case "5":
					return 0.75f;
				case "6":
					return 0.50f;
				default:
					return 1.0f;
			}
		}
		public static float SetVictoryBoost(string rateOption)
		{
			switch (rateOption)
			{
				case "0":
					return 0.5f;
				case "1":
					return 1.0f;
				case "2":
					return 2.0f;
				case "3":
					return 3.0f;
				case "4":
					return 4.0f;
				default:
					return 1.0f;
			}
		}
	}
}
