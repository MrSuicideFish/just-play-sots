using System;
using UnityEngine.Serialization;

[Serializable]
public class ArticleOptionEffect
{
    public float donations;
    public float popularity;
}

[Serializable]
public class ArticleOption
{
    public string content;

    public float fundsCost;
    public int staffCost;

    public ArticleOptionEffect civilianEffect;
    public ArticleOptionEffect politicianEffect;
    public ArticleOptionEffect companiesEffect;

    public PopularityRequirement popularityRequirement;
    public ArticleOptionResponse response;
    
    public string GetContent()
    {
        return content.Replace("{{OrgName}}", GameManager.Instance.OrganizationName);
    }
}