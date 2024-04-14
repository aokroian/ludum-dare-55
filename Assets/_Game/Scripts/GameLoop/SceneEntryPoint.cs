using _Game.Scripts.GameState;
using _Game.Scripts.Summon;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.GameLoop
{
    public class SceneEntryPoint: MonoBehaviour
    {
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
        }
        
        private void InitSummonerService()
        {
            _dungeon.Init(_gameStateProvider.TransientGameState.DungeonData);
            var summoners = FindObjectsByType<Summoner>(FindObjectsSortMode.None);
            _summonerService.Init(summoners);
            
        }
    }
}