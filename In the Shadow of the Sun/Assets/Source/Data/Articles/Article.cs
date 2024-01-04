using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "In the Shadow of the Sun/New Article")]
public class Article : ScriptableObject
{
    public string id;
    public string headline;
    [TextArea(1,30)]public string content;
    public ArticleOption[] options;

    [NonSerialized] public int selectedOption = -1;
}
