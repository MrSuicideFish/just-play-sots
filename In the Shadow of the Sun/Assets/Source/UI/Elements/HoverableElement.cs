using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HoverableElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public FadeController fadeController;
    public float fadeDuration = 1.0f;

    private void OnEnable()
    {
        fadeController.Opacity = 0.0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DOTween.To(
            () => fadeController.Opacity,
            x => fadeController.Opacity = x,
            1.0f, fadeDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DOTween.To(
            () => fadeController.Opacity,
            x => fadeController.Opacity = x,
            0.0f, fadeDuration);
    }
}
