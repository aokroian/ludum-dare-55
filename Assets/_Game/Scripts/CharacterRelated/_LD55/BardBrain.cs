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

        [Inject]
        private SummonedObjectsHolder _objectsHolder;

        public void Init(Collider2D walkArea)
        {
            WalkArea = walkArea;
            _isInit = true;
        }

        private void InitMovementState()
        {
            _movementState = new WanderState(
                WalkArea,
                transform,
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
                InitMovementState();
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