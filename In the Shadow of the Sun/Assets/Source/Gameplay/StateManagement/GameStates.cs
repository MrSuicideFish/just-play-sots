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
        Newspaper.Instance.Hide();
        LawsuitNotice.Instance.Hide();
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

public class GameState_Lawsuit : IGameState
{
    public string StateName { get; } = "Lawsuit";
    public void OnStateEnter(GameManager gameManager, GameStateMachine sm)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Lawsuit);
        CameraManager.Instance.GoToCamera(ECameraType.Lawsuit);
        gameManager.playerController.enabled = false;
    }

    public void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.ReturnToHome();
        }
    }

    public void OnStateExit(GameManager gameManager, GameStateMachine sm)
    {
    }
}

public class GameState_Newspaper : IGameState
{
    public string StateName { get; } = "Newspaper";
    public void OnStateEnter(GameManager gameManager, GameStateMachine sm)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Article);
        CameraManager.Instance.GoToCamera(ECameraType.Newspaper);
        gameManager.playerController.enabled = false;
    }

    public void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.ReturnToHome();
        }
    }

    public void OnStateExit(GameManager gameManager, GameStateMachine sm)
    {
        
    }
}

public class GameState_Staff : IGameState
{
    public string StateName { get; } = "Staff Management";
    public void OnStateEnter(GameManager gameManager, GameStateMachine sm)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Staff);
        CameraManager.Instance.GoToCamera(ECameraType.Phone);
        gameManager.playerController.enabled = false;
    }

    public void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.ReturnToHome();
        }
    }

    public void OnStateExit(GameManager gameManager, GameStateMachine sm)
    {
    }
}

public class GameState_Results : IGameState
{
    public string StateName { get; } = "Results";

    public void OnStateEnter(GameManager gameManager, GameStateMachine sm)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Result);
        CameraManager.Instance.GoToCamera(ECameraType.Newspaper);
        gameManager.playerController.enabled = false;
    }

    public void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {
    }

    public void OnStateExit(GameManager gameManager, GameStateMachine sm)
    {
    }
}