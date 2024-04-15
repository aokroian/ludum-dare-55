using _Game.Scripts.CharacterRelated._LD55;
using UnityEngine;

namespace _Game.Scripts.Map
{
    public enum LootType
    {
        Coin,
        Key,
    }

    public class SimpleLoot : MonoBehaviour
    {
        public LootType type;
        private PlayerInventoryService _playerInventoryService;

        public void Init(PlayerInventoryService inventoryService)
        {
            _playerInventoryService = inventoryService;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInventoryService.AddItem(type);
                Destroy(gameObject);
            }
        }
    }
}