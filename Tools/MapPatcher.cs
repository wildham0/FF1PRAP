using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SocialPlatforms;
using System.Xml.Linq;
using System.Net.NetworkInformation;
using Last.Data.Master;
using Last.Interpreter;
using static FF1PRAP.Patches;
//using Il2CppSystem.Collections.Generic;

namespace FF1PRAP
{
	public enum ConditionState
	{
		Always,
		On,
		Off
	}
	public enum FlagMode
	{
		Randoflag,
		Gameflag,
	}
	public class RandoCondition
	{
		private static Dictionary<string, DataStorage.Category> gameflagCategory = new()
		{
			{ "ScenarioFlag1", DataStorage.Category.kScenarioFlag1 },
			{ "ScenarioFlag2", DataStorage.Category.kScenarioFlag2 },
			{ "ScenarioFlag3", DataStorage.Category.kScenarioFlag3 },
			{ "ScenarioFlag4", DataStorage.Category.kScenarioFlag4 },
			{ "TreasureFlag1", DataStorage.Category.kTreasureFlag1 },
			{ "TreasureFlag2", DataStorage.Category.kTreasureFlag2 },
			{ "TreasureFlag3", DataStorage.Category.kTreasureFlag3 },
			{ "TreasureFlag4", DataStorage.Category.kTreasureFlag4 },
		};
		public ConditionState State { get; set; }
		public string FlagGroup { get; set; }
		public string Value { get; set; }
		public FlagMode Mode { get; set; }

		public RandoCondition(ConditionState state, string flagGroup, string value, FlagMode mode)
		{
			State = state;
			FlagGroup = flagGroup;
			Value = value;
			Mode = mode;
		}
		public bool Resolve()
		{
			bool result = false;
			if (Mode == FlagMode.Randoflag)
			{
				result = (FF1PR.SessionManager.Options[FlagGroup] == Value);
			}
			else if (Mode == FlagMode.Gameflag)
			{
				if (FF1PR.DataStorage != null)
				{
					result = FF1PR.DataStorage.Get(gameflagCategory[FlagGroup], int.Parse(Value)) == 1;
				}
			}

			return (State == ConditionState.On && result) || (State == ConditionState.Off && !result);
		}
	}


	public class PatchOp
	{
		public int X;
		public int Y;
		public int Value;
		public PatchOp(int x, int y, int value)
		{
			X = x;
			Y = y;
			Value = value;
		}

		public int Position(int mapx)
		{
			return X + mapx * Y;
		}
	}
	public class PatchOpGroup
	{
		public int DataId;
		public List<PatchOp> Operations;
		//public Dictionary<int, List<PatchOp>> Operations;
		public int MapX;
		public int MapY;
		public List<RandoCondition> Conditions;
		public PatchOpGroup(int x, int dataid, List<RandoCondition> conditions, List<PatchOp> operations)
		{
			DataId = dataid;
			MapX = x;
			Operations = operations;
			Conditions = conditions;
		}

		public bool Resolve()
		{
			bool trigger = true;
			foreach (var condition in Conditions)
			{
				if (!condition.Resolve())
				{
					trigger = false;
					break;
				}
			}

			return trigger;
		}
	}

	public static class MapPatcher
	{
		public static string Patch(string mapfile, List<PatchOpGroup> opgroups, int mapx)
		{
			//InternalLogger.LogInfo(mapfile);

			InternalLogger.LogInfo("---MapPatcher---");

			int dataindex = mapfile.IndexOf("data", StringComparison.InvariantCultureIgnoreCase);


			Dictionary<int, List<PatchOp>> operationsGroups = new();
			foreach (var opgroup in opgroups)
			{
				if (opgroup.Resolve())
				{
					if (!operationsGroups.ContainsKey(opgroup.DataId))
					{
						operationsGroups[opgroup.DataId] = new(opgroup.Operations);
					}
					else
					{
						operationsGroups[opgroup.DataId].AddRange(opgroup.Operations);
					}
					
				}
			}

			operationsGroups = operationsGroups.OrderBy(o => o.Key).ToDictionary(o => o.Key, o => o.Value);


			//InternalLogger.LogInfo($"{dataindex}, {testindex}, {vindex}");
			int currentgroup = 0;

			// Order to be safe
			//opgroups = opgroups.OrderBy(g => g.DataId).ToList();

			foreach (var datagroup in operationsGroups)
			{
				for (int i = currentgroup; i < datagroup.Key; i++)
				{
					dataindex = mapfile.IndexOf("data", dataindex + 1);
				}

				currentgroup = datagroup.Key;

				InternalLogger.LogInfo($"Processing DataId {datagroup.Key} at {dataindex}");

				int currentoffset = dataindex;
				int currentposition = 0;

				// Order to be safe
				var operations = datagroup.Value.OrderBy(o => o.Position(mapx)).ToList();

				foreach (var op in operations)
				{
					for (int i = currentposition; i < op.Position(mapx); i++)
					{
						currentoffset = mapfile.IndexOf(",", currentoffset + 1);
					}

					var first = currentoffset;
					var last = mapfile.IndexOf(",", first + 1) - 1;


					mapfile = mapfile.Remove(first + 1, last - first);
					mapfile = mapfile.Insert(first + 1, $"{op.Value}");
					currentposition = op.Position(mapx);

					InternalLogger.LogInfo($"Insert {op.Value} at ({op.X},{op.Y}) > Offset: {currentoffset}");
				}
			}

			return mapfile;
		}
	}
}
