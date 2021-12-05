using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactions;

public class ppLinker : MonoBehaviour
{

    [HideInInspector]
    public int numSpawned = 0;
    public int maxToSpawn = 3;

    //What to do when the linked PP is being pressured
    public void startOfInteraction()
    {
        // Example of use:
        if (gameObject.GetComponent<TowerSpawnerController>() && numSpawned <= maxToSpawn - 1)
        {
            gameObject.GetComponent<TowerSpawnerController>().startSpawning = true;
        }
    }

    // What to do when the linked PP is released
    public void endOfInteraction()
    {
        // Example of use:
        if (gameObject.GetComponent<TowerSpawnerController>())
        {
            gameObject.GetComponent<TowerSpawnerController>().startSpawning = false;
        }
    }
}
