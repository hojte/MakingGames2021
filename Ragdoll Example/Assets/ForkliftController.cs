using System.Collections;
using System.Collections.Generic;
using Sound;
using UnityEngine;
using UnityEngine.AI;

public class ForkliftController : MonoBehaviour
{
    [Header("Sounds")]
    [Tooltip("Sound played when recieving damages")]
    public AudioClip onDamage;
    public GameObject enemyPrefab;
    private bool isStunned = false;
    

    private float returnFromStunTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
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
        if (Input.GetKeyDown(KeyCode.Keypad1))
            AudioUtility.CreateSFX(onDamage, transform.position, 1f, 15f);
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision");
        if (collision.gameObject.tag == "Item" || collision.gameObject.tag == "HeavyItem")
        {
        }
    }

    public void die()
    {
        AudioUtility.CreateSFX(onDamage, transform.position, 1f, 15f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().enemySlain();
        Destroy(gameObject);
        GetComponent<Animator>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
    }
    public void stun()
    {
        isStunned = true;
        GetComponent<Animator>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
    }
    void returnFromStun()
    {
        isStunned = false;

    }
}