using _Game.Scripts.GameplayEvents.Ending;
using _Game.Scripts.GameState.Data;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Installers
{
    [CreateAssetMenu(menuName = "Custom/GameConfigInstaller")]
    public class GameConfigInstaller: ScriptableObjectInstaller<GameConfigInstaller>
    {
        public DefaultGameState defaultGameState;
        public EndingService.Config endingService;
        
        
        public override void InstallBindings()
        {
            Container.BindInstance(defaultGameState).IfNotBound();
            Container.BindInstance(endingService).IfNotBound();
        }
    }
}