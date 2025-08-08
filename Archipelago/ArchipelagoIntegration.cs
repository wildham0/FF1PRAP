using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static TunicRandomizer.SaveFlags;
//using static TunicRandomizer.ERData;
using Archipelago.MultiClient.Net.Packets;
using Newtonsoft.Json.Linq;
using Archipelago.MultiClient.Net.Converters;
using FF1PRAP;
using Last.Interpreter;

namespace FF1PRAP {
	public class ArchipelagoIntegration {
		
		public bool connected {
			get { return session != null ? session.Socket.Connected : false; }
		}

		public ArchipelagoSession session;
		private IEnumerator<bool> incomingItemHandler;
		private IEnumerator<bool> checkItemsReceived;
		private ConcurrentQueue<(ItemInfo ItemInfo, int index)> incomingItems;
		//private DeathLinkService deathLinkService;
		public Dictionary<string, object> slotData;
		public bool disableSpoilerLog = false;
		public bool sentCompletion = false;
		public bool sentRelease = false;
		public bool sentCollect = false;
		public int ItemIndex = 0;
		public List<string> locationsToSend = new List<string>();
		public float locationsToSendTimer = 0.0f;
		public float locationsToSendDelay = 5.0f;
		private Version archipelagoVersion = new Version("0.6.2");

		public void Update()
		{
			if (FF1PR.SessionManager.GameMode != GameModes.Archipelago || !connected)
			{
				return;
			}

			if (checkItemsReceived != null)
			{
				checkItemsReceived.MoveNext();
			}

			if (FF1PR.GameState == GameStates.InGame)
			{
				if (incomingItemHandler != null) {
					incomingItemHandler.MoveNext();
				}
				if ((locationsToSendTimer > locationsToSendDelay && locationsToSend.Count > 0) || locationsToSend.Count >= 10) {
					SendQueuedLocations();
					locationsToSendTimer = 0.0f;
				}
				locationsToSendTimer += Time.fixedUnscaledDeltaTime;
			}
						
			if (FF1PR.DataStorage.Get(DataStorage.Category.kScenarioFlag1, (int)ScenarioFlags.ChaosDefeated) != 0 && !sentCompletion)
			{
				sentCompletion = true;
				SendCompletion();
			}
		}

		public string TryConnect() {
			
			if (connected && FF1PR.SessionManager.Data.Player == session.Players.GetPlayerName(session.ConnectionInfo.Slot)) {
				return "";
			}

			TryDisconnect();

			LoginResult LoginResult;

			if (session == null) {
				try {
					session = ArchipelagoSessionFactory.CreateSession(FF1PR.SessionManager.Data.Host, int.Parse(FF1PR.SessionManager.Data.Port));
				} catch (Exception e) {
					InternalLogger.LogInfo("Failed to create archipelago session!");
					InternalLogger.LogInfo(e.GetBaseException().Message);
				}
			}

			
			incomingItemHandler = IncomingItemHandler();
			checkItemsReceived = CheckItemsReceived();
			incomingItems = new ConcurrentQueue<(ItemInfo ItemInfo, int index)>();
			locationsToSend = new List<string>();

			try {
				LoginResult = session.TryConnectAndLogin("FF1 Pixel Remaster", FF1PR.SessionManager.Data.Player, ItemsHandlingFlags.AllItems, version: archipelagoVersion, requestSlotData: true, password: FF1PR.SessionManager.Data.Password);
			} catch (Exception e) {
				LoginResult = new LoginFailure(e.GetBaseException().Message);
			}

			if (LoginResult is LoginSuccessful LoginSuccess)
			{
				slotData = LoginSuccess.SlotData;
				ItemIndex = 0;

				InternalLogger.LogInfo("Successfully connected to Archipelago Multiworld server!");

				if (slotData.ContainsKey("disable_local_spoiler") && slotData["disable_local_spoiler"].ToString() == "1") {
					disableSpoilerLog = true;
					// Spoiler log setting
				} else {
					disableSpoilerLog = false;
				}

				FF1PR.SessionManager.Data.WorldSeed = Archipelago.instance.integration.session.RoomState.Seed;
				foreach (var option in Options.Dict)
				{
					if (slotData.ContainsKey(option.Key))
					{
						//InternalLogger.LogInfo($"{option.Key}: {slotData[option.Key].ToString()}");
						FF1PR.SessionManager.Options[option.Key] = slotData[option.Key].ToString();
					}
					else
					{
						//InternalLogger.LogInfo($"{option.Key} not found.");
						FF1PR.SessionManager.Options[option.Key] = option.Value.Default;
					}
				}

				

				//Archipelago.instance.integration.session.Locations.S
				/*
				if (Locations.LocationIdToArchipelagoId.Count == 0) {
					foreach (string Key in Locations.LocationDescriptionToId.Keys) {
						Locations.LocationIdToArchipelagoId.Add(Locations.LocationDescriptionToId[Key], Archipelago.instance.integration.session.Locations.GetLocationIdFromName("FF1 Pixel Remaster", Key));
						Archipelago.instance.integration.session.Locations.

					}
				}*/
				ScoutLocations();
				SetupDataStorage();

			} else {
				LoginFailure loginFailure = (LoginFailure)LoginResult;
				InternalLogger.LogInfo("Error connecting to Archipelago:");
				string TopLine = $"\"Failed to connect to Archipelago!\"";
				string BottomLine = $"\"Check your settings and/or log output.\"";
				foreach (string Error in loginFailure.Errors) {
					BottomLine = $"\"{Error}\"";
				}
			   // Notifications.Show(TopLine, BottomLine);
				foreach (ConnectionRefusedError Error in loginFailure.ErrorCodes) {
					InternalLogger.LogInfo(Error.ToString());
				}
				TryDisconnect();
				return BottomLine;
			}

			return "";
		}

