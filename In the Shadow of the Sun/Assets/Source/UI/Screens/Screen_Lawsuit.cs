using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Lawsuit : GameScreen
{
    public enum ELawsuitScreenState
    {
        Select,
        Settlement
    }

    public ELawsuitScreenState screenState;
    public GameObject selectScreen, settlementScreen;
    public TMP_Text text_headline;
    public TMP_Text text_content;
    public TMP_Text text_cost;
    public ToggleGroup selectionToggleGroup;

    public Button button_payFunds;
    public UILawsuitEntry entryPrototype;

    public Transform entriesParent;
    private List<UILawsuitEntry> entries = new();
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.Lawsuit;
    }

    private void OnEnable()
    {
        GoToLawsuitSelect();
    }

    public void GoToLawsuitSelect()
    {
        for (int i = 0; i > entries.Count; i++)
        {
            Destroy(entries[i].gameObject);
        }

        for (int i = 0; i < GameManager.Instance.lawsuits.Count; i++)
        {
            Lawsuit lawsuit = GameManager.Instance.lawsuits[i];
            UILawsuitEntry newEntry = Instantiate(entryPrototype.gameObject)
                .GetComponent<UILawsuitEntry>();
            
            newEntry.text_title.text = lawsuit.header;
            newEntry.lawsuitId = lawsuit.id;
            
            newEntry.transform.SetParent(entriesParent);
        }
        
        selectScreen.SetActive(true);
        settlementScreen.SetActive(false);
        screenState = ELawsuitScreenState.Select;
    }

    public void SelectLawsuit(int index)
    {
        GameManager.Instance.SelectLawsuit(index);
        
        Lawsuit currentLawsuit = GameManager.Instance.CurrentLawsuit;
        if (currentLawsuit != null)
        {
            text_headline.text = currentLawsuit.header;
            text_content.text = currentLawsuit.content;
            text_cost.text = Funds.Format(currentLawsuit.cost);

            button_payFunds.interactable 
                = GameManager.Instance.OrganizationFunds.Value >= currentLawsuit.cost;
        }
        
        settlementScreen.SetActive(true);
        selectScreen.SetActive(false);
        screenState = ELawsuitScreenState.Settlement;
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