using System;
using UnityEngine;
using UnityEngine.Events;

public enum EScreenType
{
    Intro,
    Home,
    Article,
    Result,
    Lawsuit,
    EndGame,
    Staff,
    Tutorial,
    Empty
}

public abstract class GameScreen : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnScreenShow;

    public abstract EScreenType GetScreenType();

    private bool HasShownFirstTime = false;
    
    public void ShowFTUE()
    {
        GameManager.Instance.StateMachine.currentState.ShowFTUE();
    }

    private void OnEnable()
    {
        Show();
    }

    private void OnDisable()
    {
        Hide();
    }

    public virtual void Show()
    {
        
    }

    public virtual void Hide()
    {
        
    }
}