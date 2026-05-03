using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject settingsPanel;

    [Header("Settings Sliders")]
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [Header("Scene")]
    [SerializeField] string gameSceneName = "Level";

    void Start()
    {
        ShowMain();

        if (AudioManager.Instance == null) return;
        musicSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusicVolume());
        sfxSlider.SetValueWithoutNotify(AudioManager.Instance.GetSFXVolume());
    }

    public void ShowMain()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);

    }

    public void ShowSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }


    public void OnMusicSliderChanged(float value) => AudioManager.Instance?.SetMusicVolume(value);
    public void OnSFXSliderChanged(float value)   => AudioManager.Instance?.SetSFXVolume(value);

    public void PlayGame() => SceneManager.LoadScene(gameSceneName);

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
