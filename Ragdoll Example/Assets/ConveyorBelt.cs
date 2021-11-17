using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{

    public float speed;

    public Vector3 direction;

    public List<GameObject> onBelt; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(onBelt.Count);
        for (int i = 0; i < onBelt.Count; i++)
        {
            onBelt[i].GetComponent<Rigidbody>().velocity = speed * direction * Time.deltaTime; 
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision belt");
        collision.gameObject.GetComponent<Rigidbody>().freezeRotation = true; 
       // collision.gameObject.GetComponent<Rigidbody>().velocity = speed * direction * Time.deltaTime;
        onBelt.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("sdf");
        collision.gameObject.GetComponent<Rigidbody>().freezeRotation = false; 
        onBelt.Remove(collision.gameObject);
    }
}
