using Il2CppSystem.Common;
using Last.Data.Master;
using Last.Entity.Field;
using Last.Interpreter;
using Last.Message;
using Last.Systems.EndRoll;
using Last.Systems.Indicator;
using Last.Systems.Message;
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
		public static void GetLoadingState_Post(SystemIndicator.Mode mode)
		{
			if (Monitor.instance != null)
			{
				Monitor.instance.SetLoadingState(mode);
			}
		}
	}
}
