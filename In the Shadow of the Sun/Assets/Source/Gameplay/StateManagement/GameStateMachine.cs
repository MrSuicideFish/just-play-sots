public class GameStateMachine
{
    public IGameState currentState { get; private set; }
    
    public GameStateMachine(GameManager gameManager){}

    public void GoToState(GameManager gameManager, IGameState newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit(gameManager,this);
            currentState = null;
        }

        currentState = newState;
        currentState.OnStateEnter(gameManager,this);
    }

    public void TickStateMachine(GameManager gameManager)
    {
        if (currentState != null)
        {
            currentState.OnStateUpdate(gameManager,this);
        }
    }
}