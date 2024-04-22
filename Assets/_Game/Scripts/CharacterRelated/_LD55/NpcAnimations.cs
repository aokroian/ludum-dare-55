using UnityEngine;

namespace _Game.Scripts.CharacterRelated._LD55
{
    public class NpcAnimations : MonoBehaviour
    {
        
        private Animator _animator;
        private static readonly int AnimPlayerMove = Animator.StringToHash("AminPlayerMove");
        
        private Vector2 _previousPosition;
        
        private bool _upDirection;
        private bool _leftDirection;

        private bool _stayStill;
        private bool _dead;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            Vector2 direction = CalcCurrentDirection();
            _previousPosition = transform.position;
            
            if (direction.y > 0)
                _upDirection = true;
            else
                _upDirection = false;
            
            if (_leftDirection && direction.x > 0)
                _animator.transform.Rotate(Vector3.up, 180f);
            else if (!_leftDirection && direction.x < 0)
                _animator.transform.Rotate(Vector3.up, -180f);
            
            if (direction.x > 0)
                _leftDirection = false;
            else if (direction.x < 0)
                _leftDirection = true;
            
            Animate(direction);
        }

        private Vector2 CalcCurrentDirection()
        {
            Vector2 currentPosition = transform.position;
            Vector2 deltaPosition = currentPosition - _previousPosition;
            return deltaPosition;
        }
        
        private void Animate(Vector2 direction)
        {
            if (_dead)
            {
                _animator.SetInteger(AnimPlayerMove, 10);
                return;
            }
            if (_stayStill)
            {
                _animator.SetInteger(AnimPlayerMove, 9);
                return;
            }
            
            var animIndex = 0;

            if (direction != Vector2.zero)
            {
                animIndex = _upDirection ? 2 : 1;
            }

            _animator.SetInteger(AnimPlayerMove, animIndex);
        }
        
        public void SetDead()
        {
            _dead = true;
            _animator.SetInteger(AnimPlayerMove, 7);
        }
        
        public void SetStayStill()
        {
            _stayStill = true;
            _animator.SetInteger(AnimPlayerMove, 6);
        }
    }
}