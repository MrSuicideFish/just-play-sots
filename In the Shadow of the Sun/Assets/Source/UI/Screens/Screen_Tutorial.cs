using System;

public class Screen_Tutorial : GameScreen
{
    public bool isComplete { get; private set; }
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.Tutorial;
    }

    private void OnEnable()
    {
        isComplete = false;
    }

    public void Continue()
    {
        isComplete = true;
    }
}