using _Game.Scripts.Message;
using _Game.Scripts.Story;
using _Game.Scripts.Summon.Data;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.Summon.View
{
    public abstract class SummonedObject : MonoBehaviour
    {
        [field: SerializeField] public string Id { get; private set; }

        [Inject]
        public SummonedObjectsHolder ObjectsHolder;
        [Inject]
        public MessageService MessageService;

        public SummonedRoom CurrentRoom { get; protected set; }

        private Vector3 _initialScale;

        protected virtual void Start()
        {
            SummonEffectAnimation();
        }

        public abstract IGameplayEvent GetEventIfAny();

        public virtual void OnMovedToRoom(SummonedRoom room)
        {
            CurrentRoom = room;
        }

        private void SummonEffectAnimation()
        {
            _initialScale = transform.localScale;
            transform.DOPunchScale(_initialScale * 0.3f, 0.5f).OnComplete(() =>
            {
                transform.localScale = _initialScale;
            });
        }
    }
}