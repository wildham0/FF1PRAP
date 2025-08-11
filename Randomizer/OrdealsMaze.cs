using RomUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Last.Data.Master;
using UnityEngine.Bindings;
using Mono.Cecil;
using Steamworks;

namespace FF1PRAP
{
	partial class Randomizer
    {
		public class TrialsRoom
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public List<TrialsPillars> Pillars { get; set; }

			public TrialsRoom() { }
			public TrialsRoom(TrialsRoom copy)
			{
				Id = copy.Id;
				Name = copy.Name;
				Pillars = copy.Pillars.Select(p => new TrialsPillars(p)).ToList();
			}

		}
		public class TrialsPillars
		{ 
			public int Id { get; set; }
			public int TargetRoom { get; set; }
			public List<int> PreviousRooms { get; set; }
			public int OriginalTargetPoint { get; set; }
			public int TargetPoint { get; set; }
			public TrialsPillars() { }
			public TrialsPillars(TrialsPillars copy)
			{
				Id = copy.Id;
				TargetRoom = copy.TargetRoom;
				OriginalTargetPoint = copy.OriginalTargetPoint;
				TargetPoint = copy.TargetPoint;
				PreviousRooms = new();
			}
		}
		public static List<TrialsRoom> TrialsRooms = new()
		{
			new TrialsRoom() { Id = 0, Name = "CenterSE", Pillars = new() { new() { Id = 0, TargetRoom = 1, OriginalTargetPoint = 3 } } },
			new TrialsRoom() { Id = 1, Name = "CenterNE", Pillars = new() { new() { Id = 1, TargetRoom = 2, OriginalTargetPoint = 4 } } },
			new TrialsRoom() { Id = 2, Name = "CenterSplit", Pillars = new() { new() { Id = 2, TargetRoom = 0, OriginalTargetPoint = 6 }, new() { Id = 3, TargetRoom = 3, OriginalTargetPoint = 5 } } },
			new TrialsRoom() { Id = 3, Name = "CenterSW", Pillars = new() { new() { Id = 4, TargetRoom = 4, OriginalTargetPoint = 7 } } },
			new TrialsRoom() { Id = 4, Name = "LowerCornerSplit", Pillars = new() { new() { Id = 5, TargetRoom = 3, OriginalTargetPoint = 300 }, new() { Id = 6, TargetRoom = 5, OriginalTargetPoint = 8 } } },
			new TrialsRoom() { Id = 5, Name = "SideVertical", Pillars = new() { new() { Id = 7, TargetRoom = 6, OriginalTargetPoint = 10 } } },
			new TrialsRoom() { Id = 6, Name = "UpperCornerSplit", Pillars = new() { new() { Id = 8, TargetRoom = 2, OriginalTargetPoint = 12 }, new() { Id = 9, TargetRoom = 7, OriginalTargetPoint = 11 } } },
			new TrialsRoom() { Id = 7, Name = "FourPillars", Pillars = new() { new() { Id = 10, TargetRoom = 4, OriginalTargetPoint = 301 }, new() { Id = 11, TargetRoom = 1, OriginalTargetPoint = 302 }, new() { Id = 12, TargetRoom = 8, OriginalTargetPoint = 13 }, new() { Id = 13, TargetRoom = 1, OriginalTargetPoint = 303 } } },
			new TrialsRoom() { Id = 8, Name = "CornerExit", Pillars = new() },
			new TrialsRoom() { Id = 9, Name = "LowerHorizontal", Pillars = new() { new() { Id = 14, TargetRoom = 9, OriginalTargetPoint = 304 } } },
		};

		public static Dictionary<int, int> RoomToPoint = new()
		{
			{ 0, 6 },
			{ 1, 3 },
			{ 2, 4 },
			{ 3, 5 },
			{ 4, 7 },
			{ 5, 8 },
			{ 6, 10 },
			{ 7, 11 },
			{ 8, 13 },
			{ 9, 304 }
		};

