using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Screen_Home : GameScreen
{
    [FormerlySerializedAs("firstTimeIntro")] public FadeController orgNamePanel;
    [FormerlySerializedAs("orgNameIntro")] public TMP_Text text_orgName;

    [Header("Intro Article")] 
    public Animation introArticleParent;
    public TMP_Text text_introArticleHeadline;
    public TMP_Text text_introArticleContent;
    public Button button_introArticleContinue;
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.Home;
    }

    public override IEnumerator Show(bool isFirstShow)
    {
        
        yield return null;
    }

    public override void Hide()
    {
        
    }

    public void BeginIntroArticle(UnityAction onComplete)
    {
        button_introArticleContinue.onClick.AddListener(onComplete);
        text_introArticleHeadline.text = GameConfig.Instance
            .introArticleHeadline.Replace("{{OrgName}}",
                GameManager.Instance.OrganizationName);
        text_introArticleContent.text = GameConfig.Instance
            .introArticleContent.Replace("{{OrgName}}",
                GameManager.Instance.OrganizationName);
        
        introArticleParent.gameObject.SetActive(true);
        introArticleParent.Play();

    }

    public void EndIntroArticle()
    {
        introArticleParent.gameObject.SetActive(false);
    }
}