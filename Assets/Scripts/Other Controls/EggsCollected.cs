using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


/*************************************************************************
 * This script will keep track of the amount of eggs total to be collected, 
 * and how many the player has collected.
 * Once all eggs are collected, the UI dissapears.
 * This script won't do anything in the coop, forest, or boss battle scenes
 * 
 * Alex Collier-Lake
 * March 27, 2024
 ************************************************************************/

public class EggsCollected : MonoBehaviour
{
    private TMP_Text siblingsSaved;
    private Canvas canvas;

    private int eggsInScene;

    private int startEggsInScene;
    private string sceneName;

    private GameManager gameManager;
    private SpawnManager spawnManager;

    private GameObject portal;

    private void Update()
    {
        EggsExists[] eggs = FindObjectsOfType<EggsExists>();

        eggsInScene = eggs.Length;

        siblingsSaved.text = "Siblings Saved: " + (startEggsInScene - eggsInScene) + "/" + startEggsInScene;

        if (FindObjectsOfType<EggsExists>().Length == 0 && !spawnManager.portalActive)
        {
            spawnManager.SetObjectActive(portal, 1.0f);
            spawnManager.portalActive = true;
        }
    }

    private void OnEnable()
    {
        //AssignLevelValues();
        portal = Resources.FindObjectsOfTypeAll<ZoomInAnimator>()[0].gameObject;
        startEggsInScene = FindObjectsOfType<EggsExists>().Length;
        //canvas.gameObject.SetActive(false);
        sceneName = SceneManager.GetActiveScene().name;
        siblingsSaved = GameObject.FindObjectOfType<Canvas>().GetComponentInChildren<TMP_Text>();
        canvas = GameObject.FindObjectOfType<Canvas>();
        gameManager = FindAnyObjectByType<GameManager>().GetComponent<GameManager>();
        spawnManager = FindAnyObjectByType<SpawnManager>().GetComponent<SpawnManager>();


        //only show UI in scenes where there are eggs to be collected
        if (sceneName == "coop" || sceneName == "forest" || sceneName == "bossBattle")
        {
            canvas.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        //
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Egg"))
        {
            Destroy(other.gameObject);

            // This will open up the portal if there a no more eggs in the scene
            if (FindObjectsOfType<EggsExists>().Length == 0 && !spawnManager.portalActive)
            {
                spawnManager.portalActive = true;
                spawnManager.SetObjectActive(portal, 1.0f);
            }
        }
    }

    /******

    private void AssignLevelValues()
    {

        siblingsSaved = GameManager.Instance.siblingsSaved;
        canvas = GameManager.Instance.canvas;
        spawnManager = GameManager.Instance.spawnManager;
        gameManager = GameManager.Instance.gameManager;
        portal = GameManager.Instance.portal;
        eggsCollected = GameManager.Instance.eggsCollected;
        startEggsInScene = GameManager.Instance.startEggsInScene;
        sceneName = GameManager.Instance.sceneName;




    }

    ****/
}


