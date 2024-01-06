using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampAnimResponse : MonoBehaviour
{
    public void Play()
    {
        AudioManager.Instance.PlayEffect(ESoundEffect.PaperSlap);
    }
}
