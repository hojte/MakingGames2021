using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactions;

public class PressurePlate : MonoBehaviour
{
    public GameObject linkedGameObject;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        linkedGameObject.GetComponent<ppLinker>().startOfInteraction();
        GetComponent<Renderer>().material.color = Color.green;
    }

    private void OnTriggerExit(Collider collision)
    {
        linkedGameObject.GetComponent<ppLinker>().endOfInteraction();
        GetComponent<Renderer>().material.color = Color.gray;
    }
}
