using System.Collections.Generic;
using _Game.Scripts.CharacterRelated._LD55.Enemy;
using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using _Game.Scripts.Story;
using _Game.Scripts.Story.GameplayEvents;

namespace _Game.Scripts.Summon.View
{
    public class SummonedEnemy: SummonedObject
    {
        private IGameplayEvent _currentEvent;

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
            Destroy(gameObject);
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
            _currentEvent = new TextGameplayEvent(new List<TextGameplayEvent.TextEventParams>(){p}, null);
        }

        public override void OnMovedToRoom(SummonedRoom room)
        {
            base.OnMovedToRoom(room);
            GetComponent<GuardianBrain>().Init(this, room.WalkArea, room.PatrolArea);
        }
    }
}