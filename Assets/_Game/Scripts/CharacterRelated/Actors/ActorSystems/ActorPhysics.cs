using _Game.Scripts.CharacterRelated.Actors.Upgrades;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.ActorSystems
{
    public class ActorPhysics : ActorSystem, IActorStatsReceiver
    {
        [SerializeField] private ActorStatsController actorStatsController;
        [SerializeField] private float defaultSpeed = 5f;

        private float _defaultScale;
        private float _currentSpeed;
        private float _currentScale;

        private Rigidbody2D _rigidbody2D;

        protected override void Awake()
        {
            base.Awake();
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _currentSpeed = defaultSpeed;
            _defaultScale = transform.localScale.x;
            _currentScale = _defaultScale;

            RegisterActorStatsReceiver();
        }

        private void OnDestroy()
        {
            UnregisterActorStatsReceiver();
        }

        public void RegisterActorStatsReceiver()
        {
            if (actorStatsController != null)
                actorStatsController.AddReceiver(this);
        }

        public void UnregisterActorStatsReceiver()
        {
            if (actorStatsController != null)
                actorStatsController.RemoveReceiver(this);
        }

        public void ReceiveActorStats(ActorStatsSo actorStatsSo)
        {
            // _currentSpeed = defaultSpeed + actorStatsSo.addedMovementSpeed;
            // _currentScale = _defaultScale + actorStatsSo.addedScaleModifier;
            // transform.localScale = new Vector3(_currentScale, _currentScale, 1);
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void Movement()
        {
            _rigidbody2D.velocity = new Vector2(
                ActorInput.Movement.x * _currentSpeed,
                ActorInput.Movement.y * _currentSpeed);
        }
    }
}