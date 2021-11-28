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
        for (int i = 0; i < onBelt.Count; i++)
        {
            onBelt[i].transform.position -= transform.right * Time.deltaTime * speed; 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
        onBelt.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        onBelt.Remove(collision.gameObject);
    }
}
