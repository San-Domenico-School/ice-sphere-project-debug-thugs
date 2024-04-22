using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float launchForce;
    public float boostForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    public Transform startPosition;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private Rigidbody playerRB;                   // To utilized player physics
    private SphereCollider playerCollider;        // Places collider around player not model.
    private Light powerUpIndicator;               // Component to emit light when triggering a powerup
    private Transform focalpoint;                 // Makes sure the the force is always pointing toward the focal point
    private PlayerInputActions inputAction;       // C# script of Input Action
    private float moveForceMagnitude;             // Force of forward movement
    private float forwardOrBackward;              // Direction of movement (forward or backwards)
    public bool hasPowerUp { get; private set; }  // Allows SpawnManager to detect powerup on player

    // Initializes ready to jump and player rigidbody on start
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerCollider = GetComponent<SphereCollider>();
        readyToJump = true;
    }

    //Updates method every frame 
    private void Update()
    {
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); //Searches for ground on frame update if ground is detected player = grounded

        MyInput();
        SpeedControl();
    }

    //countinuesly checks the move player field 
    private void FixedUpdate()
    {
        MovePlayer();
    }

    // executes method on specified player input
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); // moves player horizontal on horizantal input A/D
        verticalInput = Input.GetAxisRaw("Vertical");   // moves player vertical on vertical input W/S

        if (Input.GetKey(jumpKey) && readyToJump && grounded) //if the player is ready to jump and grounded while jumpkey(spacebar) is pressed the following is executed
        {
            readyToJump = false;    //player is no longer ready to jump

            Jump(); // jump action is performed

            Invoke(nameof(ResetJump), jumpCooldown);    //jump cooldown begins 
        }
    }

    private void MyInput2()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Debug.Log("Horizontal Input: " + horizontalInput);
        Debug.Log("Vertical Input: " + verticalInput);
    }

    // when the player collides with the launch pad tagged object they will perform the launch action
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Startup") || collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
        // Changes Startup to ground so that the player is not constantly updating while colliding with the ground
        if (collision.gameObject.CompareTag("Startup"))
        {
            collision.gameObject.tag = "Ground";
            playerCollider.material.bounciness = GameManager.Instance.playerBounce;
            AssignLevelValues();
        }
    }

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

    // moves the player in the relative direction of player input
    // if the player is grounded they move at normal move speed
    //if the player is not grounded they move at normal speed times air multiplier
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 1000f, ForceMode.Force);

        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 1000f * airMultiplier, ForceMode.Force);
    }

    //limits player movement speed to player movement speed 
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    // when the jump action is performed the player will launch into the air at jump speed
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    // when the launch action is invoked the player will launch into the air at launch speed
    private void Launch()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * launchForce, ForceMode.Impulse);
    }

    /*private void Boost()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.forward * boostForce, ForceMode.Impulse);
    }*/
    // resets ready to jump 
    private void ResetJump()
    {
        readyToJump = true;
    }

    // Triggers are on portals and powerups
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Portal"))
        {
            gameObject.layer = LayerMask.NameToLayer("Portal");
        }

        if (other.gameObject.CompareTag("PowerUp"))
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
            if (transform.position.y < other.transform.position.y - 1)
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