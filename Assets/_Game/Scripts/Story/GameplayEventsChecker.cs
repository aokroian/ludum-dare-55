using System.Threading.Tasks;
using _Game.Scripts.GameLoop.Events;
using _Game.Scripts.Story.Ending;
using _Game.Scripts.Summon.Data;
using _Game.Scripts.Summon.View;
using Zenject;

namespace _Game.Scripts.Story
{
    public class GameplayEventsChecker: ITickable
    {
        private DiContainer _diContainer;
        private EndingService _endingService;
        private SummonedObjectsHolder _objectsHolder;
        
        private bool _isEventInProgress = false;

        public bool CheckingEnabled = true;

        public GameplayEventsChecker(DiContainer diContainer, EndingService endingService, SummonedObjectsHolder objectsHolder)
        {
            _diContainer = diContainer;
            _endingService = endingService;
            _objectsHolder = objectsHolder;
            
        }

        
        // TODO: disable with input
        public async void Tick()
        {
            if (!CheckingEnabled || _isEventInProgress)
                return;
            
            _isEventInProgress = true;
            foreach (var obj in _objectsHolder.ObjectsOutOfRoom)
            {
                var isEnding = await CheckAndStartEvent(obj);
                if (isEnding)
                    break;
            }
            
            if (_objectsHolder.RealRoomCount == 0)
            {
                _isEventInProgress = false;
                return;
            }
                
            var currentRoom = _objectsHolder.GetPlayerRoomOrFirst();
            foreach (var obj in currentRoom.Objects)
            {
                var isEnding = await CheckAndStartEvent(obj);
                if (isEnding)
                    break;
            }

            _isEventInProgress = false;
        }
        
        private async Task<bool> CheckAndStartEvent(SummonedObject obj)
        {
            var ev = obj.GetEventIfAny();
            if (ev != null)
            {
                var ending = await ev.StartEvent(_diContainer);
                if (ending != null)
                {
                    _endingService.ShowEnding(ending);
                    return true;
                }
            }

            return false;
        }
    }
}