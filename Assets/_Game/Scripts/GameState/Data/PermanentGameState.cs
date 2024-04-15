using System;
using System.Collections.Generic;

namespace _Game.Scripts.GameState.Data
{
    [Serializable]
    public class PermanentGameState
    {
        public List<string> knownCommands;
        public List<string> knownEndings;
    }
}