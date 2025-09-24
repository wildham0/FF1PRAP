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
		public static Dictionary<string, string> AssetsToReplace = new()
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
			{ "Assets/GameAssets/Serial/Res/Map/Map_20031/Map_20031_1/ev_e_0012", "ev_matoyascave" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20071/Map_20071_1/ev_e_0014", "ev_elflandcastle" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20051/Map_20051_1/ev_e_0014", "ev_dwarf_cave" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30031/Map_30031_3/ev_e_0016", "ev_earth_b3" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30041/Map_30041_1/ev_e_0018", "ev_titan" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30071/Map_30071_1/ev_e_0046", "ev_ordeals" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20141/Map_20141_1/entity_default", "ev_caravan" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20130/Map_20130/ev_e_0030", "ev_onrac" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30071/Map_30071_2/entity_default", "ev_ordealsmaze" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20121/Map_20121_3/entity_default", "ev_bahamut" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20090/Map_20090/ev_e_0034", "ev_melmond" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30101/Map_30101_3/ev_e_0027", "ev_cubewarp" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30121/Map_30121_3/ev_e_0039", "ev_lute_floor" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30061/Map_30061_1/entity_default", "ev_cavern_of_ice_1f" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_30061/Map_30061_4/sc_telepor_cache", "sc_ice_teleporter_cache" },
			// Fix bugged town's teleporter
			{ "Assets/GameAssets/Serial/Res/Map/Map_20061/Map_20061_3/entity_default", "ev_elfheim_weapon_shop" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20061/Map_20061_4/entity_default", "ev_elfheim_item_shop" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20061/Map_20061_5/entity_default", "ev_elfheim_white_magic_shop3" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20111/Map_20111_5/entity_default", "ev_crescent_weapon_shop" },
			// Add warp protection to combo shops
			{ "Assets/GameAssets/Serial/Res/Map/Map_20161/Map_20161_1/entity_default", "ev_lufenia_magic_shop" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20151/Map_20151_6/entity_default", "ev_gaia_magic_shop8" },
			{ "Assets/GameAssets/Serial/Res/Map/Map_20151/Map_20151_1/entity_default", "ev_gaia_gear_shop" },
		};
	}
}
