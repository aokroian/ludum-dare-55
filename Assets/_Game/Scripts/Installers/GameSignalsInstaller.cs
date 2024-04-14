using _Game.Scripts.GameLoop.Events;
using Zenject;

namespace _Game.Scripts.Installers
{
    public class GameSignalsInstaller: Installer<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<GameEndEvent>();
        }
    }
}