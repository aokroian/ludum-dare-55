using System.Collections.Generic;
using _Game.Scripts.CharacterRelated._LD55.Events;
using _Game.Scripts.GameLoop.Events;
using _Game.Scripts.Map;
using _Game.Scripts.Map.Events;
using Zenject;

namespace _Game.Scripts.CharacterRelated._LD55
{
    public class PlayerInventoryService
    {
        private SignalBus _signalBus;

        public readonly Dictionary<LootType, int> Inventory = new();

        public PlayerInventoryService(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<GameStartEvent>(ClearInventory);
            ClearInventory();
        }
        
        public int GetItemCount(LootType type)
        {
            return Inventory.GetValueOrDefault(type, 0);
        }

        private void ClearInventory()
        {
            Inventory.Clear();
            Inventory.Add(LootType.Coin, 0);
            Inventory.Add(LootType.Key, 0);
            _signalBus.Fire<PlayerInventoryChangedEvent>();
        }

        public void AddItem(LootType type)
        {
            if (type == LootType.Coin)
                _signalBus.Fire(new PlayerCoinPickupEvent());
            else if (type == LootType.Key)
                _signalBus.Fire(new PlayerKeyPickupEvent());

            Inventory.TryAdd(type, 0);
            Inventory[type]++;
            _signalBus.Fire<PlayerInventoryChangedEvent>();
        }

        public bool TryRemoveItem(LootType type)
        {
            if (Inventory.ContainsKey(type) && Inventory[type] > 0)
            {
                Inventory[type]--;
                _signalBus.Fire<PlayerInventoryChangedEvent>();
                return true;
            }

            return false;
        }
    }
}