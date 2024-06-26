﻿using _Game.Scripts.CharacterRelated._LD55;
using _Game.Scripts.Common;
using _Game.Scripts.Console;
using _Game.Scripts.GameLoop;
using _Game.Scripts.GameState;
using _Game.Scripts.Map;
using _Game.Scripts.Message;
using _Game.Scripts.Story;
using _Game.Scripts.Story.Ending;
using _Game.Scripts.Summon;
using _Game.Scripts.Summon.Data;
using Zenject;

namespace _Game.Scripts.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            GameSignalsInstaller.Install(Container);

            Container.Bind<GameLoopController>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameStatePlayerPrefsProvider>().AsSingle();
            Container.Bind<ConsoleCommandsHandler>().AsSingle();
            Container.Bind<SummonerService>().AsSingle();
            Container.Bind<SummonedObjectsHolder>().AsSingle();
            Container.Bind<SoundManager>().FromComponentInNewPrefabResource("Prefabs/SoundManager").AsSingle();
            Container.BindInterfacesAndSelfTo<GlobalInputSwitcher>().AsSingle();

            Container.Bind<PlayerInventoryService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayEventsChecker>().AsSingle().NonLazy();
            Container.Bind<EndingService>().AsSingle();
            Container.Bind<MessageService>().AsSingle();
            Container.Bind<RoomSwitcher>().AsSingle();
        }
    }
}