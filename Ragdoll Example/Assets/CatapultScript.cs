using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerScripts;
using Interactions;

public class CatapultScript : MonoBehaviour
{
    Vector3 forceDirection;
    // Start is called before the first frame update
    void Start()
    {
        forceDirection = (transform.Find("endPoint").gameObject.transform.position - transform.Find("startingPoint").gameObject.transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            Debug.Log(forceDirection);
            other.gameObject.GetComponent<ForceSimulator>().AddImpact(forceDirection, 500);
            other.gameObject.GetComponent<BetterMovement>().flyRagdoll(other.gameObject);
        }
        else if (other.gameObject.GetComponent<Throwable>())
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(300, 700, 0), ForceMode.Impulse);
        }
    }
}
