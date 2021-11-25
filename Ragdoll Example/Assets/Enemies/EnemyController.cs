using System.Collections;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Sounds")]
    [Tooltip("Sound played when recieving damages")]
    public AudioClip onDamage;
    public GameObject enemyPrefab; 
    private bool isStunned = false;

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
        if (isStunned)
        {
            returnFromStunTimer += Time.deltaTime;
            if (returnFromStunTimer > 3f)
            {
                returnFromStun();
                returnFromStunTimer = 0;
            }
        }
        if(Input.GetKeyDown(KeyCode.Keypad1))
            AudioUtility.CreateSFX(onDamage, transform.position, 1f, 15f);
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision");
        if (collision.gameObject.tag == "Item" || collision.gameObject.tag == "HeavyItem") {
            //Debug.Log("Collision Item");
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 1)
                die();
        }

        if (collision.gameObject.tag == "Shelf") {
            //Debug.Log("Collision shelf");
            if (collision.gameObject.GetComponent<Rigidbody>().isKinematic == false)
            {
                //Debug.Log("Collision Moving Shelf");
                die();
            }
        }
       
    }

    /*private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("Collision");
        if (collision.gameObject.tag == "Item" || collision.gameObject.tag == "HeavyItem")
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
        AudioUtility.CreateSFX(onDamage, transform.position, 1f, 15f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().enemySlain();
        Destroy(gameObject, 7f);
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
    void returnFromStun()
    {
        var clone = Instantiate(enemyPrefab, transform.position, transform.rotation);
        clone.GetComponent<AIController>().Player = GameObject.FindGameObjectWithTag("Player").transform;
        clone.GetComponent<EnemyController>().enemyPrefab = enemyPrefab; 
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
