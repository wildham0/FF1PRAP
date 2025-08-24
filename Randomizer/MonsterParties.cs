using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last.Data.Master;
using UnityEngine.Bindings;

namespace FF1PRAP
{
	public enum MonsterIds
	{
		None = 0,
		Goblin = 1,
		GoblinGuard,
		Wolf,
		WargWolf,
		Werewolf,
		WinterWolf,
		Lizard,
		FireLizard,
		Basilisk,
		HillGigas,
		IceGigas,
		FireGigas,
		Sahagin,
		SahaginChief,
		SahaginPrince,
		Pirate,
		Buccaneer,
		Shark,
		WhiteShark,
		Bigeyes,
		Deepeyes,
		Skeleton,
		Bloodbones,
		GigasWorm,
		Crawler,
		Hyenadon,
		Hellhound,
		Ogre,
		OgreChief,
		OgreMage,
		Cobra,
		Anaconda,
		SeaSnake,
		Scorpion,
		SeaScorpion,
		Minotaur,
		MinotaurZombie,
		Troll,
		SeaTroll,
		Shadow,
		Wraith,
		Specter,
		Ghost,
		Zombie,
		Ghoul,
		Ghast,
		Wight,
		PurpleWorm,
		SandWorm,
		LavaWorm,
		EvilEye,
		DeathEye,
		Medusa,
		EarthMedusa,
		Weretiger,
		Rakshasa,
		Ankheg,
		Remorazz,
		LesserTiger,
		Sabertooth,
		Vampire,
		VampireLord,
		Gargoyle,
		HornedDevil,
		EarthElemental,
		FireElemental,
		WhiteDragon,
		RedDragon,
		DragonZombie,
		GreenSlime,
		GrayOoze,
		OchreJelly,
		BlackFlan,
		BlackWidow,
		Tarantula,
		Manticore,
		Sphinx,
		Baretta,
		DesertBaretta,
		Mummy,
		KingMummy,
		Cockatrice,
		Pyrolisk,
		Wyvern,
		Wyrm,
		Allosaurus,
		Tyrannosaur,
		Piranha,
		RedPiranha,
		Crocodile,
		WhiteCroc,
		Ochu,
		Neochu,
		Hydra,
		FireHydra,
		Guardian,
		Soldier,
		WaterElemental,
		AirElemental,
		WaterNaga,
		SpiritNaga,
		Chimera,
		Rhyos,
		Piscodemon,
		Mindflayer,
		Garland,
		GreenDragon,
		BlueDragon,
		ClayGolem,
		StoneGolem,
		IronGolem,
		BlackKnight,
		DeathKnight,
		Astos,
		DarkWizard,
		DarkFighter,
		CrazyHorse,
		Nightmare,
		Warmech,
		Lich,
		Lich2,
		Marilith,
		Marilith2,
		Kraken,
		Kraken2,
		Tiamat,
		Tiamat2,
		Chaos,
		VampireBoss,
	}

