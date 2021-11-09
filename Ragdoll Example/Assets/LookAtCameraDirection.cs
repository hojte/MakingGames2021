using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraDirection : MonoBehaviour
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

        /*if (Input.GetMouseButtonDown(0))
        {
            throwItem(rotation);
        }*/
    }
    
    void throwItem(Quaternion rotation)
    {
        //Spawn
        float x = this.gameObject.transform.position.x;
        float y = this.gameObject.transform.position.y+5;
        float z = this.gameObject.transform.position.z; 
            
        clone = Instantiate(ball, new Vector3(x,y,z), rotation);
        clone.useGravity = false;

        //move direction
        //clone.velocity = clone.transform.TransformDirection(Vector3.forward * 30);
        //Destroy(clone.gameObject, 3f);
    }

    private Vector3 mousePressDownPos;

    private Vector3 mouseReleasePos;


    private LineRenderer lineRenderer;
    private void OnMouseDown()
    {
        throwItem(rotation);
        
        //For creating line renderer object
        lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.11f;
        lineRenderer.endWidth = 0.11f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;    
                

        lineRenderer.SetPosition(0, clone.transform.position); //x,y and z position of the starting point of the line

        
        lineRenderer.SetPosition(1, clone.transform.TransformDirection(Vector3.forward * 360)); //x,y and z position of the end point of the line
       
        
        Destroy(lineRenderer.gameObject, 3f);
        

    }
    private void OnMouseUp()
    {
        //move direction
        clone.velocity = clone.transform.TransformDirection(Vector3.forward * 100);
        clone.useGravity = true;
        Destroy(clone.gameObject, 3f);

    }
}