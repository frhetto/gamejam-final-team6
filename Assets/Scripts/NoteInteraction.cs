using UnityEngine;

public class NoteInteraction : MonoBehaviour
{
    public GameObject notePanel;
    public GameObject pressEText;
    public float interactRadius = 3f;
    public string playerTag = "Player";

    private bool _isOpen = false;
    private Transform _player;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null) _player = p.transform;

        if (notePanel != null)
            notePanel.SetActive(false);
        if (pressEText != null)
            pressEText.SetActive(false);
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

        float dist = Vector3.Distance(transform.position, _player.position);
        bool inRange = dist <= interactRadius;

        if (pressEText != null) pressEText.SetActive(inRange && !_isOpen);

        if (inRange && !_isOpen && Input.GetKeyDown(KeyCode.E))
            OpenNote();
        else if (_isOpen && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
            CloseNote();
    }

    void OpenNote()
    {
        _isOpen = true;
        notePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseNote()
    {
        _isOpen = false;
        notePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}