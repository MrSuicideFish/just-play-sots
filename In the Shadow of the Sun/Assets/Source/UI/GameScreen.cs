using System;
using System.Collections;
using System.Collections.Generic;
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

    private bool HasShown = false;
    
    public void ShowFTUE()
    {
        GameManager.Instance.StateMachine.currentState.ShowFTUE();
    }

    private void OnEnable()
    {
        StartCoroutine(Show(!HasShown));
        HasShown = true;
    }

    private void OnDisable()
    {
        Hide();
    }

    public abstract IEnumerator Show(bool isFirstShow);

    public abstract void Hide();
}