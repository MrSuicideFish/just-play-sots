using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Screen_EndGame : GameScreen
{
    public TMP_Text text_headline;
    public TMP_Text text_subtext;
    
    public GameObject winScreen, loseScreen;
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.EndGame;
    }

    private void OnEnable()
    {
        winScreen.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(false);
    }

    public IEnumerator DoWin(int ending)
    {
        winScreen.gameObject.SetActive(true);
        yield break;
    }

    public IEnumerator DoLose()
    {
        loseScreen.gameObject.SetActive(true);
        yield break;
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