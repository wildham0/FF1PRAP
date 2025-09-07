using Last.Entity.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF1PRAP
{
	partial class Patches
	{
		public static void Treasure_Prefix(ref FieldTresureBox tresureBoxEntity)
		{
			var prop = tresureBoxEntity.tresureBoxProperty;
			if (SessionManager.GameMode == GameModes.Archipelago)
			{
				if (GameData.DataStorage.Get(Last.Interpreter.DataStorage.Category.kTreasureFlag1, prop.FlagId) == 1)
				{
					return;
				}
				
				prop.ContentId = 43;
				prop.ContentNum = 0;
				prop.MessageKey = "MSG_OTHER_11";

				if(Randomizer.ApLocations.TryGetValue(prop.FlagId, out var location))
				{
					GameData.MessageManager.GetMessageDictionary()["MSG_OTHER_11"] = $"You obtained {location.Content}.";
					Archipelago.instance.ActivateCheck(Randomizer.FlagToLocationName[prop.FlagId]);
				}
				else
				{
					InternalLogger.LogWarning("Treasure (Archipelago): Couldn't find Location Id, make sure you're correctly connected to Archipelago.");
				}
			}
			else if (Randomizer.Data.PlacedItems.TryGetValue(prop.FlagId, out var item))
			{
				prop.ContentId = item.Id;
				prop.ContentNum = item.Qty;

				if (item.Id == (int)Items.Gil)
				{
					prop.MessageKey = "MSG_OTHER_12";
				}
				else
				{
					prop.MessageKey = "MSG_OTHER_11";
				}
			}
			else
			{
				InternalLogger.LogWarning($"Treasure Error: Treasure {prop.FlagId} wasn't assigned an item. Using default value.");
			}
		}
	}
}
