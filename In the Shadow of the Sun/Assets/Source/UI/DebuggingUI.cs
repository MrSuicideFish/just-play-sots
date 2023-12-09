using TMPro;
using UnityEngine;

public class DebuggingUI : MonoBehaviour
{
    public TMP_Text text_np_name;
    public TMP_Text text_funds;
    public TMP_Text text_pop_civ,
                    text_pop_pol,
                    text_pop_org;

    private void Update()
    {
        text_np_name.text = GameManager.Instance.OrganizationName;
        text_funds.text = GameManager.Instance.OrganizationFunds.ToString();
        text_pop_civ.text = $"Civilians: {GameManager.Instance.Popularity.Civilian}";
        text_pop_pol.text = $"Politicians: {GameManager.Instance.Popularity.Politician}";
        text_pop_org.text = $"Organizations: {GameManager.Instance.Popularity.Companies}";
    }
}
