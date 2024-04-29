using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    public GameObject player;

    

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
    public bool playerActive { private get; set; }      // Set from player with it falls below -10 meter

    public bool resetLevel { get; set; }


    /*****

    //egg fields
    public int startEggsInScene;
    public TMP_Text siblingsSaved;
    public Canvas canvas;
    public int eggsCollected;
    public string sceneName;
    public GameManager gameManager;
    public SpawnManager spawnManager;
    public GameObject portal;

    *****/

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


        GameObject player = Resources.FindObjectsOfTypeAll<PlayerController>()[0].gameObject;
        player.transform.position = Vector3.up * 25;
        player.SetActive(true);
        //find the player and set it active


    }

    

    private void OnEnable()
    {
        player = FindAnyObjectByType<PlayerController>().gameObject;

        //startEggsInScene = FindObjectsOfType<EggsExists>().Length;
        //siblingsSaved = GameObject.FindObjectOfType<Canvas>().GetComponentInChildren<TMP_Text>();
        //siblingsSaved.text = "Siblings Saved: " + eggsCollected + "/" + startEggsInScene;
        //sceneName = SceneManager.GetActiveScene().name;
        //gameManager = this.GetComponent<GameManager>();
        //spawnManager = FindAnyObjectByType<SpawnManager>().GetComponent<SpawnManager>();
        //portal = Resources.FindObjectsOfTypeAll<ZoomInAnimator>()[0].gameObject;
        //canvas = GameObject.FindObjectOfType<Canvas>();
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
        Debug.Log(currentScene);

        // Extract the level number from the scene name
        int nextLevel = int.Parse(currentScene.Substring(5)) + 1;

        SceneManager.LoadScene("Level" + nextLevel.ToString());

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

    public void RestartScene()
    {
        // Get the current scene index
        string sceneName = SceneManager.GetActiveScene().name;

        // Reload the current scene
        SceneManager.LoadScene(sceneName);
    }



}
