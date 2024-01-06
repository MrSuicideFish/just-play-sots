using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public abstract class GameState
{
    public abstract EScreenType ScreenType { get; }
    private Dictionary<Type, GameScreen> screenCache = new();

    public T GetScreen<T>() where T : GameScreen
    {
        if (!screenCache.ContainsKey(typeof(T)))
        {
            screenCache[typeof(T)] = GameUIController.Instance.GetScreen(ScreenType);
        }
        return screenCache[typeof(T)] as T;
    }
    
    public abstract void OnStateEnter(GameManager gameManager, GameStateMachine sm, bool isFirstEnter);
    public abstract void OnStateUpdate(GameManager gameManager, GameStateMachine sm);
    public abstract void OnStateExit(GameManager gameManager, GameStateMachine sm, bool isFirstExit);
    
    public void ShowFTUE(string overrideTitle = null, UnityAction onComplete = null)
    {
        switch (ScreenType)
        {
            case EScreenType.Home:
                GameUIController.Instance
                    .ShowGameMessage(overrideTitle ?? GameConfig.Instance.HomeTutorialTitle,
                        GameConfig.Instance.HomeTutorialContent, onComplete);
                break;
            case EScreenType.Article:
                GameUIController.Instance
                    .ShowGameMessage(overrideTitle ?? GameConfig.Instance.ArticlesTutorialTitle,
                        GameConfig.Instance.ArticlesTutorialContent, onComplete);
                break;
            case EScreenType.Result:
                GameUIController.Instance
                    .ShowGameMessage(overrideTitle ?? GameConfig.Instance.ResultsTutorialTitle,
                        GameConfig.Instance.ResultsTutorialContent, onComplete);
                break;
            case EScreenType.Lawsuit:
                GameUIController.Instance
                    .ShowGameMessage(overrideTitle ?? GameConfig.Instance.LawsuitsTutorialTitle,
                        GameConfig.Instance.LawsuitsTutorialContent, onComplete);
                break;
            case EScreenType.Staff:
                GameUIController.Instance
                    .ShowGameMessage(overrideTitle ?? GameConfig.Instance.StaffTutorialTitle,
                        GameConfig.Instance.StaffTutorialContent, onComplete);
                break;
        }
    }
}

public class Gamestate_Entry : GameState
{
    public override EScreenType ScreenType { get; } = EScreenType.Intro;

    public override void OnStateEnter(GameManager gameManager, GameStateMachine sm, bool isFirstEnter)
    {
        Newspaper.Instance.Hide();
        LawsuitNotice.Instance.Hide();
        Phone.Instance.Hide();
        gameManager.playerController.enabled = false;
        CameraManager.Instance.GoToCamera(ECameraType.Intro);
        GameUIController.Instance.GoToScreen(ScreenType);
    }

    public override void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {
        if (GetScreen<Screen_Intro>().IsComplete)
        {
            GetScreen<Screen_Intro>().radioAudioSrc.Play();
            sm.GoToState(new GameState_Home());
        }
    }

    public override void OnStateExit(GameManager gameManager, GameStateMachine sm, bool isFirstExit)
    {
    }
}

public class GameState_Home : GameState
{
    public override EScreenType ScreenType { get; } = EScreenType.Home;
    public override void OnStateEnter(
        GameManager gameManager, 
        GameStateMachine sm,
        bool isFirstEnter)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Home);
        CameraManager.Instance.GoToCamera(ECameraType.Home);
        if (isFirstEnter)
        {
            GetScreen<Screen_Home>().BeginIntroArticle(() =>
            {
                GetScreen<Screen_Home>().EndIntroArticle();
                ShowFTUE(null,() =>
                {
                    // enable player controls
                    gameManager.playerController.enabled = true;
                
                    GetScreen<Screen_Home>().orgNamePanel.Opacity = 0;
                    GetScreen<Screen_Home>().text_orgName.text = gameManager.OrganizationName;
                    GetScreen<Screen_Home>().orgNamePanel.Set(0.0f);
                
                    GameUIController.Instance.GoToScreen(EScreenType.Home);
                
                    // fade in org name and deliver first article
                    DOTween.To(
                        () => GetScreen<Screen_Home>().orgNamePanel.Opacity,
                        (x) => GetScreen<Screen_Home>().orgNamePanel.Opacity = x,
                        1.0f, 5.0f).OnComplete(() =>
                    {
                    
                        gameManager.DeliverArticle();
                    
                        // fade out org name
                        DOTween.To(
                                () => GetScreen<Screen_Home>().orgNamePanel.Opacity,
                                (x) => GetScreen<Screen_Home>().orgNamePanel.Opacity = x,
                                0.0f, 2.5f)
                            .SetDelay(3.0f);
                    });
                });
            });
           
            return;
        }

        if (gameManager.CurrentArticle != null)
        {
            Newspaper.Instance.Show(false);
        }
        else if (gameManager.CompletedArticles.Count > 0
                  && gameManager.completedLawsuits.Count > 0)
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
            
            if (firstShow && gameManager.CompletedArticles.Count == 1)
            {
                if (!gameManager.hasHiredFirstStaff)
                {
                    Phone.Instance.Show(true);
                    GameUIController.Instance.ShowGameMessage(
                        GameConfig.Instance.StaffNoticeTitle,
                        GameConfig.Instance.StaffNoticeContent,
                        null);    
                }
                else
                {
                    LawsuitNotice.Instance.Show(true);
                }
            }
            else
            {
                if (!gameManager.isHomeStateClean)
                {
                    firstShow = false;
                }
                else
                {
                    // settle lawsuits
                    int lawsuitCount = gameManager.expiredLawsuits.Count;
                    if (lawsuitCount > 0)
                    {
                        for (int i = 0; i < gameManager.expiredLawsuits.Count; i++)
                        {
                            gameManager.SettleLawsuit(gameManager.expiredLawsuits[i]);
                            gameManager.expiredLawsuits.RemoveAt(i);
                        }

                        GameUIController.Instance.ShowGameMessage(
                            "Expired Lawsuits",
                            $"{lawsuitCount} lawsuit(s) have been settled automatically.", null);
                    }
                    
                }
                
                LawsuitNotice.Instance.Show(firstShow);   
            }
        }

        if (gameManager.Popularity.Civilian <= 0
            || gameManager.Popularity.Companies <= 0
            || gameManager.Popularity.Politician <= 0)
        {
            gameManager.EndGame(false, true);
            return;
        }
        
        gameManager.playerController.enabled = true;
    }
    
    public override void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {

    }

    public override void OnStateExit(GameManager gameManager, GameStateMachine sm, bool isFirstExit)
    {
    }
}

