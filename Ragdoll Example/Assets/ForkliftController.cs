using System.Collections;
using System.Collections.Generic;
using Interactions;
using Sound;
using UI;
using UnityEngine;
using UnityEngine.AI;

public class ForkliftController : MonoBehaviour
{
    [Header("Sounds")]
    [Tooltip("Sound played when recieving damages")]
    public List<AudioClip> onDamageClips;
    public List<AudioClip> onHitClips;
    public List<AudioClip> onDeathClips;
    public GameObject enemyPrefab;
    private bool isStunned = false;
    float lastHitTime = 0.0f;
    public int hp = 3;
    public GameObject deathParticles;
    public GameObject fireParticles;
    public GameObject smokeParticles;
    private GameController _gameController;


    private float returnFromStunTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _gameController.bossCombat = true;
        for (int i = 0; i < GetComponentsInChildren <ParticleSystem>().Length; i++)
        {
            GetComponentsInChildren<ParticleSystem>()[i].enableEmission = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Keypad2) && onDamageClips.Count>0)
        {
            var onDamage = onDamageClips[new System.Random().Next(onDamageClips.Count)];
            Destroy(AudioUtility.CreateSFX(onDamage, transform, 1f), onDamage.length);
        }*/

        /*if (_gameController.enemiesInCombat < 1 && hp > 0)
            _gameController.newEnemyInCombat();*/

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
            if (onDamageClips.Count > 0)
            {
                var onDamage = onDamageClips[new System.Random().Next(onDamageClips.Count)];
                Destroy(AudioUtility.CreateSFX(onDamage, transform, 1f,  1f, rolloffDistanceMin:25f), onDamage.length);
            }
            
            hp--;
            if (hp == 2)
                GetComponentInChildren<ParticleSystem>().enableEmission = true;
            else if (hp == 1)
                for (int i = 0; i < GetComponentsInChildren<ParticleSystem>().Length; i++)
                    GetComponentsInChildren<ParticleSystem>()[i].enableEmission = true;
            else if (hp <= 0)
            {
                FindObjectOfType<GameController>().bossCombat = false;
                FindObjectOfType<GameController>().enemySlain();

                if (FindObjectOfType<ScoreController>())
                {
                    FindObjectOfType<ScoreController>().EnemyKilled();
                    FindObjectOfType<ScoreController>().EnemyKilled();
                    FindObjectOfType<ScoreController>().EnemyKilled();
                    FindObjectOfType<ScoreController>().EnemyKilled();
                }
                

                GameObject deathExplosion = Instantiate(deathParticles, GetComponentInChildren<ParticleSystem>().transform.position, Quaternion.identity);
                deathExplosion.transform.localScale = new Vector3(30, 30, 30);
                if (onDeathClips.Count > 0)
                {
                    var onDeath = onDeathClips[new System.Random().Next(onDeathClips.Count)];
                    Destroy(AudioUtility.CreateSFX(onDeath, transform, 1f, 0.075f, rolloffDistanceMin:25f), onDeath.length);
                }
                

                for (int i = 0; i < GetComponentsInChildren<MeshRenderer>().Length; i++)
                {
                    GameObject d = Instantiate(deathParticles, GetComponentsInChildren<MeshRenderer>()[i].transform.position, Quaternion.identity);
                    d.transform.localScale = new Vector3(30, 30, 30);
                    if (onDeathClips.Count > 0)
                    {
                        var onDeath = onDeathClips[new System.Random().Next(onDeathClips.Count)];
                        Destroy(AudioUtility.CreateSFX(onDeath, transform, 1f, 0.075f, rolloffDistanceMin:25f), onDeath.length);
                    }
                    

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
        if (onHitClips.Count > 0)
        {
            var onHit = onHitClips[new System.Random().Next(onHitClips.Count)];
            var tmpGo = Instantiate(new GameObject("soundDummy"), transform);
            Destroy(AudioUtility.CreateSFX(onHit, tmpGo.transform, 1f,  1f, rolloffDistanceMin:25f), onHit.length);
        }
        
    }
}