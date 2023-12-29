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
    
    public void Show()
    {
        view.SetActive(true);
    }

    public void Hide()
    {
        view.SetActive(false);
    }
}