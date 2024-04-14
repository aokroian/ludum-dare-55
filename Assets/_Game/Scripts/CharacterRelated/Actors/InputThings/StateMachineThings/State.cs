using System;
using UnityEngine;

namespace Actors.InputThings.StateMachineThings
{
    public abstract class State
    {
        public event Action OnEnter;
        public event Action OnExit;

        public void Enter()
        {
            OnEnter?.Invoke(); 
        }

        public void Exit()
        {
            OnExit?.Invoke(); 
        }

        public abstract void Execute();
    }
}