using UnityEngine;

public class LawsuitNotice : ClickableEntity
{
    protected override void OnSelected()
    {
        GameManager.Instance.StateMachine.GoToState(new GameState_Lawsuit());
    }
}