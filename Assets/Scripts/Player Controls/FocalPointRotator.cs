using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*************************************************************************
 * FocalPointRotator is attached to a Focal Point.  It moves the camera
 * around the island with user controls.
 * 
 * Bruce Gustin
 * November 23, 2023
 ************************************************************************/

public class FocalPointRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;     // How fast the user can rotate around the island
    private PlayerInputActions inputAction;           // Player's input controller for clockwise/counterclockwise motion
    private float moveDirection;                      // Determins if clockwise or counterclockwisw

    // Create a new InputAction object
    void Awake()
    {
        inputAction = new PlayerInputActions();
    }

    // Adds OnMovement events to inputAction's Player's movement
    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.Player.Movement.performed += OnMovementPerformed;
        inputAction.Player.Movement.canceled += OnMovementCanceled;
    }

    // Remove OnMovement events to inputAction's Player's movement
    private void OnDisable()
    {
        inputAction.Disable();
        inputAction.Player.Movement.performed -= OnMovementPerformed;
        inputAction.Player.Movement.canceled -= OnMovementCanceled;
    }

    // Rotate the focal point which the camera is attached to causing the camera to turn
    void Update()
    {
        transform.Rotate(Vector3.up, moveDirection * rotationSpeed * Time.deltaTime);
    }

    // Called when movement binding is performed
    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveDirection = value.ReadValue<Vector2>().x;
    }

    // Called when movement binding has been completed
    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveDirection = 0;
    }
}
