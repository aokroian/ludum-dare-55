using _Game.Scripts.CharacterRelated._LD55;
using _Game.Scripts.Map.Events;
using _Game.Scripts.Message;
using _Game.Scripts.Summon.View;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Map.Interactables
{
    public class Door : Interactable
    {
        [SerializeField] public bool forward;

        [Inject]
        private RoomSwitcher _roomSwitcher;
        [Inject]
        private SignalBus _signalBus;
        [Inject]
        private PlayerInventoryService _playerInventoryService;
        [Inject]
        private MessageService _messageService;

        public bool isLocked = true;

        public override async void Interact(SummonedPlayer player)
        {
            if (isLocked)
            {
                if (!_playerInventoryService.TryRemoveItem(LootType.Key))
                {
                    _messageService.Speak(player, "Looks like this door is closed.\nI'm gonna need a key.");
                    return;
                }

                _messageService.Speak(player, "That door took my key!");
                isLocked = false;
            }

            _signalBus.Fire(new PlayerDoorOpenEvent());
            await _roomSwitcher.SwitchRoom(forward);
        }
    }
}