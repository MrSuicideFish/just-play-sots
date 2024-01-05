using System.Collections;
using TMPro;

public class Screen_Staff : GameScreen
{
    public TMP_Text text_count;
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.Staff;
    }

    public override IEnumerator Show(bool isFirstShow)
    {
        text_count.text = GameManager.Instance.Staff.ToString();
        yield return null;
    }

    public override void Hide()
    {
        
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
