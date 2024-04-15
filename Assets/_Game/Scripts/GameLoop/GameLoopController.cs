using _Game.Scripts.Common;
using _Game.Scripts.GameLoop.Events;
using _Game.Scripts.Map;
using _Game.Scripts.Story;
using _Game.Scripts.Summon.Data;
using Zenject;

namespace _Game.Scripts.GameLoop
{
    public class GameLoopController
    {
        private SignalBus _signalBus;
        private GameplayEventsChecker _eventsChecker;
        private GlobalInputSwitcher _inputSwitcher;
        private SummonedObjectsHolder _objectsHolder;
        private CameraWrapper _cameraWrapper;

        public GameLoopController(SignalBus signalBus,
            GameplayEventsChecker eventsChecker,
            GlobalInputSwitcher inputSwitcher,
            SummonedObjectsHolder objectsHolder, 
            CameraWrapper cameraWrapper)
        {
            _cameraWrapper = cameraWrapper;
            _signalBus = signalBus;
            _eventsChecker = eventsChecker;
            _inputSwitcher = inputSwitcher;
            _objectsHolder = objectsHolder;
            
            _signalBus.Subscribe<GameEndEvent>(OnEndGame);
            _signalBus.Subscribe<GameStartEvent>(StartGame);
        }
        
        public bool IsGameEnded { get; private set; }
        
        private void StartGame(GameStartEvent startEvent)
        {
            IsGameEnded = false;
            // TODO: Restart everything
            _objectsHolder.ClearEverything();
            
            _cameraWrapper.ResetPosition();
            
            _eventsChecker.CheckingEnabled = true;
            _inputSwitcher.ToggleLockToConsole(false);
        }
        
        private void OnEndGame(GameEndEvent endEvent)
        {
            IsGameEnded = true;
            _eventsChecker.CheckingEnabled = false;
            _inputSwitcher.EnableAllInput();
            _inputSwitcher.ToggleLockToConsole(true);
        }

    }
}