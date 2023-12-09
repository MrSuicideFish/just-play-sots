using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Screen_Article : GameScreen
{
    public TMP_Text text_headline;
    public TMP_Text text_content;
    public ToggleGroup optionsToggleGroup;
    
    public UIArticleOption placeholderOption;
    private UIArticleOption[] options;
    private int selectedOption;
    public override EScreenType GetScreenType()
    {
        return EScreenType.Article;
    }

    private void OnEnable()
    {
        placeholderOption.gameObject.SetActive(false);
        Article article = GameManager.Instance.CurrentArticle;
        text_headline.text = article.headline;
        text_content.text = article.content;
        SetupOptions(article.options);
    }

    private void ClearOptions()
    {
        if (options == null)
        {
            return;
        }

        for (int i = 0; i < options.Length; i++)
        {
            Destroy(options[i].gameObject);
        }

        options = Array.Empty<UIArticleOption>();
    }

    private void SetupOptions(ArticleOption[] newOptions)
    {
        ClearOptions();
        options = new UIArticleOption[newOptions.Length];
        for (int i = 0; i < newOptions.Length; i++)
        {
            options[i] = Instantiate(placeholderOption);
            options[i].transform.SetParent(optionsToggleGroup.transform);
            options[i].transform.localScale = Vector3.one;
            options[i].gameObject.SetActive(true);
            options[i].content.text = newOptions[i].content;
            options[i].ToggleAvailability(
                newOptions[i].requirement.Party == EParty.None || GameManager.Instance.Popularity.CompareRequirement(
                    newOptions[i].requirement));
            var index = i;
            options[i].toggle.onValueChanged.AddListener(isOn =>
            {
                selectedOption = index;
            });
            options[i].toggle.group = optionsToggleGroup;
        }
    }

    public void SubmitOption()
    {
        GameManager.Instance.SelectArticleOption(selectedOption);
    }
}