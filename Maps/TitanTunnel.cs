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
	public static class MapPatchesTitanTunnel
	{
		public static PatchOpGroup TilemapTiles = new(2, new(), new()
			{
				new PatchOp(11,13,715),
			});
		public static PatchOpGroup Collision = new(1, new(), new()
			{
				new PatchOp(11,13,1),
			});
	}
}
