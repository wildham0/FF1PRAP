using Last.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF1PRAP
{

	partial class Patches
	{
		public static void Gameflags_Postfix(string c, int index, int value)
		{
			if (FF1PR.SessionManager.GameMode == GameModes.Archipelago)
			{
				InternalLogger.LogInfo($"Setting flag: {c} - {index}");
				if (c == "TreasureFlag1" && value == 1)
				{
					Archipelago.instance.ActivateCheck(Randomizer.FlagToLocationName[index]);
				}
			}
		}
	}

	public enum ScenarioFlags
	{ 
		IntroDone = 0,
		KingVisited,
		PrincessSaved,
		Flag003,
		BuildBridgeCutscene,
		Lute,
		Bridge,
		BridgeIntro,
		MatoyaCutscene,
		Ship,
		Crown,
		CrystalEye,
		JoltTonic,
		MysticKey,
		NitroPowder,
		Canal = 15,
		VampireDefeated,
		StarRuby,
		TitanFed,
		EarthRod,
		SlabLifted,
		LichDefeated,
		Canoe,
		KaryDefeated,
		Levistone,
		Airship,
		WarpCube,
		BottledFaerie,
		FaerieReleased,
		Oxyale,
		SubActivated = 30,
		RosettaStone,
		StoneTranslated,
		Chime,
		KrakenDefeated = 34,
		TiamatDefeated,
		BlackOrbRemoved,
		LuteSlabRemoved,
		OrdealManTalkedTo = 44,
		RatTail,
		BahamutPromoted,
		Adamant,
		XcalForged,
		MysticKeyBugFix // I'm not sure how it works, but it's probably to make sure you exit with the mystic key




	}

	public enum TreasureFlags
	{ 
		Princess = 400,
		Bikke = 401,
		MarshChest = 501,
		Astos = 246,
		Matoya = 247,
		ElfPrince = 248,
		ConeriaChest = 505,
		Smitt = 405,
		VampireChest = 508,
		Sarda = 253,
		CanoeSage = 402,
		EyeChest = 254,
		MouseChest = 512,
		Caravan = 406,
		Fairy = 403,
		MermaidsChest = 507,
		CubeBot = 404,
		Lefeinman = 255,
		SkyChest = 250,
		

		//Caravan





	}

    class GameFlag
    {

    }
}
