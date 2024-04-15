using System;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Common
{
    public class GoLifecycleEventEmitter : MonoBehaviour
    {
        public event Action OnEnableEvent;
        public event Action OnDisableEvent;

        private void OnEnable()
        {
            OnEnableEvent?.Invoke();
        }

        private void OnDisable()
        {
            OnDisableEvent?.Invoke();
        }
    }
}