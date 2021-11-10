using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ThrowItem : MonoBehaviour
{
    public Transform cam;
    public Rigidbody ball;
    // Start is called before the first frame update
    private Quaternion rotation; 
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Rigidbody clone;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotation = Quaternion.LookRotation(cam.forward, cam.up);

        if (Input.GetMouseButtonDown(0))
        {
            throwItem(rotation);
        }
    }
    
    void throwItem(Quaternion rotation)
    {
        //Spawn
        float x = this.gameObject.transform.position.x;
        float y = this.gameObject.transform.position.y+5;
        float z = this.gameObject.transform.position.z; 
            
        clone = Instantiate(ball, new Vector3(x,y,z), rotation);
       // clone.useGravity = false;

        //move direction
        clone.velocity = clone.transform.TransformDirection(Vector3.forward * 30);
        Destroy(clone.gameObject, 3f);
    }


    
    private void OnMouseDown()
    {
        
        //throwItem(rotation);
        //updateLine();

    }
    private void OnMouseUp()
    {
        //move direction
        //clone.velocity = clone.transform.TransformDirection(Vector3.forward * 30);
        
       // clone.useGravity = true;
        //Destroy(clone.gameObject, 3f);

    }
    

}

