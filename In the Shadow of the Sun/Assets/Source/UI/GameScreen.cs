using UnityEngine;
using UnityEngine.Events;

public enum EScreenType
{
    StartGame,
    Article,
    Result,
    Lawsuit,
    EndGame
}

public abstract class GameScreen : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnScreenShow;

    public abstract EScreenType GetScreenType();
}