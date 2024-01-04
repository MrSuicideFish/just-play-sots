using TMPro;

public class Screen_Result : GameScreen
{
    public TMP_Text text_headline;
    public TMP_Text text_content;
    public TMP_Text text_donation_civ,
                    text_donation_pol,
                    text_donation_org;
    
    public TMP_Text text_charges;
    public TMP_Text text_insurance;
    public override EScreenType GetScreenType()
    {
        return EScreenType.Result;
    }

    private void OnEnable()
    {
        ArticleOption lastSelectedOption = GameManager.Instance.SelectedOption;
        if (lastSelectedOption == null)
        {
            return;
        }

        text_headline.text = lastSelectedOption.response.GetHeadline();
        text_content.text = lastSelectedOption.response.GetContent();
        
        // donations
        text_donation_civ.text = Funds.Format(lastSelectedOption.civilianEffect.donations);
        text_donation_pol.text = Funds.Format(lastSelectedOption.politicianEffect.donations);
        text_donation_org.text = Funds.Format(lastSelectedOption.companiesEffect.donations);
        
        // charges
        text_charges.text = Funds.Format(lastSelectedOption.cost);
        text_insurance.text = GameManager.Instance.Insurance.fee.ToString();
    }

    public void Continue()
    {
        GameManager.Instance.ReturnToHome();
    }
}