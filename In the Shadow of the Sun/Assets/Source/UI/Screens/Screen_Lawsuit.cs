using System.Collections;
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

    public Button button_payFunds, button_payInsurance;
    public TMP_Text text_payFunds;
    public FadeController payFundsFade;
    
    public UILawsuitEntry entryPrototype;

    public Transform entriesParent;
    public Animation paidStampAnim;
    private List<UILawsuitEntry> entries = new();

    public bool hasSettled;
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
        hasSettled = false;
        button_payFunds.gameObject.SetActive(true);
        button_payInsurance.gameObject.SetActive(true);
        paidStampAnim.gameObject.SetActive(false);
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

            if (GameManager.Instance.OrganizationFunds.Value < currentLawsuit.cost)
            {
                text_payFunds.text = "Not Enough Funds";
                text_payFunds.color = new Color(1, 0, 0, 1.0f);
                payFundsFade.Opacity = 0.3f;
                button_payFunds.interactable = false;
            }
            else
            {
                text_payFunds.text = "Pay With Funds";
                text_payFunds.color = new Color(1, 1, 1, 1.0f);
                payFundsFade.Opacity = 1.0f;
                button_payFunds.interactable = true;
            }
        }
        
        settlementScreen.SetActive(true);
        selectScreen.SetActive(false);
        screenState = ELawsuitScreenState.Settlement;
    }

    public void PayWithFunds()
    {
        StartCoroutine(DoSettlement(Lawsuit.ESettlementType.Funds));
    }

    public void PayWithInsurance()
    {
        StartCoroutine(DoSettlement(Lawsuit.ESettlementType.Insurance));
    }

    private IEnumerator DoSettlement(Lawsuit.ESettlementType settlementType)
    {
        button_payFunds.gameObject.SetActive(false);
        button_payInsurance.gameObject.SetActive(false);
        hasSettled = true;
        paidStampAnim.gameObject.SetActive(true);
        paidStampAnim.Play();
        yield return new WaitForSeconds(2.0f);
        GameManager.Instance.SettleLawsuit(settlementType);
        yield return null;
    }
}