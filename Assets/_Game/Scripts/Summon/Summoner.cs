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
        
        protected async Task MoveCameraToAsync(CameraWrapper camera, Vector3 position)
        {
            var cameraPosition = camera.transform.position;
            var cameraTargetPosition = new Vector3(position.x, position.y, cameraPosition.z);
            var cameraMoveTime = 0.5f;
            var cameraMoveTimeElapsed = 0f;
            while (cameraMoveTimeElapsed < cameraMoveTime)
            {
                cameraMoveTimeElapsed += Time.deltaTime;
                camera.transform.position = Vector3.Lerp(cameraPosition, cameraTargetPosition, cameraMoveTimeElapsed / cameraMoveTime);
                await Task.Yield();
            }
        }

        
        public struct SummonParams
        {
            public readonly CameraWrapper _camera;

            public SummonParams(CameraWrapper camera)
            {
                _camera = camera;
            }
        }
        
        protected void Summoned(SummonedObject obj)
        {
            OnSummoned?.Invoke(obj);
        }
    }
}