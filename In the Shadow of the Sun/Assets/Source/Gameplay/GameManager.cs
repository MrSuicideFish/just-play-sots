using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    
    // Cache
    private bool gameHasStarted;
    private bool gameHasEnded;
    
    public Article CurrentArticle { get; private set; }
        
    public ArticleOption SelectedOption { get; set; }
    public ArticleOptionResponse CurrentResponse { get; private set; }


    private int articleIndex = 0;
    public string OrganizationName { get; set; } = String.Empty;
    public Popularity Popularity { get; private set; }
    public Funds OrganizationFunds { get; private set; }
    public Insurance Insurance { get; private set; }
    
    // Events
    public UnityEvent<Popularity> OnPopularityChanged;
    public UnityEvent<float> OnFundsChanged;

    private void Awake()
    {
        CurrentArticle = ArticleDb.Instance.GetArticleByIndex(0);
    }

    private void Start()
    {
        Popularity = new(GameConfig.Instance.StarterPopularity);
        OrganizationFunds = new(GameConfig.Instance.StarterFunds);
        OrganizationFunds.OnValueChange += (val) => OnFundsChanged.Invoke(val);
        Insurance = new(GameConfig.Instance.StarterInsuranceFee);
        
        GameUIController.Instance.GoToScreen(EScreenType.StartGame);
    }

    public void SelectArticleOption(int optionIndex)
    {
        ArticleOption option = CurrentArticle.options[optionIndex];
        CurrentResponse = option.response;
        
        OrganizationFunds -= option.cost;
        OrganizationFunds -= Insurance.fee;
        Insurance.totalContributions += Insurance.fee;
        
        OrganizationFunds += option.civilianEffect.donations;
        OrganizationFunds += option.politicianEffect.donations;
        OrganizationFunds += option.companiesEffect.donations;
        
        Popularity.Apply(EParty.Civilian, option.civilianEffect.popularity);
        Popularity.Apply(EParty.Politician, option.politicianEffect.popularity);
        Popularity.Apply(EParty.Companies, option.companiesEffect.popularity);
        
        SelectedOption = option;
        GameUIController.Instance.GoToScreen(EScreenType.Result);
    }

    public void EndArticle()
    {
        articleIndex++;
        CurrentArticle = ArticleDb.Instance.GetArticleByIndex(articleIndex);
        if (CurrentArticle != null)
        {
            GameUIController.Instance.GoToScreen(EScreenType.Article);
        }
        else
        {
            GameUIController.Instance.GoToScreen(EScreenType.EndGame);
        }
    }

    public void StartGame()
    {
        if (gameHasStarted)
        {
            return;
        }
        
        gameHasStarted = true;
        CurrentArticle = ArticleDb.Instance.GetArticleByIndex(0);
        GameUIController.Instance.GoToScreen(EScreenType.Article);
    }

    public void EndGame()
    {
        if (gameHasEnded)
        {
            return;
        }
        
        gameHasEnded = true;
    }
}