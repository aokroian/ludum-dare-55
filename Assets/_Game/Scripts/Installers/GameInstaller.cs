﻿using _Game.Scripts.Console;
using _Game.Scripts.GameLoop;
using _Game.Scripts.GameState;
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
            Container.BindInterfacesAndSelfTo<GameStatePlayerPrefsProvider>().AsSingle();
            Container.Bind<ConsoleCommandsHandler>().AsSingle();
            Container.Bind<SummonerService>().AsSingle();
            Container.Bind<SummonedObjectsHolder>().AsSingle();
            Container.Bind<SoundManager>().FromComponentInNewPrefabResource("Prefabs/SoundManager").AsSingle();

            Container.Bind<InputEnabledHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayEventsChecker>().AsSingle().NonLazy();
            Container.Bind<EndingService>().AsSingle();
        }
    }
}