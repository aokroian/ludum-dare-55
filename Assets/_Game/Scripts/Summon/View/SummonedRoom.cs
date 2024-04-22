using System.Collections.Generic;
using _Game.Scripts.Story;
using _Game.Scripts.Story.GameplayEvents;
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
            if (ObjectsHolder.RealRoomCount > 3)
            {
                return new SystemTextGameplayEvent("This is too many rooms", "tooManyRooms");
            }

            return null;
        }
        
        private List<TextGameplayEvent.TextEventParams> PrepareMessage(string message)
        {
            var p = new TextGameplayEvent.TextEventParams()
            {
                disableInput = false,
                Duration = 0,
                Speaker = this,
                Text = message
            };
            return new List<TextGameplayEvent.TextEventParams>() { p };
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

        public Vector3 GetRandomPointInsideWalkArea()
        {
            Vector2 point;
            do
            {
                var x = Random.Range(WalkArea.bounds.min.x, WalkArea.bounds.max.x);
                var y = Random.Range(WalkArea.bounds.min.y, WalkArea.bounds.max.y);
                point = new Vector2(x, y);
            } while (!WalkArea.OverlapPoint(point));

            return point;
        }
    }
}