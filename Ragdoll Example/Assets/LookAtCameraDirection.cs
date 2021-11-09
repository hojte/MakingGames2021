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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         rotation = Quaternion.LookRotation(cam.forward, cam.up);

         if (Input.GetMouseButtonDown(0))
         {
             //Spawn
             Debug.Log(cam.transform.rotation);
             float x = this.gameObject.transform.position.x;
             float y = this.gameObject.transform.position.y+5;
             float z = this.gameObject.transform.position.z; 
             Rigidbody clone;
             clone = Instantiate(ball, new Vector3(x,y,z), rotation);
          
             clone.velocity = clone.transform.TransformDirection(Vector3.forward * 60);
             Destroy(clone.gameObject, 3f);
         }
           


    }

      void throwItem(Quaternion rotation)
    {
        Debug.Log(cam.transform.rotation);
        float x = this.gameObject.transform.position.x;
        float y = this.gameObject.transform.position.y+5;
        float z = this.gameObject.transform.position.z; 
        Rigidbody clone;
        Vector3 camDir = cam.transform.forward;
        clone = Instantiate(ball, new Vector3(x,y,z), cam.transform.rotation);
        //clone.transform.LookAt(camDir);
        //clone.transform.LookAt(new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z));
        clone.velocity = clone.transform.TransformDirection(cam.transform.forward * 30);
        
        
    }
    
    
}
