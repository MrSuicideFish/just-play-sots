using System;
using DG.Tweening;
using UnityEngine;

public class ClickableEntity : MonoBehaviour
{
    public GameObject view;
    public Animation animComponent;
    public FadeController indicator;
    
    protected virtual void OnSelected(){}

    private void Start()
    {
        indicator.Opacity = 0;
    }

    private void OnMouseOver()
    {
        if (view.activeInHierarchy
            && GameManager.Instance.StateMachine
                .currentState.ScreenType == EScreenType.Home)
        {
            DOTween.To(
                () => indicator.Opacity,
                (x) => indicator.Opacity = x,
                1.0f, 1.0f);    
        }
    }

    private void OnMouseExit()
    {
        DOTween.To(
            () => indicator.Opacity,
            (x) => indicator.Opacity = x,
            0.0f, 1.0f);
    }

    private void OnMouseDown()
    {
        Debug.Log("Selected World Entity");
        if (view != null && view.activeInHierarchy 
                         && GameManager.Instance.playerController.enabled)
        {
            OnSelected();
        }
    }

    protected virtual void OnShow(bool firstShow){}
    public void Show(bool firstShow)
    {
        view.SetActive(true);
        if (firstShow && animComponent != null)
        {
            animComponent.Play();
        }
        
        OnShow(firstShow);
    }

    protected virtual void OnHide(){}
    public void Hide()
    {
        view.SetActive(false);
        OnHide();
    }
}