		public static Dictionary<int, int> ShuffleOrdealsMaze(bool enable, MT19337 rng)
		{
			if (!enable) return new Dictionary<int, int>();

			InternalLogger.LogInfo($"Shuffling Citadel of Trials' Maze");

			List<int> placedRoomIds = new();
			List<TrialsPillars> placedPillars = new();
			List<TrialsPillars> remainingPillars = new();
			List<TrialsRoom> rooms = TrialsRooms.Select(r => new TrialsRoom(r)).ToList();

			// Take out End Room (we always finish there)
			var endRoom = rooms.Find(r => r.Id == 8);
			rooms.Remove(endRoom);

			// Take out Start Room (we always start there)
			var startRoom = rooms.Find(r => r.Id == 0);
			rooms.Remove(startRoom);

			// Pick Second Room, will always be a single Pillar room, help when placing backward pillars
			var secondRoom = rng.PickFrom(rooms.Where(r => r.Pillars.Count == 1).ToList());
			rooms.Remove(secondRoom);

			// Shuffle other rooms, put second room first
			rooms.Shuffle(rng);
			rooms.Insert(0, secondRoom);

			// Take a pillar and place it
			TrialsPillars nextpillar = rng.TakeFrom(startRoom.Pillars);
			placedRoomIds.Add(startRoom.Id);

			// go throught each room to create a path
			foreach (var room in rooms)
			{
				//InternalLogger.LogInfo($"Maze - Next Room: {room.Name}");

				// Assign pillar
				nextpillar.TargetRoom = room.Id;
				nextpillar.TargetPoint = RoomToPoint[room.Id];
				placedPillars.Add(nextpillar);

				// Take a random pillar from the room
				nextpillar = rng.TakeFrom(room.Pillars);
				
				// get rooms already visited for the remaining pillars
				foreach (var pillar in room.Pillars)
				{
					pillar.PreviousRooms = new(placedRoomIds);
					remainingPillars.Add(pillar);
				}

				placedRoomIds.Add(room.Id);
			}

			// Final pillars point to final room
			nextpillar.TargetRoom = endRoom.Id;
			nextpillar.TargetPoint = RoomToPoint[endRoom.Id];
			placedPillars.Add(nextpillar);

			// Take a random pillar from the remaining one, and point to any room
			var rngPillar = rng.TakeFrom(remainingPillars);
			var rngRoom = rng.PickFrom(RoomToPoint.Keys.ToList());
			rngPillar.TargetRoom = rngRoom;
			rngPillar.TargetPoint = RoomToPoint[rngRoom];
			placedPillars.Add(rngPillar);

			//InternalLogger.LogInfo($"Maze - Rng Room: {TrialsRooms.Find(r => r.Id == rngRoom).Name}");

			// Have all other remaining pillars point backward
			foreach (var pillar in remainingPillars)
			{
				bool capdistance = rng.Between(1, 5) > 1;
				var validRooms = RoomToPoint.Keys.ToList().Where(r => (capdistance & pillar.PreviousRooms.Count > 3) ?
					pillar.PreviousRooms.GetRange(pillar.PreviousRooms.Count - 3, 3).Contains(r) :
					pillar.PreviousRooms.Contains(r)).ToList();
				var targetRoom = rng.PickFrom(validRooms);
				pillar.TargetRoom = targetRoom;
				pillar.TargetPoint = RoomToPoint[targetRoom];
				placedPillars.Add(pillar);
				//InternalLogger.LogInfo($"Maze - Loop Room: {TrialsRooms.Find(r => r.Id == targetRoom).Name}");

			}

			var finalLayout = placedPillars.ToDictionary(p => p.OriginalTargetPoint, p => p.TargetPoint);

			/*
			foreach (var entry in finalLayout)
			{
				InternalLogger.LogInfo($"Maze - Final: {entry.Key} > {entry.Value}");
			}*/

			return finalLayout;
		}
	}
}
