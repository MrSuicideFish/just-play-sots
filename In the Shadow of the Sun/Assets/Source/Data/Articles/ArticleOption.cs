using System;
using UnityEngine.Serialization;

[Serializable]
public class ArticleOptionEffect
{
    public float donations;
    public int popularity;
}

[Serializable]
public class ArticleOption
{
    public string content;

    public float fundsCost;
    public int staffCost;

    [FormerlySerializedAs("civilianEffect")] public ArticleOptionEffect citizenEffect;
    public ArticleOptionEffect politicianEffect;
    public ArticleOptionEffect companiesEffect;

    public PopularityRequirement popularityRequirement;
    public ArticleOptionResponse response;
    
    public string GetContent()
    {
        return content.Replace("{{OrgName}}", GameManager.Instance.OrganizationName);
    }
}