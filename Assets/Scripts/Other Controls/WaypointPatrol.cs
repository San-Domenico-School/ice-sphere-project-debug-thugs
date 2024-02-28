using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/************************************************************************
 * This class is attached to the IceSphere prefabs.  This class uses 
 * NavMesh with waypoints as destination.
 * 
 * Bruce Gustin
 * November 27, 2023
 ************************************************************************/

public class WaypointPatrol : MonoBehaviour
{ 
    private GameObject[] waypoints;
    private NavMeshAgent navMeshAgent;
    private int waypointIndex;

    // Sets a random waypoint as the initial destination
    void Start()
    {
        waypoints = GameManager.Instance.waypoints;
        navMeshAgent = GetComponent<NavMeshAgent>();
        waypointIndex = Random.Range(0, waypoints.Length);
    }

    // Update is called once per frame
    void Update()
    {
        MoveToNextWaypoint();
    }

    // Rotates linearly through all the waypoints.
    private void MoveToNextWaypoint()
    {
        navMeshAgent.SetDestination(waypoints[waypointIndex].transform.position);
        if (navMeshAgent.remainingDistance < 0.1f && !navMeshAgent.pathPending)
        {
            waypointIndex = ++waypointIndex % waypoints.Length;
        }
    }
}
