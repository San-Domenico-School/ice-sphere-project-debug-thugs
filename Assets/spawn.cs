using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
   // [SerializeField] private GameObject PlayerPos;
 //   [SerializeField] private GameObject FoxPos;
    private GameObject Fox;
    private GameObject Player;
    public float spawnDistanceThreshold = 10f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // get the distance between player and Fox
        float distance = Vector3.Distance(Fox.GetComponent<Rigidbody>().position, transform.position);

        // Check if the distance is less than the spawn distance threshold
        if (distance < spawnDistanceThreshold)
        {
            //spawns the fox
            gameObject.SetActive(true);
        }
    }
}
