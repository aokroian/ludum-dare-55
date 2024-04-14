using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Game.Scripts.Summon.Data;
using _Game.Scripts.Summon.View;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon
{
    public abstract class Summoner: MonoBehaviour
    {
        [field:SerializeField] public string Id { get; private set; }
        [SerializeField] protected List<SummonedObject> prefabs;

        [Inject]
        protected SummonedObjectsHolder _objectsHolder;
        
        public virtual string Validate(SummonParams summonParams)
        {
            return null;
        }
        
        public abstract Task SummonAsync(SummonParams summonParams);

        
        public struct SummonParams
        {
            public readonly Camera _camera;

            public SummonParams(Camera camera)
            {
                _camera = camera;
            }
        }
    }
}