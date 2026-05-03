using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Settings Sliders")]
    [SerializeField] private Slider volumeSlider;

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Level";

    private void Start()
    {
        ShowMain();
        SetupVolumeSlider();
    }

    private void SetupVolumeSlider()
    {
        if (AudioManager.Instance == null || volumeSlider == null)
            return;

        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.wholeNumbers = false;

        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.SetValueWithoutNotify(AudioManager.Instance.GetVolume());
        volumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetVolume);
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

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}