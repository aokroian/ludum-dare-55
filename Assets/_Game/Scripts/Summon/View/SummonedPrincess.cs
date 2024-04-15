using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Story;
using _Game.Scripts.Story.Characters.Princess;
using _Game.Scripts.Story.Events;
using _Game.Scripts.Story.GameplayEvents;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon.View
{
    public class SummonedPrincess: SummonedObject
    {
        private PrincessWandering _princessWandering;
        
        [Inject]
        private SignalBus _signalBus;

        private int _roomIndex;
        
        private bool _helpSaid;

        protected override void Start()
        {
            base.Start();
            _signalBus.Subscribe<EndingStartedEvent>(OnEnding);
            
            _roomIndex = ObjectsHolder.GetRoomIndexForObject(this);
        }

        public override IGameplayEvent GetEventIfAny()
        {
            if (ObjectsHolder.GetObjectsById(Id).Count > 1)
            {
                return new ManyPrincessesGameplayEvent(this, MessageService);
            }

            if (_roomIndex < 2 && Vector3.Distance(transform.position, ObjectsHolder.GetPlayer().transform.position) < 1.5f)
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
                return new TextGameplayEvent(new List<TextGameplayEvent.TextEventParams>() { p, p2 }, "rejectedByPrincess");
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

            if (_roomIndex >= 2 &&
                ObjectsHolder.Rooms[_roomIndex].Objects.FirstOrDefault(it => it is SummonedEnemy) == null &&
                Vector3.Distance(transform.position, ObjectsHolder.GetPlayer().transform.position) < 1.5f)
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
        }
        
        private void OnEnding()
        {
            _princessWandering.TogglePause(true);
        }
    }
}