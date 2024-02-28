using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*************************************************************************
 * Startup manager is attached to a Player Selection scene's camera.  It
 * allows the player to select thier player choice.
 * 
 * Bruce Gustin
 * November 23, 2023
 ************************************************************************/

public class StartupManager : MonoBehaviour
{
    private PlayerController[] players;        //Array representing all available player choices

    private void Start()
    {
        // initializes the player's array with player objects
        players = FindObjectsOfType<PlayerController>();
    }

    // Update calls SelectPlayerAndStartGame once per frame as it waits for user choice of player
    void Update()
    {
        SelectPlayerAndStartGame();
    }

    private void SelectPlayerAndStartGame()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits an object
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the object has a player selection component
                PlayerController playerSelected = hit.collider.GetComponent<PlayerController>();

                // Check if a chouce was made
                if (playerSelected != null)
                {
                    // Deletes all the unchosen players from the scene.
                    foreach(PlayerController player in players)
                    {
                        if(!playerSelected.Equals(player))
                        {
                            Destroy(player.gameObject);
                        }
                    }

                    // Moves the chosen player above the scene 
                    playerSelected.gameObject.transform.position = Vector3.up * 25;
                    SceneManager.LoadScene("Level1");
                }

            }
        }
    }

}
