using System;
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

    private int selectedLawsuit;
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.Lawsuit;
    }

    private void Start()
    {
        entryPrototype.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GoToLawsuitSelect();
    }

    public void GoToLawsuitSelect()
    {
        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i] != null)
            {
                Destroy(entries[i].gameObject);   
            }
        }

        for (int i = 0; i < GameManager.Instance.lawsuits.Count; i++)
        {
            Lawsuit lawsuit = GameManager.Instance.lawsuits[i];
            UILawsuitEntry newEntry = Instantiate(entryPrototype.gameObject)
                .GetComponent<UILawsuitEntry>();

            newEntry.toggle.group = selectionToggleGroup;
            newEntry.text_title.text = lawsuit.GetHeader();
            newEntry.lawsuitId = lawsuit.id;
            
            newEntry.transform.SetParent(entriesParent, true);
            newEntry.transform.localScale = Vector3.one;

            int index = i;
            newEntry.toggle.onValueChanged.AddListener(isOn =>
            {
                selectedLawsuit = index;
            });
            newEntry.gameObject.SetActive(true);
            entries.Add(newEntry);
        }
        
        selectScreen.SetActive(true);
        settlementScreen.SetActive(false);
        screenState = ELawsuitScreenState.Select;
    }

    public void SelectLawsuit()
    {
        GameManager.Instance.SelectLawsuit(selectedLawsuit);
        
        Lawsuit currentLawsuit = GameManager.Instance.CurrentLawsuit;
        if (currentLawsuit != null)
        {
            text_headline.text = currentLawsuit.GetHeader();
            text_content.text = currentLawsuit.GetContent();
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
}