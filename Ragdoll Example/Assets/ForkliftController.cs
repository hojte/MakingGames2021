using System.Collections;
using System.Collections.Generic;
using Interactions;
using Sound;
using UnityEngine;
using UnityEngine.AI;

public class ForkliftController : MonoBehaviour
{
    [Header("Sounds")]
    [Tooltip("Sound played when recieving damages")]
    public AudioClip onDamage;
    public AudioClip onHit;
    public AudioClip onDeath;
    public GameObject enemyPrefab;
    private bool isStunned = false;
    float lastHitTime = 0.0f;
    int hp = 3;
    public GameObject deathParticles;
    public GameObject fireParticles;
    public GameObject smokeParticles;


    private float returnFromStunTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GetComponentsInChildren <ParticleSystem>().Length; i++)
        {
            GetComponentsInChildren<ParticleSystem>()[i].enableEmission = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            AudioUtility.CreateSFX(onDamage, transform.position, 1f, 15f);
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision");
        if (collision.gameObject.GetComponent<Throwable>())
        {
            
        }
    }

    public void die()
    {
        if (Time.time > (lastHitTime + 3.0f))
        {
            lastHitTime = Time.time;
            AudioUtility.CreateSFX(onDamage, transform.position, 1f, 15f);
            hp--;
            if (hp == 2)
                GetComponentInChildren<ParticleSystem>().enableEmission = true;
            else if (hp == 1)
                for (int i = 0; i < GetComponentsInChildren<ParticleSystem>().Length; i++)
                    GetComponentsInChildren<ParticleSystem>()[i].enableEmission = true;
            else if (hp <= 0)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().enemySlain();
                GameObject deathExplosion = Instantiate(deathParticles, GetComponentInChildren<ParticleSystem>().transform.position, Quaternion.identity);
                deathExplosion.transform.localScale = new Vector3(30, 30, 30);
                AudioUtility.CreateSFX(onDeath, transform.position, 1f, 15f);

                for (int i = 0; i < GetComponentsInChildren<MeshRenderer>().Length; i++)
                {
                    GameObject d = Instantiate(deathParticles, GetComponentsInChildren<MeshRenderer>()[i].transform.position, Quaternion.identity);
                    d.transform.localScale = new Vector3(30, 30, 30);
                    AudioUtility.CreateSFX(onDeath, transform.position, 1f, 15f);

                    GameObject f = Instantiate(fireParticles, GetComponentsInChildren<MeshRenderer>()[i].transform.position, Quaternion.identity);
                    f.transform.localScale = new Vector3(30, 30, 30);

                    GameObject s = Instantiate(smokeParticles, GetComponentsInChildren<MeshRenderer>()[i].transform.position, Quaternion.identity);
                    s.transform.localScale = new Vector3(30, 30, 30);
                }

                //Destroy(gameObject, 1f);
                GetComponentInChildren<Animator>().enabled = false;
                GetComponent<NavMeshAgent>().enabled = false;
            }
            
        }
    }

    public void futileHit()
    {
        AudioUtility.CreateSFX(onHit, transform.position, 1f, 15f);
    }
}