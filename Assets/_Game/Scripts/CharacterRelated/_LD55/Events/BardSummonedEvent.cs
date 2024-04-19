using _Game.Scripts.Summon.View;

namespace _Game.Scripts.CharacterRelated._LD55.Events
{
    public class BardSummonedEvent
    {
        public readonly SummonedBard Bard;

        public BardSummonedEvent(SummonedBard bard)
        {
            Bard = bard;
        }
    }
}