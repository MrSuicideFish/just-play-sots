
using TMPro;
using UnityEngine.Events;

public class Screen_Staff : GameScreen
{
    public TMP_Text text_count;
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.Staff;
    }

    private void OnEnable()
    {
        text_count.text = GameManager.Instance.Staff.ToString();
    }

    public void HireEmployees()
    {
        GameManager.Instance.Staff.Hire();
        text_count.text = GameManager.Instance.Staff.ToString();
    }

    public void FireEmployees()
    {
        GameManager.Instance.Staff.Fire();
        text_count.text = GameManager.Instance.Staff.ToString();
    }

    public void Continue()
    {
        GameManager.Instance.ReturnToHome();
    }
}
