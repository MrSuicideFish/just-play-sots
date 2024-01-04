using System.Collections;
using DG.Tweening;
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
    private Screen_Intro introScreen;
    
    public string StateName { get; } = "Entry";
    public void OnStateEnter(GameManager gameManager, GameStateMachine sm)
    {
        Newspaper.Instance.Hide();
        LawsuitNotice.Instance.Hide();
        gameManager.playerController.enabled = false;
        CameraManager.Instance.GoToCamera(ECameraType.Intro);

        introScreen = GameUIController.Instance.GetScreen(EScreenType.Intro) as Screen_Intro;
        GameUIController.Instance.GoToScreen(EScreenType.Intro);
    }

    public void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {
        if (introScreen.IsComplete)
        {
            introScreen.radioAudioSrc.Play();
            sm.GoToState(new GameState_Home());
        }
    }

    public void OnStateExit(GameManager gameManager, GameStateMachine sm)
    {
    }
}

public class GameState_Home : IGameState
{
    public static bool firstTimeUser = true;
    public static bool hasCompletedFirstArticle = false;
    
    public string StateName { get; } = "Home";
    public void OnStateEnter(GameManager gameManager, GameStateMachine sm)
    {
        CameraManager.Instance.GoToCamera(ECameraType.Home);
        if (firstTimeUser)
        {
            gameManager.StartCoroutine(WaitForFTUE(gameManager));
            return;
        }

        if (gameManager.CurrentArticle != null)
        {
            Newspaper.Instance.Show(false);
        }

        if (gameManager.lawsuits.Count > 0)
        {
            bool firstShow = gameManager.lawsuits.Count == 0;
            LawsuitNotice.Instance.Show(firstShow);
        }
        
        GameUIController.Instance.GoToScreen(EScreenType.Home);
        gameManager.playerController.enabled = true;
    }

    private IEnumerator WaitForFTUE(GameManager gameManager)
    {
        firstTimeUser = false;
        Screen_Home homeScreen = GameUIController.Instance.GetScreen(EScreenType.Home) as Screen_Home;
        homeScreen.firstTimeIntro.Opacity = 0;
        homeScreen.orgNameIntro.text = gameManager.OrganizationName;
        
        GameUIController.Instance.GoToScreen(EScreenType.Tutorial);
        Screen_Tutorial ftueScreen = GameUIController.Instance.GetScreen(EScreenType.Tutorial) as Screen_Tutorial;
        while (!ftueScreen.isComplete)
        {
            yield return null;
        }
        
        gameManager.playerController.enabled = true;
        homeScreen.firstTimeIntro.Set(0.0f);
        GameUIController.Instance.GoToScreen(EScreenType.Home);
        DOTween.To(
            () => homeScreen.firstTimeIntro.Opacity,
            (x) => homeScreen.firstTimeIntro.Opacity = x,
            1.0f, 5.0f).OnComplete(() =>
        {
            // show first article
            gameManager.DeliverArticle();
        });
        
        yield return new WaitForSeconds(8.0f);
        DOTween.To(
            () => homeScreen.firstTimeIntro.Opacity,
            (x) => homeScreen.firstTimeIntro.Opacity = x,
            0.0f, 1.0f);
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
    private Screen_Lawsuit lawsuitScreen;
    public string StateName { get; } = "Lawsuit";
    public void OnStateEnter(GameManager gameManager, GameStateMachine sm)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Lawsuit);
        CameraManager.Instance.GoToCamera(ECameraType.Lawsuit);
        lawsuitScreen = GameUIController.Instance.GetScreen(EScreenType.Lawsuit) as Screen_Lawsuit;
        gameManager.playerController.enabled = false;
    }

    public void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (lawsuitScreen.screenState ==
                Screen_Lawsuit.ELawsuitScreenState.Settlement)
            {
                lawsuitScreen.GoToLawsuitSelect();
            }
            else
            {
                gameManager.ReturnToHome();    
            }
        }
    }

    public void OnStateExit(GameManager gameManager, GameStateMachine sm)
    {
        if (gameManager.lawsuits.Count == 0)
        {
            LawsuitNotice.Instance.Hide();
        }
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
        Newspaper.Instance.Hide();
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
    public static bool isFirstResults = true;
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
        ///
        ///  deliver lawsuits after results
        /// 
        
        // obligatory lawsuit
        gameManager.DeliverLawsuit(EParty.None);

        // if pop is low, we MUST throw a new lawsuit for that party
        if (!isFirstResults)
        {
            if (gameManager.Popularity.Civilian 
                < GameConfig.Instance.LawsuitPopularityLimit)
            {
                gameManager.DeliverLawsuit(EParty.Civilian);
            }
        
            if (gameManager.Popularity.Companies 
                < GameConfig.Instance.LawsuitPopularityLimit)
            {
                gameManager.DeliverLawsuit(EParty.Companies);
            }
        
            if (gameManager.Popularity.Politician
                < GameConfig.Instance.LawsuitPopularityLimit)
            {
                gameManager.DeliverLawsuit(EParty.Politician);
            }
            
            // random chance lawsuit
            float rnd = Random.Range(0.00f, 1.00f);
            if (rnd < GameConfig.Instance.RandomChanceLawsuit)
            {
                gameManager.DeliverLawsuit(EParty.None);
            }
        }

        isFirstResults = false;
    }
}