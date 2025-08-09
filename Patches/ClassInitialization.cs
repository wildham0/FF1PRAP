using Last.Entity.Field;
using Last.Message;
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
			if (Monitor.instance == null)
			{
				GameObject monitorObject = new GameObject("gameMonitor");
				Monitor.instance = monitorObject.AddComponent<Monitor>();
				GameObject.DontDestroyOnLoad(monitorObject);
			}

			if (Archipelago.instance == null)
			{
				GameObject ArchipelagoObject = new GameObject("archipelago");
				Archipelago.instance = ArchipelagoObject.AddComponent<Archipelago>();
				GameObject.DontDestroyOnLoad(ArchipelagoObject);
			}

			if (ApItemWindow.instance == null)
			{
				GameObject apItemWindowObject = new GameObject("apItemWindow");
				ApItemWindow.instance = apItemWindowObject.AddComponent<ApItemWindow>();
				GameObject.DontDestroyOnLoad(apItemWindowObject);
			}
		}
	}
}
