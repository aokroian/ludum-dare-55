using _Game.Scripts.Summon.View;

namespace _Game.Scripts.GameLoop.Events
{
    public class SummonBardEvent
    {
        public readonly SummonedBard Bard;

        public SummonBardEvent(SummonedBard bard)
        {
            Bard = bard;
        }
    }
}