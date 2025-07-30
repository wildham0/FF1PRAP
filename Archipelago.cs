using Archipelago.MultiClient.Net.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FF1PRAP {
    public class Archipelago : MonoBehaviour {
        public static Archipelago instance { get; set; }

        public ArchipelagoIntegration integration;

        public void Start() {
            integration = new ArchipelagoIntegration();
			InternalLogger.LogInfo("Ap Instance Started here");
        }

        public void Update() {
            integration.Update();
        }

        public void OnDestroy() {
            integration.TryDisconnect();
        }

        public string Connect() {
            return integration.TryConnect();
        }

        public void SilentReconnect() {
            integration.TrySilentReconnect();
        }

        public void Disconnect() {
            integration.TryDisconnect();
        }

        public void ActivateCheck(string LocationName) {
            integration.ActivateCheck(LocationName);
        }

        public void CompleteLocationCheck(string LocationName) {
            integration.CompleteLocationCheck(LocationName);
        }

        public void UpdateDataStorage(string Key, object Value) {
			/* if (SaveFlags.IsArchipelago()) {
				 integration.UpdateDataStorage(Key, Value);
			 }*/
			integration.UpdateDataStorage(Key, Value);
		}

        public void Release() {
            integration.Release();
        }

        public void Collect() {
            integration.Collect();
        }

		/*public Il2CppSystem.Collections.Generic.Dictionary<string, object> GetPlayerSlotData() {

			var slotdatadict = new Il2CppSystem.Collections.Generic.Dictionary<string, object>();

			foreach (var entry in integration.slotData)
			{
				slotdatadict.Add(entry.Key, entry.Value);
			}

            return slotdatadict;
        }*/

		public Dictionary<string, object> GetPlayerSlotData()
		{
			return integration.slotData;
		}
		public int GetPlayerSlot() {
            return integration.session.ConnectionInfo.Slot;
        }

        public string GetPlayerName(int Slot) {
            return integration.session.Players.GetPlayerName(Slot).Replace("{", "").Replace("}", "");
        }

        public string GetPlayerGame(int Slot) {
            return integration.session.Players.Players[0][Slot].Game;
        }

        public bool IsFF1PRPlayer(int Slot) {
            return GetPlayerGame(Slot) == "FF1 Pixel Remaster" && integration.session.Players.GetPlayerInfo(Slot).GetGroupMembers(integration.session.Players) == null;
        }

        public string GetItemName(long id, string game) {
            return integration.session.Items.GetItemName(id, game);
        }

        public string GetLocationName(long id, string game) { 
            return integration.session.Locations.GetLocationNameFromId(id, game);
        }

        public long GetLocationId(string name, string game) {
            return integration.session.Locations.GetLocationIdFromName(game, name);
        }

		public void SetItemIndex(int index)
		{
			integration.ItemIndex = index;
		}
        public bool IsConnected() {
            return integration != null ? integration.connected : false;
        }

    }
}
