using System.Threading.Tasks;
using _Game.Scripts.Summon.View;
using Zenject;

namespace _Game.Scripts.Summon.Summoners
{
    public class PlayerSummoner: Summoner
    {
        // Very bad :(
        [Inject]
        private DiContainer _diContainer;
        
        public override async Task Summon(SummonParams summonParams)
        {
            _diContainer.InstantiatePrefabForComponent<SummonedObject>(prefabs[0]);
        }
    }
}