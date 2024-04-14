using System;
using Actors.InputThings.StateMachineThings;
using UnityEngine;

namespace Actors.InputThings.AI.States
{
    public class FireState : State
    {
        private readonly Transform _targetTransform;
        private readonly Action<Vector3> _setLookAction;
        private readonly Action<bool> _setFireAction;

        private readonly float _fireRate;
        private float _prevFireTime;

        public FireState(
            Transform targetTransform,
            float fireRate,
            Action<Vector3> setLookAction,
            Action<bool> setFireAction
        )
        {
            _fireRate = fireRate;
            _targetTransform = targetTransform;
            _setLookAction = setLookAction;
            _setFireAction = setFireAction;
        }

        public override void Execute()
        {
            if (_targetTransform == null || !_targetTransform.gameObject.activeInHierarchy)
            {
                _setFireAction(false);
                return;
            }

            var targetPos = _targetTransform.position;

            if (Time.time - _prevFireTime > 1 / _fireRate)
            {
                _setFireAction(true);
                _prevFireTime = Time.time;
            }
            else
                _setFireAction(false);

            _setLookAction(targetPos);
        }
    }
}