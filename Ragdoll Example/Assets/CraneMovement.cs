using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneMovement : MonoBehaviour
{
    public Transform plate;

    public Transform cyl1;

    public Transform rotate1;

    public Transform tip1;

    public Transform rotate2;

    public Transform tip2;

    private float tip1MinX = 0.35f;
    private float tip1MaxX = -0.35f;

    private int flipTime = 3;
    private float flipTimeCounter = 0f;

    public bool tip1Flip = false;

    private float speed = 30; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        flipTimeCounter += Time.deltaTime; 
        rotate1.RotateAround(rotate1.position, rotate1.up, Time.deltaTime * speed);
        
        
        if (tip1Flip)
        {
            tip1.Rotate(Vector3.right * Time.deltaTime * speed);
            tip2.Rotate(Vector3.right * Time.deltaTime * speed);
        }
        else
        {
            tip1.Rotate(Vector3.left * Time.deltaTime * speed);
            tip2.Rotate(Vector3.left * Time.deltaTime * speed);
        }

        if (flipTime<flipTimeCounter )
        {
            flipTime = 6; //To make it rotate all the way back 
            tip1Flip = !tip1Flip;
            flipTimeCounter = 0; 
        }
       






    }
}
