using System.Threading.Tasks;
using _Game.Scripts.GameLoop.Events;
using Zenject;

namespace _Game.Scripts.Summon.Summoners
{
    public class GameSummoner: Summoner
    {
        [Inject]
        private SignalBus _signalBus;

        public override async Task Summon(SummonParams summonParams)
        {
            _signalBus.Fire(new GameStartEvent());
        }
    }
}