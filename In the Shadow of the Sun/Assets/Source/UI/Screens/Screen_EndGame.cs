using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EGameFailType
{
    Funds,
    Popularity
}

public class Screen_EndGame : GameScreen
{
    public TMP_Text text_headline;
    public TMP_Text text_subtext;
    
    public Animation winScreen, loseScreen;
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.EndGame;
    }

    public override IEnumerator Show(bool isFirstShow)
    {
        winScreen.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(false);
        yield return null;
    }

    public override void Hide()
    {
        
    }

    public IEnumerator DoWin(int ending)
    {
        winScreen.gameObject.SetActive(true);
        winScreen.Play();
        yield break;
    }

    public IEnumerator DoLose(bool failedByPop = false)
    {
        // fail headline
        text_headline.text = GameConfig.Instance
            .GameFailHeadline.Replace("{{OrgName}}",
                GameManager.Instance.OrganizationName);
        
        // fail reason
        text_subtext.text = failedByPop
            ? GameConfig.Instance.GameFailReasonPopularity.Replace("{{OrgName}}",
                GameManager.Instance.OrganizationName)
            : GameConfig.Instance.GameFailReasonFunds.Replace("{{OrgName}}",
                GameManager.Instance.OrganizationName);
        
        loseScreen.gameObject.SetActive(true);
        loseScreen.Play();
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