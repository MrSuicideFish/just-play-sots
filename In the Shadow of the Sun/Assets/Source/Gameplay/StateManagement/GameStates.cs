using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public interface IGameState<T> where T : IGameScreen
{
    EScreenType ScreenType { get; }

    T Screen
    {
        get
        {
            return GameUIController.Instance.GetScreen(ScreenType) as T;
        }
    }

    void OnStateEnter(GameManager gameManager, GameStateMachine sm);
    void OnStateUpdate(GameManager gameManager, GameStateMachine sm);
    void OnStateExit(GameManager gameManager, GameStateMachine sm);
    
    void ShowFTUE(UnityAction onComplete = null)
    {
        switch (ScreenType)
        {
            case EScreenType.Home:
                GameUIController.Instance
                    .ShowGameMessage(GameConfig.Instance.HomeTutorialTitle,
                        GameConfig.Instance.HomeTutorialContent, onComplete);
                break;
            case EScreenType.Lawsuit:
                GameUIController.Instance
                    .ShowGameMessage(GameConfig.Instance.LawsuitsTutorialTitle,
                        GameConfig.Instance.LawsuitsTutorialContent, onComplete);
                break;
            case EScreenType.Article:
                GameUIController.Instance
                    .ShowGameMessage(GameConfig.Instance.ArticlesTutorialTitle,
                        GameConfig.Instance.ArticlesTutorialContent, onComplete);
                break;
            case EScreenType.Staff:
                GameUIController.Instance
                    .ShowGameMessage(GameConfig.Instance.StaffTutorialTitle,
                        GameConfig.Instance.StaffTutorialContent, onComplete);
                break;
        }
    }
}

public class Gamestate_Entry : IGameState<Screen_Intro>
{
    private Screen_Intro introScreen;
    public EScreenType ScreenType { get; } = EScreenType.Intro;

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
            GameState_Home home = new GameState_Home();
            sm.GoToState()
            sm.GoToState<GameState_Home>();
        }
    }

    public void OnStateExit(GameManager gameManager, GameStateMachine sm)
    {
    }
}

public class GameState_Home : IGameState<Screen_Home>
{
    public EScreenType ScreenType { get; } = EScreenType.Home;
    public void OnStateEnter(GameManager gameManager, GameStateMachine sm)
    {
        CameraManager.Instance.GoToCamera(ECameraType.Home);
        if (!gameManager.hasCompletedFirstHome)
        {
            gameManager.StartCoroutine(WaitForFTUE(gameManager));
            return;
        }

        if (gameManager.CurrentArticle != null)
        {
            Newspaper.Instance.Show(false);
        }
        else if (gameManager.hasCompletedFirstArticle
                  && gameManager.hasCompletedFirstLawsuit)
        {
            if (!gameManager.DeliverArticle())
            {
                // we've delivered the last article already, game is over
                gameManager.EndGame(isWin: true);
            }
        }
        
        if (gameManager.lawsuits.Count > 0)
        {
            bool firstShow = gameManager.completedLawsuits.Count == 0
            || gameManager.lawsuitsAddedThisArticle == gameManager.lawsuits.Count;
            
            if (!gameManager.isHomeStateClean)
            {
                firstShow = false;
            }
            LawsuitNotice.Instance.Show(firstShow);
        }
        
        GameUIController.Instance.GoToScreen(EScreenType.Home);
        gameManager.playerController.enabled = true;
    }

    private IEnumerator WaitForFTUE(GameManager gameManager)
    {
        Screen_Home homeScreen = GameUIController.Instance.GetScreen(EScreenType.Home) as Screen_Home;
        homeScreen.firstTimeIntro.Opacity = 0;
        homeScreen.orgNameIntro.text = gameManager.OrganizationName;

        // do ftue here
        
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

public class GameState_Lawsuit : IGameState<Screen_Lawsuit>
{
    public EScreenType ScreenType { get; } = EScreenType.Lawsuit;
    public void OnStateEnter(GameManager gameManager, GameStateMachine sm)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Lawsuit);
        CameraManager.Instance.GoToCamera(ECameraType.Lawsuit);
        lawsuitScreen = GameUIController.Instance.GetScreen(EScreenType.Lawsuit) as Screen_Lawsuit;
        gameManager.playerController.enabled = false;
        gameManager.isHomeStateClean = false;
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

public class GameState_Newspaper : IGameState<Screen_Article>
{
    public EScreenType ScreenType { get; } = EScreenType.Article;
    public void OnStateEnter(GameManager gameManager, GameStateMachine sm)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Article);
        CameraManager.Instance.GoToCamera(ECameraType.Newspaper);
        gameManager.playerController.enabled = false;
        Newspaper.Instance.Hide();
        gameManager.isHomeStateClean = false;
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

public class GameState_Staff : IGameState<Screen_Staff>
{
    public EScreenType ScreenType { get; } = EScreenType.Staff;
    public void OnStateEnter(GameManager gameManager, GameStateMachine sm)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Staff);
        CameraManager.Instance.GoToCamera(ECameraType.Phone);
        gameManager.playerController.enabled = false;
        gameManager.isHomeStateClean = false;
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

public class GameState_Results : IGameState<Screen_Result>
{
    public EScreenType ScreenType { get; } = EScreenType.Result;
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
        if (gameManager.CompletedArticles.Count > 1)
        {
            float rnd = Random.Range(0.00f, 1.00f);
            if (rnd < GameConfig.Instance.RandomChanceLawsuit)
            {
                gameManager.DeliverLawsuit(EParty.None);
            }
        }
    }
}