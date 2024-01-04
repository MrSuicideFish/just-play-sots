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
    
    [Header("Lawsuits")]
    public float RandomChanceLawsuit = 0.25f;
    public float LawsuitPopularityLimit = 30;
    
    [Header("Insurance")]
    public float StarterInsuranceFee = 250.0F;
    public float InsuranceMultiplier = 2.1f;
}