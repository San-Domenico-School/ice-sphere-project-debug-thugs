using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************************************************
 * PlayerInteractor is attached to a Ice Sphere.  It is what add forces
 * to either the player or the ice shpere upon collision.
 * 
 * Bruce Gustin
 * November 23, 2023
 ************************************************************************/

public class PowerInteractor : MonoBehaviour
{
    [SerializeField] private float pushForce;            // Force applied to player by ice sphere
    private Rigidbody iceSphereRB;
    private IceSphereController iceSphereController;
    

    // Assigns components to fields
    void Start()
    {
        iceSphereRB = GetComponent<Rigidbody>();
        iceSphereController = GetComponent<IceSphereController>();
    }

    // Detects collision with the player/
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            Rigidbody playerRB = player.GetComponent<Rigidbody>();

            // Gets the players controlling script component
            PlayerController playerController = player.GetComponent<PlayerController>();

            // Creates a normalized vector that points from the IceSphere towards the player
            Vector3 direction = (player.transform.position - transform.position).normalized;

            // Determines if the ice sphere pushes the player or if the player repels the ice sphere
            // based on it the powerup is active.
            if (playerController.hasPowerUp)
            { 
                iceSphereRB.AddForce(-direction * playerRB.mass * GameManager.Instance.playerRepelForce, ForceMode.Impulse);
            }
            else
            {
                playerRB.AddForce(direction * iceSphereRB.mass * pushForce, ForceMode.Impulse);
            }
        }
    }
}
