using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class CraneMovement : MonoBehaviour
{
    public Transform plate;

   

    private Transform _player;
    private Animator anim;
    public float attackRange; 
    public bool constantlyAttacking = false;
    public float attackingTimer;
    private float timer; 

  
    void Start()
    {
        _player = FindObjectOfType<PlayerController>().transform;
        anim = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerController>().transform;
        }
        if (!constantlyAttacking)
        {

            Vector3 relativePos = _player.position - plate.position;
            relativePos.y = 0;

            // the second argument, upwards, defaults to Vector3.up
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up * Time.deltaTime);
            plate.rotation = rotation;

            float dist = Vector3.Distance(_player.position, plate.position);
            if (dist < attackRange)
            {
                anim.SetBool("isAttacking", true);
            }
            else
            {
                anim.SetBool("isAttacking", false);
            }
        }
        else {
            timer += Time.deltaTime;
            if (attackingTimer < timer)
            {
                anim.SetBool("isAttacking", true);
                timer = 0;
            }
            else {
                anim.SetBool("isAttacking", false);
            }
        }


    }
}
