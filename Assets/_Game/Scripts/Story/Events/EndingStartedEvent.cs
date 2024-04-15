using _Game.Scripts.Story.Ending;

namespace _Game.Scripts.Story.Events
{
    public class EndingStartedEvent
    {
        public readonly EndingService.EndingData EndingData;

        public EndingStartedEvent(EndingService.EndingData endingData)
        {
            EndingData = endingData;
        }
    }
}