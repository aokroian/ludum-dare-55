using _Game.Scripts.Message;
using _Game.Scripts.Story;
using _Game.Scripts.Summon.Data;
using UnityEngine;

namespace _Game.Scripts.Summon.View
{
    public abstract class SummonedObject : MonoBehaviour
    {
        [field:SerializeField] public string Id { get; private set; }

        public SummonedObjectsHolder ObjectsHolder;
        public MessageService MessageService;
        
        public abstract IGameplayEvent GetEventIfAny();
    }
}