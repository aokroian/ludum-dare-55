using _Game.Scripts.Common;
using _Game.Scripts.Console;
using _Game.Scripts.GameState;
using _Game.Scripts.Summon;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.GameLoop
{
    public class SceneEntryPoint : MonoBehaviour
    {
        [SerializeField] private ConsoleView consoleView;
        [SerializeField] private ControlsHelpUI controlsHelpUI;

        [Inject]
        private IGameStateProvider _gameStateProvider;
        [Inject]
        private SummonerService _summonerService;
        [Inject]
        private ConsoleCommandsHandler _consoleCommmandsHandler;
        [Inject]
        private GlobalInputSwitcher _globalInputSwitcher;

        private void Start()
        {
            _gameStateProvider.Load();
            _globalInputSwitcher.Init(consoleView, controlsHelpUI);

            _consoleCommmandsHandler.InjectKnownCommands(_gameStateProvider.PermanentGameState);
            InitSummonerService();
            InitViews();
        }

        private void InitSummonerService()
        {
            // Init dungeon with default data
            var summoners = FindObjectsByType<Summoner>(FindObjectsSortMode.None);
            _summonerService.Init(summoners, _globalInputSwitcher);
        }

        private void InitViews()
        {
            consoleView.Init();
        }

        private void OnDestroy()
        {
            _gameStateProvider.Save();
        }
    }
}