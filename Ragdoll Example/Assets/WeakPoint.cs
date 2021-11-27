using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
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
        Debug.Log("hit registered");
        if (other.gameObject.tag == "Item" || other.gameObject.tag == "HeavyItem")
        {
            GetComponentInParent<ForkliftController>().die();
        }
    }
}
