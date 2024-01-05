using System.Collections;
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
    Empty
}

public interface IGameScreen
{
    
}
public abstract class GameScreen : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnScreenShow, OnScreenHide;

    public abstract EScreenType GetScreenType();

    private bool HasShown = false;
    
    public void ShowFTUE(UnityAction onComplete)
    {
        GameManager.Instance
            .StateMachine.currentState.ShowFTUE(onComplete);
    }

    private void OnEnable()
    {
        StartCoroutine(Show(!HasShown));
        OnScreenShow?.Invoke();
        HasShown = true;
    }

    private void OnDisable()
    {
        OnScreenHide?.Invoke();
        Hide();
    }

    public abstract IEnumerator Show(bool isFirstShow);

    public abstract void Hide();
}