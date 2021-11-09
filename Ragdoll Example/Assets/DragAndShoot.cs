using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndShoot : MonoBehaviour
{

    private Vector3 mousePressDownPos;

    private Vector3 mouseReleasePos;

    public Rigidbody rb;

    private bool isShoot; 
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>(); 
    }

    private void OnMouseDrag()
    {
        Vector3 forceInit = (Input.mousePosition - mousePressDownPos);
        Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y))*forceMultiplier;

        if (!isShoot)
        {
            DrawTrajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);
        }
    }

    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        mouseReleasePos = Input.mousePosition;
        Shoot(mouseReleasePos-mousePressDownPos);
    }

    private float forceMultiplier = 3f; 

    void Shoot(Vector3 Force)
    {
        Debug.Log("shooot");
        if (isShoot) return; 
        
        rb.AddForce(new Vector3(Force.x, Force.y, Force.y)* forceMultiplier);
        //isShoot = true;

    }
}
