using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactions;

public class ppLinker : MonoBehaviour
{
    //What to do when the linked PP is being pressured
    public void startOfInteraction()
    {
        // Example of use:
        if (gameObject.GetComponent<DoorController>())
        {
            gameObject.GetComponent<DoorController>().SetClosed(false);
        }
    }

    // What to do when the linked PP is released
    public void endOfInteraction()
    {
        // Example of use:
        if (gameObject.GetComponent<DoorController>())
        {
            gameObject.GetComponent<DoorController>().SetClosed(true);
        }
    }
}
