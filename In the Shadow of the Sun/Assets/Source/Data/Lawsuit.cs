using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "In the Shadow of the Sun/New Lawsuit")]
public class Lawsuit : ScriptableObject
{
    public enum ESettlementType
    {
        Funds,
        Insurance
    }
    
    public string id;
    public string header;
    [TextArea(1,3)]public string content;
    public EParty party;
    public float cost;
}