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
using static UnityEngine.GridBrushBase;

namespace FF1PRAP
{
	partial class Patches
	{
		public static void CheckCompleteAsset_Post(ref bool __result, string addressName)
		{
			//InternalLogger.LogInfo($"Loading Asset: {addressName}");

			if (Randomizer.AssetsToReplace.TryGetValue(addressName, out var assetfilename))
			{
				var extension = (assetfilename.Split(".").Count() > 1) ? assetfilename.Split(".")[1] : "json";
				var assetfile = GetFile(assetfilename, extension);
				var textasset = new TextAsset(UnityEngine.TextAsset.CreateOptions.CreateNativeObject, assetfile);
				var assetname = addressName.Split('/').Last();

				GameData.ResourceManager.completeAssetDic[addressName] = textasset;
				InternalLogger.LogInfo($"Asset loading task added for {assetname} > {assetfilename}");
				__result = true;
			}
			else if (Monitor.instance != null && Randomizer.MapAssetsToPatch.ContainsKey(addressName))
			{
				Monitor.instance.AddPatchesToProcess(addressName);
			}
		}
	}
}