		private async void ScoutLocations()
		{

			Dictionary<long, ApLocationData> locdata = new();

			foreach (var entry in Randomizer.FlagToLocationName)
			{
				var apid = Archipelago.instance.integration.session.Locations.GetLocationIdFromName("FF1 Pixel Remaster", entry.Value);
				locdata.Add(apid, new ApLocationData() { Flag = entry.Key, Id = apid, Name = entry.Value });
			}

			var content = await Archipelago.instance.integration.session.Locations.ScoutLocationsAsync(locdata.Select(l => l.Value.Id).ToArray());

			Dictionary<int, ApLocationData> aplocdata = new();

			foreach (var item in content)
			{
				string itemstring;
				if (item.Value.Player.Name == FF1PR.SessionManager.Data.Player)
				{
					itemstring = item.Value.ItemDisplayName;
				}
				else
				{
					itemstring = $"{item.Value.Player.Name}'s {item.Value.ItemDisplayName}";
				}

				if (locdata.TryGetValue(item.Key, out var location))
				{
					aplocdata.Add(location.Flag, new ApLocationData() { Flag = location.Flag, Id = item.Key, Name = location.Name, Content = itemstring });
				}
			}
			Randomizer.ApLocations = aplocdata;
			// End result is treasure flag dict > item info
			// we need the long id to get this
			// we get long ids by sending each location name
		}
		public void TrySilentReconnect() {
			LoginResult LoginResult;
			try {
				LoginResult = session.TryConnectAndLogin("FF1 Pixel Remaster", FF1PR.SessionManager.Data.Player, ItemsHandlingFlags.AllItems, version: archipelagoVersion, requestSlotData: true, password: FF1PR.SessionManager.Data.Password);
			} catch (Exception e) {
				LoginResult = new LoginFailure(e.GetBaseException().Message);
			}
		}

		public void TryDisconnect() {

			try {
				if (connected) {
					InternalLogger.LogInfo("Disconnected from Archipelago");
				}
				if (session != null) {
					session.Socket.DisconnectAsync();
					session = null;
				}

				incomingItemHandler = null;
				checkItemsReceived = null;
				disableSpoilerLog = false;
				incomingItems = new ConcurrentQueue<(ItemInfo ItemInfo, int ItemIndex)>();
				//locationsToSend = new List<long>();
				locationsToSend = new List<string>();
				slotData = null;
				ItemIndex = 0;
				//Locations.CheckedLocations.Clear();
				//ItemLookup.ItemList.Clear();

			} catch (Exception e) {
				InternalLogger.LogInfo("Encountered an error disconnecting from Archipelago! " + e);
			}
		}

