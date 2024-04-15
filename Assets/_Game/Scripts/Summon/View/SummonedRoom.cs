using System.Collections.Generic;
using _Game.Scripts.Story;
using _Game.Scripts.Summon.Enums;
using UnityEngine;

namespace _Game.Scripts.Summon.View
{
    public class SummonedRoom : SummonedObject
    {
        [field: SerializeField] public RoomType RoomType { get; private set; }
        [field: SerializeField] public Transform entrance { get; private set; }
        [field: SerializeField] public Transform exit { get; private set; }
        
        public IReadOnlyList<SummonedObject> Objects => _objects;
        
        private readonly List<SummonedObject> _objects = new();

        public override IGameplayEvent GetEventIfAny()
        {
            throw new System.NotImplementedException();
        }

        public void AddObject(SummonedObject summonedObject)
        {
            _objects.Add(summonedObject);
        }
        
        public void RemoveObject(SummonedObject summonedObject)
        {
            _objects.Remove(summonedObject);
        }
        
        public void DestroyObjects()
        {
            foreach (var summonedObject in _objects)
            {
                Destroy(summonedObject.gameObject);
            }
            _objects.Clear();
        }
        
    }
}