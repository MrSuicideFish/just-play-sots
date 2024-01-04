using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class FadeController : MonoBehaviour
{
    private List<Graphic> cache = new();
    public float Opacity = 1.0f;

    private void OnEnable()
    {
        PopulateCache(gameObject);
    }

    public void Set(float value)
    {
        PopulateCache(gameObject);
        for (int i = 0; i < cache.Count; i++)
        {
            Color currentColor = cache[i].color;
            currentColor.a = value;
            cache[i].color = currentColor;
        }
    }

    private void PopulateCache(GameObject parent)
    {
        Graphic component = parent.GetComponent<Graphic>();
        if (component != null)
        {
            cache.Add(component);
        }
        
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            PopulateCache(parent.transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        for (int i = 0; i < cache.Count; i++)
        {
            if (cache[i])
            {
                Color currentColor = cache[i].color;
                currentColor.a = Opacity;
                cache[i].color = currentColor;
            }
        }
    }
}