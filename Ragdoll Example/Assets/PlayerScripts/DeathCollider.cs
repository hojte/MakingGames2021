using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class DeathCollider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyAttack")
        {
            
            Debug.Log("Collision with enemy");
            playerDie(GameObject.FindWithTag("PlayerAnimator"), GameObject.FindWithTag("Player"));
        }

        
    }
    
    void playerDie(GameObject playerAnimator, GameObject player)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        

        //Destroy(gameObject, 7f);
        // player.GetComponent<CapsuleCollider>().enabled = false;
        //player.GetComponent<CharacterController>().enabled = false; 

        //playerAnimator.GetComponent<Animator>().enabled = false;


        //gameObject.GetComponent<NavMeshAgent>().enabled = false;
        //setRigidBodyState(false);
        //setColliderState(true);
    }
}
