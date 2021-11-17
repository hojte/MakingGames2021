using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        try
        { // todo might lag??
            other.GetComponent<PlayerController>().pickupables.Add(GetComponent<Rigidbody>());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        try
        {
            other.GetComponent<PlayerController>().pickupables.Remove(GetComponent<Rigidbody>());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
