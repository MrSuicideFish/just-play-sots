using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIArticleOption : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Text content;

    public Image staffImage;
    
    public UnityEvent OnBecameAvailable;
    public UnityEvent OnBecameUnavailable;
}