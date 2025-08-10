using Last.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF1PRAP
{
	partial class Patches
	{
		private static void GameBooster_Post(ref Last.UI.KeyInput.ConfigController __instance)
		{
			if (!Randomizer.RandomizerData.BoostMenu)
			{
				__instance.InitializeNone();
			}
		}
	}
}
