using UnityEngine;

public class LawsuitNotice : ClickableEntity
{
    private static LawsuitNotice instance;
    public static LawsuitNotice Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LawsuitNotice>();
            }
            return instance;
        }
    }
    
    protected override void OnSelected()
    {
        GameManager.Instance.StateMachine.GoToState(new GameState_Lawsuit());
    }
}