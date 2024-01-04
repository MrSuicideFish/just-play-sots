using UnityEngine;
using UnityEngine.SceneManagement;

public class Screen_EndGame : GameScreen
{
    public override EScreenType GetScreenType()
    {
        return EScreenType.EndGame;
    }
    
    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    
    public void EndGame()
    {
        Application.Quit(0);
    }

}