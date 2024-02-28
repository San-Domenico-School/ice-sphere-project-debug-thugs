using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/************************************************************************
 * This class is attached to the IceSphere prefabs.  This class controls
 * the melting and destruction of the ice spheres over time.
 * 
 * Bruce Gustin
 * November 27, 2023
 ************************************************************************/

public class IceSphereController : MonoBehaviour
{
    [SerializeField] private float startDelay;            // Used in the InvokeRepeating method
    [SerializeField] private float reductionEachRepeat;   // Determines speed of melting via GDD.
    [SerializeField] private float minimumVolume;         // Size at destruction

    private Rigidbody iceRB;
    private ParticleSystem iceVFX;

    // Start is called before the first frame update
    void Start()
    {
        // Accelerates wave spawning for debuggin purposes
        if(GameManager.Instance.debugSpawnWaves)
        {
            reductionEachRepeat = .5f;
        }

        // Assigns components to fields
        iceRB = GetComponent<Rigidbody>();
        iceVFX = GetComponent<ParticleSystem>();

        // Calls private sub-method
        RandomizeSizeAndMass();

        // Begins melting of iceSphere.  Repeat set to look smooth.
        InvokeRepeating("Melt", startDelay, 0.4f);
    }

    // Prevents each wave from being destroyed simultaneously
    private void RandomizeSizeAndMass()
    {
        float randomValue = Random.value * 0.5f + 0.5f;
        iceRB.mass *= randomValue;
        transform.localScale *= randomValue;
    }

    // Destroys ice sphere when volume is less than minimum volume
    private void Dissolution()
    {
        float volume = 4f / 3f * Mathf.PI * Mathf.Pow(transform.localScale.x, 2);
        if(volume < 0.8 && FindObjectsOfType<IceSphereController>().Length > 1)
        {
            iceVFX.Stop();
        }

        if(volume < minimumVolume)
        {
            Destroy(gameObject);
        }
    }

    // Melts ice sphere based on reduction rate from GDD
    private void Melt()
    {
        iceRB.mass *= reductionEachRepeat;
        transform.localScale *= reductionEachRepeat;
        Dissolution();
    }
}
