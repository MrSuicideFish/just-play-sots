
using System;
using UnityEngine;

[Serializable]
public class PopularityRequirement
{
    public EParty Party;
    public float Value;
}

public class Popularity
{
    public float Civilian;
    public float Politician;
    public float Companies;

    public Popularity(float startPopularity)
    {
        Civilian = startPopularity;
        Politician = startPopularity;
        Companies = startPopularity;
    }

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

    public bool CompareRequirement(PopularityRequirement requirement)
    {
        switch (requirement.Party)
        {
            case EParty.Civilian:
                return Civilian > requirement.Value;
            case EParty.Politician:
                return Politician > requirement.Value;
            case EParty.Companies:
                return Companies > requirement.Value;
            default:
                return false;
        }
    }

    public static string Format(float value)
    {
        if (value < 0)
        {
            return $"-{Mathf.Abs(value).ToString()}";
        }
        return $"+{Mathf.Abs(value).ToString()}";
    }
}