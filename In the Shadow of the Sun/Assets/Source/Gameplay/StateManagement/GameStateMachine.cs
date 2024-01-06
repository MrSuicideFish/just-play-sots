using System.Collections.Generic;

public class GameStateMachine
{
    public GameState currentState { get; private set; }
    private List<EScreenType> firstEnter = new();
    private List<EScreenType> firstExit = new();
    
    public GameStateMachine(GameManager gameManager){}
    public void GoToState(GameState newState)
    {
        if (currentState != null)
        {
            currentState.OnStateExit(GameManager.Instance,
                this, !firstExit.Contains(currentState.ScreenType));
            firstExit.Add(currentState.ScreenType);
            currentState = null;
        }

        currentState = newState;
        currentState.OnStateEnter(GameManager.Instance,
            this, !firstEnter.Contains(currentState.ScreenType));
        firstEnter.Add(currentState.ScreenType);
    }

    public void TickStateMachine(GameManager gameManager)
    {
        if (currentState != null)
        {
            currentState.OnStateUpdate(gameManager,this);
        }
    }
}