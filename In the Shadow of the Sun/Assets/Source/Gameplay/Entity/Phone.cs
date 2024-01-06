public class Phone : ClickableEntity
{
    private static Phone instance;
    public static Phone Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Phone>();
            }
            return instance;
        }
    }
    
    protected override void OnSelected()
    {
        GameManager.Instance.StateMachine.GoToState(new GameState_Staff());
    }
}