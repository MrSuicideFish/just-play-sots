using UnityEngine;
using UnityEngine.Timeline;

public class LawsuitNotice : ClickableEntity
{
    private static LawsuitNotice instance;
    public static LawsuitNotice Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LawsuitNotice>();
            }
            return instance;
        }
    }

    public GameObject finalNotice;

    protected override void OnShow(bool firstShow)
    {
        base.OnShow(firstShow);

        bool showFinalNotice = false;
        for (int i = 0; i < GameManager.Instance.lawsuits.Count; i++)
        {
            if (GameManager.Instance.lawsuits[i].isMature)
            {
                showFinalNotice = true;
                break;
            }
        }

        finalNotice.gameObject.SetActive(showFinalNotice);
    }

    protected override void OnHide()
    {
        base.OnHide();
        finalNotice.gameObject.SetActive(false);
    }

    protected override void OnSelected()
    {
        GameManager.Instance.StateMachine.GoToState(new GameState_Lawsuit());
    }
}