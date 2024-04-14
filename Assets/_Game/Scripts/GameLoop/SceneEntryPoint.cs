using _Game.Scripts.Console;
using _Game.Scripts.GameState;
using _Game.Scripts.Summon;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.GameLoop
{
    public class SceneEntryPoint: MonoBehaviour
    {
        [SerializeField] private ConsoleView consoleView;
        
        [Inject]
        private IGameStateProvider _gameStateProvider;
        [Inject]
        private SummonerService _summonerService;
        [Inject]
        private Dungeon _dungeon;
        
        private void Start()
        {
            _gameStateProvider.Load();
            
            InitSummonerService();

            InitViews();
        }
        
        private void InitSummonerService()
        {
            // Init dungeon with default data
            _dungeon.Init(_gameStateProvider.TransientGameState.DungeonData);
            var summoners = FindObjectsByType<Summoner>(FindObjectsSortMode.None);
            _summonerService.Init(summoners);
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