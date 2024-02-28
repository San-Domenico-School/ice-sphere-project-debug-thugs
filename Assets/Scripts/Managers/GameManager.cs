using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*************************************************************************
 * Game manager is attached to a Game Manager game objects in every scene 
 * except the Player Selector scene.  It's primary purpose is to hold all
 * the non-spawn metric from the GDD for use in the scene.
 * 
 * Bruce Gustin
 * November 23, 2023
 ************************************************************************/

public class GameManager : MonoBehaviour
{
    // Field values need to be added from the GDD
    public static GameManager Instance;
    [Header("Player Fields")]
    public Vector3 playerScale;
    public float playerMass;
    public float playerDrag;
    public float playerMoveForce;
    public float playerBounce;
    public float playerRepelForce;

    // Length determined in the GDD
    [Header("Levels Fields")]
    public GameObject[] waypoints;

    // Toggles during prototype development
    [Header("Debug Fields")]
    public bool debugSpawnWaves;
    public bool debugPowerUpRepel;
    public bool debugSpawnPortal;
    public bool debugSpawnPowerUp;
    public bool debugTransport;

    public bool switchLevel { private get; set; }   // Set from player trigger when player passes through portal
    public bool gameOver { private get; set; }      // Set from player with it falls below -10 meter 

    // Awake is called before any Start methods are called  It insures that the Game Manager is
    // there for the Player and Ice SPhere to get field valued from the GDD
    void Awake()
    {
        //This is a common approach to handling a class with a reference to itself.
        //If instance variable doesn't exist, assign this object to it
        if (Instance == null)
            Instance = this;
        //Otherwise, if the instance variable does exist, but it isn't this object, destroy this object.
        //This is useful so that we cannot have more than one GameManager object in a scene at a time.
        else if (Instance != this)
            Destroy(this);
    }

    // Update is called once per frame checking if ready to switch levels
    void Update()
    {
        if(switchLevel)
        {
            SwitchLevels();
        }
    }

    // Extracts the level number from the string to set then load the next level.
    private void SwitchLevels()
    {
        // Stops class from calling this method again
        switchLevel = false;

        // Get the name of the currently active scene
        string currentScene = SceneManager.GetActiveScene().name;

        // Extract the level number from the scene name
        int nextLevel = int.Parse(currentScene.Substring(5)) + 1;

        // Checks to see if you're at the last level
        if (nextLevel <= SceneManager.sceneCountInBuildSettings - 1)
        {
            // Load the next scene
            SceneManager.LoadScene("Level" + nextLevel.ToString());

        }
        //If you are at the last level, ends the game.  //*****   More will go here after Prototype  ***** //
        else
        {
            gameOver = true;
            Debug.Log("You won");
        }
    }
}
