using System;
using System.Collections;

public class Screen_Tutorial : GameScreen
{
    public bool isComplete { get; private set; }
    
    public override EScreenType GetScreenType()
    {
        return EScreenType.Tutorial;
    }

    public override IEnumerator Show(bool isFirstShow)
    {
        isComplete = false;
        yield return null;
    }

    public override void Hide()
    {
        
    }

    public void Continue()
    {
        isComplete = true;
    }
}