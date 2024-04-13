using System;
using _Game.Scripts.Summon.Data;
using UnityEngine;

namespace _Game.Scripts.Summon
{
    public class SummonerService
    {
        private SummonerCommandBindings _commandBindings;

        private Summoner.SummonParams _summonParams;
        private DungeonService _dungeonService;
        private Config _config;

        public SummonerService(Config config, DungeonService dungeonService)
        {
            _config = config;
            _commandBindings = config.CommandBindings;
            _dungeonService = dungeonService;
        }

        public string Summon(string summonId)
        {
            var summoner = _commandBindings.Get(summonId);
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
        
        
        [Serializable]
        public class Config
        {
            public SummonerCommandBindings CommandBindings;
        }
    }
}