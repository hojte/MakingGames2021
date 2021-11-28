using System.Collections;
using System.Collections.Generic;
using Interactions;
using PlayerScripts;
using UnityEngine;

public class ShelfImpact : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        Throwable throwable = collision.gameObject.gameObject.GetComponent<Throwable>();
        if (throwable && throwable.canTiltShelves)
        {
            
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        else if (collision.gameObject.GetComponent<PlayerController>())
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(this.gameObject.GetComponent<Rigidbody>().velocity.x, 0, this.gameObject.GetComponent<Rigidbody>().velocity.z);

        }
    }
}
