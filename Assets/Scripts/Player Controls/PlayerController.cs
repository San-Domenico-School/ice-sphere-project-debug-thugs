using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/************************************************************************
 * PlayerController is attached to the Player  It moves the player with 
 * user controls and detects collisions.
 * 
 * Bruce Gustin
 * November 23, 2023
 ***********************************************************************/

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRB;                   // To utilized player physics
    private SphereCollider playerCollider;        // Places collider around player not model.
    private Light powerUpIndicator;               // Component to emit light when triggering a powerup
    private Transform focalpoint;                 // Makes sure the the force is always pointing toward the focal point
    private PlayerInputActions inputAction;       // C# script of Input Action
    private float moveForceMagnitude;             // Force of forward movement
    private float forwardOrBackward;              // Direction of movement (forward or backwards)
    public bool hasPowerUp { get; private set; }  // Allows SpawnManager to detect powerup on player

   // Creates a new InputAction object
   void Awake()
    {
        inputAction = new PlayerInputActions();
    }

    // Assigns components to fields
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerCollider = GetComponent<SphereCollider>();
        powerUpIndicator = GetComponent<Light>();

        playerCollider.material.bounciness = 0.4f;  // Set bounciness of rigidbody
        powerUpIndicator.intensity = 0;             // Makes sure the powerup indicator light if off on startup
        DontDestroyOnLoad(gameObject);              // Allows player to move between scenes
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

    // FixedUpdate is called once per frame using physics system
    void FixedUpdate()
    {
        Move();

        // End of game condition
        if(transform.position.y < -10)
        {
            GameManager.Instance.gameOver = true;
            Debug.Log("You Lost");                 //*****   More will go here after Prototype  ***** //
            gameObject.SetActive(false);
        }
    }

    // Updates player's field between levels when the player hits the ground triggered in OnCollisionEnter
    private void AssignLevelValues()
    { 
        transform.localScale = GameManager.Instance.playerScale;
        playerRB.mass = GameManager.Instance.playerMass;
        playerRB.drag = GameManager.Instance.playerDrag;
        moveForceMagnitude = GameManager.Instance.playerMoveForce;
        focalpoint = GameObject.Find("Focal Point").transform;
        gameObject.layer = LayerMask.NameToLayer("Player");
        if (GameManager.Instance.debugPowerUpRepel)
        {
            hasPowerUp = true;
        }
    }

    // Called when movement binding is performed
    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        forwardOrBackward = value.ReadValue<Vector2>().y;
    }

    // Called when movement binding has been completed
    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        forwardOrBackward = 0;    // Neither forward or backward
    }

    // Adds force for player motion
    private void Move()
    {
        if (focalpoint != null)
        {
            playerRB.AddForce(focalpoint.forward.normalized * forwardOrBackward * moveForceMagnitude);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Changes Startup to ground so that the player is not constantly updating while colliding with the ground
        if(collision.gameObject.CompareTag("Startup"))
        {
            collision.gameObject.tag = "Ground";
            playerCollider.material.bounciness = GameManager.Instance.playerBounce;
            AssignLevelValues();
        }
    }

    // Triggers are on portals and powerups
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Portal"))
        {
            gameObject.layer = LayerMask.NameToLayer("Portal");
        }

        if(other.gameObject.CompareTag("PowerUp"))
        {
            PowerUpController powerUpController = other.gameObject.GetComponent<PowerUpController>();
            other.gameObject.SetActive(false);
            StartCoroutine(Cooldown(powerUpController.GetCooldown()));
        }
    }

    // Resets portal state when exiting the portal.  
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Portal"))
        {
            gameObject.layer = LayerMask.NameToLayer("Player");

            // differentiates between the player going into the portal and popping up over the portal
            if(transform.position.y < other.transform.position.y - 1)
            {
                transform.position = Vector3.up * 25;
                GameManager.Instance.switchLevel = true;
            }
        }
    }

    // Toggles on the powerup indicator for "cooldown" seconds.
    IEnumerator Cooldown(float cooldown)
    {
        hasPowerUp = true;
        powerUpIndicator.intensity = 3.5f;
        yield return new WaitForSeconds(cooldown);
        hasPowerUp = false;
        powerUpIndicator.intensity = 0.0f;
    }
}
