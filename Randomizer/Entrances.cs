using Last.Data.Master;
using Last.Interpreter;
using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Bindings;
using UnityEngine.Tilemaps;

namespace FF1PRAP
{
	partial class Randomizer
    {
		public enum ShuffleEntrancesMode
		{
			NoShuffle = 0,
			DungeonInternal,
			DungeonMixed,
			All
		}
		public enum ShuffleTownsMode
		{
			NoShuffle = 0,
			BetweenTowns,
			ShuffleShallow,
			ShuffleDeep
		}

		public static Dictionary<string, string> ProcessEntrances2()
		{
			Dictionary<string, string> processedTeleporters = new();
			Teleporter tele;

			foreach (var teleporter in NewTeleporters)
			{
				InternalLogger.LogInfo($"Processing entrance: {teleporter.Key} > {teleporter.Value}");
				
				if (!Randomizer.Teleporters.TryFind(t => t.Name == teleporter.Key, out tele))
				{
					InternalLogger.LogInfo($"Entrances: {teleporter.Key} not found.");
				}

				if (!Randomizer.Teleporters.TryFind(t => t.Name == Randomizer.TeleporterPairs[teleporter.Value], out tele))
				{
					InternalLogger.LogInfo($"Entrances: {Randomizer.TeleporterPairs[teleporter.Value]} not found.");
				}

				if (!Randomizer.Teleporters.TryFind(t => t.Name == Randomizer.TeleporterPairs[teleporter.Key], out tele))
				{
					InternalLogger.LogInfo($"Entrances: {Randomizer.TeleporterPairs[teleporter.Key]} not found.");
				}

				if (!Randomizer.Teleporters.TryFind(t => t.Name == teleporter.Value, out tele))
				{
					InternalLogger.LogInfo($"Entrances: {teleporter.Value} not found.");
				}

				var newReturnPoint = Randomizer.Teleporters.Find(t => t.Name == teleporter.Key);
				var originalReturnPoint = Randomizer.Teleporters.Find(t => t.Name == Randomizer.TeleporterPairs[teleporter.Value]);

				var originalDestinationPoint = Randomizer.Teleporters.Find(t => t.Name == Randomizer.TeleporterPairs[teleporter.Key]);
				var newDestinationPoint = Randomizer.Teleporters.Find(t => t.Name == teleporter.Value);


				processedTeleporters.Add(originalReturnPoint.Name, newReturnPoint.Name);
				processedTeleporters.Add(originalDestinationPoint.Name, newDestinationPoint.Name);


				//processedTeleporters.Add(teleporter.Key, teleporter.Value);
				//processedTeleporters.Add(teleporter.Value, teleporter.Key);
			}

			return processedTeleporters;
		}
		public static Dictionary<(int, int), Teleporter> ProcessEntrances()
		{
			Dictionary<(int, int), Teleporter> processedTeleporters = new();

			foreach (var teleporter in NewTeleporters)
			{

				InternalLogger.LogInfo($"Processing entrance: {teleporter.Key} > {teleporter.Value}");

				Teleporter tele;

				if (!Randomizer.Teleporters.TryFind(t => t.Name == teleporter.Key, out tele))
				{
					InternalLogger.LogInfo($"Entrances: {teleporter.Key} not found.");
				}

				if (!Randomizer.Teleporters.TryFind(t => t.Name == Randomizer.TeleporterPairs[teleporter.Value], out tele))
				{
					InternalLogger.LogInfo($"Entrances: {Randomizer.TeleporterPairs[teleporter.Value]} not found.");
				}

				if (!Randomizer.Teleporters.TryFind(t => t.Name == Randomizer.TeleporterPairs[teleporter.Key], out tele))
				{
					InternalLogger.LogInfo($"Entrances: {Randomizer.TeleporterPairs[teleporter.Key]} not found.");
				}

				if (!Randomizer.Teleporters.TryFind(t => t.Name == teleporter.Value, out tele))
				{
					InternalLogger.LogInfo($"Entrances: {teleporter.Value} not found.");
				}

				var newReturnPoint = Randomizer.Teleporters.Find(t => t.Name == teleporter.Key);
				var originalReturnPoint = Randomizer.Teleporters.Find(t => t.Name == Randomizer.TeleporterPairs[teleporter.Value]);

				var originalDestinationPoint = Randomizer.Teleporters.Find(t => t.Name == Randomizer.TeleporterPairs[teleporter.Key]);
				var newDestinationPoint = Randomizer.Teleporters.Find(t => t.Name == teleporter.Value);

				InternalLogger.LogInfo($"Processing entrance: {teleporter.Key} > {teleporter.Value}");

				processedTeleporters.Add((originalReturnPoint.MapId, originalReturnPoint.PointId), newReturnPoint);
				processedTeleporters.Add((originalDestinationPoint.MapId, originalDestinationPoint.PointId), newDestinationPoint);
			}

			return processedTeleporters;
			/*
			//Data.Teleporters = processedTeleporters;

			foreach (var tele in Data.Teleporters)
			{
				InternalLogger.LogInfo($"Processed Entrance: {tele.Key} > {tele.Value.Name}");
			}*/
		}

		public static void InitializeEntrances()
		{
			if (Randomizer.Data.EntrancesShuffled)
			{
				GameData.DataStorage.Set(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.PrincessNoWarp, 1);
			}
		}

		public static bool ProcessEntrancesOptions(bool shuffleoverworld, ShuffleEntrancesMode entrancesmode)
		{
			return shuffleoverworld || entrancesmode == ShuffleEntrancesMode.All;
		}
	}
}
