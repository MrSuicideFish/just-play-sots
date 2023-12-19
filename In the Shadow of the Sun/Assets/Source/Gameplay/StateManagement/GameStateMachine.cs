public class GameStateMachine
{
    public IGameState currentState { get; private set; }
    
    public GameStateMachine(GameManager gameManager){}

    public void GoToState(IGameState newState)
    {
        if (currentState != null)
        {
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