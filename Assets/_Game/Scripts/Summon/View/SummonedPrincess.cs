using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.CharacterRelated._LD55;
using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using _Game.Scripts.Story;
using _Game.Scripts.Story.Characters.Princess;
using _Game.Scripts.Story.Events;
using _Game.Scripts.Story.GameplayEvents;
using _Game.Scripts.Summon.Enums;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon.View
{
    public class SummonedPrincess : SummonedObject
    {
        private PrincessWandering _princessWandering;

        [Inject]
        private SignalBus _signalBus;

        private int _roomIndex;

        private bool _helpSaid;
        private ActorHealth _actorHealth;
        private bool _isDead;
        private NpcAnimations _animations;

        protected override void Start()
        {
            base.Start();
            _signalBus.Subscribe<EndingStartedEvent>(OnEnding);
            _actorHealth = GetComponent<ActorHealth>();
            _animations = GetComponent<NpcAnimations>();
            _actorHealth.OnDeath += OnDeath;

            _roomIndex = ObjectsHolder.GetRoomIndexForObject(this);
        }

        private void OnDestroy()
        {
            _actorHealth.OnDeath -= OnDeath;
        }

        private void OnDeath(ActorHealth actorHealth)
        {
            _isDead = true;
            _animations.SetDead();
        }

        public override IGameplayEvent GetEventIfAny()
        {
            if (ObjectsHolder.GetObjectsById(Id).Count > 1)
            {
                return new ManyPrincessesGameplayEvent(this, MessageService);
            }

            var player = ObjectsHolder.GetPlayer();
            if (_roomIndex < 2 && player != null &&
                Vector3.Distance(transform.position, ObjectsHolder.GetPlayer().transform.position) < 1.5f)
            {
                var p = new TextGameplayEvent.TextEventParams()
                {
                    disableInput = true,
                    Duration = 1,
                    Speaker = ObjectsHolder.GetPlayer(),
                    Text = "Princess, I'm here to save you!"
                };
                var p2 = new TextGameplayEvent.TextEventParams()
                {
                    disableInput = true,
                    Duration = 1,
                    Speaker = this,
                    Text = "What do you mean, I am just wandering around!"
                };
                return new TextGameplayEvent(new List<TextGameplayEvent.TextEventParams>() { p, p2 },
                    "rejectedByPrincess");
            }

            if (!_helpSaid && _roomIndex >= 2)
            {
                _helpSaid = true;
                var p = new TextGameplayEvent.TextEventParams()
                {
                    disableInput = false,
                    Duration = 0,
                    Speaker = this,
                    Text = "Help me!"
                };
                return new TextGameplayEvent(new List<TextGameplayEvent.TextEventParams>() { p }, null);
            }

            if (_isDead)
            {
                var p = new TextGameplayEvent.TextEventParams()
                {
                    disableInput = true,
                    Duration = 0.5f,
                    Speaker = this,
                    Text = "Oh dear, this wasn't in the script!"
                };
                return new TextGameplayEvent(new List<TextGameplayEvent.TextEventParams>() { p }, "deadPrincess");
            }

            if (_roomIndex >= 2 &&
                ObjectsHolder.Rooms[_roomIndex].Objects.FirstOrDefault(it => it is SummonedEnemy) == null &&
                player != null &&
                Vector3.Distance(transform.position, player.transform.position) < 1.5f)
            {
                var p = new TextGameplayEvent.TextEventParams()
                {
                    disableInput = true,
                    Duration = 0.5f,
                    Speaker = this,
                    Text = "You saved me! Thank you!"
                };
                return new TextGameplayEvent(new List<TextGameplayEvent.TextEventParams>() { p }, "savePrincess");
            }

            return null;
        }

        public override void OnMovedToRoom(SummonedRoom room)
        {
            base.OnMovedToRoom(room);
            _princessWandering = GetComponent<PrincessWandering>();
            _princessWandering.Init(room.WalkArea);
            if (room.RoomType == RoomType.Prison)
                _princessWandering.TogglePause(true);
        }

        private void OnEnding()
        {
            _princessWandering?.TogglePause(true);
        }
    }
}