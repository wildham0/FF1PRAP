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
		public static void AddLoadingTask_Pre(string addressName, ref ResourceLoadTask task)
		{
			if (assetsToReplace.TryGetValue(addressName, out var assetfilename))
			{
				var assetfile = GetFile(assetfilename);
				var textasset = new TextAsset(UnityEngine.TextAsset.CreateOptions.CreateNativeObject, assetfile);
				var assetname = addressName.Split('/').Last();
				textasset.name = assetname;
				Monitor.instance.SetAssetTask(addressName, textasset, task);
				InternalLogger.LogInfo($"Asset loading task added for {assetname} > {assetfilename}");
			}
		}

		public static Dictionary<string, string> assetsToReplace = new()
		{
			{ "Assets/GameAssets/Serial/Res/Map/Map_10010/Map_10010/ev_e_0025", "ev_overworld" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20011/Map_20011_1/ev_e_0014", "ev_coneriacastle_1" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30011/Map_30011_1/ev_e_0014", "ev_templeoffiends" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30011/Map_30011_1/ev_e_0038", "ev_templeoffiends_blackorb" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30011/Map_30011_1/ev_e_0039", "ev_templeoffiends_beyond" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30021/Map_30021_3/ev_e_0014", "ev_marsh_bottom" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20081/Map_20081_1/ev_e_0010", "ev_nwcastle" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20031/Map_20021_1/ev_e_0012", "ev_matoyascave" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20071/Map_20071_1/ev_e_0014", "ev_elflandcastle" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20051/Map_20051_1/ev_e_0014", "ev_dwarf_cave" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30031/Map_30041_3/ev_e_0016", "ev_earth_b3" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30071/Map_30071_1/ev_e_0046", "ev_ordeals" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20090/Map_20090/ev_e_0034", "ev_melmond" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30101/Map_30101_3/ev_e_0027", "ev_cubewarp" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30121/Map_30121_3/ev_e_0039", "ev_lute_floor" },
		};
		public static void GetAsset_Pre(ref string addressName)
		{
			InternalLogger.LogInfo($"Getting Asset: {addressName}");
			/*if (addressName == "Assets/GameAssets/Serial/Res/Map/Map_20051/Map_20051_1/ev_e_0014")
			{
				addressName = "void";

				var evfile = GetFile("ev_e_0014");
				InternalLogger.LogInfo($"Interception load task.");
				var evasset = new TextAsset(UnityEngine.TextAsset.CreateOptions.CreateNativeObject, evfile);
				evasset.name = "ev_e_0014";
				FF1PR.ResourceManager.completeAssetDic["Assets/GameAssets/Serial/Res/Map/Map_20051/Map_20051_1/ev_e_0014"] = evasset;
			}*/
		}

	}
}
