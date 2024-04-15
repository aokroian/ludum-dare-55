using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts.Common
{
    public class PointerDownHandler : MonoBehaviour, IPointerDownHandler
    {
        public event Action OnPointerDownEvent = delegate { };

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEvent?.Invoke();
        }
    }
}