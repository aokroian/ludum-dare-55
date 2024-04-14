using System.Threading.Tasks;
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

        public GameplayEventsChecker(DiContainer diContainer, EndingService endingService, SummonedObjectsHolder objectsHolder)
        {
            _diContainer = diContainer;
            _endingService = endingService;
            _objectsHolder = objectsHolder;
        }

        
        // TODO: disable with input
        public async void Tick()
        {
            // foreach (var obj in _objectsHolder.objectsOutOfRoom)
            // {
            //     var isEnding = await CheckAndStartEvent(obj);
            //     if (isEnding)
            //         return;
            // }
            //
            // var currentRoom = _objectsHolder.GetCurrentPlayerRoom();
            // if (currentRoom == null)
            //     return;
            // foreach (var obj in currentRoom.Objects)
            // {
            //     var isEnding = await CheckAndStartEvent(obj);
            //     if (isEnding)
            //         return;
            // }
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