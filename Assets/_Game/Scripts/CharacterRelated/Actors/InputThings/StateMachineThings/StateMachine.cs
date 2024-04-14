namespace Actors.InputThings.StateMachineThings
{
    public class StateMachine
    {
        public State CurrentState { get; set; }
        public State PreviousState { get; set; }
        public State NextState { get; set; }

        public void ExecuteCurrentState()
        {
            CurrentState?.Execute();
            if (NextState != null && NextState != CurrentState)
                ChangeState(NextState);
        }

        private void ChangeState(State newState)
        {
            CurrentState?.Exit();
            PreviousState = CurrentState;
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}