using UnityEngine;

public class ClickableEntity : MonoBehaviour
{
    protected virtual void OnSelected(){}
    private void OnMouseDown()
    {
        Debug.Log("Selected World Entity");
        if (GameManager.Instance.playerController.enabled)
        {
            OnSelected();
        }
    }
}