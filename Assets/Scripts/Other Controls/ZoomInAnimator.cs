using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************************************************
 * The ZoonInAnimator is attached to the portal.  It causes the portal 
 * to zoom into the scene  
 * 
 * Bruce Gustin
 * November 23, 2023
 ************************************************************************/

public class ZoomInAnimator : MonoBehaviour
{
    private Vector3 desiredScale;                            // Current scale of portal
    private Vector3 initialScale = Vector3.one.normalized;   // Set an initial scale to approx (.57, .57, .57)
    private float zoomInRate = 1.06f;                        // Growth per cycle
    private float zoomInFrequency = 0.03f;                   // Time of cycle

    // Initializes fields and begins Invocation
    private void OnEnable()
    {
        desiredScale = transform.localScale;
        transform.localScale = initialScale;
        InvokeRepeating("ZoomIn", 0, zoomInFrequency);
    }

    // Resets original scale and makes sure that the portal reacts to player physics
    private void OnDisable()
    {
        transform.localScale = desiredScale;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    // What gets invoked on enable to zoom in the portal.
    private void ZoomIn()
    {
        if (transform.localScale.magnitude < desiredScale.magnitude)
        {
            transform.localScale *= zoomInRate;
        }
        else
        {
            CancelInvoke();
        }
    }


}
