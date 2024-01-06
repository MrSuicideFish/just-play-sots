using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public AudioMixer targetMixer;
    public Slider volumeSlider;
    public GameObject view;

    private static SettingsController instance;
    public static SettingsController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SettingsController>();
            }

            return instance;
        }
    }

    public void Show()
    {
        view.SetActive(true);

        float currentVol;
        targetMixer.GetFloat("MasterVolume", out currentVol);
        volumeSlider.SetValueWithoutNotify(currentVol);
        
        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener(OnSliderValueChange);
    }

    public void OnSliderValueChange(float value)
    {
        targetMixer.SetFloat("MasterVolume", value);
        AudioManager.Instance.PlayEffect(ESoundEffect.PhoneDial3);
    }

    public void Hide()
    {
        view.SetActive(false);
    }

    public void Done()
    {
        Hide();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
