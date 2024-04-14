using System.Linq;
using _Game.Scripts.Story;
using _Game.Scripts.Story.GameplayEvents;

namespace _Game.Scripts.Summon.View
{
    public class SummonedPlayer : SummonedObject
    {
        public override IGameplayEvent GetEventIfAny()
        {
            if (ObjectsHolder.rooms.Count == 0)
            {
                return new PlayerInSpaceGameplayEvent(this, MessageService);
            }

            if (ObjectsHolder.GetCurrentPlayerRoom().Objects.Count(it => it is SummonedPlayer) > 1)
            {
            }

            return null;
        }
    }
}