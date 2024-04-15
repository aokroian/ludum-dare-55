using System;
using _Game.Scripts.CharacterRelated.Actors.InputThings.StateMachineThings;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.InputThings.AI.States
{
    public class WanderState : State
    {
        private readonly Collider2D _walkArea;
        private readonly Transform _actorTransform;
        private readonly Action<Vector2> _setMovementAction;
        private readonly Action<Vector3> _setLookAction;
        private readonly float _timeToWander;
        private readonly float _timeToIdle;

        private float _wanderStartTime;
        private float _idleStartTime;
        private bool _isIdle;
        private Vector2 _prevMovement;
        private Vector3 _prevLook;

        public WanderState(
            Collider2D walkArea,
            Transform actorTransform,
            Action<Vector2> setMovementAction,
            Action<Vector3> setLookAction,
            float timeToWander = 1f,
            float timeToIdle = 1f
        )
        {
            _walkArea = walkArea;
            _actorTransform = actorTransform;
            _setMovementAction = setMovementAction;
            _setLookAction = setLookAction;
            _timeToWander = timeToWander;
            _timeToIdle = timeToIdle;

            _isIdle = true;
            _idleStartTime = Time.time - timeToIdle;
        }

        public override void Execute()
        {
            var time = Time.time;
            var movement = _prevMovement;
            if (_isIdle)
            {
                if (time - _idleStartTime > _timeToIdle)
                {
                    movement = UnityEngine.Random.insideUnitCircle.normalized;
                    if (IsMovingOurOfWalkArea(_walkArea, movement))
                    {
                        _setMovementAction(Vector2.zero);
                        return;
                    }

                    _isIdle = false;
                    _wanderStartTime = time;
                    _setMovementAction(movement);
                }
            }
            else
            {
                if (time - _wanderStartTime > _timeToWander)
                {
                    _isIdle = true;
                    _idleStartTime = time;
                    movement = Vector2.zero;
                }
            }

            var look = _actorTransform.position + new Vector3(movement.x, 0f, movement.y);
            _setMovementAction.Invoke(movement);
            _setLookAction.Invoke(look);
            _prevMovement = movement;
            _prevLook = look;
        }

        private bool IsMovingOurOfWalkArea(Collider2D walkArea, Vector2 movementInput)
        {
            var pos = _actorTransform.position;
            var pos2d = new Vector2(pos.x, pos.y);
            var pos2dNext = pos2d + movementInput;
            return !walkArea.OverlapPoint(pos2dNext);
        }
    }
}