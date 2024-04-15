using System;
using _Game.Scripts.CharacterRelated.Actors.InputThings.StateMachineThings;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.InputThings.AI.States
{
    public class ChaseState : State
    {
        private readonly Collider2D _walkArea;
        private readonly Transform _actorTransform;
        private readonly Transform _targetTransform;
        private readonly Action<Vector2> _setMovementAction;
        private readonly Action<Vector3> _setLookAction;

        private Vector2 _prevMovement;
        private Vector3 _prevLook;

        public ChaseState(
            Collider2D walkArea,
            Transform actorTransform,
            Transform targetTransform,
            Action<Vector2> setMovementAction,
            Action<Vector3> setLookAction
        )
        {
            _walkArea = walkArea;
            _actorTransform = actorTransform;
            _targetTransform = targetTransform;
            _setMovementAction = setMovementAction;
            _setLookAction = setLookAction;
        }

        public override void Execute()
        {
            if (_targetTransform == null || !_targetTransform.gameObject.activeInHierarchy)
            {
                _setMovementAction(Vector2.zero);
                return;
            }

            var targetPos = _targetTransform.position;
            var direction = targetPos - _actorTransform.position;
            var movement = new Vector2(direction.x, direction.y).normalized;

            _setMovementAction(
                IsMovingOurOfWalkArea(_walkArea, movement)
                    ? Vector2.zero
                    : movement);

            _setLookAction(targetPos);
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