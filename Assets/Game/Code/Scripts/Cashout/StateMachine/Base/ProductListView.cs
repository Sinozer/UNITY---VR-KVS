using FiniteStateMachine;

public class ProductListView : BaseState
{
    public override void Enter(StateMachine manager, params object[] args)
    {
        base.Enter(manager, args);
        
        gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        
        gameObject.SetActive(false);
    }
}
