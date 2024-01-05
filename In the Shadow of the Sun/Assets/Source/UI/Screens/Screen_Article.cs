using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Button = UnityEngine.UI.Button;

public class Screen_Article : GameScreen
{
    public Button button_select;
    public TMP_Text text_headline;
    public TMP_Text text_content;
    public ToggleGroup optionsToggleGroup;
    
    public UIArticleOption placeholderOption;
    public ScrollRect scrollView;
    
    private UIArticleOption[] options;
    private int selectedOption;
    public override EScreenType GetScreenType()
    {
        return EScreenType.Article;
    }

    public override IEnumerator Show(bool isFirstShow)
    {
        scrollView.normalizedPosition = new Vector2(0, 1);
        scrollView.Rebuild(CanvasUpdate.Prelayout);
        placeholderOption.gameObject.SetActive(false);
        Article article = GameManager.Instance.CurrentArticle;
        text_headline.text = article.GetHeadline();
        text_content.text = article.GetContent();
        SetupOptions(article.options);
        button_select.interactable = false;

        yield return null;
    }

    public override void Hide()
    {
        
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
            options[i].content.text = newOptions[i].GetContent();
            var index = i;
            options[i].toggle.onValueChanged.AddListener(isOn =>
            {
                selectedOption = index;
                button_select.interactable = optionsToggleGroup.AnyTogglesOn();
            });
            options[i].toggle.group = optionsToggleGroup;
        }
    }

    public void SubmitOption()
    {
        GameManager.Instance.SelectArticleOption(selectedOption);
    }
}