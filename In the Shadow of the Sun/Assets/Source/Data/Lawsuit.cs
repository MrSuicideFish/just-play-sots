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
    
    public string GetHeader()
    {
        return header.Replace("{{OrgName}}", GameManager.Instance.OrganizationName);
    }
    
    public string GetContent()
    {
        return content.Replace("{{OrgName}}", GameManager.Instance.OrganizationName);
    }
}

public class DeliveredLawsuit
{
    public Lawsuit lawsuit;
    public bool isMature;
}