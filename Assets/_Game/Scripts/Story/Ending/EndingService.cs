using System;
using System.Collections.Generic;
using _Game.Scripts.GameLoop.Events;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Story.Ending
{
    public class EndingService
    {
        private SignalBus _signalBus;

        public EndingService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void ShowEnding(string endingId)
        {
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
        }
    }
}