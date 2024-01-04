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

    [Header("Payroll")] 
    public float costPerEmployee = 1.0f;
    public float overtimeMultiplier = 1.0f;
    
    [Header("Lawsuits")]
    public float RandomChanceLawsuit = 0.25f;
    public float LawsuitPopularityLimit = 30;
    
    [Header("Insurance")]
    public float StarterInsuranceFee = 250.0F;
    public float InsuranceMultiplier = 2.1f;
    
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

    public float overtimeStampDelay = 1.0f;
}