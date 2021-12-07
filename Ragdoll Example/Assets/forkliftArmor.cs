using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactions;

public class forkliftArmor : MonoBehaviour
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
        if (collision.gameObject.CompareTag("Item") || collision.gameObject.CompareTag("HeavyItem"))
            if (!collision.gameObject.GetComponent<Throwable>().getHasHitBoss())
            {
                GetComponentInParent<ForkliftController>().futileHit();
                Debug.Log("beep");
            }
    }
    
}
