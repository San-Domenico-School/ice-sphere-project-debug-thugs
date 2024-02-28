using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*************************************************************************
 * Spawn manager is attached to a Spawn Manager game objects in every scene 
 * except the Player Selector scene.  It's primary purpose is to hold all
 * the spawn metric from the GDD for use in the scene.
 * 
 * Bruce Gustin
 * November 23, 2023
 ************************************************************************/

public class SpawnManager : MonoBehaviour
{
    [Header("Objects to Spawn")]
    [SerializeField] private GameObject iceSphere;
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject powerUp;

    [Header("Wave Fields")]
    [SerializeField] private int initialWave;
    [SerializeField] private int increaseEachWave;
    [SerializeField] private int maximumWave;

    [Header("Portal Fields")]
    [SerializeField] private int portalFirstAppearance;
    [SerializeField] private float portalByWaveProbability;
    [SerializeField] private float portalByWaveDuration;

    [Header("PowerUp Fields")]
    [SerializeField] private int powerUpFirstAppearance;
    [SerializeField] private float powerUpByWaveProbability;
    [SerializeField] private float powerUpByWaveDuration;

    [Header("Island")]
    [SerializeField] private GameObject island;

    private Vector3 islandSize;     // Use to insure spawn location is on island
    private int waveNumber;         // Keeps track of which spawn number your on.
    private bool portalActive;      // Toggle true when a portal is in the scene so only one portal is on scene at a time
    private bool powerUpActive;     // Toggle true when a powerUp is in the scene so only one powerUp is on scene at a time

    // Start is called before the first frame update
    void Start()
    {
        // Initializes dimensions of island
        islandSize = island.GetComponent<MeshCollider>().bounds.size;
        // Initializes waveNumber as dictated by the GDD
        waveNumber = initialWave;

        // Used for debugging.  Keeps portal on for 99 second so it is easy to enter a portal
        if(GameManager.Instance.debugTransport)
        {
            portalByWaveDuration = 99;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Makes sure that the postal does not appear until what is stated in the GDD and only if one is not already present
        if ((waveNumber > portalFirstAppearance || GameManager.Instance.debugSpawnPortal) && !portalActive)
        {
            SetObjectActive(portal, portalByWaveProbability);
        }

        // Makes sure that the powerUp does not appear until what is stated in the GDD and only if one is not already present
        if ((waveNumber > powerUpFirstAppearance || GameManager.Instance.debugSpawnPowerUp)
             && !powerUpActive)
        {
            SetObjectActive(powerUp, powerUpByWaveProbability);
        }

        // Initiates a new wave when there is no longer an Ice Spheres present
        if (FindObjectsOfType<IceSphereController>().Length == 0 &&
           GameObject.Find("Player") != null)
        {
            SpawnIceWave();
        }
    }

    // Spawns ice spheres
    private void SpawnIceWave()
    {
        for(int i = 0; i <= increaseEachWave * (waveNumber - 1) ; i++)
        {
            Instantiate(iceSphere, SetRandomPosition(0), iceSphere.transform.rotation);
        }

        if(waveNumber < maximumWave)
        {
            waveNumber++;
        }
    }

    // Calls the spawn routine 
    private void SetObjectActive(GameObject obj, float byWaveProbability)
    {
        if(Random.value < waveNumber * byWaveProbability * Time.deltaTime ||
           GameManager.Instance.debugSpawnPortal || GameManager.Instance.debugSpawnPowerUp)
        {
            obj.transform.position = SetRandomPosition(obj.transform.position.y);
            StartCoroutine(CountdownTimer(obj.tag));
        }
    }

    // Called from SetObjectActive to return a viable position vector.  posY parameter makes sure
    // that each object is at the proper y-position.
    private Vector3 SetRandomPosition(float posY)
    {
        float posX = Random.Range(-islandSize.x / 2.75f, islandSize.x / 2.75f);
        float posZ = Random.Range(-islandSize.z / 2.75f, islandSize.z / 2.75f);
        return new Vector3(posX, posY, posZ);
    }

    // Sets the object spawned active, waits, the set it inactive.
    IEnumerator CountdownTimer(string objectTag)
    {
        float byWaveDuration = 0;

        switch (objectTag)
        {
            case "Portal":
                portal.SetActive(true);
                portalActive = true;
                byWaveDuration = portalByWaveDuration;
                break;
            case "PowerUp":
                powerUp.SetActive(true);
                powerUpActive = true;
                byWaveDuration = powerUpByWaveDuration;
                break;
        }

        yield return new WaitForSeconds(waveNumber * byWaveDuration);

        switch (objectTag)
        {
            case "Portal":
                portal.SetActive(false);
                portalActive = false;
                break;
            case "PowerUp":
                powerUp.SetActive(false);
                powerUpActive = false;
                break;
        }
    }
}
