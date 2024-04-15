using System;
using System.Collections.Generic;
using _Game.Scripts.GameLoop.Events;
using _Game.Scripts.Story.Events;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Story.Ending
{
    public class EndingService
    {
        private SignalBus _signalBus;
        private Config _config;

        public EndingService(SignalBus signalBus, Config config)
        {
            _signalBus = signalBus;
            _config = config;
        }

        public void ShowEnding(string endingId)
        {
            var ending = _config.endings.Find(e => e.EndingId == endingId);
            _signalBus.Fire(new EndingStartedEvent(ending));
            Debug.LogWarning("Ending " + endingId);
            _signalBus.Fire(new GameEndEvent(endingId));
        }
        
        [Serializable]
        public class Config
        {
            public List<EndingData> endings;
        }

        [Serializable]
        public class EndingData
        {
            public string EndingId;
            public string EndingName;
            public string EndingDescription;
            public bool IsGoodEnding;
        }
    }
}