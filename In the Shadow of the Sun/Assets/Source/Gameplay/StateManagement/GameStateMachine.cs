using System;

public class GameStateMachine
{
    public IGameState<GameScreen> currentState { get; private set; }
    
    public GameStateMachine(GameManager gameManager){}

    public void GoToState(IGameState<IGameScreen> state)
    {
        
    }
    
    public void GoToState<T>() where T : IGameState<GameScreen>
    {
        if (currentState != null)
        {
            currentState.OnStateExit(GameManager.Instance, this);
            currentState = null;
        }

        currentState = Activator.CreateInstance<T>();
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