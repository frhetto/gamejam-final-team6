using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject keypadPanel;
    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] TextMeshProUGUI feedbackText;
    [SerializeField] GameObject pressEText;

    [Header("Settings")]
    [SerializeField] string correctCode = "1000";
    [SerializeField] int maxDigits = 4;
    [SerializeField] float interactRadius = 3f;
    [SerializeField] string playerTag = "Player";

    [Header("On Correct")]
    [SerializeField] GameObject winPanel;

    string currentInput = "";
    bool solved = false;
    Transform _player;

    void Start()
    {
        if (keypadPanel != null && !IsAncestorOrSelf(keypadPanel.transform))
            keypadPanel.SetActive(false);

        if (feedbackText != null) feedbackText.text = "";
        if (displayText != null) displayText.text = "";
        if (pressEText != null) pressEText.SetActive(false);
        else Debug.LogWarning("KeypadManager: Press E Text is not assigned!", this);

        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null) _player = p.transform;
        else Debug.LogWarning($"KeypadManager: No GameObject with tag '{playerTag}' found.", this);
    }

    void Update()
    {
        if (HUDManager.IsGameOver) return;

        if (_player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag(playerTag);
            if (p != null) _player = p.transform;
            else return;
        }

        bool panelOpen = keypadPanel != null && keypadPanel.activeSelf;
        bool inRange = Vector3.Distance(transform.position, _player.position) <= interactRadius;

        if (pressEText != null) pressEText.SetActive(inRange && !solved && !panelOpen);

        if (panelOpen && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)))
        {
            CloseKeypad();
            return;
        }

        if (inRange && !solved && !panelOpen && Input.GetKeyDown(KeyCode.E))
            OpenKeypad();
    }

    bool IsAncestorOrSelf(Transform t)
    {
        Transform check = transform;
        while (check != null)
        {
            if (check == t) return true;
            check = check.parent;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }

    public void OpenKeypad()
    {
        if (solved) return;
        keypadPanel.SetActive(true);
        currentInput = "";
        UpdateDisplay();
        feedbackText.text = "";
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseKeypad()
    {
        keypadPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Hook each number button's OnClick to this, passing the digit as a string
    public void PressDigit(string digit)
    {
        if (currentInput.Length >= maxDigits) return;
        currentInput += digit;
        UpdateDisplay();
    }

    public void PressDelete()
    {
        if (currentInput.Length == 0) return;
        currentInput = currentInput.Substring(0, currentInput.Length - 1);
        UpdateDisplay();
    }

    public void PressEnter()
    {
        if (currentInput == correctCode)
            Correct();
        else
            Wrong();
    }

    void Correct()
    {
        solved = true;
        feedbackText.text = "ACCESS GRANTED";
        feedbackText.color = Color.green;

        Invoke(nameof(ShowWin), 1.5f);
    }

    void ShowWin()
    {
        keypadPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        if (winPanel != null)
        {
            HUDManager.IsGameOver = true; // ensure flag is set even when using local winPanel
            winPanel.SetActive(true);
        }
        else
            HUDManager.Instance?.ShowWin();
    }

    void Wrong()
    {
        feedbackText.text = "WRONG CODE";
        feedbackText.color = Color.red;
        currentInput = "";
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        displayText.text = currentInput.PadRight(maxDigits, '_');
    }
}
