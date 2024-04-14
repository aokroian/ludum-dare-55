namespace _Game.Scripts.GameLoop.Events
{
    public class GameEndEvent
    {
        public string EndingId { get; }
        
        public GameEndEvent(string endingId)
        {
            EndingId = endingId;
        }
    }
}