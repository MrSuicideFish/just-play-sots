using UnityEngine;
using System.Collections.Generic;

public class ArticleDb : ScriptableObject
{
    private static ArticleDb instance;

    public static ArticleDb Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<ArticleDb>("ArticleDatabase");
            }
            return instance;
        }
    }

    public Article[] mainArticles;
    public Article[] sideArticles;
    public Lawsuit[] lawsuits;

    public Lawsuit[] CivillianLawsuits
    {
        get
        {
            List<Lawsuit> result = new List<Lawsuit>(lawsuits);
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].party != EParty.Civilian)
                {
                    result.RemoveAt(i);
                }
            }

            return result.ToArray();
        }
    }
    public Lawsuit[] PoliticianLawsuits 
    {
        get
        {
            List<Lawsuit> result = new List<Lawsuit>(lawsuits);
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].party != EParty.Politician)
                {
                    result.RemoveAt(i);
                }
            }

            return result.ToArray();
        }
    }
    public Lawsuit[] OrganizationLawsuits 
    {
        get
        {
            List<Lawsuit> result = new List<Lawsuit>(lawsuits);
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].party != EParty.Companies)
                {
                    result.RemoveAt(i);
                }
            }

            return result.ToArray();
        }
    }

    public Article GetArticleByIndex(int index)
    {
        if (index < 0 || index >= mainArticles.Length)
        {
            return null;
        }
        
        return mainArticles[index];
    }
}