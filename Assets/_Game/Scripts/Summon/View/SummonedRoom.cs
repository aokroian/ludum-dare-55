using System.Collections.Generic;
using _Game.Scripts.Summon.Enums;
using UnityEngine;

namespace _Game.Scripts.Summon.View
{
    public class SummonedRoom : SummonedObject
    {
        [field: SerializeField] public RoomType RoomType { get; private set; }
        
        public IReadOnlyList<SummonedObject> Objects => _objects;
        
        private List<SummonedObject> _objects;
        
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