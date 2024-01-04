using UnityEngine;

public class ClickableEntity : MonoBehaviour
{
    public GameObject view;
    
    protected virtual void OnSelected(){}
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
        OnShow(firstShow);
    }

    protected virtual void OnHide(){}
    public void Hide()
    {
        view.SetActive(false);
        OnHide();
    }
}