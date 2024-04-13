using _Game.Scripts.GameState.Data;
using _Game.Scripts.Summon;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Installers
{
    [CreateAssetMenu(menuName = "Custom/GameConfigInstaller")]
    public class GameConfigInstaller: ScriptableObjectInstaller<GameConfigInstaller>
    {
        public DefaultGameState defaultGameState;
        
        public SummonerService.Config summonerService;
        
        public override void InstallBindings()
        {
            Container.BindInstance(defaultGameState).IfNotBound();
            Container.BindInstance(summonerService).IfNotBound();
        }
    }
}