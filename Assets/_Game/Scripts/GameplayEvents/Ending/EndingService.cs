using System;
using System.Collections.Generic;

namespace _Game.Scripts.GameplayEvents.Ending
{
    public class EndingService
    {
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