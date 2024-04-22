using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;
    public float distanceFromPlayer = 2f; // Distance from the player where the camera should be positioned

    private Transform player; // Reference to the player's transform

    public Transform orientation;
    public Transform playerCamera; // Reference to the main camera's transform

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player = transform.parent; // Assuming the player is a child of this camera GameObject
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        // Rotate the player (parent) horizontally based on mouse input
        player.Rotate(Vector3.up, mouseX);

        // Rotate the camera vertically based on mouse input
        transform.Rotate(Vector3.right, -mouseY); // Invert mouseY to match typical FPS camera controls

        // Calculate the desired camera position in front of the player
        Vector3 desiredCameraPosition = player.position - player.forward * distanceFromPlayer;

        // Update the camera's position to the desired position
        transform.position = desiredCameraPosition;
    }
}
