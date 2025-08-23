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
	public static class MapPatchesCanal
	{
		public static PatchOpGroup BridgeCanalBottom = new(1, new()
			{
				//new RandoCondition(ConditionState.On, "ScenarioFlag1", $"{(int)ScenarioFlags.Canal}", FlagMode.Gameflag),
				//new RandoCondition(ConditionState.On, "ScenarioFlag1", $"{(int)ScenarioFlags.WestwardProgressionMode}", FlagMode.Gameflag),
			}, new()
			{
				new PatchOp(95,153,856),
				new PatchOp(95,160,856),
				/*
				new PatchOp(95,156,1635),
				new PatchOp(95,157,1699),
				new PatchOp(95,158,1766),*/
			});
		public static PatchOpGroup BridgeCanalAttribute = new(0, new()
		{
		}, new()
			{
				new PatchOp(95,153,19),
				new PatchOp(95,160,19),
			});

		public static PatchOpGroup TransportationCanalShip = new(3, new()
			{
				new RandoCondition(ConditionState.Off, "ScenarioFlag1", $"{(int)ScenarioFlags.Canal}", FlagMode.Gameflag),
			}, new()
			{
				// Block ship if no canal
				new PatchOp(95,157,1),
			});
		public static PatchOpGroup TransportationFoot = new(0, new()
			{
				new RandoCondition(ConditionState.On, "ScenarioFlag1", $"{(int)ScenarioFlags.Canal}", FlagMode.Gameflag),
				//new RandoCondition(ConditionState.On, "progression_mode", "eastward", FlagMode.Randoflag),
			}, new()
			{
				// Block walk if canal
				new PatchOp(95,157,1),
			});
	}
}
