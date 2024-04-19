using System;
using _Game.Scripts.CharacterRelated.Actors.InputThings.StateMachineThings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.CharacterRelated.Actors.InputThings.AI.States
{
    public class WanderState : State
    {
        private readonly Collider2D _walkArea;
        private readonly Transform _actorTransform;
        private readonly Action<Vector2> _setMoveInputAction;
        private readonly Action<Vector3> _setLookInputAction;
        private readonly float _timeToIdle;

        private float _idleStartTime;
        private float _currentWanderStartTime;

        public WanderState(
            Collider2D walkArea,
            Transform actorTransform,
            Action<Vector2> setMoveInputAction,
            Action<Vector3> setLookInputAction,
            float timeToIdle = 1f
        )
        {
            _walkArea = walkArea;
            _actorTransform = actorTransform;
            _setMoveInputAction = setMoveInputAction;
            _setLookInputAction = setLookInputAction;
            _timeToIdle = timeToIdle;
            _idleStartTime = Time.time - timeToIdle;
        }

        private Vector3? _targetWanderPoint;
        private Vector2 CurrentPos2D => new(_actorTransform.position.x, _actorTransform.position.y);

        private Vector2? GetRandTargetWanderPoint()
        {
            var walkAreaSmallerSide = Math.Min(_walkArea.bounds.size.x, _walkArea.bounds.size.y);
            var randPos = CurrentPos2D + Random.insideUnitCircle * walkAreaSmallerSide;
            var attempts = 8;
            while (attempts > 0 && !IsPointInsideWalkableArea(randPos))
            {
                randPos = CurrentPos2D + Random.insideUnitCircle * (walkAreaSmallerSide * .5f * attempts);
                attempts--;
            }

            return IsPointInsideWalkableArea(randPos) ? randPos : null;
        }

        private bool IsPointInsideWalkableArea(Vector2 point)
        {
            return _walkArea.OverlapPoint(point);
        }

        public override void Execute()
        {
            if (_targetWanderPoint != null)
            {
                var dist = Vector2.Distance((Vector2)_targetWanderPoint, CurrentPos2D);
                const float MaxSingleWanderTime = 2f;
                if (dist <= 0.2f || Time.time - _currentWanderStartTime > MaxSingleWanderTime)
                {
                    _targetWanderPoint = null;
                    _idleStartTime = Time.time;
                    return;
                }

                var dir = ((Vector2)_targetWanderPoint - CurrentPos2D).normalized;
                var look = _actorTransform.position + new Vector3(dir.x, 0f, dir.y);
                _setMoveInputAction.Invoke(dir);
                _setLookInputAction.Invoke(look);
            }
            else
            {
                _setMoveInputAction.Invoke(Vector2.zero);

                if (Time.time - _idleStartTime > _timeToIdle)
                {
                    _targetWanderPoint = GetRandTargetWanderPoint();
                    _currentWanderStartTime = Time.time;
                }
            }
        }
    }
}