using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class CraneMovement : MonoBehaviour
{
    public Transform plate;

   

    private Transform _player; 

  
    void Start()
    {
        _player = FindObjectOfType<PlayerController>().transform;
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
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        plate.rotation = rotation;
        
        
        /*
        flipTimeCounter += Time.deltaTime; 
       // rotate1.RotateAround(rotate1.position, rotate1.up, Time.deltaTime * speed);
        
        
        if (tip1Flip)
        {
            tip1.Rotate(Vector3.left * Time.deltaTime * speed);
            tip2.Rotate(Vector3.left * Time.deltaTime * (speed/2));
        }
        else
        {
            tip1.Rotate(Vector3.right * Time.deltaTime * speed);
            tip2.Rotate(Vector3.right * Time.deltaTime * (speed/2));
        }

        if (flipTime<flipTimeCounter )
        {
            //flipTime = 1.5; //To make it rotate all the way back 
            tip1Flip = !tip1Flip;
            flipTimeCounter = 0; 
        }
       
*/





    }
}
