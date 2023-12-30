using UnityEngine;

public class GameUIController : MonoBehaviour
{
    private static GameUIController _instance;
    public static GameUIController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameUIController>();
            }
            return _instance;
        }
    }

    [SerializeField] private GameScreen[] screens;
    public void GoToScreen(EScreenType screen)
    {
        for (int i = 0; i < screens.Length; i++)
        {
            screens[i].gameObject.SetActive(screens[i].GetScreenType() == screen);
        }
    }

    public GameScreen GetScreen(EScreenType screen)
    {
        GameScreen result = null;
        for (int i = 0; i < screens.Length; i++)
        {
            if (screens[i].GetScreenType() == screen)
            {
                result = screens[i];
            }
        }

        return result;
    }
}
