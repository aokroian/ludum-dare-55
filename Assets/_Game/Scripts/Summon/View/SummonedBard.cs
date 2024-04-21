using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.CharacterRelated._LD55;
using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using _Game.Scripts.Common;
using _Game.Scripts.GameLoop.Events;
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
        private bool _isTouchedPrincess;

        public bool IsKillOneselfScheduled { get; private set; }

        protected override void Start()
        {
            base.Start();
            _actorHealth = GetComponent<ActorHealth>();
            _actorHealth.OnDeath += _ => OnDeath();
            _signalBus.Subscribe<SummonBardEvent>(OnBardSummoned);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<SummonBardEvent>(OnBardSummoned);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.TryGetComponent<SummonedPrincess>(out var princess);
            if (princess == null)
                return;
            _isTouchedPrincess = true;
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
                brain.WanderAroundPlayer(player.bardWalkArea);
            }

            TryToChasePrincess();

            if (_isTouchedPrincess)
            {
                var p = new TextGameplayEvent.TextEventParams()
                {
                    disableInput = true,
                    Duration = 0.5f,
                    Speaker = this,
                    Text = "Let's go, my princess!"
                };
                return new TextGameplayEvent(new List<TextGameplayEvent.TextEventParams>() { p }, "bardSavesPrincess");
            }

            return null;
        }

        private void OnBardSummoned(SummonBardEvent summonBardEvent)
        {
            if (summonBardEvent.Bard == this)
                return;
            Say("...why did you need another one"); // need one frame delay to process the event
            IsKillOneselfScheduled = true;
            Invoke(nameof(KillOneself), 0.1f);
        }

        private void TryToChasePrincess()
        {
            if (CurrentRoom == null)
                return;
            var princessInRoom = CurrentRoom.Objects.FirstOrDefault(obj => obj is SummonedPrincess);
            if (princessInRoom == null)
                return;
            if (brain.IsChasingPrincess)
                return;
            Say("I will protect you, my princess!");
            brain.ChasePrincess(princessInRoom.transform, CurrentRoom.WalkArea);
        }

        private void KillOneself()
        {
            _actorHealth.TakeDamage(100000);
        }

        private void OnDeath()
        {
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