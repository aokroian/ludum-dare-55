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
        
        public override async void Interact(SummonedPlayer player)
        {
            await _roomSwitcher.SwitchRoom(forward);
        }
    }
}