	public enum MonsterPartyCapModes
	{ 
		None,
		LowerBound,
		UpperBound,
	}
	public enum MonsterPartyRangeModes
	{
		NoVariance,
		LowVariance,
		HighVariance,
	}
	partial class Randomizer
    {
		private static Dictionary<MonsterIds, int> MonsterToPowerGroup = new()
		{
			{ MonsterIds.Goblin, 0 },
			{ MonsterIds.Skeleton, 0 },
			{ MonsterIds.GoblinGuard, 0 },
			{ MonsterIds.Wolf, 0 },
			{ MonsterIds.Zombie, 0 },
			{ MonsterIds.Sahagin, 0 },
			{ MonsterIds.BlackWidow, 0 },
			{ MonsterIds.Pirate, 0 },
			{ MonsterIds.Bigeyes, 0 },
			{ MonsterIds.Buccaneer, 0 },
			{ MonsterIds.GigasWorm, 0 },
			{ MonsterIds.CrazyHorse, 0 },
			{ MonsterIds.GreenSlime, 1 },
			{ MonsterIds.Shadow, 1 },
			{ MonsterIds.WargWolf, 1 },
			{ MonsterIds.Ghoul, 1 },
			{ MonsterIds.SahaginChief, 1 },
			{ MonsterIds.Ghast, 1 },
			{ MonsterIds.Cobra, 1 },
			{ MonsterIds.Gargoyle, 1 },
			{ MonsterIds.Werewolf, 1 },
			{ MonsterIds.Tarantula, 1 },
			{ MonsterIds.Wight, 1 },
			{ MonsterIds.Lizard, 1 },
			{ MonsterIds.Anaconda, 2 },
			{ MonsterIds.Crawler, 2 },
			{ MonsterIds.Cockatrice, 2 },
			{ MonsterIds.Ogre, 2 },
			{ MonsterIds.Scorpion, 2 },
			{ MonsterIds.Wraith, 2 },
			{ MonsterIds.Piranha, 2 },
			{ MonsterIds.OchreJelly, 2 },
			{ MonsterIds.GrayOoze, 2 },
			{ MonsterIds.Shark, 2 },
			{ MonsterIds.Piscodemon, 2 },
			{ MonsterIds.OgreChief, 2 },
			{ MonsterIds.Hyenadon, 2 },
			{ MonsterIds.Mummy, 2 },
			{ MonsterIds.Bloodbones, 3 },
			{ MonsterIds.HornedDevil, 3 },
			{ MonsterIds.WinterWolf, 3 },
			{ MonsterIds.Pyrolisk, 3 },
			{ MonsterIds.Specter, 3 },
			{ MonsterIds.LesserTiger, 3 },
			{ MonsterIds.Minotaur, 3 },
			{ MonsterIds.RedPiranha, 3 },
			{ MonsterIds.Rakshasa, 3 },
			{ MonsterIds.Troll, 3 },
			{ MonsterIds.SeaScorpion, 3 },
			{ MonsterIds.Medusa, 3 },
			{ MonsterIds.OgreMage, 3 },
			{ MonsterIds.Weretiger, 4 },
			{ MonsterIds.Crocodile, 4 },
			{ MonsterIds.Mindflayer, 4 },
			{ MonsterIds.Sabertooth, 4 },
			{ MonsterIds.SeaTroll, 4 },
			{ MonsterIds.HillGigas, 4 },
			{ MonsterIds.SahaginPrince, 4 },
			{ MonsterIds.Hydra, 4 },
			{ MonsterIds.SeaSnake, 4 },
			{ MonsterIds.KingMummy, 4 },
			{ MonsterIds.Ghost, 4 },
			{ MonsterIds.MinotaurZombie, 4 },
			{ MonsterIds.DarkWizard, 5 },
			{ MonsterIds.BlackFlan, 5 },
			{ MonsterIds.Sphinx, 5 },
			{ MonsterIds.Wyvern, 5 },
			{ MonsterIds.Hellhound, 5 },
			{ MonsterIds.Ankheg, 5 },
			{ MonsterIds.Vampire, 5 },
			{ MonsterIds.FireHydra, 5 },
			{ MonsterIds.EarthMedusa, 5 },
			{ MonsterIds.Wyrm, 5 },
			{ MonsterIds.Ochu, 5 },
			{ MonsterIds.Guardian, 5 },
			{ MonsterIds.ClayGolem, 5 },
			{ MonsterIds.BlackKnight, 5 },
			{ MonsterIds.Nightmare, 5 },
			{ MonsterIds.Manticore, 5 },
			{ MonsterIds.Baretta, 6 },
			{ MonsterIds.FireGigas, 6 },
			{ MonsterIds.EarthElemental, 6 },
			{ MonsterIds.AirElemental, 6 },
			{ MonsterIds.FireElemental, 6 },
			{ MonsterIds.LavaWorm, 6 },
			{ MonsterIds.WhiteDragon, 6 },
			{ MonsterIds.IceGigas, 6 },
			{ MonsterIds.WhiteCroc, 6 },
			{ MonsterIds.WaterElemental, 6 },
			{ MonsterIds.Basilisk, 6 },
			{ MonsterIds.Chimera, 7 },
			{ MonsterIds.Remorazz, 7 },
			{ MonsterIds.DragonZombie, 7 },
			{ MonsterIds.WaterNaga, 7 },
			{ MonsterIds.WhiteShark, 7 },
			{ MonsterIds.VampireLord, 7 },
			{ MonsterIds.StoneGolem, 7 },
			{ MonsterIds.FireLizard, 7 },
			{ MonsterIds.DesertBaretta, 7 },
			{ MonsterIds.SandWorm, 7 },
			{ MonsterIds.DeathKnight, 7 },
			{ MonsterIds.RedDragon, 7 },
			{ MonsterIds.Neochu, 8 },
			{ MonsterIds.EvilEye, 8 },
			{ MonsterIds.BlueDragon, 8 },
			{ MonsterIds.Allosaurus, 8 },
			{ MonsterIds.DarkFighter, 8 },
			{ MonsterIds.SpiritNaga, 8 },
			{ MonsterIds.Deepeyes, 8 },
			{ MonsterIds.Soldier, 8 },
			{ MonsterIds.GreenDragon, 8 },
			{ MonsterIds.PurpleWorm, 8 },
			{ MonsterIds.Rhyos, 8 },
			{ MonsterIds.IronGolem, 8 },
			{ MonsterIds.Tyrannosaur, 8 },
			{ MonsterIds.Garland, 255 },
			{ MonsterIds.Astos, 255 },
			{ MonsterIds.Lich, 255 },
			{ MonsterIds.Lich2, 255 },
			{ MonsterIds.VampireBoss, 255 },
			{ MonsterIds.Marilith, 255 },
			{ MonsterIds.Marilith2, 255 },
			{ MonsterIds.Kraken, 255 },
			{ MonsterIds.Kraken2, 255 },
			{ MonsterIds.Tiamat, 255 },
			{ MonsterIds.Tiamat2, 255 },
			{ MonsterIds.DeathEye, 255 },
			{ MonsterIds.Chaos, 255 },
		};

		private static Dictionary<int, List<MonsterIds>> PowerGroupToMonsters = MonsterToPowerGroup.GroupBy(x => x.Value).ToDictionary(x => x.Key, x => x.Select(m => m.Key).ToList());

