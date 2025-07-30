using Last.Entity.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FF1PRAP
{
	partial class Patches
	{
		public static void CreateInstance_Post()
		{
			InternalLogger.LogInfo($"Creating objects.");

			GameObject monitorObject = new GameObject("gameMonitor");
			Monitor.instance = monitorObject.AddComponent<Monitor>();
			GameObject.DontDestroyOnLoad(monitorObject);

			GameObject ArchipelagoObject = new GameObject("archipelago");
			Archipelago.instance = ArchipelagoObject.AddComponent<Archipelago>();
			GameObject.DontDestroyOnLoad(ArchipelagoObject);
		}
	}
}
