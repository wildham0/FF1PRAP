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
			if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
			{
				if (FF1PR.DataStorage.Get(Last.Interpreter.DataStorage.Category.kTreasureFlag1, prop.FlagId) == 1)
				{
					return;
				}
				
				prop.ContentId = 43;
				prop.ContentNum = 0;
				prop.MessageKey = "MSG_OTHER_11";

				var location = Randomizer.ApLocations[prop.FlagId];

				FF1PR.MessageManager.GetMessageDictionary()["MSG_OTHER_11"] = $"You obtained {location.Content}.";
				Archipelago.instance.ActivateCheck(Randomizer.FlagToLocationName[prop.FlagId]);
			}
			else if (FF1PR.PlacedItems.TryGetValue(prop.FlagId, out var item))
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
				InternalLogger.LogInfo($"Treasure Error: Treasure {prop.FlagId} wasn't assigned an item. Using default value.");
			}
		}
	}
}
