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
		public static float SetEncounterRate(int rateOption)
		{
			List<float> scale = new() { 5000.0f, 4.0f, 2.0f, 1.5f, 1.0f, 0.75f, 0.5f };
			return scale[rateOption];
		}
		public static float SetVictoryBoost(int rateOption)
		{
			List<float> scale = new() { 0.5f, 1.0f, 2.0f, 3.0f, 4.0f };
			return scale[rateOption];
		}
		public static void InitializeBoost()
		{
			FF1PR.UserData.CheatSettingsData.GilRate = Randomizer.RandomizerData.GilBoost;
			FF1PR.UserData.CheatSettingsData.ExpRate = Randomizer.RandomizerData.XpBoost;
		}

		public static void ApplyBoost()
		{
			if (!Randomizer.RandomizerData.BoostMenu)
			{
				FF1PR.MessageManager.GetMessageDictionary()["MSG_SYSTEM_CS_0_006"] = "Boost as been disabled by your settings.";
			}
		}
	}
}
