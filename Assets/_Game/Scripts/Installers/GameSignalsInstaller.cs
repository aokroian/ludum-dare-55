﻿using _Game.Scripts.GameLoop.Events;
using _Game.Scripts.Map.Events;
using _Game.Scripts.Story.Events;
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
            Container.DeclareSignal<EndingStartedEvent>();
            
            Container.DeclareSignal<PlayerWeaponPickupEvent>();
            Container.DeclareSignal<PlayerCoinPickupEvent>();
            Container.DeclareSignal<PlayerKeyPickupEvent>();
            Container.DeclareSignal<PlayerDoorOpenEvent>();
        }
    }
}