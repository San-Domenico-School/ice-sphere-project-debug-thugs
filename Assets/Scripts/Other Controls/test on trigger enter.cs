using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testontriggerenter : MonoBehaviour
{
    private UIController uIController;

    private void OnTriggerEnter(Collider other)
    {
        UIController.textTrigger = true;
    }
}
