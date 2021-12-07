using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Sound;
public class ForkliftAI : MonoBehaviour
{

    public Transform Player;
    int maxDist = 20;
    int minDist = 5;
    public int aggroRange = 90;
    bool inCombat = false;
    public Vector3 patrollingWayPoint;
    NavMeshAgent agent;
    float timeOfLastAttack = 0;
    public bool patrollingEnemy = false;
    bool firstHalfOfPatrol = true;
    Vector3 spawnPoint;
    private Animator anim;
    float timeOfLastCharge = -15;
    public float chargeCooldown = 20;
    bool isCharging = false;
    float beginningOfCharge = 0;
    float chargeChannelDuration = 3.3f;
    bool isCompletingCharge = false;
    bool isStunned = false;
    float timeOfLastStun = 0;
    public float stunDuration = 5;
    public float baseSpeed = 15;
    GameObject lift;
    Quaternion defaultLiftRotation;
    private GameController _gameController;
    public AudioClip chargeSound;
    public AudioClip engineClipSound;
    private AudioSource engineSound;
    public AudioClip crashSound;
    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        Player = FindObjectOfType<PlayerController>().transform;
        agent = GetComponent<NavMeshAgent>();
        anim = this.GetComponentInChildren<Animator>();

        _gameController.bossCombat = true;
        _gameController.newEnemyInCombat();
        if (patrollingEnemy)
        {
            spawnPoint = transform.position;
            patrollingWayPoint = spawnPoint + new Vector3(20, 0, 0);
        }
        else
            Wander();

        anim.SetBool("isMoving", true);
        
        lift = transform.Find("ForkliftNew").gameObject;
        defaultLiftRotation = lift.transform.localRotation;

    }

    void Update()
    {
        if (!engineSound)
            engineSound = gameObject.GetComponent<Audio>().GetComponent<AudioSource>();

        if (!Player) Player = FindObjectOfType<PlayerController>().transform;
        if (agent.enabled)
        {
            if (!isStunned)
            {
                if ((Vector3.Distance(transform.position, Player.position) < aggroRange) || inCombat)
                {
                    if (!inCombat)
                    {
                        inCombat = true;
                        timeOfLastCharge = Time.time - (chargeCooldown-5);
                    }

                    if (!isCharging && Time.time > timeOfLastCharge + chargeCooldown)
                    {
                        isCharging = true;
                        beginningOfCharge = Time.time;
                        agent.isStopped = true;
                        anim.SetBool("isCharging", true);
                        agent.destination = Player.position + (Player.position - transform.position) + new Vector3(0,20,0);
                        engineSound.Stop();
                        engineSound.clip = chargeSound;
                        engineSound.loop = false;
                        engineSound.Play();
                    }

                    if (isCharging)
                    {

                        if (agent.isStopped && Time.time > beginningOfCharge + chargeChannelDuration)
                        {
                            isCompletingCharge = true;
                            agent.isStopped = false;
                        }

                        if (isCompletingCharge) {
                            agent.speed = 125;
                            agent.acceleration = 1000;
                            timeOfLastCharge = Time.time;
                            anim.SetBool("isCharging", false);

                            if (Time.time > beginningOfCharge + chargeChannelDuration * 2 || Vector3.Distance(agent.destination,transform.position) < 4)
                            {
                                agent.speed = baseSpeed;
                                isCharging = false;
                                isCompletingCharge = false;
                                
                                anim.SetBool("isMoving", false);
                                lift.transform.localRotation = defaultLiftRotation;
                                if (engineSound != null)
                                {
                                    engineSound.Stop();
                                    engineSound.clip = engineClipSound;
                                    engineSound.loop = true;
                                    engineSound.Play();
                                }
                            }
                        }

                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, Player.position) >= minDist)
                        {
                            agent.destination = Player.position;

                            if (anim.GetBool("isMoving") == false && !isStunned){
                                anim.SetBool("isMoving", true);
                            }

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
            else // is stunned
            {
                if (Time.time > timeOfLastStun + stunDuration)
                {
                    agent.isStopped = true;
                    isStunned = false;
                }
            }
        }
    }

    void Wander()
    {
        if (agent.enabled)
        {
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

    public void wallHit()
    {
        if (timeOfLastStun < timeOfLastCharge && isCompletingCharge)
        {
            isStunned = true;
            timeOfLastStun = Time.time;
            agent.isStopped = true;
            Debug.Log("boss stunned");
            anim.SetBool("isMoving", false);

            engineSound.Stop();
            engineSound.clip = crashSound;
            engineSound.loop = false;
            engineSound.Play();
        }
    }

    public bool isForkliftCharging()
    {
        return (isCompletingCharge || isCharging);
    }
    
}