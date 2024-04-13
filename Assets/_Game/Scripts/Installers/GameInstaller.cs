using _Game.Scripts.Console;
using _Game.Scripts.GameState;
using Zenject;

namespace _Game.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameStatePlayerPrefsProvider>().AsSingle();
            Container.Bind<ConsoleCommandsHandler>().AsSingle();
        }
    }
}