using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Screen_Home : GameScreen
{
    public Text text_funds, text_insurance, text_staff, text_popularity;

    [Header("Staff")] 
    public TMP_Text text_costPerEmployee;
    public TMP_Text text_overtimePay;
    
    [Header("Insurance")]
    public TMP_Text text_payout;
    public TMP_Text text_contributions;

    [Header("Popularity")] 
    public TMP_Text text_civilians;
    public TMP_Text text_politicians;
    public TMP_Text text_organizations;

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

    private void Update()
    {
        GameManager gm = GameManager.Instance;
        if (gm != null)
        {
            text_funds.text = $"FUNDS: {gm.OrganizationFunds.ToString()}";
            text_insurance.text = $"INSURANCE: {gm.Insurance.fee.ToString()}";
            text_staff.text = $"STAFF: {gm.Staff.Count} / {gm.Staff.Total}";

            float totalPop = gm.Popularity.Companies + gm.Popularity.Politician + gm.Popularity.Civilian;
            text_popularity.text = $"POPULARITY: {totalPop} / 300";
            
            // staff hover
            text_costPerEmployee.text = Funds.Format(GameConfig.Instance.costPerEmployee);
            text_overtimePay.text = $"{GameConfig.Instance.overtimeMultiplier}x";

            // insurance hover
            text_contributions.text = gm.Insurance.totalContributions.ToString();
            text_payout.text = gm.Insurance.totalPayout.ToString();
            
            // popularity hover
            text_civilians.text = gm.Popularity.Civilian.ToString();
            text_politicians.text = gm.Popularity.Politician.ToString();
            text_organizations.text = gm.Popularity.Companies.ToString();
        }
    }
}