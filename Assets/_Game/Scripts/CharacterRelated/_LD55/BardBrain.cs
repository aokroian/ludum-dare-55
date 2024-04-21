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
        private bool _isMovementStateInit;
        private SummonedPlayer _player;
        private SummonedPrincess _princess;

        [Inject]
        private SummonedObjectsHolder _objectsHolder;

        public void Init(Collider2D walkArea)
        {
            WalkArea = walkArea;
            _isInit = true;
        }

        public void ChasePrincess(SummonedPrincess princess, Collider2D walkArea)
        {
            _princess = princess;
            InitChaseState(walkArea);
        }

        private void InitWanderState()
        {
            _movementState = new WanderState(
                WalkArea,
                transform,
                SetMovement,
                SetLook);
            _isMovementStateInit = true;
        }

        private void InitChaseState(Collider2D walkArea)
        {
            _movementState = new ChaseState(
                walkArea,
                transform,
                _princess.transform,
                SetMovement,
                SetLook);
            _isMovementStateInit = true;
        }

        private void Update()
        {
            if (!_isInit)
                return;

            if (!_isMovementStateInit)
            {
                InitWanderState();
            }
            else
            {
                if (StateMachine.CurrentState != _movementState)
                {
                    StateMachine.NextState = _movementState;
                }
            }

            StateMachine.ExecuteCurrentState();
        }
    }
}