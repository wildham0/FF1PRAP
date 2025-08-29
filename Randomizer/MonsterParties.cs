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
	public enum MinionsRangeModes
	{
		None,
		Weak,
		Strong,
		WeakStrong
	}
	public enum MinionGroupSizes
	{
		Small,
		Large
	}
	partial class Randomizer
    {
		private struct BossMinionConfig
		{
			public int WeakRange;
			public int MidRange;
			public int StrongRange;
			public int MaxSmall;
			public int MaxBig;
		}

		private struct MiniBossExtendConfig
		{
			public int WeakMin;
			public int WeakMax;
			public int StrongMin;
			public int StrongMax;
			public MonsterIds Monster;
			public List<int> Free;
		}

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

		private static List<int> Bosses = new() { 338, 339, 340, 341, 342, 343, 344, 345, 346, 347, 348, 349, 350 };
		private static List<int> MiniBosses = new() { 65, 88, 97, 102, 115, 117, 118, 119, 123, 128, 134, 138, 175, 192, 195, 197, 202, 208, 229, 239, 241, 312, 327, 329, 827, 828, 829, 830 };
		private static List<int> Warmech = new() { 261 };
		private static List<int> ExcludedAll = Bosses.Concat(MiniBosses).Concat(Warmech).ToList();

		// 3-4 weak, 1-2 strong
		private static Dictionary<int, BossMinionConfig> BossMinionConfigs = new()
		{
			{ 350, new() { WeakRange = 0, MidRange = 1, StrongRange = 1 } }, // Garland > 0 - Special one
			// 349 - Pirates
			{ 348, new() { WeakRange = 1, MidRange = 2, StrongRange = 3 } }, // Astos > 2
			{ 347, new() { WeakRange = 2, MidRange = 3, StrongRange = 4 } }, // Vampire > 3
			{ 345, new() { WeakRange = 3, MidRange = 4, StrongRange = 5 } }, // Lich
		
			{ 344, new() { WeakRange = 4, MidRange = 5, StrongRange = 6 } }, // Marilith

			{ 343, new() { WeakRange = 5, MidRange = 6, StrongRange = 7 } }, // Kraken

			{ 342, new() { WeakRange = 5, MidRange = 6, StrongRange = 7 } }, // Tiamat

			{ 338, new() { WeakRange = 6, MidRange = 7, StrongRange = 8 } }, // Lich2
			{ 339, new() { WeakRange = 6, MidRange = 7, StrongRange = 8 } }, // Marilith2
			{ 340, new() { WeakRange = 6, MidRange = 7, StrongRange = 8 } }, // Kraken2
			{ 341, new() { WeakRange = 6, MidRange = 7, StrongRange = 8 } }, // Tiamat2

			{ 346, new() { WeakRange = 7, MidRange = 7, StrongRange = 8 } }, // Chaos - Special one
		};

		private static Dictionary<int, MiniBossExtendConfig> MiniBossesConfigs = new()
		{
			//{ 65, new() { WeakMin = 1, WeakMax = 2, StrongMin = 2, StrongMax = 3, Free = new() { 1, 2, 3 } } }, // Anaconda Group
			{ 88, new() { Monster = MonsterIds.Piscodemon, WeakMin = 1, WeakMax = 2, StrongMin = 2, StrongMax = 3, Free = new() { 1, 2, 3, 4, 6 } } }, // Marsh Chest Pisco Demons
			//{ 97, new() { WeakMin = 1, WeakMax = 2, StrongMin = 2, StrongMax = 3, Free = new() { 1, 2, 3, 4, 6 } } }, // Lizard+Fire Giants
			//{ 102, new() { WeakMin = 1, WeakMax = 2, StrongMin = 2, StrongMax = 3, Free = new() { 1, 2, 3, 4, 6 } } }, // Single Earth Elemental
			//{ 115, new() { WeakMin = 1, WeakMax = 2, StrongMin = 2, StrongMax = 3, Free = new() { 1, 2, 3, 4, 6 } } }, // Two Fire Elemental
			// 117 - Lava Worm
			// 118 - Fire Lizard
			// 119 - Red Dragon
			// 123 - Ice Undead Pack
			// 128 - ??? party of #6, but there's no #6 monster
			// 134 - Dark Wizard x4
			// 138 - White Dragon x2
			// 175 - Clay Golem x3
			// 192 - Sea Food Pack
			// 195 - White Shark Pack
			{ 197, new() { Monster = MonsterIds.DeathEye, WeakMin = 1, WeakMax = 1, StrongMin = 2, StrongMax = 3, Free = new() { 1, 2, 3, 4, 5, 6, 7, 9 } } }, // 197 - Death Eye (Wrong eye)
			// 202 - Water Elemental x3
			// 208 - Waterfall Mummy Pack
			{ 229, new() { Monster = MonsterIds.DragonZombie, WeakMin = 1, WeakMax = 2, StrongMin = 3, StrongMax = 4, Free = new() { 1, 2, 3, 4, 5, 6, 8 } } }, // 229 - Dragon Zombie x2
			{ 239, new() { Monster = MonsterIds.BlueDragon, WeakMin = 1, WeakMax = 1, StrongMin = 2, StrongMax = 3, Free = new() { 1, 2, 3, 4, 5, 6, 7, 9 } } }, // 239 - Blue Dragon
			// 241 - Nightmare x2
			{ 312, new() { Monster = MonsterIds.EvilEye, WeakMin = 1, WeakMax = 1, StrongMin = 2, StrongMax = 3, Free = new() { 1, 2, 3, 4, 5, 6, 7, 9 } } }, // 312 - Evil Eye
			// 327 - Ogre+Hyena
			// 329 - Sphinx x2
			// 827 - Mummy Pack
			// 828 - Wraith x5
			// 829 - Mummy x5
			// 830 - Death Eye (runnable?)
		};
		public static Dictionary<int, Dictionary<int, MonsterIds>> RandomizeMonsterParties(bool enable, MonsterPartyRangeModes rangemode, MonsterPartyCapModes capmode, MT19337 rng)
		{
			if (!enable)
			{
				return new();
			}

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

		public static Dictionary<int, Dictionary<int, MonsterIds>> AddBossMinions(bool enable, MinionsRangeModes rangemode, MT19337 rng)
		{
			if (!enable)
			{
				return new();
			}

			Dictionary<int, Dictionary<int, MonsterIds>> newMonsterParties = new();

			// Do Bosses Minions first
			foreach (var boss in BossMinionConfigs)
			{
				int power = 0;
				int pick = 0;
				MinionGroupSizes size = MinionGroupSizes.Small;

				switch (rangemode)
				{
					case MinionsRangeModes.Weak:
						pick = rng.Between(0, 1);
						if (pick == 0)
						{
							size = MinionGroupSizes.Large;
							power = boss.Value.WeakRange;
						}
						else
						{
							size = MinionGroupSizes.Small;
							power = boss.Value.MidRange;
						}
						break;
					case MinionsRangeModes.Strong:
						pick = rng.Between(0, 1);
						if (pick == 0)
						{
							size = MinionGroupSizes.Large;
							power = boss.Value.MidRange;
						}
						else
						{
							size = MinionGroupSizes.Small;
							power = boss.Value.StrongRange;
						}
						break;
					case MinionsRangeModes.WeakStrong:
						pick = rng.Between(0, 1);
						if (pick == 0)
						{
							size = MinionGroupSizes.Large;
							pick = rng.Between(0, 1);
							if (pick == 0)
							{
								power = boss.Value.WeakRange;
							}
							else
							{
								power = boss.Value.MidRange;
							}
						}
						else
						{
							size = MinionGroupSizes.Small;
							pick = rng.Between(0, 1);
							if (pick == 0)
							{
								power = boss.Value.MidRange;
							}
							else
							{
								power = boss.Value.StrongRange;
							}
						}
						break;
				}

				if (size == MinionGroupSizes.Large)
				{
					List<int> positions = new() { 1, 2, 3, 4, 5, 6 };
					var qty = rng.Between(4, 6);
					if (boss.Key == 350) qty /= 2; // garland special condition
					var monster = rng.PickFrom(PowerGroupToMonsters[power]);

					Dictionary<int, MonsterIds> newmonsters = new();

					for (int i = 0; i < qty; i++)
					{
						var position = rng.TakeFrom(positions);
						newmonsters.Add(position, monster);
					}
					newMonsterParties.Add(boss.Key, newmonsters);
				}
				else
				{
					List<int> positions = new() { 1, 2, 3, 4, 5, 6 };
					var qty = rng.Between(1, 2);
					if (boss.Key == 350) qty /= 2; // garland special condition

					var monster1 = rng.PickFrom(PowerGroupToMonsters[power]);
					var monster2 = rng.PickFrom(PowerGroupToMonsters[power]);

					if (boss.Key == 346)
					{
						monster1 = rng.PickFrom(new List<MonsterIds> { MonsterIds.Lich, MonsterIds.Marilith, MonsterIds.Kraken, MonsterIds.Tiamat, MonsterIds.Warmech });
					}

					Dictionary<int, MonsterIds> newmonsters = new();
					var position = rng.TakeFrom(positions);
					newmonsters.Add(position, monster1);

					if (qty > 1)
					{
						position = rng.TakeFrom(positions);
						newmonsters.Add(position, monster2);
					}

					newMonsterParties.Add(boss.Key, newmonsters);
				}
			}

			// Mini Bosses
			foreach (var miniboss in MiniBossesConfigs)
			{
				int count = 0;
				int pick = 0;

				switch (rangemode)
				{
					case MinionsRangeModes.Weak:
						count = rng.Between(miniboss.Value.WeakMin, miniboss.Value.WeakMax);
						break;
					case MinionsRangeModes.Strong:
						count = rng.Between(miniboss.Value.StrongMin, miniboss.Value.StrongMax);
						break;
					case MinionsRangeModes.WeakStrong:
						count = rng.Between(miniboss.Value.WeakMin, miniboss.Value.StrongMax);
						break;
				}
				Dictionary<int, MonsterIds> newmonsters = new();
				List<int> positions = new(miniboss.Value.Free);
				for (int i = 0; i < count; i++)
				{
					var position = rng.TakeFrom(positions);
					newmonsters.Add(position, miniboss.Value.Monster);
				}

				newMonsterParties.Add(miniboss.Key, newmonsters);
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
