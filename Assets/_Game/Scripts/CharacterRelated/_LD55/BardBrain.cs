using _Game.Scripts.CharacterRelated.Actors.InputThings.AI;
using _Game.Scripts.CharacterRelated.Actors.InputThings.AI.States;
using _Game.Scripts.CharacterRelated.Actors.InputThings.StateMachineThings;
using _Game.Scripts.Summon.Data;
using _Game.Scripts.Summon.View;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.CharacterRelated._LD55
{
    public class BardBrain : AIActorInput
    {
        [SerializeField] private float distToPlayer = 3f;
        private State _movementState;

        private bool _isInit;
        private SummonedPlayer _player;
        private Transform _princess;

        [Inject]
        private SummonedObjectsHolder _objectsHolder;
        
        public bool IsChasingPrincess { get; private set; }

        public void WanderAroundPlayer(Collider2D walkArea)
        {
            _isInit = true;
            IsChasingPrincess = false;
            InitWanderState(walkArea);
        }

        public void ChasePrincess(Transform princess, Collider2D walkArea)
        {
            _isInit = true;
            _princess = princess;
            IsChasingPrincess = true;
            InitChaseState(walkArea);
        }

        private void InitWanderState(Collider2D walkArea)
        {
            _movementState = new WanderState(
                walkArea,
                transform,
                SetMovement,
                SetLook);
        }

        private void InitChaseState(Collider2D walkArea)
        {
            _movementState = new ChaseState(
                walkArea,
                transform,
                _princess.transform,
                SetMovement,
                SetLook);
        }

        private void Update()
        {
            if (!_isInit)
                return;
            if (StateMachine.CurrentState != _movementState)
                StateMachine.NextState = _movementState;
            StateMachine.ExecuteCurrentState();
        }
    }
}