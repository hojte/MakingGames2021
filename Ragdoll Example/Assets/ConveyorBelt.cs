using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{

    public float speed;


    public List<GameObject> onBelt; 


    // Update is called once per frame
    void Update()
    {
        Debug.Log(onBelt.Count);
        for (int i = 0; i < onBelt.Count; i++)
        {
            onBelt[i].transform.position -= transform.right * Time.deltaTime * speed; 
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision belt");
        //collision.gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        //collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        onBelt.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        //collision.gameObject.GetComponent<Rigidbody>().freezeRotation = false; 
        collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        onBelt.Remove(collision.gameObject);
    }
}
