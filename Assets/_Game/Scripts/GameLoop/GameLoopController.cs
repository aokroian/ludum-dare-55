using _Game.Scripts.GameLoop.Events;
using _Game.Scripts.Story;
using Zenject;

namespace _Game.Scripts.GameLoop
{
    public class GameLoopController
    {
        private SignalBus _signalBus;
        private GameplayEventsChecker _eventsChecker;
        private GlobalInputSwitcher _inputSwitcher;

        public GameLoopController(SignalBus signalBus, GameplayEventsChecker eventsChecker, GlobalInputSwitcher inputSwitcher)
        {
            _signalBus = signalBus;
            _eventsChecker = eventsChecker;
            _inputSwitcher = inputSwitcher;
            
            _signalBus.Subscribe<GameEndEvent>(OnEndGame);
        }
        
        public bool IsGameEnded { get; private set; }
        
        private void OnEndGame()
        {
            IsGameEnded = true;
            _eventsChecker.CheckingEnabled = false;
            _inputSwitcher.ToggleLockToConsole(true);
        }

        public void RestartGame()
        {
            IsGameEnded = false;
            // TODO: Restart everything
            
            _eventsChecker.CheckingEnabled = true;
            _inputSwitcher.ToggleLockToConsole(false);
        }
    }
}