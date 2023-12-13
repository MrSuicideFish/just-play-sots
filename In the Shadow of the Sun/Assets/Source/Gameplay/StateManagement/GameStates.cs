using UnityEngine;

public interface IGameState
{
    string StateName { get; }
    void OnStateEnter(GameManager gameManager, GameStateMachine sm);
    void OnStateUpdate(GameManager gameManager, GameStateMachine sm);
    void OnStateExit(GameManager gameManager, GameStateMachine sm);
}

public class Gamestate_Entry : IGameState
{
    public string StateName { get; } = "Entry";
    public void OnStateEnter(GameManager gameManager, GameStateMachine sm)
    {
        gameManager.playerController.enabled = false;
        CameraManager.Instance.GoToCamera(ECameraType.Intro);
        GameUIController.Instance.GoToScreen(EScreenType.Empty);
        gameManager.introAnimation.Play();
    }

    public void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {
        if (gameManager.introAnimation.isPlaying)
        {
            return;
        }
        gameManager.introAnimation.gameObject.SetActive(false);
        gameManager.ReturnToHome();
    }

    public void OnStateExit(GameManager gameManager, GameStateMachine sm)
    {
    }
}

public class GameState_Home : IGameState
{
    public string StateName { get; } = "Home";
    public void OnStateEnter(GameManager gameManager, GameStateMachine sm)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Home);
        CameraManager.Instance.GoToCamera(ECameraType.Home);
        gameManager.playerController.enabled = true;
    }

    public void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {
    }

    public void OnStateExit(GameManager gameManager, GameStateMachine sm)
    {
    }
}