using _Game.Scripts.Summon;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.GameLoop
{
    public class SceneEntryPoint: MonoBehaviour
    {
        [Inject]
        private SummonerService summonerService;
        
        private void Start()
        {
            InitSummonerService();
        }
        
        private void InitSummonerService()
        {
            var summoners = FindObjectsByType<Summoner>(FindObjectsSortMode.None);
            summonerService.Init(summoners);
        }
    }
}