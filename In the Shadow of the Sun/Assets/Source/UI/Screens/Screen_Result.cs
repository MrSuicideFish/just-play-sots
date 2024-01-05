using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Result : GameScreen
{
    public TMP_Text text_headline;
    public TMP_Text text_content;
    public TMP_Text text_donation_civ,
                    text_donation_pol,
                    text_donation_org;
    
    public TMP_Text text_charges;
    public TMP_Text text_payroll;
    public TMP_Text text_insurance;
    public TMP_Text text_staff_allocated,
                    text_staff_total;

    public Animation funds_result_anim;
    public TMP_Text text_funds_result;
    public Button button_funds_result_back;
    public Button button_funds_result_continue;
    
    public Button button_continue;

    public Animation text_overtime;

    private Coroutine resultsRoutine;
    private bool hasShownFundsResults = false;
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.Result;
    }

    public override IEnumerator Show(bool isFirstShow)
    {
        funds_result_anim.gameObject.SetActive(false);
        hasShownFundsResults = false;
        yield break;
    }

    public override void Hide(){}

    public void CountResults(ArticleOption option)
    {
        if (resultsRoutine != null)
        {
            StopCoroutine(resultsRoutine);
            resultsRoutine = null;
        }
        
        resultsRoutine = StartCoroutine(DoResults(option));
    }

    private IEnumerator DoResults(ArticleOption option)
    {
        text_overtime.gameObject.SetActive(false);
        button_continue.interactable = false;
        
        text_headline.text = option.response.GetHeadline();
        text_content.text = option.response.GetContent();

        text_donation_civ.text = Funds.Format(0);
        text_donation_pol.text = Funds.Format(0);
        text_donation_org.text = Funds.Format(0);

        text_staff_allocated.text = "0";
        text_staff_total.text = "0";
        
        text_charges.text = Funds.Format(0);
        text_payroll.text = Funds.Format(0);
        text_insurance.text = Funds.Format(0);
        
        // donations
        float   tmpDonationCiv = 0,
                tmpDonationOrg = 0,
                tmpDonationPol = 0;

        DOTween.To(() => tmpDonationCiv,
                x => { text_donation_civ.text = Funds.Format(x); },
                option.civilianEffect.donations, GameConfig.Instance.donationDuration)
            .SetDelay(GameConfig.Instance.civillianDonationDelay);

        DOTween.To(() => tmpDonationPol,
            x => { text_donation_pol.text = Funds.Format(x); },
            option.politicianEffect.donations, GameConfig.Instance.donationDuration)
            .SetDelay(GameConfig.Instance.politicianDonationDelay);
        
        DOTween.To(() => tmpDonationOrg,
            x => { text_donation_org.text = Funds.Format(x); },
            option.companiesEffect.donations, GameConfig.Instance.donationDuration)
            .SetDelay(GameConfig.Instance.organizationDonationsDelay);
                        
        // staff
        int tmpStaffAlloc = 0,
            tmpStaffTotal = 0;

        DOTween.To(() => tmpStaffAlloc,
                x => { text_staff_allocated.text = x.ToString(); },
                option.staffCost, GameConfig.Instance.staffDuration)
            .SetDelay(GameConfig.Instance.staffAllocatedDelay);

        DOTween.To(() => tmpStaffTotal,
                x => { text_staff_total.text = x.ToString(); },
                GameManager.Instance.Staff.Count, GameConfig.Instance.staffDuration)
            .SetDelay(GameConfig.Instance.staffTotalDelay);
        
        // bill
        float tmpBill = 0;
        DOTween.To(() => tmpBill,
                x => { text_charges.text = Funds.Format(x); },
                option.civilianEffect.donations, 
                GameConfig.Instance.billDuration)
            .SetDelay(GameConfig.Instance.billDelay);
        
        // payroll
        float tmpPayroll = 0;
        float payroll = GameManager.Instance.Staff.Total * GameConfig.Instance.costPerEmployee;
        float remainingStaff = GameManager.Instance.Staff.Count - option.staffCost;
        bool staffHasOvertime = remainingStaff < 0;
        if (staffHasOvertime)
        {
            payroll += (GameManager.Instance.Staff.Total * Mathf.Abs(remainingStaff)) *
                       (GameConfig.Instance.costPerEmployee * GameConfig.Instance.overtimeMultiplier);
        }

        DOTween.To(() => tmpPayroll,
                x => { text_payroll.text = Funds.Format(x); },
                payroll, GameConfig.Instance.payrollDuration)
            .SetDelay(GameConfig.Instance.payrollDelay)
            .OnPlay(() =>
            {
                if (staffHasOvertime)
                {
                    text_overtime.gameObject.SetActive(true);
                    text_overtime.Play();
                }
            });
        
        // insurance
        DOTween.To(() => tmpStaffAlloc,
                x => { text_insurance.text = Funds.Format(x); },
                GameManager.Instance.Insurance.fee.Value, GameConfig.Instance.insuranceDuration)
            .SetDelay(GameConfig.Instance.insuranceDelay)
            .OnPlay(() =>
            {
                button_continue.interactable = true;
            });


        yield return null;
    }

    public void Back()
    {
        funds_result_anim.gameObject.SetActive(false);
    }

    public void Continue()
    {
        button_funds_result_continue.gameObject.SetActive(false);
        button_funds_result_back.gameObject.SetActive(false);
        
        button_funds_result_continue.interactable = false;
        button_funds_result_back.interactable = false;
        if (!funds_result_anim.gameObject.activeInHierarchy)
        {
            if (!hasShownFundsResults)
            {
                StartCoroutine(DoFunds());
            }
            else
            {
                button_funds_result_continue.gameObject.SetActive(true);
                button_funds_result_back.gameObject.SetActive(true);
                button_funds_result_continue.interactable = true;
                button_funds_result_back.interactable = true;
                funds_result_anim.gameObject.SetActive(true);
                text_funds_result.text = GameManager.Instance.OrganizationFunds.ToString();
            }
        }
        else
        {
            GameManager.Instance.ReturnToHome();
        }
    }

    private IEnumerator DoFunds()
    {
        button_funds_result_continue.gameObject.SetActive(true);
        button_funds_result_back.gameObject.SetActive(true);
        
        hasShownFundsResults = true;
        float tmp = GameManager.Instance.OrganizationFunds.Value;
        float cost = GameManager.Instance.CalcTotalCost();
        float donation = GameManager.Instance.CalcTotalDonations();
        text_funds_result.text = GameManager.Instance.OrganizationFunds.ToString();
        GameManager.Instance.OrganizationFunds.Value -= cost;
        GameManager.Instance.OrganizationFunds.Value += donation;
        
        funds_result_anim.gameObject.SetActive(true);
        funds_result_anim.Play();
        DOTween.To(() => tmp,
                x =>
                {
                    tmp = x;
                    text_funds_result.text = Funds.Format(x);
                },
                (tmp - cost) + donation, GameConfig.Instance.fundsResultDuration)
            .SetDelay(GameConfig.Instance.fundsResultDelay)
            .OnComplete(() =>
            {
                if (tmp <= 0.0f)
                {
                    text_funds_result.color = Color.red;
                    GameManager.Instance.EndGame(false);
                    DOTween.PauseAll();
                }
                else
                {
                    button_funds_result_continue.interactable = true;
                    button_funds_result_back.interactable = true;
                }
            });
        
        yield return null;
    }
}