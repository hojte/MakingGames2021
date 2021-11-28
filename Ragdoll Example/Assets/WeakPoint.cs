using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    float timeOfLastHit = 0;
    float damageIntakeCooldown = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<Throwable>() && Time.time > (timeOfLastHit + damageIntakeCooldown))
        {
            Debug.Log("hit registered");
            GetComponentInParent<ForkliftController>().die();
            timeOfLastHit = Time.time;
        }
    }
}
