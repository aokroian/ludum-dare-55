using _Game.Scripts.Map.Events;
using _Game.Scripts.Summon.View;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Map.Interactables
{
    public class Door: Interactable
    {
        [SerializeField] public bool forward;

        [Inject]
        private RoomSwitcher _roomSwitcher;
        [Inject]
        private SignalBus _signalBus;
        
        public override async void Interact(SummonedPlayer player)
        {
            _signalBus.Fire(new PlayerDoorOpenEvent());
            await _roomSwitcher.SwitchRoom(forward);
        }
    }
}