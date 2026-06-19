using Last.Data.Master;
using Last.Interpreter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine.SocialPlatforms;
using static FF1PRAP.Patches;
//using Il2CppSystem.Collections.Generic;

namespace FF1PRAP
{
	
	public class EntityPatch
	{
		public string Name;
		public List<(string property, object value)> Properties;

		public EntityPatch(string _name, List<(string property, object value)> _properties)
		{
			Name = _name;
			Properties = _properties;
		}
	}

	public static class EntityPatcher
	{
		public static string Patch(string file, EntityPatch patch)
		{
			InternalLogger.LogInfo("Patching asset");
			JsonNode originalJson = JsonNode.Parse(file);

			var options = new JsonSerializerOptions
			{
				WriteIndented = false,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};

			if (originalJson["layers"]![0]!["objects"].AsArray().TryFind(n => n["name"].GetValue<string>() == patch.Name, out var entity))
			{
				foreach (var prop in patch.Properties)
				{
					if (prop.value is bool)
					{
						entity[prop.property] = (bool)prop.value;
					}
					else if (prop.value is string)
					{
						entity[prop.property] = (string)prop.value;
					}
					else if (prop.value is int)
					{
						entity[prop.property] = (int)prop.value;
					}
				}
			}

			file = originalJson.ToJsonString(options);

			return file;
		}
	}
}
