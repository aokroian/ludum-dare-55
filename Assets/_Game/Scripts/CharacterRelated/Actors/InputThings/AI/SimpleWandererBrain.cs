using _Game.Scripts.CharacterRelated.Actors.InputThings.AI.States;
using UnityEngine;

namespace _Game.Scripts.CharacterRelated.Actors.InputThings.AI
{
    public class SimpleWandererBrain : AIActorInput
    {
        [SerializeField] [Range(.5f, 10f)] private float timeToWander = 1f;
        [SerializeField] [Range(.5f, 10f)] private float timeToIdle = 1f;

        private WanderState _wanderState;

        private bool _isInit;

        private void Init()
        {
            _wanderState = new WanderState(WalkArea,transform, SetMovement, SetLook, timeToWander, timeToIdle);
            _isInit = true;
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

            StateMachine.CurrentState = _wanderState;
            StateMachine.ExecuteCurrentState();
        }
    }
}