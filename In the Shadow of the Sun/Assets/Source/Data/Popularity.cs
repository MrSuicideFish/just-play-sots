
using Newtonsoft.Json;

public class PopularityRequirement
{
    [JsonProperty]
    public string Party { get; set; }
    
    [JsonProperty]
    public float Value { get; set; }
}

public class Popularity
{
    public float Civilian;
    public float Politician;
    public float Companies;

    public void Apply(EParty party, float value)
    {
        switch (party)
        {
            case EParty.Civilian:
                Civilian += value;
                break;
            case EParty.Politician:
                Politician += value;
                break;
            case EParty.Companies:
                Companies += value;
                break;
        }
    }
}