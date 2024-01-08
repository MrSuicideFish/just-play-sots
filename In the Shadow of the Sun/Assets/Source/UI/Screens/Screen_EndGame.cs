using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Screen_EndGame : GameScreen
{
    public TMP_Text text_headline;
    public TMP_Text text_subtext;

    public TMP_Text text_winHeadline, text_winSubtext, text_winContent;
    public Animation winScreen, loseScreen;
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.EndGame;
    }

    public override IEnumerator Show(bool isFirstShow)
    {
        winScreen.gameObject.SetActive(false);
        loseScreen.gameObject.SetActive(false);
        GameUIController.Instance.HideStats();
        yield return null;
    }

    public override void Hide()
    {
        
    }

    public IEnumerator DoWin(bool altEnding)
    {
        if (!altEnding)
        {
            text_winHeadline.text = GameConfig.Instance.GameWinHeadline;
            text_winSubtext.text = GameConfig.Instance.GameWinSubtext;
            text_winContent.text = GameConfig.Instance.GameWinContent;
        }
        else
        {
            text_winHeadline.text = GameConfig.Instance.GameWinAltHeadline;
            text_winSubtext.text = GameConfig.Instance.GameWinAltSubtext;
            text_winContent.text = GameConfig.Instance.GameWinAltContent;
        }
        
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