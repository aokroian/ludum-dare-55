using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Summon.Data
{
    public class SummonerCommandBindings: ScriptableObject
    {
        [SerializeField] private List<Binding> _bindings;

        // Create dictionary?
        public Summoner Get(string command)
        {
            foreach (var binding in _bindings)
            {
                if (binding.command == command)
                    return binding.summoner;
            }

            return null;
        }
        
        [Serializable]
        public struct Binding
        {
            public string command;
            public Summoner summoner;
        }
    }
}