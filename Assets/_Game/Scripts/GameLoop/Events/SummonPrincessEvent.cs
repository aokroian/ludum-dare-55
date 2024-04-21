using _Game.Scripts.Summon.View;

namespace _Game.Scripts.GameLoop.Events
{
    public class SummonPrincessEvent
    {
        public readonly SummonedPrincess Princess;

        public SummonPrincessEvent(SummonedPrincess princess)
        {
            Princess = princess;
        }
    }
}