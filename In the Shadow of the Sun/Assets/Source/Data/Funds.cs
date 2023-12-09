using System;
using UnityEngine;

[Serializable]
public class Funds
{
    [SerializeField] private float value;

    public float Value
    {
        get => value;
        set
        {
            this.value = value;
            OnValueChange?.Invoke(value);
        }
    }
    
    public Funds(){}
    public Funds(float value)
    {
        this.Value = value;
    }

    public override string ToString()
    {
        return Format(value);
    }

    public static Funds operator+(Funds funds, float value)
    {
        funds.Value += value;
        return funds;
    }
    
    public static Funds operator+(Funds fundsA, Funds fundsB)
    {
        fundsA.Value += fundsB.Value;
        return fundsA;
    }
    
    public static Funds operator-(Funds funds, float value)
    {
        funds.Value -= value;
        return funds;
    }
    
    public static Funds operator-(Funds fundsA, Funds fundsB)
    {
        fundsA.Value -= fundsB.Value;
        return fundsA;
    }

    public delegate void FundsDelegate(float value);
    public event FundsDelegate  OnValueChange;
    public static string Format(float value)
    {
        if (value < 0)
        {
            return String.Format("-{0:C}", Mathf.Abs(value));
        }
        return String.Format("{0:C}", value);
    }
}