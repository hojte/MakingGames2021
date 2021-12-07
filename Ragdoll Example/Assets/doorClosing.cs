using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorClosing : MonoBehaviour
{
    public GameObject linkedObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BetterMovement>()){
            linkedObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("immad");
        }
    }
}
