using MonoMod.Utils;
using RomUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF1PRAP
{

	partial class Randomizer
    {
		// 10 npcs
		public static List<int> PriorityNPCs = new()
		{
			(int)TreasureFlags.Princess,
			(int)TreasureFlags.Astos,
			(int)TreasureFlags.Matoya,
			(int)TreasureFlags.ElfPrince,
			(int)TreasureFlags.Sarda,
			(int)TreasureFlags.CanoeSage,
			(int)TreasureFlags.CubeBot,
			(int)TreasureFlags.Smitt,
			(int)TreasureFlags.Fairy,
			(int)TreasureFlags.Lefeinman,
		};

		// 7 chests
		public static List<int> PriorityChests = new()
		{
			(int)TreasureFlags.MarshChest,
			(int)TreasureFlags.ConeriaChest,
			(int)TreasureFlags.VampireChest,
			(int)TreasureFlags.EyeChest,
			(int)TreasureFlags.MouseChest,
			(int)TreasureFlags.MermaidsChest,
			(int)TreasureFlags.SkyChest,
		};

		// 44 chests
		public static List<int> TrappedChests = new()
		{
			// Marsh
			(int)TreasureFlags.MarshChest, 30, 31, 32,
			// NW Castle
			18, 19, 20,
			// Earth Cave
			43, 45, 46, 47,
			55, 57,
			58, 60, 61, 62, 63, 64, 65,
			// Ice Cave
			105,
			107, 110, 113,
			// Ordeals
			217, (int)TreasureFlags.MouseChest,
			// Volcano
			66, 67, 69, 71, 77, 79, 82,
			84, 85, 88, 89, 94, 95,
			98,
			// Sea
			120, 123, 124, 126
		};
		public static List<Items> goodItems = new()
		{
			Items.Elixir,
			Items.VorpalSword, Items.FlameSword, Items.IceBrand, Items.Defender, Items.SunBlade, Items.Excalibur,
			Items.SasukesBlade, Items.Masamune,
			Items.LightAxe, Items.ThorsHammer,
			Items.HealingStaff, Items.MagesStaff, Items.WizardsStaff,
			Items.WhiteRobe, Items.BlackRobe,
			Items.IceArmor, Items.FlameMail, Items.DiamondArmor, Items.DragonMail,
			Items.DiamondArmlet, Items.IceShield, Items.FlameMail, Items.DiamondShield, Items.AegisShield,
			Items.ProtectCloak, Items.Ribbon, Items.ProtectRing,
			Items.HealingHelm, Items.GiantsGloves
		};



		public static Dictionary<Items, AccessRequirements> ItemIdToAccess = new()
		{
			{ Items.Lute, AccessRequirements.Lute },
			{ Items.Crown, AccessRequirements.Crown },
			{ Items.CrystalEye, AccessRequirements.CrystalEye },
			{ Items.JoltTonic, AccessRequirements.JoltTonic },
			{ Items.MysticKey, AccessRequirements.MysticKey },
			{ Items.NitroPowder, AccessRequirements.NitroPowder },
			{ Items.StarRuby, AccessRequirements.StarRuby },
			{ Items.EarthRod, AccessRequirements.EarthRod },
			{ Items.Canoe, AccessRequirements.Canoe },
			{ Items.Levistone, AccessRequirements.Levistone },
			{ Items.WarpCube, AccessRequirements.WarpCube },
			{ Items.Oxyale, AccessRequirements.Oxyale },
			{ Items.RosettaStone, AccessRequirements.RosettaStone },
			{ Items.Bell, AccessRequirements.Bell },
			{ Items.Adamantite, AccessRequirements.Adamantite },
			{ Items.BottledFaerie, AccessRequirements.BottledFaerie },
			{ Items.Ship, AccessRequirements.Ship },
		};
		private static List<Items> keyItems = new() { Items.Lute, Items.Ship, Items.Crown, Items.CrystalEye, Items.JoltTonic, Items.MysticKey, Items.NitroPowder, Items.StarRuby, Items.EarthRod, Items.Canoe, Items.RatsTail, Items.Levistone, Items.Oxyale, Items.RosettaStone, Items.Bell, Items.WarpCube, Items.Adamantite };
		private static List<Items> progItems = new() { Items.Ship, Items.NitroPowder, Items.Canoe, Items.Levistone };
		private static List<int> chestLocations = new() { (int)TreasureFlags.MarshChest, (int)TreasureFlags.VampireChest, (int)TreasureFlags.ConeriaChest, (int)TreasureFlags.EyeChest, (int)TreasureFlags.MouseChest, (int)TreasureFlags.MermaidsChest, (int)TreasureFlags.SkyChest };
		private static List<int> npcLocations = new() { (int)TreasureFlags.Princess, (int)TreasureFlags.Bikke, (int)TreasureFlags.Astos, (int)TreasureFlags.ElfPrince, (int)TreasureFlags.Matoya, (int)TreasureFlags.Sarda, (int)TreasureFlags.CanoeSage, (int)TreasureFlags.CubeBot, (int)TreasureFlags.Fairy, (int)TreasureFlags.Lefeinman, (int)TreasureFlags.Smitt };
		private static List<Items> plandoItems = new() { Items.Ship };


		private static bool PrioritizeChests = true;
		private static bool PrioritizeNPCs = true;
		public static Dictionary<int, ItemData> ItemPlacement(MT19337 rng)
		{

			List<ItemData> items = FixedLocations.Where(l => l.Type != LocationType.Event).Select(l => new ItemData() { Id = l.Content, Qty = l.Qty }).ToList();


			
			var standardItems = items.Where(i => !keyItems.Contains((Items)i.Id)).ToList();
			var adamantitecraft = FF1PR.SessionManager.GetGlobal<int>("adamantite_craft");
			List<Items> extraItems = new();

			if (adamantitecraft == -1)
			{
				extraItems.Add(rng.PickFrom(goodItems));
			}
			else
			{
				extraItems.Add((Items)adamantitecraft);
			}

			foreach (var extraItem in extraItems)
			{
				var item = standardItems.Find(i => i.Id == (int)extraItem);
				standardItems.Remove(item);
			}

			// priority locations
			List<(string setting, List<int> locations)> prioritySetitngs = new()
			{
				(FF1PR.SessionManager.GetGlobal<string>("npcs_priority"), PriorityNPCs),
				(FF1PR.SessionManager.GetGlobal<string>("keychests_priority"), PriorityChests),
				(FF1PR.SessionManager.GetGlobal<string>("trapped_priority"), TrappedChests),
			};

			List<int> priorizedLocations = new();
			List<int> excludedLocations = new();

			foreach (var setting in prioritySetitngs)
			{
				if (setting.setting == "prioritize") priorizedLocations.AddRange(setting.locations);
				else if (setting.setting == "exclude") excludedLocations.AddRange(setting.locations);
			}


			// build logic
			List<LocationData> allLocations = new(FixedLocations);
			List<LocationData> updatedLocations = new();

			foreach (var location in allLocations)
			{
				List<List<AccessRequirements>> adjustedReqs = new();
				InternalLogger.LogInfo($"SanityCheck - Region: {location.Flag} - {location.Region}");
				var regionAccess = FixedRegions.Find(r => r.Region == location.Region).Access;
				var locationAccess = location.Access;

				if (regionAccess.Count == 0)
				{
					regionAccess.Add(new List<AccessRequirements>());
				}

				if (locationAccess.Count == 0)
				{
					locationAccess.Add(new List<AccessRequirements>());
				}


				foreach (var raccess in regionAccess)
				{
					foreach (var laccess in locationAccess)
					{
						adjustedReqs.Add(raccess.Concat(laccess).ToList());
					}
				}

				updatedLocations.Add(new LocationData() { Flag = location.Flag, Type = location.Type, Access = adjustedReqs, Trigger = location.Trigger });

				InternalLogger.LogInfo($"SanityCheck - Location: {location.Flag}");
				foreach (var reqs in adjustedReqs)
				{
					InternalLogger.LogInfo($"SanityCheck - Reqs: {String.Join(", ", reqs)}");
				}
			}

			InternalLogger.LogInfo($"SanityCheck - AllItems: {items.Count}, KeyItems: {keyItems.Count}, StandardItems: {standardItems.Count}, Initial Locations: {allLocations.Count(l => l.Type != LocationType.Event)}, Adjusted Locations: {updatedLocations.Count(l => l.Type != LocationType.Event)}");

			bool goodPlacement = false;
			List<LocationData> remainingLocations = new();
			Dictionary<int, ItemData> placedItems = new();

			while (!goodPlacement)
			{
				int progPlaced = 0;
				int progCounter = 1;

				List<int> priorityLocations = new(priorizedLocations);
				List<int> excludeLocations = new(excludedLocations);
				//if (PrioritizeNPCs) priorityLocations.AddRange(npcLocations);
				//if (PrioritizeChests) priorityLocations.AddRange(chestLocations);

				List<Items> itemsToPlace = new(keyItems.Concat(extraItems));
				List<Items> progItemsToPlace = new(progItems);

				int priorityItemsCount = Math.Min(priorityLocations.Count, itemsToPlace.Except(plandoItems).ToList().Count);
				int looseItemsCount = Math.Max(0, itemsToPlace.Except(plandoItems).ToList().Count - priorityLocations.Count);


				InternalLogger.LogInfo($"ItemCount - Priority: {priorityItemsCount} - Loose: {looseItemsCount} - Total: {keyItems.Except(plandoItems).ToList().Count}");
				bool softlock = false;

				placedItems = new();
				List<AccessRequirements> access = new();

				List<LocationData> unaccessibleLocations = new(updatedLocations);
				List<LocationData> accessibleLocations = new();
				List<LocationData> placedLocations = new();

				itemsToPlace.Shuffle(rng);

				ProcessRequirements(AccessRequirements.None, access, unaccessibleLocations, accessibleLocations);

				while (itemsToPlace.Any())
				{
					int diceRoll = rng.Between(1, itemsToPlace.Count);
					var priorityAccessLocations = accessibleLocations.Where(l => priorityLocations.Contains(l.Flag)).ToList();
					var looseAccessLocations = accessibleLocations.Where(l => !priorityLocations.Contains(l.Flag) && !excludedLocations.Contains(l.Flag)).ToList();
					List<LocationData> validLocations = new();

					if (itemsToPlace.Contains(Items.Ship))
					{
						validLocations = accessibleLocations.Where(l => l.Flag == (int)TreasureFlags.Bikke).ToList();
					}
					else if ((priorityAccessLocations.Any() && looseAccessLocations.Any() && diceRoll <= priorityItemsCount) ||
							(priorityAccessLocations.Any() && !looseAccessLocations.Any()))
					{
						InternalLogger.LogInfo($"Sanity Checker - Placing at priority Locations: PriorityLoc: {priorityAccessLocations.Count}  - LooseLocations: {looseAccessLocations.Count}- Priority Item Count: {priorityItemsCount} - Losse Item Count {looseItemsCount} - Dice Roll {diceRoll}");
						
						validLocations = priorityAccessLocations;
						priorityItemsCount--;
					}
					else if (looseItemsCount > 0)
					{
						InternalLogger.LogInfo($"Sanity Checker - Placing at loose Locations: PriorityLoc: {priorityAccessLocations.Count}  - LooseLocations: {looseAccessLocations.Count}- Priority Item Count: {priorityItemsCount} -  Losse Item Count {looseItemsCount} - Dice Roll {diceRoll}");

						validLocations = looseAccessLocations;
						looseItemsCount--;
					}

					Items itemToPlace;

					if (itemsToPlace.Contains(Items.Ship))
					{
						itemToPlace = Items.Ship;
					}
					else if ((progCounter <= 0 && progItemsToPlace.Any()) || (validLocations.Count == 1 && progItemsToPlace.Any()))
					{
						itemToPlace = progItemsToPlace.First();
					}
					else
					{
						itemToPlace = itemsToPlace.First();
						progCounter--;
					}

					if (progItemsToPlace.Contains(itemToPlace))
					{
						progPlaced++;
						progCounter = progPlaced + 1;
					}

					progItemsToPlace.Remove(itemToPlace);
					itemsToPlace.Remove(itemToPlace);

					if (!validLocations.Any())
					{
						softlock = true;
						InternalLogger.LogInfo($"SanityCheck - Softlocked.");
						foreach (var item in itemsToPlace)
						{
							InternalLogger.LogInfo($"SanityCheck - Items Left: {item}");
						}

						foreach (var loc in unaccessibleLocations)
						{
							InternalLogger.LogInfo($"SanityCheck - Unaccessible Locations: {loc.Flag}");
						}
						break;
					}

					var location = rng.PickFrom(validLocations);
					
					var removal = accessibleLocations.Remove(location);
					InternalLogger.LogInfo($"SanityCheck - Location {location.Flag} remove? {removal}");

					placedItems.Add(location.Flag, new ItemData() { Id = (int)itemToPlace, Qty = 1 });

					if (ItemIdToAccess.TryGetValue(itemToPlace, out var newaccess))
					{
						ProcessRequirements(newaccess, access, unaccessibleLocations, accessibleLocations);
					}
				}

				foreach (var location in unaccessibleLocations)
				{
					InternalLogger.LogInfo($"SanityCheck - Unreached Location: {location.Flag}");
				}

				if (softlock)
				{
					continue;
				}
				else
				{
					goodPlacement = true;
					//PlacedItems = placedItems;
					remainingLocations = accessibleLocations;
				}
			}


			// keylocationweight
			// favoredopeners

			InternalLogger.LogInfo($"SanityCheck - Item Left: {standardItems.Count}, Location Left: {remainingLocations.Count}");

			standardItems.Shuffle(rng);
			var remainingItems = remainingLocations.Select((l, i) => (l.Flag, standardItems[i])).ToDictionary(y => y.Flag, y => y.Item2);
			placedItems.AddRange(remainingItems);

			InternalLogger.LogInfo($"-- SanityCheck - Full Spoiler --");
			foreach (var item in placedItems)
			{
				InternalLogger.LogInfo($"Location Flag: {item.Key} - {(Items)item.Value.Id}");
			}

			return placedItems;
		}
		private static void ProcessRequirements(AccessRequirements newaccess, List<AccessRequirements> currentaccess, List<LocationData> unaccessibleLocations, List<LocationData> accessiblesLocations)
		{

			List<AccessRequirements> accessToProcess = new() { newaccess };
			while (accessToProcess.Any())
			{
				currentaccess.Add(accessToProcess.First());
				InternalLogger.LogInfo($"SanityCheck - Access To Process: {accessToProcess.First()}");
				accessToProcess.RemoveAt(0);
				List<LocationData> locationToRemove = new();

				foreach (var location in unaccessibleLocations)
				{
					bool accessible = false;

					foreach (var acccereqs in location.Access)
					{
						if (!acccereqs.Except(currentaccess).Any())
						{
							accessible = true;
							InternalLogger.LogInfo($"SanityCheck - Location Became Accessible: {location.Flag} - {location.Name}");
							break;
						}
					}

					if (accessible)
					{
						if (location.Type == LocationType.Event)
						{
							accessToProcess.AddRange(location.Trigger);
						}
						else
						{
							accessiblesLocations.Add(location);
						}
						locationToRemove.Add(location);
					}
				}

				unaccessibleLocations.RemoveAll(l => locationToRemove.Contains(l));

				//unaccessibleLocations = unaccessibleLocations.Except(locationToRemove).ToList();
				InternalLogger.LogInfo($"SanityCheck - Unaccessible Locations Count: {unaccessibleLocations.Count}");
				InternalLogger.LogInfo($"SanityCheck - Accessible Locations Count: {accessiblesLocations.Count}");
			}
		}


	}

}
