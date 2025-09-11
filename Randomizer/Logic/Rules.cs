using Last.Data.Master;
using Last.Interpreter;
using RomUtilities;
using Serial.Template.Management;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Bindings;
using static FF1PRAP.Randomizer;
using static UnityEngine.UI.Image;

namespace FF1PRAP
{
	public class LogicData
	{
		public Dictionary<string, string> Entrances = new();
		public List<Location> Locations = new();

		public LogicData(Dictionary<string, string> entrances, List<Location> locations)
		{
			Entrances = entrances;
			Locations = locations;
		}
	}
	partial class Logic
	{
		public static Dictionary<string, Dictionary<string, string>> Regions = new(); // region name > (target region > entrance name)
		public static Dictionary<string, string> Entrances = new(); // entrance name > target region name
		public static Dictionary<string, List<List<AccessRequirements>>> EntranceAccess = new(); // entrance name > access list
		public static Dictionary<string, List<List<AccessRequirements>>> CrawledRegions = new();
		public static List<Location> CrawledLocations = new();

		public static LogicData BuildLogic(bool shuffle_overworld, ShuffleEntrancesMode shuffle_entrances, ShuffleTownsMode shuffle_towns, EarlyProgressionModes early_progression, bool northerndocks, MT19337 rng)
		{
			Init();
			CreateRegions();

			ShuffleEntrances(shuffle_overworld, shuffle_entrances, shuffle_towns, early_progression, rng);

			ConnectEntrancesToRegions();

			SetEntranceAccess(early_progression == EarlyProgressionModes.MarshPath, northerndocks);
			CrawlRegions();

			return new LogicData(result_entrances, CrawledLocations);
		}

		public static void Init()
		{
			Regions = new();
			Entrances = new();
			EntranceAccess = new();
			CrawledRegions = new();
			CrawledLocations = new();
		}
		public static void CreateRegions()
		{
			Regions.Add("Overworld", new());
			foreach (var region in overworld_regions)
			{
				Regions.Add(region, new());
				Regions["Overworld"].Add(region, "Overworld -> " + region);
				Entrances.Add("Overworld -> " + region, region);
				EntranceAccess.Add("Overworld -> " + region, new());
			}

			foreach (var region in location_regions)
			{
				Regions.Add(region, new());
			}
		}
		private static void ConnectEntrancesToRegions()
		{
			foreach (var region in region_dict)
			{
				Regions[region.Key] = region.Value;
			}
		}
		public static void SetEntranceAccess(bool marshpath, bool northerndocks)
		{

			foreach (var accessrules in default_overworld_entrances_rules)
			{
				EntranceAccess[accessrules.Key] = accessrules.Value.ToList();
			}

			if (marshpath)
			{
				foreach (var accessrules in marshpath_overworld_entrances_rules)
				{
					EntranceAccess[accessrules.Key] = accessrules.Value.ToList();
				}
			}

			if (northerndocks)
			{
				foreach (var accessrules in northerndocks_overworld_entrances_rules)
				{
					EntranceAccess[accessrules.Key] = accessrules.Value.ToList();
				}
			}

			foreach (var accessrules in entrances_rules)
			{
				EntranceAccess[accessrules.Key] = accessrules.Value.ToList();
			}
		}

		public static void CrawlRegions()
		{
			CrawlRegion(RegionNames.overworld, new(), new());
			foreach (var location in FixedLocations)
			{
				List<List<AccessRequirements>> newreqs = new();

				if (CrawledRegions[location.Region].Count > 0)
				{
					foreach (var access in CrawledRegions[location.Region])
					{
						newreqs.Add(access.Concat(location.BaseAccess).ToList());
					}
				}
				else
				{
					newreqs.Add(location.BaseAccess.ToList());
				}

				CrawledLocations.Add(new Location(location, newreqs));
			}
		}

		public static void CrawlRegion(string origin, List<string> visitedRegions, List<List<AccessRequirements>> accessReqs)
		{
			InternalLogger.LogTesting($"Crawling; {origin}");
			if (CrawledRegions.ContainsKey(origin))
			{
				CrawledRegions[origin] = CrawledRegions[origin].Concat(accessReqs).ToList();
			}
			else
			{
				CrawledRegions[origin] = accessReqs.ToList();
			}

			foreach (var exit in Regions[origin])
			{
				if (!visitedRegions.Contains(exit.Key))
				{
					List<List<AccessRequirements>> newreqs = new();

					if (EntranceAccess.ContainsKey(exit.Value))
					{
						foreach (var destinationreqs in EntranceAccess[exit.Value])
						{
							if (accessReqs.Count > 0)
							{
								foreach (var originreqs in accessReqs)
								{
									newreqs.Add(destinationreqs.Concat(originreqs).Distinct().ToList());
								}
							}
							else
							{
								newreqs.Add(destinationreqs);
							}
						}
					}
					else
					{
						newreqs = accessReqs;
					}

					CrawlRegion(exit.Key, visitedRegions.Append(exit.Key).ToList(), newreqs);
				}
			}
		}
	}
}
