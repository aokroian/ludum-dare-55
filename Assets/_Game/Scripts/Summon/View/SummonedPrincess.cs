using _Game.Scripts.Story;
using _Game.Scripts.Story.Characters.Princess;

namespace _Game.Scripts.Summon.View
{
    public class SummonedPrincess: SummonedObject
    {
        private PrincessWandering _princessWandering;

        public override IGameplayEvent GetEventIfAny()
        {
            return null;
        }

        public override void OnMovedToRoom(SummonedRoom room)
        {
            base.OnMovedToRoom(room);
            _princessWandering = GetComponent<PrincessWandering>();
            _princessWandering.Init(room.WalkArea);
        }
    }
}