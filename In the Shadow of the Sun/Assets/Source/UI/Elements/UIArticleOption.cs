using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIArticleOption : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Text content;

    public Image staffImage;
    public Image politicianImage;
    public Image organizationImage;
    public Image citizenImage;
    
    public UnityEvent OnBecameAvailable;
    public UnityEvent OnBecameUnavailable;

    public void SetPopularity(EParty party, int popularity)
    {
        Image targetImg = null;
        switch (party)
        {
            case EParty.Civilian:
                targetImg = citizenImage;
                break;
            case EParty.Companies:
                targetImg = organizationImage;
                break;
            case EParty.Politician:
                targetImg = politicianImage;
                break;
            default:
                return;
        }

        if (popularity == 0)
        {
            targetImg.color = GameConfig.Instance.neutralColor;
        }
        else
        {
            Color col = GameConfig.Instance.proponentColor;

            if (popularity < 0)
            {
                col = GameConfig.Instance.criticColor;
                float strength = (float) Mathf.Abs(popularity) / GameConfig.Instance.MaxPopularity;
                strength = Mathf.Clamp(strength, 0.0f, 0.7f);
            
                col.r *= 1 - strength;
                col.g *= 1 - strength;
                col.b *= 1 - strength;    
            }
            
            targetImg.color = col;
        }
    }
}