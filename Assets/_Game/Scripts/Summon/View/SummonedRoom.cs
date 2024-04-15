using System.Collections.Generic;
using _Game.Scripts.Story;
using _Game.Scripts.Summon.Enums;
using UnityEngine;

namespace _Game.Scripts.Summon.View
{
    public class SummonedRoom : SummonedObject
    {
        [field: SerializeField] public Transform PrincessSpawnPoint { get; private set; }
        [field: SerializeField] public Collider2D WalkArea { get; private set; }
        [field: SerializeField] public Collider2D PatrolArea { get; private set; }
        
        [field: SerializeField] public RoomType RoomType { get; private set; }
        [field: SerializeField] public Transform entrance { get; private set; }
        [field: SerializeField] public Transform exit { get; private set; }
        
        public IReadOnlyList<SummonedObject> Objects => _objects;
        
        protected List<SummonedObject> _objects = new();

        public override IGameplayEvent GetEventIfAny()
        {
            throw new System.NotImplementedException();
        }

        public virtual void AddObject(SummonedObject summonedObject)
        {
            _objects.Add(summonedObject);
            summonedObject.OnMovedToRoom(this);
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