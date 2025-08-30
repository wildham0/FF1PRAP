using Last.Data.Master;
using Last.Interpreter;
using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Bindings;
using UnityEngine;

namespace FF1PRAP
{
	partial class Randomizer
    {
		public static Dictionary<int, int> ItemIdToFlag = new()
		{
			{ (int)Items.Lute, (int)ScenarioFlags.Lute },
			{ (int)Items.Crown, (int)ScenarioFlags.Crown },
			{ (int)Items.CrystalEye, (int)ScenarioFlags.CrystalEye },
			{ (int)Items.JoltTonic, (int)ScenarioFlags.JoltTonic },
			{ (int)Items.MysticKey, (int)ScenarioFlags.MysticKey },
			{ (int)Items.NitroPowder, (int)ScenarioFlags.NitroPowder },
			{ (int)Items.StarRuby, (int)ScenarioFlags.StarRuby },
			{ (int)Items.EarthRod, (int)ScenarioFlags.EarthRod },
			{ (int)Items.Canoe, (int)ScenarioFlags.Canoe },
			{ (int)Items.Levistone, (int)ScenarioFlags.Levistone },
			{ (int)Items.WarpCube, (int)ScenarioFlags.WarpCube },
			{ (int)Items.Oxyale, (int)ScenarioFlags.Oxyale },
			{ (int)Items.RosettaStone, (int)ScenarioFlags.RosettaStone },
			{ (int)Items.Chime, (int)ScenarioFlags.Chime },
			{ (int)Items.RatsTail, (int)ScenarioFlags.RatTail },
			{ (int)Items.Adamantite, (int)ScenarioFlags.Adamant },
			{ (int)Items.BottledFaerie, (int)ScenarioFlags.BottledFaerie },
			{ (int)Items.Ship, (int)ScenarioFlags.Ship },
			{ (int)Items.LuteTablature, (int)ScenarioFlags.TablatureReceived },
		};
	}
}
