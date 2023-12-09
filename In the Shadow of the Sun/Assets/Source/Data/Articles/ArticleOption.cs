using System;

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

    public float cost;

    public ArticleOptionEffect civilianEffect;
    public ArticleOptionEffect politicianEffect;
    public ArticleOptionEffect companiesEffect;
   
    public PopularityRequirement requirement;
    public ArticleOptionResponse response;
}