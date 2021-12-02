using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{


    public Light myLight1;
    public Light myLight2;
    public Light point1;
    public float pulseSpeed;
    public float speed;
    public float timer; 
    void Update()
    {
        myLight1.transform.Rotate(0, speed*Time.deltaTime, 0);
        myLight2.transform.Rotate(0, -speed * Time.deltaTime, 0);

        timer += Time.deltaTime;
        if (timer > pulseSpeed)
        {
            timer = 0;
            point1.enabled = !point1.enabled;
        }

    }
}
