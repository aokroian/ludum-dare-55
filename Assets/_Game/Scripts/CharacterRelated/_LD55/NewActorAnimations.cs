using _Game.Scripts.CharacterRelated.Actors.ActorSystems;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated._LD55
{
    public class NewActorAnimations : ActorSystem
    {
        private Animator _animator;
        private static readonly int AnimPlayerMove = Animator.StringToHash("AminPlayerMove");

        private bool _upDirection;
        private bool _leftDirection;

        private bool _stayStill;
        private bool _dead;
        
        
        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponentInChildren<Animator>();
        }

        public void Update()
        {
            if (ActorInput.Movement.y > 0)
                _upDirection = true;
            else
                _upDirection = false;
            
            if (_leftDirection && ActorInput.Movement.x > 0)
                _animator.transform.Rotate(Vector3.up, 180f);
            else if (!_leftDirection && ActorInput.Movement.x < 0)
                _animator.transform.Rotate(Vector3.up, -180f);
            
            if (ActorInput.Movement.x > 0)
                _leftDirection = false;
            else if (ActorInput.Movement.x < 0)
                _leftDirection = true;

            Animate();
        }

        private void Animate()
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
            
            // var direction = _upDirection ? 2 : 1;
            //
            // if (_upDirection && ActorInput.Movement.x > 0)
            //     direction = 5;
            // else if (_upDirection && ActorInput.Movement.x < 0)
            //     direction = 4;
            // else if (ActorInput.Movement.x > 0)
            //     direction = 2;
            // else if (ActorInput.Movement.x < 0)
            //     direction = 1;

            var animIndex = 0;

            if (ActorInput.Movement != Vector2.zero)
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