using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Game.Scripts.Map;
using _Game.Scripts.Summon.Data;
using _Game.Scripts.Summon.View;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon
{
    public abstract class Summoner: MonoBehaviour
    {
        public event Action<SummonedObject> OnSummoned = delegate { };
        
        [field:SerializeField] public string Id { get; private set; }
        [SerializeField] protected List<SummonedObject> prefabs;

        [Inject]
        protected SummonedObjectsHolder _objectsHolder;
        
        // Very bad :(
        [Inject]
        protected DiContainer _diContainer;
        
        public virtual string Validate(SummonParams summonParams)
        {
            return null;
        }
        
        // public abstract Task SummonAsync(SummonParams summonParams);

        public abstract Task Summon(SummonParams summonParams);
        
        protected async Task MoveCameraToAsync(CameraWrapper cameraWrapper, Vector3 position)
        {
            await cameraWrapper.MoveCameraToAsync(position);
        }

        
        public struct SummonParams
        {
            public readonly CameraWrapper camera;

            public SummonParams(CameraWrapper camera)
            {
                this.camera = camera;
            }
        }
        
        protected void Summoned(SummonedObject obj)
        {
            OnSummoned?.Invoke(obj);
        }
    }
}