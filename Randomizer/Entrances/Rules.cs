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
using Serial.Template.Management;
using System.Collections.Specialized;

namespace FF1PRAP
{
	partial class Logic
	{
		public static Dictionary<string, Dictionary<string, string>> Regions = new(); // region name > (target region > entrance name)
		public static Dictionary<string, string> Entrances = new(); // entrance name > target region name
		public static Dictionary<string, List<List<AccessRequirements>>> EntranceAccess = new(); // entrance name > access list
		//public static List<string> Regions = new(); // region name

		public Dictionary<string, List<List<AccessRequirements>>> CrawledRegions = new();
		public List<Location> CrawledLocations = new();


		public void CreateRegions(bool marshpath, bool northerndocks)
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


			// Shuffle them?
			/*
			foreach (var location in FixedLocations)
			{
				Regions[location.Name].Locations.Add(location);
			}*/

			SetEntranceAccess(marshpath, northerndocks);
		}

		public void SetEntranceAccess(bool marshpath, bool northerndocks)
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

		public void CrawlRegions()
		{
			CrawlRegion(RegionNames.overworld, new(), new());
			foreach (var location in FixedLocations)
			{
				List<List<AccessRequirements>> newreqs = new();

				foreach (var access in CrawledRegions[location.Region])
				{
					newreqs.Add(access.Concat(location.BaseAccess).ToList());
				}

				CrawledLocations.Add(new Location(location, newreqs));
			}
		}

		public void CrawlRegion(string origin, List<string> visitedRegions, List<List<AccessRequirements>> accessReqs)
		{
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
					
					foreach (var destinationreqs in EntranceAccess[exit.Value])
					{
						foreach (var originreqs in accessReqs)
						{
							newreqs.Add(destinationreqs.Concat(originreqs).Distinct().ToList());
						}
					}

					CrawlRegion(exit.Key, visitedRegions.Append(exit.Key).ToList(), newreqs);
				}
			}
		}
	}
}
