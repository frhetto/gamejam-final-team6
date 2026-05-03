using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float moveZ = Input.GetAxis("Vertical");   // W/S
        float moveX = Input.GetAxis("Horizontal"); // A/D

        Vector3 move = transform.forward * moveZ + transform.right * moveX;
        controller.Move(move * moveSpeed * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * turnSpeed * Time.deltaTime);
    }
}