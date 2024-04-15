using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game.Scripts.CharacterRelated.Actors.InputThings
{
    public class PlayerActorInput : MonoBehaviour, IActorInput
    {
        public Vector2 Movement { get; private set; }
        public Vector3 Look { get; private set; }
        public bool Fire { get; private set; }
        public bool Interact { get; private set; }

        private Camera _mainCamera;

        private bool _isActive = true;

        private Vector3 _liveLook;
        private Vector2 _liveMovement;
        private bool _liveFire;
        private bool _liveInteract;

        public void OnFire(InputValue value)
        {
            _liveFire = Math.Abs(value.Get<float>() - 1f) < 0.1f;
            if (!_isActive)
                return;
            Fire = _liveFire;
        }

        public void OnLook(InputValue value)
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = _mainCamera.farClipPlane * .5f;
            var worldPoint = _mainCamera.ScreenToWorldPoint(mousePos);
            _liveLook = worldPoint;
            if (!_isActive)
                return;
            Look = _liveLook;
        }

        public void OnMove(InputValue context)
        {
            _liveMovement = context.Get<Vector2>();
            if (!_isActive)
                return;
            Movement = _liveMovement;
        }

        public void OnInteract(InputValue value)
        {
            _liveInteract = Math.Abs(value.Get<float>() - 1f) < 0.1f;
            if (!_isActive)
                return;
            Interact = _liveInteract;
        }

        public void ResetInteract()
        {
            Interact = false;
        }

        public void ToggleInput(bool isActive)
        {
            _isActive = isActive;

            if (!_isActive)
            {
                Look = default;
                Movement = default;
                Fire = default;
            }
            else
            {
                Look = _liveLook;
                Movement = _liveMovement;
                Fire = _liveFire;
            }
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }
    }
}