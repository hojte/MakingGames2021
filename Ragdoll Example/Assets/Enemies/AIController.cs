using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{

    public Transform Player;
    int moveSpeed = 4;
    int maxDist = 20;
    int minDist = 5;
    int aggroRange = 30;
    bool inCombat = false;
    public Vector3 patrollingWayPoint;
    int newWayPointDistance = 10;
    NavMeshAgent agent;
    float timeOfLastAttack = 0;
    public bool patrollingEnemy = false;
    bool firstHalfOfPatrol = true;
    Vector3 spawnPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (patrollingEnemy)
        {
            spawnPoint = transform.position;
            patrollingWayPoint = spawnPoint + new Vector3(20, 0, 0);
        }
        else
            Wander();

    }

    void Update()
    {
        if (Player == null)
        {
            Player = GameObject.FindWithTag("Player").transform; 
        }

        if (agent.enabled)
        {
            if ((Vector3.Distance(transform.position, Player.position) < aggroRange) || inCombat)
            {
                if (!inCombat)
                {
                    inCombat = true;
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().newEnemyInCombat();
                }

                if (Vector3.Distance(transform.position, Player.position) >= minDist)
                {
                    agent.destination = Player.position;

                    if (Vector3.Distance(transform.position, Player.position) <= maxDist)
                    {
                        if (Time.time > timeOfLastAttack + 2.0)
                        {
                            if (GetComponent<ScrewdriverAttack>())
                                GetComponent<ScrewdriverAttack>().attack(transform, Player);
                            else
                                Debug.Log("No attack");
                            timeOfLastAttack = Time.time;
                        }
                    }
                }
            }
            else
            {
                if (patrollingEnemy)
                {
                    patrollingWayPoint.y = transform.position.y;
                    spawnPoint.y = transform.position.y;
                    Patrol();
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
        }
    }

    void Wander()
    {
        if (agent.enabled)
        {
            patrollingWayPoint = new Vector3(Mathf.Clamp(Random.Range(transform.position.x - newWayPointDistance, transform.position.x + newWayPointDistance), 0.0f, 70.0f), transform.position.y, Mathf.Clamp(Random.Range(transform.position.z - newWayPointDistance, transform.position.z + newWayPointDistance), 0.0f, 60.0f));

            transform.LookAt(patrollingWayPoint);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            agent.destination = patrollingWayPoint;
        }
    }

    void Patrol()
    {
        if (firstHalfOfPatrol)
        {
            transform.LookAt(patrollingWayPoint);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            agent.destination = patrollingWayPoint;

            if (Vector3.Distance(transform.position, patrollingWayPoint) <= 3)
                firstHalfOfPatrol = false;
        }
        else
        {
            transform.LookAt(spawnPoint);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            agent.destination = spawnPoint;

            if (Vector3.Distance(transform.position, spawnPoint) <= 3)
                firstHalfOfPatrol = true;
        }
    }
}