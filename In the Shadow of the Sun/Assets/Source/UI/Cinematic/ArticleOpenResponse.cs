using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticleOpenResponse : MonoBehaviour
{
    public ESoundEffect paperOpenEffect = ESoundEffect.PaperOpen1;
    public void Play()
    {
        AudioManager.Instance.PlayEffect(paperOpenEffect);
    }
}
