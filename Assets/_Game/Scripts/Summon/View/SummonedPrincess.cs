using System.Linq;
using _Game.Scripts.Story;
using _Game.Scripts.Story.Characters.Princess;
using _Game.Scripts.Story.Events;
using _Game.Scripts.Story.GameplayEvents;
using Zenject;

namespace _Game.Scripts.Summon.View
{
    public class SummonedPrincess: SummonedObject
    {
        private PrincessWandering _princessWandering;
        
        [Inject]
        private SignalBus _signalBus;

        private void Start()
        {
            _signalBus.Subscribe<EndingStartedEvent>(OnEnding);
        }

        public override IGameplayEvent GetEventIfAny()
        {
            if (ObjectsHolder.GetPlayerRoomOrFirst().Objects.Count(it => it is SummonedPrincess) > 1)
            {
                return new ManyPrincessesGameplayEvent(this, MessageService);
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