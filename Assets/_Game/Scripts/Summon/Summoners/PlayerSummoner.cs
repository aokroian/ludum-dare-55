using System.Threading.Tasks;
using _Game.Scripts.GameLoop.Events;
using _Game.Scripts.Summon.View;
using Zenject;

namespace _Game.Scripts.Summon.Summoners
{
    public class PlayerSummoner : Summoner
    {
        [Inject]
        private GlobalInputSwitcher _globalInputSwitcher;
        [Inject] 
        private SignalBus _signalBus;

        public override async Task Summon(SummonParams summonParams)
        {
            var player = _diContainer.InstantiatePrefabForComponent<SummonedPlayer>(prefabs[0]);

            if (_objectsHolder.rooms.Count > 0)
                _objectsHolder.rooms[0].AddObject(player);
            else
                _objectsHolder.objectsOutOfRoom.Add(player);

            _objectsHolder.SetPlayerRef(player);

            Summoned(player);
            _signalBus.Fire(new SummonPlayerEvent());
            _globalInputSwitcher.SwitchToPlayerControls();
        }
    }
}