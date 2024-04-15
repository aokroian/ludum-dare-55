using System.Collections.Generic;
using _Game.Scripts.CharacterRelated._LD55;
using _Game.Scripts.CharacterRelated._LD55.Enemy;
using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using _Game.Scripts.Map;
using _Game.Scripts.Story;
using _Game.Scripts.Story.GameplayEvents;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon.View
{
    public class SummonedEnemy : SummonedObject
    {
        [SerializeField] private SimpleLoot[] lootOptions;
        [Inject] private PlayerInventoryService _playerInventoryService;

        private IGameplayEvent _currentEvent;
        private SummonedRoom _room;

        private void Start()
        {
            GetComponent<ActorHealth>().OnDeath += _ => OnDeath();
        }

        public override IGameplayEvent GetEventIfAny()
        {
            if (_currentEvent != null)
            {
                var ev = _currentEvent;
                _currentEvent = null;
                return ev;
            }

            return null;
        }

        private void OnDeath()
        {
            CurrentRoom.RemoveObject(this);
            DropLoot();
            Destroy(gameObject);
        }

        private void DropLoot()
        {
            var lootIndex = Random.Range(-1, lootOptions.Length);
            if (lootIndex == -1)
                return;
            var spawned = Instantiate(lootOptions[lootIndex], _room.transform);
            spawned.Init(_playerInventoryService);
        }

        public void Say(string message)
        {
            var p = new TextGameplayEvent.TextEventParams()
            {
                disableInput = false,
                Duration = 0,
                Speaker = this,
                Text = message
            };
            _currentEvent = new TextGameplayEvent(new List<TextGameplayEvent.TextEventParams>() { p }, null);
        }

        public override void OnMovedToRoom(SummonedRoom room)
        {
            base.OnMovedToRoom(room);
            _room = room;
            GetComponent<GuardianBrain>().Init(this, room.WalkArea, room.PatrolArea);
        }
    }
}