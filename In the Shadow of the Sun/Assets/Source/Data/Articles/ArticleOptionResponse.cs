[System.Serializable]
public class ArticleOptionResponse
{
    public string headline;
    public string content;

    public string GetHeadline()
    {
        return headline.Replace("{{OrgName}}", GameManager.Instance.OrganizationName);
    }
    
    public string GetContent()
    {
        return content.Replace("{{OrgName}}", GameManager.Instance.OrganizationName);
    }
}