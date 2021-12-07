using System.Collections.Generic;
using Interactions;
using Sound;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    [Header("Sounds")]
    [Tooltip("Sounds played when dying")]
    public List<AudioClip> onDieClips;
    public GameObject enemyPrefab; 
    private bool isStunned = false;
    public GameObject rig;
    bool isCatapulted = false;
    bool beenCatapulted = false;
    float timeOfCatapult = 0.0f;
    Vector3 catapultDirection;
    float returnFromCatapultTimer = 0;
    public List<AudioClip> midairScreams;
    
    public ParticleSystem onStunnedVFX;



    private float returnFromStunTimer =0f;
    // Start is called before the first frame update
    void Start()
    {
        setRigidBodyState(true);
        setColliderState(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad3))
            GetComponent<CompassElement>().RegisterFromCompass();

        if (isStunned)
        {
            if(!onStunnedVFX.isPlaying) onStunnedVFX.Play();
            returnFromStunTimer += Time.deltaTime;
            if (returnFromStunTimer > 3f)
            {
                returnFromStun();
                returnFromStunTimer = 0;
            }
        }
        else
        {
            if (onStunnedVFX.isPlaying) onStunnedVFX.Stop();
        }

        if (Input.GetKeyDown(KeyCode.Keypad1) && onDieClips.Count>0)
        {
            var onDie = onDieClips[new System.Random().Next(onDieClips.Count)];
            Destroy(AudioUtility.CreateSFX(onDie, transform, 1f), onDie.length);
        }
            

        if (beenCatapulted)
            if (Time.time > (timeOfCatapult + 0.1))
            {
                GetComponent<Rigidbody>().AddForce(800 * catapultDirection, ForceMode.Impulse);
                beenCatapulted = false;
                if (midairScreams.Count > 0)
                {
                    var midairScream = midairScreams[new System.Random().Next(midairScreams.Count)];
                    Destroy(AudioUtility.CreateSFX(midairScream, transform, 1f, volume: 1f), midairScream.length);
                }
                
            }

        if (isCatapulted)
        {
            returnFromCatapultTimer += Time.deltaTime;
            if (returnFromCatapultTimer > 6f)
            {
                returnFromStun();
                returnFromCatapultTimer = 0;
                isCatapulted = false;
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Throwable>()) {
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 1 && collision.gameObject.GetComponent<Throwable>().getHasBeenPickedUp())
                die();
        }

        if (collision.gameObject.tag == "Shelf") {
            if (collision.gameObject.GetComponent<Rigidbody>().isKinematic == false && collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 1.0)
            {
                die();
            }
        }

        if (collision.gameObject.GetComponent<ForkliftAI>())
        {
            if (collision.gameObject.GetComponent<ForkliftAI>().isForkliftCharging())
                stun();
        }
    }

    /*private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("Collision");
        if (collision.gameObject.GetComponent<Throwable>()))
        {
            //Debug.Log("Collision Item");
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 1)
                die();
        }

        if (collision.gameObject.tag == "Shelf")
        {
            //Debug.Log("Collision shelf");
            if (collision.gameObject.GetComponent<Rigidbody>().isKinematic == false)
            {
                //Debug.Log("Collision Moving Shelf");
                die();
            }
        }
    }*/

     void die()
    {
        if (onDieClips.Count > 0)
        {
            var onDie = onDieClips[new System.Random().Next(onDieClips.Count)];
            Destroy(AudioUtility.CreateSFX(onDie, transform, 1f, volume: 0.7f), onDie.length);
        }

        GetComponent<CompassElement>().UnregisterFromCompass();
        FindObjectOfType<GameController>().enemySlain();
        //Destroy(gameObject, 7f);
        GetComponent<Animator>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        setRigidBodyState(false);
        setColliderState(true);
    }
    public void stun()
    {
        isStunned = true; 
        GetComponent<Animator>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        setRigidBodyState(false);
        setColliderState(true);
    }

    public void getCatapulted(Vector3 forceDirection)
    {
        isCatapulted = true;
        GetComponent<Animator>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        beenCatapulted = true;
        timeOfCatapult = Time.time;
        catapultDirection = forceDirection;
        //setRigidBodyState(false);
        //setColliderState(true);
    }
    void returnFromStun()
    {

        var clone = Instantiate(
            Resources.Load<GameObject>("Prefabs/AIEnemy"), rig.transform.position, transform.rotation); 
        clone.GetComponent<Animator>().enabled = true;
        clone.GetComponent<EnemyController>().enemyPrefab = enemyPrefab;
        clone.GetComponent<AIController>().inCombat = true;
        
        Destroy(this.gameObject);
        isStunned = false;
        
    }
    
    void setRigidBodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies) {
            rigidbody.isKinematic = state;
        }
        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void setColliderState(bool state) {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders) {
            collider.enabled = state;
        }
        GetComponent<Collider>().enabled = !state;
    }
}
