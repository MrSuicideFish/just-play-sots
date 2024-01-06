using UnityEngine;
using UnityEngine.Events;

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

    public GameMessageUI gameMessage;

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

    public void ShowGameMessage(
        string title,
        string content,
        UnityAction onCompleteAction)
    {
        gameMessage.gameObject.SetActive(true);
        gameMessage.text_title.text = title;
        gameMessage.text_content.text = content;
        gameMessage.button_continue.onClick.RemoveAllListeners();

        if (onCompleteAction != null)
        {
            gameMessage.button_continue.onClick.AddListener(onCompleteAction);
        }

        gameMessage.button_continue.onClick.AddListener(() =>
        {
            gameMessage.gameObject.SetActive(false);
        });
    }
}
