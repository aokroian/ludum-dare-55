using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Story.Ending
{
    public class EndingService
    {
        public void ShowEnding(string endingId)
        {
            Debug.LogWarning("Ending " + endingId);
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