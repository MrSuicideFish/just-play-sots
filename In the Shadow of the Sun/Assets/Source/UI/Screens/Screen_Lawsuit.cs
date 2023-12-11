using TMPro;
using UnityEngine.UI;

public class Screen_Lawsuit : GameScreen
{
    public TMP_Text text_headline;
    public TMP_Text text_content;
    public TMP_Text text_cost;

    public Button button_payFunds, button_payInsurance;
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.Lawsuit;
    }

    private void OnEnable()
    {
        Lawsuit currentLawsuit = GameManager.Instance.CurrentLawsuit;
        if (currentLawsuit != null)
        {
            text_headline.text = currentLawsuit.header;
            text_content.text = currentLawsuit.content;
            text_cost.text = Funds.Format(currentLawsuit.cost);

            button_payFunds.interactable 
                = GameManager.Instance.OrganizationFunds.Value >= currentLawsuit.cost;
        }
    }

    public void PayWithFunds()
    {
        GameManager.Instance.SettleLawsuit(Lawsuit.ESettlementType.Funds);
    }

    public void PayWithInsurance()
    {
        GameManager.Instance.SettleLawsuit(Lawsuit.ESettlementType.Insurance);
    }

    public void DissolveNonProfit()
    {
        GameManager.Instance.EndGame();
    }
}