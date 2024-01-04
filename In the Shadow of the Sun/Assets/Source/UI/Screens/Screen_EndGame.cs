using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Screen_EndGame : GameScreen
{
    public TMP_Text text_headline;
    public TMP_Text text_subtext;
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.EndGame;
    }
    
    public void Restart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    
    public void Quit()
    {
        Application.Quit(0);
    }

}