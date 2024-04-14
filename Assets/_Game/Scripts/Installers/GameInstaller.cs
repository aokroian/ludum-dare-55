using _Game.Scripts.Console;
using _Game.Scripts.GameState;
using _Game.Scripts.Summon;
using Zenject;

namespace _Game.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameStatePlayerPrefsProvider>().AsSingle();
            Container.Bind<ConsoleCommandsHandler>().AsSingle();
            Container.Bind<SummonerService>().AsSingle();
            Container.Bind<Dungeon>().AsSingle();
            Container.Bind<SoundManager>().FromComponentInNewPrefabResource("Prefabs/SoundManager").AsSingle();
        }
    }
}