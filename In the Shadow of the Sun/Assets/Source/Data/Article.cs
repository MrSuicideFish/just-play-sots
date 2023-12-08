using Newtonsoft.Json;

[System.Serializable]
public class ArticleOptionResponse
{
    [JsonProperty]
    public string Headline { get; set; }
    
    [JsonProperty]
    public string Content { get; set; }
}

[System.Serializable]
public class ArticleOption
{
    [JsonProperty]
    public string Content { get; set; }
    
    [JsonProperty]
    public bool IsSelected { get; set; }
    
    [JsonProperty]
    public float CivilianEffect { get; set; }
    
    [JsonProperty]
    public float PoliticianEffect { get; set; }
    
    [JsonProperty]
    public float CompaniesEffect { get; set; }
    
    [JsonProperty]
    public ArticleOptionResponse Response { get; set; }
}

[System.Serializable]
public class Article
{
    [JsonProperty]
    public string ID { get; set; }
    
    [JsonProperty]
    public string Headline { get; set; }
    
    [JsonProperty]
    public PopularityRequirement Requirement { get; set; }
    
    [JsonProperty]
    public ArticleOption[] Options { get; set; }
}
