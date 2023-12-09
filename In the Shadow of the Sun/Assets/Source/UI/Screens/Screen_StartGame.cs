using TMPro;
using UnityEngine;

public class Screen_StartGame : GameScreen
{
    public TMP_InputField input_OrgName;
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.StartGame;
    }

    private void OnEnable()
    {
        input_OrgName.onValueChanged.RemoveListener(OnOrgNameEdit);
        input_OrgName.onValueChanged.AddListener(OnOrgNameEdit);
        input_OrgName.onEndEdit.AddListener((value)=>{SubmitOrganizationName();});
    }

    private void OnOrgNameEdit(string orgName)
    {
        GameManager.Instance.OrganizationName = orgName;
    }

    public void SubmitOrganizationName()
    {
        GameManager.Instance.StartGame();
    }
}