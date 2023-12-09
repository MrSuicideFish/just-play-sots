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
    public float StarterInsuranceFee = 250.0F;
}