using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts.Summon
{
    public class SummonerService
    {
        private Summoner.SummonParams _summonParams;
        private DungeonService _dungeonService;
        private Dictionary<string, Summoner> _summoners;

        public SummonerService(DungeonService dungeonService)
        {
            _dungeonService = dungeonService;
        }

        public void Init(IEnumerable<Summoner> summoners)
        {
            Debug.Log("Summoners amount: " + summoners.Count());
            _summoners = summoners.ToDictionary(it => it.Id);
        }

        public string Summon(string summonId)
        {
            var summoner = _summoners[summonId];
            if (summoner == null)
            {
                Debug.LogWarning($"No summoner found for command: {summonId}");
                return "No summoner found for command.";
            }

            var result = summoner.Validate(GetSummonParams());
            summoner.SummonAsync(GetSummonParams());

            return result;
        }
        
        private Summoner.SummonParams GetSummonParams()
        {
            if (_summonParams._camera == null)
                _summonParams = new Summoner.SummonParams(Camera.main, _dungeonService);

            return _summonParams;
        }
    }
}