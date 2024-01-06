using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Screen_Staff : GameScreen
{
    public TMP_Text text_count;

    public Button button_hire, button_fire;

    private int startedWith;
    public override EScreenType GetScreenType()
    {
        return EScreenType.Staff;
    }

    public override IEnumerator Show(bool isFirstShow)
    {
        GameManager.Instance.hasHiredFirstStaff = true;
        text_count.text = GameManager.Instance.Staff.ToString();
        button_fire.interactable = !isFirstShow;
        yield return null;
    }

    public override void Hide()
    {
        
    }

    public void HireEmployees()
    {
        GameManager.Instance.Staff.Hire();
        AudioManager.Instance.PlayEffect(ESoundEffect.StaffHire);
        text_count.text = GameManager.Instance.Staff.ToString();
        button_fire.interactable = true;
    }


    public void FireEmployees()
    {
        AudioManager.Instance.PlayEffect(ESoundEffect.StaffFire);
        GameManager.Instance.Staff.Fire();
        text_count.text = GameManager.Instance.Staff.ToString();
        button_fire.interactable = GameManager.Instance.Staff.Total >= 0;
    }

    public void Continue()
    {
        GameManager.Instance.ReturnToHome();
    }
}
