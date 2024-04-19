using _Game.Scripts.CharacterRelated.Actors.InputThings.AI;
using _Game.Scripts.CharacterRelated.Actors.InputThings.AI.States;
using _Game.Scripts.Summon.Data;
using _Game.Scripts.Summon.View;
using UnityEngine;
using Zenject;

namespace _Game.Scripts.CharacterRelated._LD55.Enemy
{
    public class GuardianBrain: AIActorInput
    {
        private WanderState _wanderState;
        private ChaseState _chaseState;
        
        private bool _isInit;
        private bool _isChaserInit;
        private SummonedPlayer _player;

        [Inject]
        private SummonedObjectsHolder _objectsHolder;

        private SummonedEnemy _summonedEnemy;


        public void Init(SummonedEnemy summonedEnemy, Collider2D walkArea, Collider2D patrolArea)
        {
            _summonedEnemy = summonedEnemy;
            WalkArea = walkArea;
            _wanderState = new WanderState(patrolArea, transform, SetMovement, SetLook, 0.5f);
            
            _isInit = true;
        }

        private void InitChaser()
        {
            _player = _objectsHolder.GetPlayer();
            if (_player == null)
                return;
            _chaseState = new ChaseState(
                WalkArea,
                transform,
                _player.transform,
                SetMovement,
                SetLook);
            _chaseState.OnEnter += () => _summonedEnemy.Say("I warned you!");
            
            _isChaserInit = true;
        }

        public void CalmDown()
        {
            _isInit = false;
        }
        
        private void Update()
        {
            if (!_isInit)
                return;

            if (!_isChaserInit)
            {
                InitChaser();
            }
            else
            {
                if (StateMachine.CurrentState != _chaseState &&
                    Vector3.Distance(_player.transform.position, transform.position) < 3f)
                {
                    StateMachine.NextState = _chaseState;
                }
            }
            
            StateMachine.ExecuteCurrentState();
            // StateMachine.CurrentState = _wanderState;
            // StateMachine.ExecuteCurrentState();
        }
    }
}