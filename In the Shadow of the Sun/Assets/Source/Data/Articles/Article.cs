using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "In the Shadow of the Sun/New Article")]
public class Article : ScriptableObject
{
    public string id;
    public string headline;
    [TextArea(2,30)]public string subtitle;
    [TextArea(3,30)]public string content;
    public ArticleOption[] options;

    [NonSerialized] public int selectedOption = -1;

    public string GetHeadline()
    {
        return headline.Replace("{{OrgName}}", GameManager.Instance.OrganizationName);
    }

    public string GetSubtitle()
    {
        return subtitle.Replace("{{OrgName}}", GameManager.Instance.OrganizationName);
    }
    
    public string GetContent()
    {
        return content.Replace("{{OrgName}}", GameManager.Instance.OrganizationName);
    }
}
