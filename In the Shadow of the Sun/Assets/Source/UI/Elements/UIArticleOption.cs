using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIArticleOption : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Text content;
    
    public UnityEvent OnBecameAvailable;
    public UnityEvent OnBecameUnavailable;

    public void ToggleAvailability(bool onOff)
    {
        toggle.interactable = onOff;
        if (onOff)
        {
            OnBecameAvailable.Invoke();
        }
        else
        {
            OnBecameUnavailable.Invoke();
        }

    }
}