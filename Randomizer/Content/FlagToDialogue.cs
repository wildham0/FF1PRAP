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
		public static Dictionary<int, string> FlagToDialogue = new()
		{
			{ (int)TreasureFlags.Princess, "MSG_NPC_SARALUTE_02" }, // Princess
			{ (int)TreasureFlags.Bikke, "MSG_SHIP_04" }, // Bikke
			{ (int)TreasureFlags.Astos, "MSG_ASTOS_04" }, // Astos
			{ (int)TreasureFlags.Matoya, "MSG_AWAKEPOT_03" }, // Matoya
			{ (int)TreasureFlags.ElfPrince, "MSG_AWAKEELF_05" }, // ElfPrince
			{ (int)TreasureFlags.Sarda, "MSG_GET_STICK_02" }, // Sarda
			{ (int)TreasureFlags.CanoeSage, "MSG_GET_CANOE_02" }, // CanoeSage
			{ (int)TreasureFlags.EyeChest, "MSG_GET_FLOAT_01" }, // Levistone
			{ (int)TreasureFlags.CubeBot, "MSG_GET_WARPCUBE_03" }, // Levistone
			{ (int)TreasureFlags.Smitt, "MSG_GET_EXCALIBAR_05" }, // Levistone
			{ (int)TreasureFlags.SkyChest, "MSG_WND_DAN_04" }, // Levistone
			{ (int)TreasureFlags.Fairy, "MSG_OXYALE_03" }, // Levistone
			{ (int)TreasureFlags.Lefeinman, "MSG_GET_CHIME_02" }, // Levistone
			{ (int)TreasureFlags.Bahamut, "MSG_CLASS_CHG_04" }, // Bahamut
		};
	}
}
