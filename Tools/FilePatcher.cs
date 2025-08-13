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
//using Il2CppSystem.Collections.Generic;

namespace FF1PRAP
{

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
		public int MapX;
		public int MapY;

		public PatchOpGroup(int dataid, int x, int y, List<PatchOp> operations)
		{
			DataId = dataid;
			MapX = x;
			MapY = y;
			Operations = operations;
		}
	}

	public static class MapPatcher
	{
		public static string Patch(string mapfile, List<PatchOpGroup> opgroups)
		{
			//InternalLogger.LogInfo(mapfile);

			InternalLogger.LogInfo("---MapPatcher---");

			int dataindex = mapfile.IndexOf("data", StringComparison.InvariantCultureIgnoreCase);

			//InternalLogger.LogInfo($"{dataindex}, {testindex}, {vindex}");
			int currentgroup = 0;

			// Order to be safe
			opgroups = opgroups.OrderBy(g => g.DataId).ToList();

			foreach (var group in opgroups)
			{
				for (int i = currentgroup; i < group.DataId; i++)
				{
					dataindex = mapfile.IndexOf("data", dataindex + 1);
				}

				currentgroup = group.DataId;

				InternalLogger.LogInfo($"Processing DataId {group.DataId} at {dataindex}");

				int currentoffset = dataindex;
				int currentposition = 0;

				// Order to be safe
				group.Operations = group.Operations.OrderBy(o => o.Position(group.MapX)).ToList();

				foreach (var op in group.Operations)
				{
					for (int i = currentposition; i < op.Position(group.MapX); i++)
					{
						currentoffset = mapfile.IndexOf(",", currentoffset + 1);
					}

					var first = currentoffset;
					var last = mapfile.IndexOf(",", first + 1) - 1;


					mapfile = mapfile.Remove(first + 1, last - first);
					mapfile = mapfile.Insert(first + 1, $"{op.Value}");
					currentposition = op.Position(group.MapX);

					InternalLogger.LogInfo($"Insert {op.Value} at ({op.X},{op.Y}) > Offset: {currentoffset}");
				}
			}

			return mapfile;
		}
	}

	public static class MapPatches
	{
		public static List<PatchOpGroup> TilemapWestward = new()
		{
			new PatchOpGroup(0, 256, 256, new()
			{
				// Mountain to Marsh
				new PatchOp(118,129,644),
				new PatchOp(119,129,901),
				new PatchOp(120,129,966),
				new PatchOp(121,129,646),
				new PatchOp(123,129,644),
				new PatchOp(124,129,966),
				new PatchOp(125,129,965),

				new PatchOp(118,130,772),
				new PatchOp(119,130,965),
				new PatchOp(120,130,965),
				new PatchOp(121,130,901),
				new PatchOp(122,130,901),
				new PatchOp(123,130,966),
				new PatchOp(124,130,773),
				new PatchOp(125,130,902),

				new PatchOp(118,131,775),
				new PatchOp(119,131,901),
				new PatchOp(120,131,966),
				new PatchOp(121,131,966),
				new PatchOp(122,131,901),
				new PatchOp(123,131,902),
				new PatchOp(124,131,966),
				new PatchOp(125,131,966),
				new PatchOp(126,131,966),
				new PatchOp(127,131,965),

				new PatchOp(118,132,898),
				new PatchOp(119,132,902),
				new PatchOp(120,132,902),
				new PatchOp(121,132,966),
				new PatchOp(122,132,901),
				new PatchOp(123,132,965),
				new PatchOp(124,132,902),
				new PatchOp(125,132,966),
				new PatchOp(126,132,901),
				new PatchOp(127,132,773),

				new PatchOp(119,133,965),
				new PatchOp(120,133,965),
				new PatchOp(121,133,902),
				new PatchOp(122,133,966),
				new PatchOp(123,133,966),
				new PatchOp(124,133,773),
				new PatchOp(125,133,965),
				new PatchOp(126,133,901),
				new PatchOp(127,133,901),
				new PatchOp(128,133,965),

				new PatchOp(119,134,965),
				new PatchOp(120,134,966),
				new PatchOp(121,134,902),
				new PatchOp(122,134,902),
				new PatchOp(123,134,966),
				new PatchOp(124,134,965),
				new PatchOp(125,134,774),
				new PatchOp(126,134,772),
				new PatchOp(127,134,966),
				new PatchOp(128,134,774),

				new PatchOp(124,135,966),

				// River after canal
				new PatchOp(96,162,961),
				new PatchOp(97,162,962),
				new PatchOp(99,162,0),

				new PatchOp(96,163,1863),
				new PatchOp(97,163,962),
				new PatchOp(98,163,961),
				new PatchOp(99,163,0),

				new PatchOp(97,164,961),

				// Bridge
				new PatchOp(144,144,72),
				new PatchOp(145,144,74),
				new PatchOp(146,144,76),
				new PatchOp(147,144,0),

				new PatchOp(144,145,210),
				new PatchOp(145,145,215),
				new PatchOp(146,145,210),
				new PatchOp(147,145,0),

				new PatchOp(144,146,962),
				new PatchOp(145,146,961),
				new PatchOp(146,146,961),

				// Canal Bridge
				new PatchOp(95,156,1410),

				//new PatchOp(95,157,0),
				new PatchOp(95,157,1699),

				new PatchOp(95,158,1538),
			}),
			new PatchOpGroup(1, 256, 256, new()
			{
				// Mountain to Marsh
				new PatchOp(120,128,154),

				new PatchOp(118,129,660),
				new PatchOp(119,129,661),
				new PatchOp(120,129,661),
				new PatchOp(121,129,662),
				new PatchOp(122,129,154),
				new PatchOp(123,129,660),
				new PatchOp(124,129,661),
				new PatchOp(125,129,661),

				new PatchOp(118,130,790),
				new PatchOp(122,130,661),
				new PatchOp(124,130,1),
				new PatchOp(125,130,1),

				new PatchOp(118,131,791),
				new PatchOp(124,131,1),
				new PatchOp(125,131,1),
				new PatchOp(126,131,1),
				new PatchOp(127,131,1),

				new PatchOp(119,132,724),
				new PatchOp(126,132,1),
				new PatchOp(127,132,1),
				new PatchOp(128,132,1),

				new PatchOp(119,133,724),
				new PatchOp(127,133,1),
				new PatchOp(128,133,726),

				new PatchOp(119,134,1),
				new PatchOp(120,134,789),
				new PatchOp(121,134,789),
				new PatchOp(122,134,789),
				new PatchOp(123,134,789),
				new PatchOp(125,134,790),
				new PatchOp(126,134,788),
				new PatchOp(127,134,789),
				new PatchOp(128,134,790),

				new PatchOp(124,135,663),

				// River
				new PatchOp(96,162,1),
				new PatchOp(97,162,1),

				new PatchOp(96,163,1879),
				new PatchOp(98,163,1),
				new PatchOp(99,163,160),

				new PatchOp(97,164,1),

				// Bridge
				new PatchOp(144,144,88),
				new PatchOp(145,144,90),
				new PatchOp(146,144,92),
				new PatchOp(147,144,93),

				new PatchOp(144,145,0),
				new PatchOp(145,145,0),
				new PatchOp(146,145,0),
				new PatchOp(147,145,0),

				new PatchOp(144,146,0),
				new PatchOp(145,146,0),
				new PatchOp(146,146,0),

				// Canal Bridge
				new PatchOp(95,156,1635),

				new PatchOp(95,157,1699),

				new PatchOp(95,158,1766),
			}),

			new PatchOpGroup(2, 256, 256, new()
			{
				// Mountain to Marsh
				new PatchOp(98,162,0),
				new PatchOp(97,163,0),

				// Canal Bridge
				new PatchOp(95,156,1635),

				new PatchOp(95,157,1699),

				new PatchOp(95,158,1766),
			}),
		};

		public static List<PatchOpGroup> TransportationWestward = new()
		{
			// On Foot
			new PatchOpGroup(0, 256, 256, new()
			{
				// Mountain to Marsh
				new PatchOp(120,128,0),
				new PatchOp(118,129,0),
				new PatchOp(119,129,0),
				new PatchOp(120,129,0),
				new PatchOp(121,129,0),
				new PatchOp(122,129,0),
				new PatchOp(123,129,0),
				new PatchOp(124,129,0),

				new PatchOp(118,130,0),
				new PatchOp(119,130,0),
				new PatchOp(120,130,0),
				new PatchOp(121,130,0),
				new PatchOp(122,130,0),
				new PatchOp(123,130,0),
				new PatchOp(124,130,0),

				new PatchOp(118,131,0),
				new PatchOp(119,131,0),
				new PatchOp(120,131,0),
				new PatchOp(121,131,0),
				new PatchOp(122,131,0),
				new PatchOp(123,131,0),
				new PatchOp(124,131,0),
				new PatchOp(125,131,0),

				new PatchOp(119,132,0),
				new PatchOp(120,132,0),
				new PatchOp(121,132,0),
				new PatchOp(122,132,0),
				new PatchOp(123,132,0),
				new PatchOp(124,132,0),
				new PatchOp(125,132,0),
				new PatchOp(126,132,0),
				new PatchOp(127,132,0),

				new PatchOp(119,133,0),
				new PatchOp(120,133,0),
				new PatchOp(121,133,0),
				new PatchOp(122,133,0),
				new PatchOp(123,133,0),
				new PatchOp(124,133,0),
				new PatchOp(125,133,0),
				new PatchOp(126,133,0),
				new PatchOp(127,133,0),
				new PatchOp(128,133,0),

				new PatchOp(119,134,0),
				new PatchOp(120,134,0),
				new PatchOp(121,134,0),
				new PatchOp(122,134,0),
				new PatchOp(123,134,0),
				new PatchOp(124,134,0),
				new PatchOp(125,134,0),
				new PatchOp(126,134,0),
				new PatchOp(127,134,0),
				new PatchOp(128,134,0),

				new PatchOp(124,135,0),

				// River
				new PatchOp(96,163,0),
				new PatchOp(97,163,0),
				new PatchOp(98,163,0),

				// Bridge
				new PatchOp(145,145,1),
			}),
			
			// Canoe
			new PatchOpGroup(1, 256, 256, new()
			{
				// Mountain to Marsh
				new PatchOp(96,163,1),
				new PatchOp(97,163,1),
				new PatchOp(98,163,1),
			}),
		};
		public static List<PatchOpGroup> AttributeWestward = new()
		{
			// Attributes
			new PatchOpGroup(0, 256, 256, new()
			{
				new PatchOp(95,157,18),

				// River
				new PatchOp(96,162,4),
				new PatchOp(97,162,4),
				new PatchOp(96,163,4),
				new PatchOp(97,163,4),
				new PatchOp(98,163,4),
				new PatchOp(97,164,4),
				new PatchOp(98,164,4),

				

			}),
		};
		public static string TestMap = "zzzzzzzzzzzzzzzzzzzzzzdata: [0,0,0,0,12,2,1,0,4,4,8,1,0,0,555,9]";
	}
}
