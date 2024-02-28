using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/************************************************************************
 * This class is attached to the PowerUp Game Objects.  Its goal is to
 * spin the PowerUp for visual effect
 * 
 * 
 * Bruce Gustin
 * November 27, 2023
 ************************************************************************/

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;   // Programmer preference
    [SerializeField] private float cooldown;        // Length of PowerUp effect as stated in the GDD

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    // Sends the value of the cooldown field to the Player so that it ends the Coroutine in the Player
    public float GetCooldown()
    {
        return cooldown;
    }
}
