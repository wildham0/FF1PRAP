using Last.Data.Master;
using Last.Interpreter;
using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Bindings;

namespace FF1PRAP
{
	partial class Randomizer
    {
		public static int ProcessLute(int qty, MT19337 rng)
		{
			Dictionary<int, int> optionValues = new()
			{
				{ 0, 0 },
				{ 18, 18 },
				{ 24, 24 },
				{ 30, 30 },
				{ 36, 36 },
				{ 1830, rng.Between(18, 30) },
				{ 2436, rng.Between(24, 36) },
				{ 1836, rng.Between(18, 36) },
			};

			if (optionValues.TryGetValue(qty, out var value))
			{
				qty = value;
			}
			else
			{
				qty = Math.Min(qty, 40);
			}

			return qty;
		}
		public static void ApplyLute()
		{
			if (Randomizer.Data.RequiredTablatures > 0)
			{
				GameData.MessageManager.GetMessageDictionary()["MSG_TABLATURE_INF"] = GameData.MessageManager.GetMessageDictionary()["MSG_TABLATURE_INF"] + $"\nA number in the corner, {Randomizer.Data.RequiredTablatures} are needed for the complete song.";
			}
		}
		public static void InitializeLute()
		{
			if (Randomizer.Data.RequiredTablatures > 0)
			{
				if (SessionManager.GameMode == GameModes.Randomizer)
				{
					GameData.OwnedItemsClient.AddOwnedItem((int)Items.Lute, 1);
					GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.Lute, 1);
				}
			}
		}
		public static int ProcessCrystals(int qty, MT19337 rng)
		{
			if (qty >= 5)
			{
				qty = rng.Between(1, 3);
			}

			return qty;
		}
		public static void ApplyCrystals()
		{
			if (Randomizer.Data.RequiredCrystals > 0 && Randomizer.Data.RequiredCrystals < 4)
			{
				GameData.MessageManager.GetMessageDictionary()["MSG_CHS_03"] = $"The Black Crystal shines ominously. The air in the room seems slightly... distorted.\nYou sense the power of {Randomizer.Data.RequiredCrystals} Crystal{(Randomizer.Data.RequiredCrystals > 1 ? "s" : "")} should be enough to destroy it.";
			}
		}
		public static void InitializeCrystals()
		{
			if (Randomizer.Data.RequiredCrystals == 0)
			{
				GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.BlackOrbReqCompleted, 1);
			}
		}
	}
}
