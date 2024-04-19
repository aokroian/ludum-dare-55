using System.Collections.Generic;
using _Game.Scripts.CharacterRelated._LD55;
using _Game.Scripts.CharacterRelated._LD55.Events;
using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using _Game.Scripts.Common;
using _Game.Scripts.Story;
using _Game.Scripts.Story.GameplayEvents;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon.View
{
    public class SummonedBard : SummonedObject
    {
        [SerializeField] private BardBrain brain;
        [SerializeField] private AudioClip musicClip;

        [Inject] private SoundManager _soundManager;
        [Inject] private SignalBus _signalBus;
        private IGameplayEvent _currentEvent;
        private bool _summonedEventMessageSaid;
        private int _roomIndex;

        protected override void Start()
        {
            base.Start();
            GetComponent<ActorHealth>().OnDeath += _ => OnDeath();
            _signalBus.Subscribe<BardSummonedEvent>(OnAnotherBardSpawned);
        }

        public override IGameplayEvent GetEventIfAny()
        {
            if (_currentEvent != null)
            {
                var ev = _currentEvent;
                _currentEvent = null;
                return ev;
            }

            if (!_summonedEventMessageSaid && ObjectsHolder.GetPlayer() != null)
            {
                Say("Listen to my beautiful music!");
                _summonedEventMessageSaid = true;
            }

            var player = ObjectsHolder.GetPlayer();
            if (player != null)
                brain.Init(player.bardWalkArea);
            _soundManager.PlayBardMusic(musicClip);
            return null;
        }

        private void OnAnotherBardSpawned(BardSummonedEvent bardSummonedEvent)
        {
            if (bardSummonedEvent.Bard == this)
                return;
            Say("...why did you need another one");
            GetComponent<ActorHealth>().TakeDamage(100000);
        }

        private void OnDeath()
        {
            // stop music here
            _signalBus.TryUnsubscribe<BardSummonedEvent>(OnAnotherBardSpawned);
            if (CurrentRoom != null)
                CurrentRoom.RemoveObject(this);
            _soundManager.PlayDefaultMusic();
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