		private static List<int> ExcludedBosses = new() { 338, 339, 340, 341, 342, 343, 344, 345, 346, 347, 348, 349, 350 };
		private static List<int> ExcludedMiniBosses = new() { 65, 88, 97, 102, 115, 117, 118, 119, 123, 128, 134, 138, 175, 192, 195, 197, 202, 208, 229, 239, 241, 312, 327, 329, 827, 828, 829, 830 };
		private static List<int> ExcludedWarmech = new() { 261 };
		private static List<int> ExcludedAll = ExcludedBosses.Concat(ExcludedMiniBosses).Concat(ExcludedWarmech).ToList();

		public static Dictionary<int, Dictionary<int, MonsterIds>> RandomizeMonsterParties(bool enable, MonsterPartyRangeModes rangemode, MonsterPartyCapModes capmode, MT19337 rng)
		{
			var partyList = FF1PR.MasterManager.GetList<MonsterParty>();
			int range = 0;

			switch (rangemode)
			{
				case MonsterPartyRangeModes.LowVariance:
					range = 1;
					break;
				case MonsterPartyRangeModes.HighVariance:
					range = 2;
					break;
			}

			bool lowerbound = capmode == MonsterPartyCapModes.LowerBound;
			bool upperbound = capmode == MonsterPartyCapModes.UpperBound;


			Dictionary<int, Dictionary<int, MonsterIds>> newMonsterParties = new();

			foreach (var party in partyList)
			{
				if (ExcludedAll.Contains(party.Key))
				{
					continue;
				}

				List<MonsterIds> monsters = new();
				List<int> positions = new();

				Dictionary<MonsterIds, MonsterIds> changedMonsters = new();
				Dictionary<int, MonsterIds> newMonsters = new();
				Dictionary<int, MonsterIds> existingMonsters = new()
				{
					{ 1, (MonsterIds)party.value.Monster1 },
					{ 2, (MonsterIds)party.value.Monster2 },
					{ 3, (MonsterIds)party.value.Monster3 },
					{ 4, (MonsterIds)party.value.Monster4 },
					{ 5, (MonsterIds)party.value.Monster5 },
					{ 6, (MonsterIds)party.value.Monster6 },
					{ 7, (MonsterIds)party.value.Monster7 },
					{ 8, (MonsterIds)party.value.Monster8 },
					{ 9, (MonsterIds)party.value.Monster9 },
				};

				foreach (var existingMonster in existingMonsters)
				{
					MonsterIds monster = existingMonster.Value;

					if (monster != MonsterIds.None)
					{
						MonsterIds newMonster;

						if (changedMonsters.TryGetValue(monster, out var previousMonster))
						{
							newMonster = previousMonster;
						}
						else
						{
							//InternalLogger.LogInfo($"RandomParty: Current Monster {monster}");
							int powergroup = MonsterToPowerGroup[monster];
							//InternalLogger.LogInfo($"RandomParty: Current PowerGroup {powergroup}");
							powergroup = rng.Between(lowerbound ? powergroup : Math.Max(powergroup - range, 0), upperbound ? powergroup : Math.Min(powergroup + range, 8));
							//InternalLogger.LogInfo($"RandomParty: Current New Power {powergroup}");
							newMonster = rng.PickFrom(PowerGroupToMonsters[powergroup]);
							//InternalLogger.LogInfo($"RandomParty: New Monster {newMonster}");

							changedMonsters.Add(monster, newMonster);
						}

						newMonsters.Add(existingMonster.Key, newMonster);
					}
				}

				newMonsterParties.Add(party.Key, newMonsters);
			}

			return newMonsterParties;
		}

		public static void LoadMonsterParties(Dictionary<int, Dictionary<int, MonsterIds>> newMonsterParties)
		{
			foreach (var party in newMonsterParties)
			{
				var monsterParty = FF1PR.MasterManager.GetData<MonsterParty>(party.Key);

				MonsterIds monster;

				if (party.Value.TryGetValue(1, out monster))
				{
					monsterParty.Monster1 = (int)monster;
				}

				if (party.Value.TryGetValue(2, out monster))
				{
					monsterParty.Monster2 = (int)monster;
				}
				
				if (party.Value.TryGetValue(3, out monster))
				{
					monsterParty.Monster3 = (int)monster;
				}

				if (party.Value.TryGetValue(4, out monster))
				{
					monsterParty.Monster4 = (int)monster;
				}

				if (party.Value.TryGetValue(5, out monster))
				{
					monsterParty.Monster5 = (int)monster;
				}

				if (party.Value.TryGetValue(6, out monster))
				{
					monsterParty.Monster6 = (int)monster;
				}

				if (party.Value.TryGetValue(7, out monster))
				{
					monsterParty.Monster7 = (int)monster;
				}

				if (party.Value.TryGetValue(8, out monster))
				{
					monsterParty.Monster8 = (int)monster;
				}

				if (party.Value.TryGetValue(9, out monster))
				{
					monsterParty.Monster9 = (int)monster;
				}
			}
		}
	}
}
