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
		public static void Gameflags_Postfix(string c, int index, int value)
		{
			if (SessionManager.GameMode == GameModes.Archipelago)
			{
				InternalLogger.LogTesting($"Setting flag: {c} - {index}");
				if (c == "TreasureFlag1" && value == 1)
				{
					Archipelago.instance.ActivateCheck(Randomizer.FlagToLocationName[index]);
				}
			}

			Randomizer.ProcessCrystals(c, index);
		}
	}
}
