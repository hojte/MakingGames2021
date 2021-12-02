using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{


    public Light myLight;
    public float pulseSpeed = 1f; //this is in seconds
    private float timer;

    void Start()
    {
        //myLight = GetComponent<Light>();
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > pulseSpeed)
        {
            Debug.Log("Puls");
            timer = 0;
            myLight.enabled = !myLight.enabled;
            //myLight.enabled = false;
        }
    }
}
