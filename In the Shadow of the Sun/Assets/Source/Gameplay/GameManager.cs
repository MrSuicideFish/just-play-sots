using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

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

    [Header("Debugging")] 
    public bool debug_SkipCutscene;
    
    public PlayerController playerController;
    public string OrganizationName { get; set; }
    public Popularity Popularity { get; private set; }
    public Funds OrganizationFunds { get; private set; }
    public Insurance Insurance { get; private set; }
    public Staff Staff { get; private set; }
    public GameStateMachine StateMachine { get; private set; }

    // Cache
    private bool gameHasStarted;
    private bool gameHasEnded;
    
    // Events
    public UnityEvent<Popularity> OnPopularityChanged;
    public UnityEvent<float> OnFundsChanged;
    public UnityEvent OnPaperDelivered, OnLawsuitDelivered;

    private void Update() => StateMachine.TickStateMachine(this);
    private void Start()
    {
        StateMachine = new GameStateMachine(this);
        Popularity = new(GameConfig.Instance.StarterPopularity);
        OrganizationFunds = new(GameConfig.Instance.StarterFunds);
        OrganizationFunds.OnValueChange += (val) => OnFundsChanged.Invoke(val);
        Insurance = new(GameConfig.Instance.StarterInsuranceFee);
        Staff = new();
        
        StartGame();
    }
    
    public void ReturnToHome()
    {
        StateMachine.GoToState(new GameState_Home());
    }

    public void StartGame()
    {
        if (gameHasStarted)
        {
            return;
        }

        gameHasStarted = true;
        Newspaper.Instance.Hide();
        LawsuitNotice.Instance.Hide();
        
        StateMachine.GoToState(new Gamestate_Entry());
    }

    public void EndGame()
    {
        if (gameHasEnded)
        {
            return;
        }

        gameHasEnded = true;
    }
    
    
    #region Article
    public Article CurrentArticle { get; private set; }
    public ArticleOption SelectedOption { get; set; }
    public ArticleOptionResponse CurrentResponse { get; private set; }
    public List<Article> CompletedArticles { get; private set; } = new();
    
    private int articleIndex = 0;
    
    public IEnumerator DeliverArticle()
    {
        CurrentArticle = ArticleDb.Instance.GetArticleByIndex(articleIndex);
        Newspaper.Instance.Show(true);
        OnPaperDelivered?.Invoke();
        yield return null;
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

        // save current article as completed
        CurrentArticle.selectedOption = optionIndex;
        CompletedArticles.Add(CurrentArticle);
        
        StateMachine.GoToState(new GameState_Results());
    }

    public void EndArticle()
    {
        articleIndex++;
        CurrentArticle = ArticleDb.Instance.GetArticleByIndex(articleIndex);
        if (CurrentArticle != null)
        {
            ReturnToHome();
        }
    }
    #endregion
    
    
    #region Lawsuits
    public Lawsuit CurrentLawsuit { get; private set; }
    public List<Lawsuit> lawsuits { get; private set;} = new();
    public List<string> completedLawsuits { get; private set; } = new();
    
    public IEnumerator DeliverLawsuit(EParty fromParty)
    {
        OnLawsuitDelivered?.Invoke();
        yield return null;
    }

    public void SelectLawsuit(int index)
    {
        if (index < 0 || index >= lawsuits.Count)
        {
            return;
        }
        
        CurrentLawsuit = lawsuits[index];
    }
    
    public void SettleLawsuit(Lawsuit.ESettlementType settlementType)
    {
        if (settlementType == Lawsuit.ESettlementType.Funds)
        {
            OrganizationFunds -= CurrentLawsuit.cost;
        }else if (settlementType == Lawsuit.ESettlementType.Insurance)
        {
            Insurance.SettleLawsuit(CurrentLawsuit.cost);
        }

        ReturnToHome();
    }
    #endregion
    
}