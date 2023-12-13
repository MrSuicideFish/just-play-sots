using System;
using System.Collections;
using System.Collections.Generic;
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
    
    // player
    public PlayerController playerController;
    
    // cinematic
    public Animation introAnimation;

    // Cache
    private bool gameHasStarted;
    private bool gameHasEnded;

    public Article CurrentArticle { get; private set; }
    public ArticleOption SelectedOption { get; set; }
    public ArticleOptionResponse CurrentResponse { get; private set; }
    public Lawsuit CurrentLawsuit { get; private set; }


    private int articleIndex = 0;
    public string OrganizationName { get; set; }
    public Popularity Popularity { get; private set; }
    public Funds OrganizationFunds { get; private set; }
    public Insurance Insurance { get; private set; }
    
    public GameStateMachine StateMachine { get; private set; }

    // Events
    public UnityEvent<Popularity> OnPopularityChanged;
    public UnityEvent<float> OnFundsChanged;

    private void Update() => StateMachine.TickStateMachine(this);
    private void Start()
    {
        CurrentArticle = ArticleDb.Instance.GetArticleByIndex(0);
        StateMachine = new GameStateMachine(this);
        Popularity = new(GameConfig.Instance.StarterPopularity);
        OrganizationFunds = new(GameConfig.Instance.StarterFunds);
        OrganizationFunds.OnValueChange += (val) => OnFundsChanged.Invoke(val);
        Insurance = new(GameConfig.Instance.StarterInsuranceFee);
        GameUIController.Instance.GoToScreen(EScreenType.Intro);
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

    private EParty RollForLawsuit()
    {
        List<EParty> unpopularity = new();
        for (int i = 0; i < EParty.Count.GetHashCode(); i++)
        {
            EParty p = (EParty) i;
            if (Popularity.Get(p) < GameConfig.Instance.UnpopularThreshold)
            {
                unpopularity.Add(p);
            }
        }

        return (unpopularity.Count > 0 && UnityEngine.Random.Range(0.0000f, 1.0000f) < GameConfig.Instance.LawsuitProbability)
            ? unpopularity[UnityEngine.Random.Range(0, unpopularity.Count)]
            : EParty.None;
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

    public void EndArticle()
    {
        articleIndex++;
        CurrentArticle = ArticleDb.Instance.GetArticleByIndex(articleIndex);
        if (CurrentArticle != null)
        {
            ReturnToHome();
        }
    }

    public void ReturnToHome()
    {
        EParty pendingLawsuitRequest = RollForLawsuit();
        if (pendingLawsuitRequest != EParty.None)
        {
            int rndLawsuit = UnityEngine.Random.Range(0, ArticleDb.Instance.lawsuits.Length);
            CurrentLawsuit = ArticleDb.Instance.lawsuits[rndLawsuit];
        }
        
        StateMachine.GoToState(this, new GameState_Home());
    }

    public void StartGame()
    {
        if (gameHasStarted)
        {
            return;
        }

        gameHasStarted = true;
        CurrentArticle = ArticleDb.Instance.GetArticleByIndex(0);
        StateMachine.GoToState(this, new Gamestate_Entry());
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