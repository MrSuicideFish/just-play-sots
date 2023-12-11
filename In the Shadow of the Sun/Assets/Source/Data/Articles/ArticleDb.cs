using UnityEngine;

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

    public Article GetArticleByIndex(int index)
    {
        if (index < 0 || index >= mainArticles.Length)
        {
            return null;
        }
        
        return mainArticles[index];
    }
}