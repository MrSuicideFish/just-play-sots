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
    public float UnpopularThreshold = 30;
    
    [Header("Lawsuits")]
    public float LawsuitProbability = 0.25f;
    
    [Header("Insurance")]
    public float StarterInsuranceFee = 250.0F;
    public float InsuranceMultiplier = 2.1f;
}