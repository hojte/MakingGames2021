using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerScripts;
using Interactions;

public class deathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BetterMovement>())
            other.gameObject.GetComponent<BetterMovement>().die(other.gameObject);
        else if (other.gameObject.GetComponent<Throwable>())
        {
            //other.gameObject.GetComponent<Throwable>().breaksOnHit = true;
            other.gameObject.GetComponent<Throwable>().setHasBeenPickedUp(true);
            
        }

    }
}
