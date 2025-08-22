using Il2CppSystem.Threading.Tasks;
using Last.Data.Master;
using Last.Entity.Field;
using Last.Management;
using Last.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

namespace FF1PRAP
{
	partial class Patches
	{
		public static void CheckGroupLoadAssetCompleted_Post(ref bool __result, string addressName)
		{
			//InternalLogger.LogInfo($"Loading Asset: {addressName}");

			if(Monitor.instance != null) Monitor.instance.CheckForMap(addressName);

			if (assetsToReplace.TryGetValue(addressName, out var assetfilename))
			{
				var assetfile = GetFile(assetfilename);
				var textasset = new TextAsset(UnityEngine.TextAsset.CreateOptions.CreateNativeObject, assetfile);
				var assetname = addressName.Split('/').Last();

				FF1PR.ResourceManager.completeAssetDic[addressName] = textasset;
				InternalLogger.LogInfo($"Asset loading task added for {assetname} > {assetfilename}");
				__result = true;
			};
		}

		public static Dictionary<string, string> assetsToReplace = new()
		{
			{ "Assets/GameAssets/Serial/Res/Map/Map_10010/Map_10010/entity_default", "ev_overworld_entity" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_10010/Map_10010/ev_e_0007", "ev_overworld_west" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_10010/Map_10010/ev_e_0025", "ev_overworld_east" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20011/Map_20011_1/ev_e_0014", "ev_coneriacastle_1" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20011/Map_20011_2/ev_e_0004", "ev_coneriacastle_from_teleport" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30011/Map_30011_1/ev_e_0014", "ev_templeoffiends" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30011/Map_30011_1/ev_e_0038", "ev_templeoffiends_blackorb" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30011/Map_30011_1/ev_e_0039", "ev_templeoffiends_beyond" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30021/Map_30021_3/ev_e_0014", "ev_marsh_bottom" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20081/Map_20081_1/ev_e_0010", "ev_nwcastle" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20031/Map_20021_1/ev_e_0012", "ev_matoyascave" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20071/Map_20071_1/ev_e_0014", "ev_elflandcastle" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20051/Map_20051_1/ev_e_0014", "ev_dwarf_cave" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30031/Map_30041_3/ev_e_0016", "ev_earth_b3" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30041/Map_30041_1/ev_e_0018", "ev_titan" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30071/Map_30071_1/ev_e_0046", "ev_ordeals" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30071/Map_30071_2/entity_default", "ev_ordealsmaze" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20121/Map_20121_3/entity_default", "ev_bahamut" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20090/Map_20090/ev_e_0034", "ev_melmond" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30101/Map_30101_3/ev_e_0027", "ev_cubewarp" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30121/Map_30121_3/ev_e_0039", "ev_lute_floor" },
		};
	}
}
