using System;
using System.Collections;


public class Screen_Tutorial : GameScreen
{
    public override EScreenType GetScreenType()
    {
        return EScreenType.Article;
    }

    public override IEnumerator Show(bool isFirstShow)
    {
        yield return null;
    }

    public override void Hide()
    {
        
    }

    public void Continue()
    {
    }
}