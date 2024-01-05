using System.Collections;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Screen_Intro : GameScreen
{
    public Button submitButton;
    public TMP_InputField input_OrgName;
    public CinemachineVirtualCamera introCamera;
    public VideoPlayer introVideo;
    public AudioSource introAudio;
    public Animation radioEntryAnim;

    public GameObject namePanel;
    public Image fadeScreen;
    public Image background;
    public AudioSource radioAudioSrc;
    public CaptionsInfo captions;
    public GameObject captionsParent;
    public FadeController captionsPanel;
    public TMP_Text text_captions;

    public bool IsComplete { get; private set; }

    public override EScreenType GetScreenType()
    {
        return EScreenType.Intro;
    }

    public override IEnumerator Show(bool isFirstShow)
    {
        captionsParent.gameObject.SetActive(false);
        submitButton.interactable = false;
        input_OrgName.onValueChanged.RemoveListener(OnOrgNameEdit);
        input_OrgName.onValueChanged.AddListener(OnOrgNameEdit);
        namePanel.SetActive(true);
        introCamera.m_Lens.FarClipPlane = 5000;
        
        yield return null;
    }

    public override void Hide()
    {
        captionsParent.gameObject.SetActive(false);
    }

    private void OnOrgNameEdit(string orgName)
    {
        GameManager.Instance.OrganizationName = orgName;
        submitButton.interactable = !string.IsNullOrEmpty(orgName);
    }

    public void SubmitOrganizationName()
    {
        submitButton.interactable = false;
        fadeScreen.DOFade(1.0f, 1.0f).OnComplete(() =>
        {
#if UNITY_EDITOR
            if (!GameManager.Instance.debug_SkipCutscene)
            {
                StartCoroutine(DoIntroduction());
            }
            else
            {
                IsComplete = true;
            }
#else
            StartCoroutine(DoIntroduction());
#endif
        });
    }

    private IEnumerator DoIntroduction()
    {
        yield return new WaitForSeconds(1.5f);
        introCamera.m_Lens.FarClipPlane = 1;
        namePanel.gameObject.SetActive(false);
        fadeScreen.color = new Color(0, 0, 0, 0);
        introVideo.Play();
        introAudio.Play();
        yield return new WaitForSeconds(0.2f);
        background.enabled = false;
        yield return new WaitForSeconds(0.7f);

        captionsParent.gameObject.SetActive(true);
        double videoTime = introVideo.time;
        int captionIndex = -1;
        while (introVideo.isPlaying)
        {
            videoTime = introVideo.time;
            if (videoTime < captions.startTime)
            {
                text_captions.text = captions.preShowLabel;
                captionsPanel.Opacity
                    = Mathf.Lerp(captionsPanel.Opacity, 1,
                        GameConfig.Instance.captionsFadeSpeed * Time.deltaTime);
            }else if (videoTime > captions.endTime)
            {
                text_captions.text = "";
                captionsPanel.Opacity
                    = Mathf.Lerp(captionsPanel.Opacity, 0,
                        GameConfig.Instance.captionsFadeSpeed * Time.deltaTime);
            }
            else
            {
                if (captionIndex + 1 < captions.captions.Length
                    && videoTime >= captions.captions[captionIndex + 1].time)
                {
                    captionIndex++;
                }

                if (captionIndex > -1)
                {
                    text_captions.color = new Color(1, 1, 1, 1);
                    text_captions.text = captions.captions[captionIndex].text;
                }
                else
                {
                    text_captions.color = new Color(0, 0, 0, 0);
                    text_captions.text = "";
                }
            }

            yield return null;
        }

        Tween fade = fadeScreen.DOFade(1, 1);
        while (fade.active)
        {
            Debug.Log("Is fading");
            yield return null;
        }

        radioAudioSrc.Play();
        radioEntryAnim.Play();
        yield return new WaitForSeconds(2.0f);
        introVideo.enabled = false;
        introCamera.m_Lens.FarClipPlane = 5000;

        fadeScreen.DOFade(0, 7.0f);
        while (radioEntryAnim.isPlaying)
        {
            yield return null;
        }

        IsComplete = true;
    }
}