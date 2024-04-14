using System;
using UnityEngine;

namespace Common
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