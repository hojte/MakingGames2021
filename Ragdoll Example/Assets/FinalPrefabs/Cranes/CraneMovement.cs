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
        Vector3 relativePos = _player.position - plate.position;
        relativePos.y = 0;
        
            // the second argument, upwards, defaults to Vector3.up
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up*Time.deltaTime);
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
}
