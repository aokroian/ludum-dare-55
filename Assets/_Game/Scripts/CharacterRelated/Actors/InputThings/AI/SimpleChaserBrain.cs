using System;
using _Game.Scripts.CharacterRelated.Actors.InputThings.AI.States;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.InputThings.AI
{
    public class SimpleChaserBrain : AIActorInput
    {
        private ChaseState _chaseState;

        private bool _isInit;
        private Transform _playerTransform;

        private void Awake()
        {
            _playerTransform = LocatePlayer();
        }

        private void Init()
        {
            if (_playerTransform == null)
                throw new Exception("Player Transform not found");
            _chaseState = new ChaseState(
                WalkArea,
                transform,
                _playerTransform,
                SetMovement,
                SetLook);
            _isInit = true;
        }

        private Transform LocatePlayer()
        {
            return FindObjectOfType<PlayerActorInput>().transform;
        }

        private void Update()
        {
            if (!_isInit)
            {
                if (WalkArea != null)
                    Init();
                else
                    return;
            }

            StateMachine.CurrentState = _chaseState;
            StateMachine.ExecuteCurrentState();
        }
    }
}