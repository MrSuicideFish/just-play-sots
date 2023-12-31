using UnityEngine;

public class GameConfig : ScriptableObject
{
    private static GameConfig instance;
    public static GameConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameConfig>("GameConfig");
            }
            return instance;
        }
    }

    public float StarterFunds = 1500.00F;
    public float StarterPopularity = 50;
    public int StarterStaff = 25;

    [Space] 
    public int MaxPopularity = 100;

    [Header("Home")] 
    [TextArea(2, 5)] public string introArticleHeadline;
    [TextArea(2, 5)] public string introArticleContent;
    
    public string HomeTutorialTitle;
    [TextArea(2, 5)] public string HomeTutorialContent;

    [Header("Articles")] 
    public string ArticlesTutorialTitle;
    [TextArea(2, 5)] public string ArticlesTutorialContent = "";
    
    public string ResultsTutorialTitle;
    [TextArea(2, 5)] public string ResultsTutorialContent = "";
    
    [Header("Staff")]
    public string StaffTutorialTitle;
    [TextArea(2, 5)] public string StaffTutorialContent = "";
    public string StaffNoticeTitle;
    [TextArea(2, 5)] public string StaffNoticeContent = "";
    [Space] [Range(0f, 0.9f)] public float staffRequirementWidth = 0.3f;
    
    [Header("Payroll")] 
    public float costPerEmployee = 1.0f;
    public float overtimeMultiplier = 1.0f;
    
    [Header("Lawsuits")]
    public float RandomChanceLawsuit = 0.25f;
    public float LawsuitPopularityLimit = 30;
    public string LawsuitsTutorialTitle;
    [TextArea(2, 5)] public string LawsuitsTutorialContent = "";
    
    [Header("Insurance")]
    public float StarterInsuranceFee = 250.0F;
    public float InsuranceMultiplier = 2.1f;
    public string InsuranceTutorialTitle;
    [TextArea(2, 5)] public string InsuranceTutorialContent = "";

    [Header("Game Over")] 
    public string GameFailHeadline;
    public string GameFailReasonPopularity;
    public string GameFailReasonFunds;
    [Space] 
    public string GameWinHeadline;
    public string GameWinSubtext;
    public string GameWinContent;

    public string GameWinAltHeadline;
    public string GameWinAltSubtext;
    public string GameWinAltContent;
    
    [Space(5)]

    [Header("Timings / Polish")]
    public float donationDuration = 1.0f;
    public float civillianDonationDelay = 1.0f;
    public float politicianDonationDelay = 1.0f;
    public float organizationDonationsDelay = 1.0f;

    public float staffDuration = 1.0f;
    public float staffAllocatedDelay = 1.0f;
    public float staffTotalDelay = 1.0f;

    public float billDuration = 1.0f;
    public float billDelay = 1.0f;

    public float payrollDuration = 1.0f;
    public float payrollDelay = 1.0f;

    public float insuranceDuration = 1.0f;
    public float insuranceDelay = 1.0f;

    public float fundsResultDuration = 1.0f;
    public float fundsResultDelay = 1.0f;

    public float captionsFadeSpeed = 1.0f;

    public Color proponentColor;
    public Color criticColor;
    public Color neutralColor;
}