		public void ClearQueue() {
			if (incomingItems != null) {
				incomingItems = new ConcurrentQueue<(ItemInfo ItemInfo, int ItemIndex)>();
			}
		}

		private IEnumerator<bool> CheckItemsReceived() {
			while (connected) {
				while (session.Items.AllItemsReceived.Count > ItemIndex) {
					ItemInfo ItemInfo = session.Items.AllItemsReceived[ItemIndex];
					InternalLogger.LogInfo("Placing item " + ItemInfo.ItemDisplayName + " with index " + ItemIndex + " in queue.");
					incomingItems.Enqueue((ItemInfo, ItemIndex));
					ItemIndex++;
					FF1PR.SessionManager.Data.ItemIndex = ItemIndex;
				}
				yield return true;
			}
		}

		private IEnumerator<bool> IncomingItemHandler() {

			while (connected) {
			
				if (!incomingItems.TryPeek(out var pendingItem)) {
					yield return true;
					continue;
				}

				InternalLogger.LogInfo($"Item incoming: {pendingItem.ItemInfo.ItemDisplayName}");
				var itemInfo = pendingItem.ItemInfo;
				var itemName = itemInfo.ItemDisplayName;
				var itemDisplayName = itemName + " (" + itemInfo.ItemId + ") at index " + pendingItem.index;

				/*
				if (SaveFile.GetInt($"randomizer processed item index {pendingItem.index}") == 1) {
					incomingItems.TryDequeue(out _);
					InternalLogger.LogInfo("Skipping item " + itemName + " at index " + pendingItem.index + " as it has already been processed.");
					continue;
				}*/

				// Delay until a few seconds after connecting/screen transition
			   /* while (SaveFile.GetFloat("playtime") < SceneLoaderPatches.TimeOfLastSceneTransition + 3.0f) {
					yield return true;
				}*/

				var handleResult = Patches.GiveItem(itemName, FF1PR.SessionManager.Data.Player != itemInfo.Player.Name);
				switch (handleResult) {
					case Patches.ItemResults.Success:
						InternalLogger.LogInfo("Received " + itemDisplayName + " from " + itemInfo.Player.Name + " at " + itemInfo.LocationDisplayName);

						incomingItems.TryDequeue(out _);
						//SaveFile.SetInt($"randomizer processed item index {pendingItem.index}", 1);

						// Wait for all interactions to finish
						/*
						while (
							GenericMessage.instance.isActiveAndEnabled ||
							GenericPrompt.instance.isActiveAndEnabled ||
							ItemPresentation.instance.isActiveAndEnabled ||
							PageDisplay.instance.isActiveAndEnabled ||
							NPCDialogue.instance.isActiveAndEnabled || 
							PlayerCharacter.InstanceIsDead) {
							yield return true;
						}
						*/
						// Pause before processing next item
						DateTime postInteractionStart = DateTime.Now;
						while (DateTime.Now < postInteractionStart + TimeSpan.FromSeconds(incomingItems.Count > 10 ? 1f : 2f))
						{
							yield return true;
						}
						break;

					case Patches.ItemResults.Busy:
						InternalLogger.LogDebug("Player is busy, will retry processing item: " + itemDisplayName);
						break;

					case Patches.ItemResults.Invalid:
						InternalLogger.LogWarning("Failed to process item " + itemDisplayName);
						incomingItems.TryDequeue(out _);
						//SaveFile.SetInt($"randomizer processed item index {pendingItem.index}", 1);
						break;
				}

				yield return true;
			}
		}

