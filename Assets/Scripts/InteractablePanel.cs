using UnityEngine;

public class InteractablePanel : MonoBehaviour
{
    public GameObject pressEText;
    public GameObject panelToOpen;

    private bool playerNearby = false;

    void Start()
    {
        pressEText.SetActive(false);
        panelToOpen.SetActive(false);
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            panelToOpen.SetActive(true);
            pressEText.SetActive(false);

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            pressEText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            pressEText.SetActive(false);
        }
    }
}