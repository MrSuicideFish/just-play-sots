using TMPro;
using UnityEngine;

public class DebuggingUI : MonoBehaviour
{
    public TMP_Text text_np_name;
    public TMP_Text text_funds;
    
    public TMP_Text text_pop_civ,
                    text_pop_pol,
                    text_pop_org;

    public TMP_Text text_insurance_contribute,
                    text_insurance_payout,
                    text_insurance_fees;

    public TMP_Text text_gamestate;
    private void Update()
    {
        text_np_name.text = GameManager.Instance.OrganizationName;
        text_funds.text = GameManager.Instance.OrganizationFunds.ToString();
        text_pop_civ.text = $"Civilians: {GameManager.Instance.Popularity.Civilian}";
        text_pop_pol.text = $"Politicians: {GameManager.Instance.Popularity.Politician}";
        text_pop_org.text = $"Organizations: {GameManager.Instance.Popularity.Companies}";

        text_insurance_contribute.text = GameManager.Instance.Insurance.totalContributions.ToString();
        text_insurance_payout.text = GameManager.Instance.Insurance.totalPayout.ToString();
        text_insurance_fees.text = GameManager.Instance.Insurance.fee.ToString();

        text_gamestate.text = GameManager.Instance.StateMachine.currentState?.StateName;
    }
}