		public void ActivateCheck(string LocationName) {
			if (LocationName != null) {
				InternalLogger.LogInfo("Checked location " + LocationName);
				//string GameObjectId = Randomizer.LocationDescriptionToId[LocationName];
				//var location = ItemLookup.ItemList[GameObjectId].LocationId;

				if (connected)
				{
					// Send the location right away
					var itemdid = session.Locations.GetLocationIdFromName("FF1 Pixel Remaster", LocationName);
					session.Locations.CompleteLocationChecks(itemdid);
				}
				else
				{
					// queue location
					locationsToSend.Add(LocationName);
				}

				//SaveFile.SetInt(ItemCollectedKey + GameObjectId, 1);
				/*
				Locations.CheckedLocations[GameObjectId] = true;

				ItemInfo itemInfo = ItemLookup.ItemList[GameObjectId];
				string receiver = itemInfo.Player.Name;
				string itemName = itemInfo.ItemDisplayName;
				InternalLogger.LogInfo("Sent " + itemName + " at " + location + " to " + receiver);
				if (itemInfo.Player != session.ConnectionInfo.Slot) {
					SaveFile.SetInt("archipelago items sent to other players", SaveFile.GetInt("archipelago items sent to other players") + 1);
					Notifications.Show($"yoo sehnt  {(TextBuilderPatches.ItemNameToAbbreviation.ContainsKey(itemName) && Archipelago.instance.IsTunicPlayer(itemInfo.Player) ? TextBuilderPatches.ItemNameToAbbreviation[itemName] : "[archipelago]")}  \"{itemName.Replace("_", " ")}\" too \"{receiver}!\"", $"hOp #A lIk it!");
					RecentItemsDisplay.instance.EnqueueItem(itemInfo, false);
				}*/

			} else {
				InternalLogger.LogWarning("Failed to get unique name for check " + LocationName);
			}
		}

		public void SendQueuedLocations() {
			if (locationsToSend.Count > 0) {
				InternalLogger.LogInfo("Sending queued checks: " + string.Join(", ", locationsToSend));
				var locationIds = new List<long>();

				foreach (var location in locationsToSend)
				{
					var itemdid = session.Locations.GetLocationIdFromName("FF1 Pixel Remaster", location);
					locationIds.Add(itemdid);
				}

				session.Locations.CompleteLocationChecks(locationIds.ToArray());
				locationsToSend.Clear();
			}
		}

		public void CompleteLocationCheck(string LocationName) {
			if (LocationName != null) {
				var location = session.Locations.GetLocationIdFromName(session.ConnectionInfo.Game, LocationName);
				session.Locations.CompleteLocationChecks(location);

			}
		}

		public void SendCompletion() {
			session.SetGoalAchieved();
			UpdateDataStorage("Chaos defeated", true);
		}

		public void Release() {
			if (connected && sentCompletion && !sentRelease) {
				session.Say("!release");
				sentRelease = true;
				InternalLogger.LogInfo("Released remaining checks.");
			}
		}

		public void Collect() {
			if (connected && sentCompletion && !sentCollect) {
				session.Say("!collect");
				sentCollect = true;
				InternalLogger.LogInfo("Collected remaining items.");
			}
		}
		private void SetupDataStorage() {
			if (session != null) {
				InternalLogger.LogInfo("Initializing DataStorage values");
				// Map to Display
				session.DataStorage[Scope.Slot, "Current Map"].Initialize("Overworld");
				session.DataStorage[Scope.Slot, "Entrance Tracker Map"].Initialize("overworld");
			}
		}

		public void UpdateDataStorage(string Key, object Value, bool Log = true) {

			if (Value is bool) {
				session.DataStorage[Scope.Slot, Key] = (bool)Value;
			}
			if (Value is int) {
				session.DataStorage[Scope.Slot, Key] = (int)Value;
			}
			if (Value is string) {
				session.DataStorage[Scope.Slot, Key] = (string)Value;
			}
			if (Log) {
				InternalLogger.LogInfo("Setting DataStorage value \"" + Key + "\" to " + Value);
			}
		}

		public void UpdateDataStorageOnLoad() {

		}

		public Dictionary<string, int> GetStartInventory() {

			Dictionary<string, int> startInventory = new Dictionary<string, int>();
			/*
			if (connected && session != null) {
				// start inventory items have a location ID of -2, add them to a dict so we can use them for first steps
				foreach (ItemInfo item in session.Items.AllItemsReceived) {
					if (item.LocationId == -2) {
						string itemName = item.ItemDisplayName;
						if (ItemLookup.Items.ContainsKey(itemName)) {
							TunicUtils.AddStringToDict(startInventory, ItemLookup.Items[itemName].ItemNameForInventory);
						}
					}
				}
			}*/
			return startInventory;
		}

		public void ShowNotConnectedError() {
			InternalLogger.LogWarning($"[archipelago] \"ERROR: Lost connection to Archipelago! Unable to send or receive items. Re-connect and try again.\"");
		}
	}
}
