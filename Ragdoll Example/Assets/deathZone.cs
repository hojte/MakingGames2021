using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerScripts;

public class deathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BetterMovement>())
            other.gameObject.GetComponent<BetterMovement>().die(other.gameObject);
    }
}
