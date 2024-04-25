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

   
    public int eggsCollected { private get; set; }



    [SerializeField] public TMP_Text siblingsSaved;
    [SerializeField] public Canvas canvas;

    private int eggsInScene;
    private int startEggsInScene;

    public string sceneName;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private SpawnManager spawnManager;

    private GameObject portal;

  
    private void Update()
    {
        EggsExists[] eggs = FindObjectsOfType<EggsExists>();
        eggsInScene = eggs.Length;

        siblingsSaved.text = "Siblings Saved: " + eggsCollected + "/" + startEggsInScene;

        if (FindObjectsOfType<EggsExists>().Length == 0 && !spawnManager.portalActive)
        {
            Debug.Log(spawnManager.portalActive);
            
            spawnManager.SetObjectActive(portal, 1.0f);
            spawnManager.portalActive = true;

        }


    }

  private void OnEnable()
  {
      portal = Resources.FindObjectsOfTypeAll<ZoomInAnimator>()[0].gameObject;
      startEggsInScene = FindObjectsOfType<EggsExists>().Length;
      Debug.Log(startEggsInScene);
      //canvas.gameObject.SetActive(false);
      sceneName = SceneManager.GetActiveScene().name;


      //only show UI in scenes where there are eggs to be collected
      if (sceneName == "coop" || sceneName == "forest" || sceneName == "bossBattle")
      {
         canvas.gameObject.SetActive(false);
      }
  }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Egg"))
        {
            Destroy(other.gameObject);
            eggsCollected++;

            // This will open up the portal if there a no more eggs in the scene
            if (FindObjectsOfType<EggsExists>().Length == 0 && !spawnManager.portalActive)
            {
                Debug.Log(spawnManager.portalActive);
                spawnManager.portalActive = true;
                spawnManager.SetObjectActive(portal, 1.0f);
               
            }
        }





    }

}
    
