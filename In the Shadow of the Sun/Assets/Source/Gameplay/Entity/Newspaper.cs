using UnityEngine;

public class Newspaper : ClickableEntity
{
    protected override void OnSelected()
    {
        GameManager.Instance.StateMachine.GoToState(new GameState_Newspaper());
    }
}