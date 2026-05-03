using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject keypadPanel;
    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] TextMeshProUGUI feedbackText;

    [Header("Settings")]
    [SerializeField] string correctCode = "1000";
    [SerializeField] int maxDigits = 4;

    [Header("On Correct")]
    [SerializeField] GameObject doorToOpen;

    string currentInput = "";
    bool solved = false;

    public static KeypadManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        keypadPanel.SetActive(false);
        feedbackText.text = "";
        displayText.text = "";
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

        if (doorToOpen != null)
            doorToOpen.SetActive(false);

        Invoke(nameof(CloseKeypad), 1.5f);
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
