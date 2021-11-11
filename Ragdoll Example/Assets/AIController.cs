using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{

    public Transform Player;
    int moveSpeed = 4;
    int maxDist = 10;
    int minDist = 5;
    int aggroRange = 30;
    bool inCombat = false;
    Vector3 patrollingWayPoint;
    int newWayPointDistance = 10;
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Wander();
    }

    void Update()
    {
        if ((Vector3.Distance(transform.position, Player.position) < aggroRange) || inCombat)
        {
            inCombat = true;

            if (Vector3.Distance(transform.position, Player.position) >= minDist)
            {
                agent.destination = Player.position;

                if (Vector3.Distance(transform.position, Player.position) <= maxDist)
                {
                    // Attack
                }
            }
        }
        else
        {
            //transform.position += transform.forward * moveSpeed * Time.deltaTime;
            patrollingWayPoint.y = transform.position.y;

            if (Vector3.Distance(transform.position, patrollingWayPoint) <= 3)
            {
                // when the distance between us and the target is less than 3
                // create a new way point target
                Wander();


            }
        }
    }

    void Wander()
    {
        patrollingWayPoint = new Vector3(Mathf.Clamp(Random.Range(transform.position.x - newWayPointDistance, transform.position.x + newWayPointDistance), 0.0f, 70.0f), transform.position.y, Mathf.Clamp(Random.Range(transform.position.z - newWayPointDistance, transform.position.z + newWayPointDistance), 0.0f, 60.0f));
        
        transform.LookAt(patrollingWayPoint);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        agent.destination = patrollingWayPoint;

        Debug.Log(patrollingWayPoint + " and " + (transform.position - patrollingWayPoint).magnitude);
    }
}