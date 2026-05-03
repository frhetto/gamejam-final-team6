using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("O2 Bar")]
    [SerializeField] Slider o2Fill;
    [SerializeField] TextMeshProUGUI o2Text;

    [Header("Low O2 Warning")]
    [SerializeField] Image warningOverlay;
    [SerializeField] float warningThreshold = 0.2f;

    [Header("Interact Progress")]
    [SerializeField] GameObject interactBarRoot;
    [SerializeField] Image interactFill;

    [Header("Screens")]
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;

    [Header("Scenes")]
    [SerializeField] string mainMenuScene = "MainMenu";

    bool lowWarning;
    float pulse;

    public static bool IsGameOver { get; set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        IsGameOver = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        winPanel.SetActive(false);
        losePanel.SetActive(false);
        interactBarRoot.SetActive(false);
        if (warningOverlay) warningOverlay.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!lowWarning) return;
        pulse += Time.deltaTime * 3f;
        warningOverlay.color = new Color(1f, 0f, 0f, 0.1f + Mathf.Abs(Mathf.Sin(pulse)) * 0.2f);
    }

    // Called by O2System every frame with a 0-1 normalized value
    public void UpdateO2(float normalized)
    {
        o2Fill.value = normalized;
        if (o2Text) o2Text.text = Mathf.RoundToInt(normalized * 100f) + "%";

        bool warn = normalized < warningThreshold;
        if (warn == lowWarning) return;
        lowWarning = warn;
        warningOverlay.gameObject.SetActive(warn);
        pulse = 0f;
    }

    // Pass 0 to hide, 0-1 while holding E
    public void SetInteractProgress(float normalized)
    {
        bool show = normalized > 0f;
        interactBarRoot.SetActive(show);
        if (show) interactFill.fillAmount = normalized;
    }

    void EndGame()
    {
        IsGameOver = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowWin()  { EndGame(); winPanel.SetActive(true); }
    public void ShowLose() { EndGame(); losePanel.SetActive(true); }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}
