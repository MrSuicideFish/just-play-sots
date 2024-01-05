using System;
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
        Insurance = new(GameConfig.Instance.StarterInsuranceFee);
        Staff = new(GameConfig.Instance.StarterStaff);
        
        //subscribe to events
        OrganizationFunds.OnValueChange += (val) => OnFundsChanged.Invoke(val);
        
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

        
        StateMachine.GoToState(new Gamestate_Entry());
    }

    public void EndGame(bool isWin)
    {
        if (gameHasEnded)
        {
            return;
        }

        Debug.Log($"GAME OVER! Is Win? {isWin}");
        playerController.enabled = false;

        Screen_EndGame endGameScreen = GameUIController.Instance
            .GetScreen(EScreenType.EndGame) as Screen_EndGame;
        endGameScreen.gameObject.SetActive(true);
        if (isWin)
        {
            StartCoroutine(endGameScreen.DoWin(0));
        }
        else
        {
            StartCoroutine(endGameScreen.DoLose());
        }
        gameHasEnded = true;
    }
    
    
    #region Article
    public Article CurrentArticle { get; private set; }
    public ArticleOption SelectedOption { get; set; }
    public ArticleOptionResponse CurrentResponse { get; private set; }
    public List<Article> CompletedArticles { get; private set; } = new();
    [NonSerialized] public bool isHomeStateClean = false;
    private int articleIndex = 0;
    
    public bool DeliverArticle()
    {
        CurrentArticle = ArticleDb.Instance.GetArticleByIndex(articleIndex);
        if (CurrentArticle == null)
        {
            return false;
        }
        Newspaper.Instance.Show(true);
        OnPaperDelivered?.Invoke();
        return true;
    }

    public float CalcTotalCost()
    {
        float total = 0.0f;
        
        // bill
        total += SelectedOption.fundsCost;
        
        // payroll
        float payroll = Staff.Total * GameConfig.Instance.costPerEmployee;
        float remainingStaff = Instance.Staff.Count - SelectedOption.staffCost;
        
        // overtime pay
        if (remainingStaff < 0)
        {
            payroll += (Staff.Total * Mathf.Abs(remainingStaff)) *
                       (GameConfig.Instance.costPerEmployee * GameConfig.Instance.overtimeMultiplier);
        }

        // staff
        total += payroll;
        
        // insurance
        total += Insurance.fee.Value;
        return total;
    }

    public float CalcTotalDonations()
    {
        float total = 0.0f;
        
        // donations
        total += SelectedOption.civilianEffect.donations;
        total += SelectedOption.politicianEffect.donations;
        total += SelectedOption.companiesEffect.donations;

        return total;
    }

    public void ApplyStaffChanges()
    {
        Staff.Count -= SelectedOption.staffCost;
        if (Staff.Count < 0)
        {
            Staff.Count = 0;
        }
    }

    public void SelectArticleOption(int optionIndex)
    {
        ArticleOption option = CurrentArticle.options[optionIndex];
        CurrentResponse = option.response;
        SelectedOption = option;
        

        
        // popularity
        Popularity.Apply(EParty.Civilian, SelectedOption.civilianEffect.popularity);
        Popularity.Apply(EParty.Politician, SelectedOption.politicianEffect.popularity);
        Popularity.Apply(EParty.Companies, SelectedOption.companiesEffect.popularity);
        
        // insurance contributions
        Insurance.totalContributions += Insurance.fee;
        
        // save current article as completed
        CurrentArticle.selectedOption = optionIndex;
        CompletedArticles.Add(CurrentArticle);
        CurrentArticle = null;
        articleIndex++;

        isHomeStateClean = true;
        lawsuitsAddedThisArticle = 0;
        StateMachine.GoToState(new GameState_Results());
    }
    #endregion
    
    
    #region Lawsuits
    public Lawsuit CurrentLawsuit { get; private set; }
    public List<Lawsuit> lawsuits { get; private set;} = new();
    public List<string> completedLawsuits { get; private set; } = new();
    public int lawsuitsAddedThisArticle = 0;
    
    public void DeliverLawsuit(EParty fromParty)
    {
        Lawsuit[] suits = (fromParty == EParty.None)
            ? ArticleDb.Instance.lawsuits
            : ArticleDb.Instance.GetLawsuitsByParty(fromParty);

        for (int i = 0; i < suits.Length; i++)
        {
            if (!completedLawsuits.Contains(suits[i].id)
                && !lawsuits.Contains(suits[i]))
            {
                lawsuits.Add(suits[i]);
                OnLawsuitDelivered?.Invoke();
                lawsuitsAddedThisArticle++;
                break;
            }
        }
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

        lawsuits.Remove(CurrentLawsuit);
        completedLawsuits.Add(CurrentLawsuit.id);
        ReturnToHome();
    }
    #endregion
    
}