public class GameState_Lawsuit : GameState
{
    public override EScreenType ScreenType { get; } = EScreenType.Lawsuit;
    public override void OnStateEnter(GameManager gameManager, GameStateMachine sm, bool isFirstEnter)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Lawsuit);
        CameraManager.Instance.GoToCamera(ECameraType.Lawsuit);
        gameManager.playerController.enabled = false;
        gameManager.isHomeStateClean = false;
        if (isFirstEnter)
        {
            ShowFTUE();
        }
    }

    public override void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GetScreen<Screen_Lawsuit>().screenState ==
                Screen_Lawsuit.ELawsuitScreenState.Settlement)
            {
                GetScreen<Screen_Lawsuit>().GoToLawsuitSelect();
            }
            else
            {
                gameManager.ReturnToHome();    
            }
        }
    }

    public override void OnStateExit(GameManager gameManager, GameStateMachine sm, bool isFirstExit)
    {
        if (gameManager.lawsuits.Count == 0)
        {
            LawsuitNotice.Instance.Hide();
        }
    }
}

public class GameState_Newspaper : GameState
{
    public override EScreenType ScreenType { get; } = EScreenType.Article;
    public override void OnStateEnter(GameManager gameManager, GameStateMachine sm, bool isFirstEnter)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Article);
        CameraManager.Instance.GoToCamera(ECameraType.Newspaper);
        gameManager.playerController.enabled = false;
        Newspaper.Instance.Hide();
        gameManager.isHomeStateClean = false;
        if (isFirstEnter)
        {
            ShowFTUE();
        }
    }

    public override void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.ReturnToHome();
        }
    }

    public override void OnStateExit(GameManager gameManager, GameStateMachine sm, bool isFirstExit)
    {
    }
}

public class GameState_Staff : GameState
{
    public override EScreenType ScreenType { get; } = EScreenType.Staff;
    public override void OnStateEnter(GameManager gameManager, GameStateMachine sm, bool isFirstEnter)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Staff);
        CameraManager.Instance.GoToCamera(ECameraType.Phone);
        gameManager.playerController.enabled = false;
        gameManager.isHomeStateClean = false;
        
        if (isFirstEnter)
        {
            ShowFTUE();
        }
    }

    public override void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.ReturnToHome();
        }
    }

    public override void OnStateExit(GameManager gameManager, GameStateMachine sm, bool isFirstExit)
    {
    }
}

public class GameState_Results : GameState
{
    public override EScreenType ScreenType { get; } = EScreenType.Result;

    private ArticleOption selectedOption;
    public override void OnStateEnter(GameManager gameManager, GameStateMachine sm, bool isFirstEnter)
    {
        GameUIController.Instance.GoToScreen(EScreenType.Result);
        CameraManager.Instance.GoToCamera(ECameraType.Newspaper);
        gameManager.playerController.enabled = false;
        if (isFirstEnter)
        {
            ShowFTUE("Public Reactions", () =>
            {
                GetScreen<Screen_Result>().CountResults(gameManager.SelectedOption);
            });
        }
        else
        {
            GetScreen<Screen_Result>().CountResults(gameManager.SelectedOption);
        }
    }

    public override void OnStateUpdate(GameManager gameManager, GameStateMachine sm)
    {
    }

    public override void OnStateExit(GameManager gameManager, GameStateMachine sm, bool isFirstExit)
    {
        ///
        ///  deliver lawsuits after results
        /// 
        
        // mature current suits
        for (int i = 0; i < gameManager.lawsuits.Count; i++)
        {
            DeliveredLawsuit lawsuit = gameManager.lawsuits[i];
            if (lawsuit.isMature)
            {
                gameManager.expiredLawsuits.Add(lawsuit.lawsuit.id);
            }
            else
            {
                gameManager.lawsuits[i].isMature = true;    
            }
        }
        
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