using System.Collections.Generic;
using _Game.Scripts.CharacterRelated._LD55;
using _Game.Scripts.CharacterRelated._LD55.Enemy;
using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using _Game.Scripts.Map;
using _Game.Scripts.Story;
using _Game.Scripts.Story.Events;
using _Game.Scripts.Story.GameplayEvents;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Summon.View
{
    public class SummonedEnemy : SummonedObject
    {
        [SerializeField] private SimpleLoot[] lootOptions;
        [Inject]
        private PlayerInventoryService _playerInventoryService;
        [Inject]
        private SignalBus _signalBus;

        private IGameplayEvent _currentEvent;
        private SummonedRoom _room;

        private int _roomIndex;
        private bool _warningSaid;

        protected override void Start()
        {
            base.Start();
            GetComponent<ActorHealth>().OnDeath += _ => OnDeath();
            _signalBus.Subscribe<EndingStartedEvent>(OnEnding);

            _roomIndex = ObjectsHolder.GetRoomIndexForObject(this);
        }

        public override IGameplayEvent GetEventIfAny()
        {
            if (_currentEvent != null)
            {
                var ev = _currentEvent;
                _currentEvent = null;
                return ev;
            }
            
            if (!_warningSaid && ObjectsHolder.GetPlayerRoomIndex() == _roomIndex)
            {
                Say("Hey, don't come any closer!");
                _warningSaid = true;
            }

            return null;
        }

        private void OnDeath()
        {
            CurrentRoom.RemoveObject(this);
            DropLoot();
            Destroy(gameObject);
        }

        private void OnEnding()
        {
            GetComponent<GuardianBrain>().CalmDown();
        }

        private void DropLoot()
        {
            var lootIndex = Random.Range(-1, lootOptions.Length);
            if (lootIndex == -1)
                return;
            var spawned = Instantiate(lootOptions[lootIndex], _room.transform);
            spawned.Init(_playerInventoryService);
            spawned.transform.position = transform.position;
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

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<EndingStartedEvent>(OnEnding);
        }
    }
}