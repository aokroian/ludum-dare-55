using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.CharacterRelated._LD55;
using _Game.Scripts.CharacterRelated._LD55.Events;
using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using _Game.Scripts.Common;
using _Game.Scripts.Story;
using _Game.Scripts.Story.GameplayEvents;
using _Game.Scripts.Summon.Data;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon.View
{
    public class SummonedBard : SummonedObject
    {
        [SerializeField] private BardBrain brain;
        [SerializeField] private AudioClip deathSound;

        [Inject] private SoundManager _soundManager;
        [Inject] private SummonedObjectsHolder _objectsHolder;
        [Inject] private SignalBus _signalBus;
        private IGameplayEvent _currentEvent;
        private bool _isPerformedOnSummonEffects;
        private int _roomIndex;
        private ActorHealth _actorHealth;

        public bool IsKillOneselfScheduled { get; private set; }

        protected override void Start()
        {
            base.Start();
            _actorHealth = GetComponent<ActorHealth>();
            _actorHealth.OnDeath += _ => OnDeath();
            _signalBus.Subscribe<BardSummonedEvent>(OnAnotherBardSpawned);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<BardSummonedEvent>(OnAnotherBardSpawned);
        }

        public override IGameplayEvent GetEventIfAny()
        {
            if (_currentEvent != null)
            {
                var ev = _currentEvent;
                _currentEvent = null;
                return ev;
            }

            var player = ObjectsHolder.GetPlayer();
            if (!_isPerformedOnSummonEffects && player != null)
            {
                Say("Listen to my beautiful music!");
                _isPerformedOnSummonEffects = true;
                brain.Init(player.bardWalkArea);
            }

            return null;
        }

        private void OnAnotherBardSpawned(BardSummonedEvent bardSummonedEvent)
        {
            if (bardSummonedEvent.Bard == this)
                return;
            Say("...why did you need another one"); // need one frame delay to process the event
            IsKillOneselfScheduled = true;
            Invoke(nameof(KillOneself), 0.1f);
        }

        public override void OnMovedToRoom(SummonedRoom room)
        {
            base.OnMovedToRoom(room);
            var princessInRoom = room.Objects.FirstOrDefault(obj => obj is SummonedPrincess);
            if (princessInRoom != null)
            {
                Say("I will protect you, my princess!");
                brain.ChasePrincess(princessInRoom as SummonedPrincess, room.WalkArea);
            }
        }


        private void KillOneself()
        {
            _actorHealth.TakeDamage(100000);
        }

        private void OnDeath()
        {
            _signalBus.TryUnsubscribe<BardSummonedEvent>(OnAnotherBardSpawned);

            if (CurrentRoom != null)
                CurrentRoom.RemoveObject(this);
            else
                _objectsHolder.ObjectsOutOfRoom.Remove(this);

            if (_isPerformedOnSummonEffects)
            {
                _soundManager.PlayBardDeathSound();
            }

            Destroy(gameObject);
        }

        private void Say(string message)
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
    }
}