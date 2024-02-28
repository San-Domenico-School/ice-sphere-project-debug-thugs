using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/************************************************************************
 * This class is attached to the IceSphere prefabs.  This class uses 
 * NavMesh with player as destination.
 * 
 * Bruce Gustin
 * November 27, 2023
 ************************************************************************/

public class MoveToTarget : MonoBehaviour
{

    private NavMeshAgent navMeshAgent;
    private GameObject target;              // Player set as target via Find method
    private Rigidbody targetRB;             // Player's rigidbody


    // Used to set Player's rigidbody as target.
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
        if(target != null)
        {
            targetRB = target.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        MoveTowardsTarget();
    }

    // Uses NavMesh to set player as destination of Ice Sphere
    private void MoveTowardsTarget()
    {
        navMeshAgent.SetDestination(targetRB.transform.position);
    }
}
