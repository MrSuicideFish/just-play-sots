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
        GameState_Results.isFirstResults = true;
        GameState_Home.firstTimeUser = true;
        GameState_Home.hasCompletedFirstArticle = false;
        
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    
    public void EndGame()
    {
        Application.Quit(0);
    }

}