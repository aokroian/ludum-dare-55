using _Game.Scripts.CharacterRelated._LD55.Events;
using _Game.Scripts.GameLoop.Events;
using _Game.Scripts.Map.Events;
using _Game.Scripts.Story.Events;
using _Game.Scripts.Story.GameplayEvents;
using Zenject;

namespace _Game.Scripts.Installers
{
    public class GameSignalsInstaller : Installer<GameSignalsInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<GameStartEvent>();
            Container.DeclareSignal<GameEndEvent>();
            Container.DeclareSignal<SummonPlayerEvent>();
            Container.DeclareSignal<SummonPrincessEvent>().OptionalSubscriber();
            Container.DeclareSignal<EndingStartedEvent>();
            
            Container.DeclareSignal<PlayerWeaponPickupEvent>();
            Container.DeclareSignal<PlayerCoinPickupEvent>();
            Container.DeclareSignal<PlayerKeyPickupEvent>();
            Container.DeclareSignal<PlayerDoorOpenEvent>();
            Container.DeclareSignal<PlayerInventoryChangedEvent>().OptionalSubscriber();
            Container.DeclareSignal<SummonBardEvent>().OptionalSubscriber();
            Container.DeclareSignal<PlayerDiedEvent>();
        }
    }
}