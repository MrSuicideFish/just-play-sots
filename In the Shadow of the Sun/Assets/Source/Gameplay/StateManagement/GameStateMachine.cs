public class GameStateMachine
{
    public string lastStateName { get; private set; }
    public IGameState currentState { get; private set; }
    
    public GameStateMachine(GameManager gameManager){}

    public void GoToState(IGameState newState)
    {
        if (currentState != null)
        {
            lastStateName = currentState.StateName;
            currentState.OnStateExit(GameManager.Instance, this);
            currentState = null;
        }

        currentState = newState;
        currentState.OnStateEnter(GameManager.Instance,this);
    }

    public void TickStateMachine(GameManager gameManager)
    {
        if (currentState != null)
        {
            currentState.OnStateUpdate(gameManager,this);
        }
    }
}