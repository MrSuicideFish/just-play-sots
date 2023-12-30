public class Phone : ClickableEntity
{
    protected override void OnSelected()
    {
        GameManager.Instance.StateMachine.GoToState(new GameState_Staff());
    }
}