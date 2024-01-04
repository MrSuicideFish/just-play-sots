using System;
using UnityEngine;

public class Newspaper : ClickableEntity
{
    private static Newspaper instance;
    public static Newspaper Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Newspaper>();
            }
            return instance;
        }
    }

    public Animation animComponent;

    protected override void OnShow(bool firstShow)
    {
        if (firstShow)
        {
            animComponent.Play();   
        }
    }

    protected override void OnHide()
    {
        
    }

    protected override void OnSelected()
    {
        GameManager.Instance.StateMachine.GoToState(new GameState_Newspaper());
    }
}