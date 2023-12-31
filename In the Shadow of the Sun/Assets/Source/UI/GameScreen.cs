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
}