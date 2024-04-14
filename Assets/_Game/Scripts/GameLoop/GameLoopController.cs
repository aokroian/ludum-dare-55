using _Game.Scripts.GameLoop.Events;
using _Game.Scripts.Story;
using Zenject;

namespace _Game.Scripts.GameLoop
{
    public class GameLoopController
    {
        private SignalBus _signalBus;
        private GameplayEventsChecker _eventsChecker;

        public GameLoopController(SignalBus signalBus, GameplayEventsChecker eventsChecker)
        {
            _signalBus = signalBus;
            _eventsChecker = eventsChecker;
            
            _signalBus.Subscribe<GameEndEvent>(OnEndGame);
        }
        
        public bool IsGameEnded { get; private set; }
        
        private void OnEndGame()
        {
            IsGameEnded = true;
            _eventsChecker.CheckingEnabled = false;
        }

        public void RestartGame()
        {
            IsGameEnded = false;
            // TODO: Restart everything
            
            _eventsChecker.CheckingEnabled = true;
        }
    }
}