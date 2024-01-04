
using System;
using System.Security.Authentication.ExtendedProtection;
using UnityEditor;
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

    public float Get(EParty party)
    {
        switch (party)
        {
            case EParty.Civilian:
                return Civilian;
            case EParty.Politician:
                return Politician;
            case EParty.Companies:
                return Companies;
        }
        return Mathf.Infinity;
    }

    public float GetLowestPopularity(out EParty party)
    {
        float tmp;
        float lowest = Mathf.Infinity;
        party = EParty.None;

        if ((Civilian == Politician) 
            && (Politician == Companies))
        {
            party = EParty.None;
            return Civilian;
        }

        if ((tmp = Get(EParty.Civilian)) < lowest)
        {
            lowest = tmp;
            party = EParty.Civilian;
        }
        
        if ((tmp = Get(EParty.Companies)) < lowest)
        {
            lowest = tmp;
            party = EParty.Companies;
        }

        if ((tmp = Get(EParty.Politician)) < lowest)
        {
            lowest = tmp;
            party = EParty.Politician;
        }

        return lowest;
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