using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
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

    public Button button_continue;

    public Animation text_overtime;

    private Coroutine resultsRoutine;
    private ArticleOption option;
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.Result;
    }

    private void OnEnable()
    {
        if (resultsRoutine != null)
        {
            StopCoroutine(resultsRoutine);
            resultsRoutine = null;
        }
        
        if (GameManager.Instance.SelectedOption == null)
        {
            return;
        }

        option = GameManager.Instance.SelectedOption;
        resultsRoutine = StartCoroutine(DoResults());
    }

    private IEnumerator DoResults()
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

    public void Continue()
    {
        GameManager.Instance.Staff.Count -= option.staffCost;
        GameManager.Instance.ReturnToHome();
    }
}