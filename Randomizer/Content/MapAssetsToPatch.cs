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
	public partial class Randomizer
	{
		public static Dictionary<string, PatchAsset> MapAssetsToPatch = new()
		{
			{ "Assets/GameAssets/Serial/Res/Map/Map_10010/Map_10010/tilemap", new(256, new() { MapPatchesWestward.TilemapGround, MapPatchesWestward.TilemapTiles, MapPatchesWestward.TilemapBottom, MapPatchesCanal.BridgeCanalBottom, MapPatchesNorthernDocks.TilemapGround, MapPatchesNorthernDocks.TilemapTiles, MapPatchesNorthernDocks.TilemapBottom } )},
			{ "Assets/GameAssets/Serial/Res/Map/Map_10010/Map_10010/transportation", new(256, new() { MapPatchesWestward.TransportationFoot, MapPatchesWestward.TransportationCanoe, MapPatchesCanal.TransportationFoot, MapPatchesCanal.TransportationCanalShip } )},
			{ "Assets/GameAssets/Serial/Res/Map/Map_10010/Map_10010/attribute", new(256, new() { MapPatchesWestward.Attributes, MapPatchesCanal.BridgeCanalAttribute, MapPatchesNorthernDocks.Attributes } )},
			{ "Assets/GameAssets/Serial/Res/Map/Map_30041/Map_30041_1/tilemap", new PatchAsset(34, new() { MapPatchesTitanTunnel.TilemapTiles } )},
			{ "Assets/GameAssets/Serial/Res/Map/Map_30041/Map_30041_1/collision", new PatchAsset(34, new() { MapPatchesTitanTunnel.Collision } )},
		};
	}
}
