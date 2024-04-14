using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Game.Scripts.Summon.View;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon
{
    public abstract class Summoner: MonoBehaviour
    {
        [field:SerializeField] public string Id { get; private set; }
        [SerializeField] protected List<SummonedObject> prefabs;
        
        public virtual string Validate(SummonParams summonParams)
        {
            return null;
        }
        
        public abstract Task SummonAsync(SummonParams summonParams);

        
        public struct SummonParams
        {
            public readonly Camera _camera;
            public readonly Dungeon Dungeon;

            public SummonParams(Camera camera, Dungeon dungeon)
            {
                _camera = camera;
                Dungeon = dungeon;
            }
        }
    }
}