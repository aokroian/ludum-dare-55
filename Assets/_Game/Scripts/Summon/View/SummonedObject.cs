using _Game.Scripts.Message;
using _Game.Scripts.Story;
using _Game.Scripts.Summon.Data;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon.View
{
    public abstract class SummonedObject : MonoBehaviour
    {
        [field:SerializeField] public string Id { get; private set; }

        [Inject]
        public SummonedObjectsHolder ObjectsHolder;
        [Inject]
        public MessageService MessageService;

        protected SummonedRoom CurrentRoom;
        
        public abstract IGameplayEvent GetEventIfAny();

        public virtual void OnMovedToRoom(SummonedRoom room)
        {
            CurrentRoom = room;
        }
    }
}