using FiniteStateMachine;

public class WaitingState : BaseState
{
    public override void Enter(StateMachine stateMachine, params object[] args)
    {
        base.Enter(stateMachine, args);
    }

    public override void Exit()
    {
        base.Exit();
    }
}