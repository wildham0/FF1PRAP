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
using System.Diagnostics;
using RomUtilities;
//using Il2CppSystem.Collections.Generic;

namespace FF1PRAP
{
	public static class Utilities
	{
		[DebuggerStepThrough]
		public static bool TryFind<T>(this IList<T> fromList, Predicate<T> query, out T result)
		{
			int resultIndex = fromList.ToList().FindIndex(query);
			if (resultIndex < 0)
			{
				result = default;
				return false;
			}
			else
			{
				result = fromList[resultIndex];
				return true;
			}
		}
		public static T PickFrom<T>(this MT19337 rng, IList<T> list)
		{
			return list[rng.Between(0, list.Count - 1)];
		}

		public static T TakeFrom<T>(this MT19337 rng, IList<T> list)
		{
			var value = rng.PickFrom(list);
			list.Remove(value);
			return value;
		}
	}
}
