public class StateMachine
{
    public State CurrentState;

    public void Initialize(State startingState)
    {
        CurrentState = startingState;
        startingState.EnterState();
    }

    public void ChangeState(State newState)
    {
        CurrentState.ExitState();

        CurrentState = newState;
        newState.EnterState();
    }
}