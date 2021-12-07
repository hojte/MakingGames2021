using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactions;
using PlayerScripts;

public class PressurePlate : MonoBehaviour
{
    Transform playerTransform;
    public GameObject linkedGameObject;
    public GameObject linkedGameObject2;
    public bool twoLinkedObjects = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.gray;
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null) playerTransform = FindObjectOfType<PlayerController>().transform;
        if (playerTransform.hasChanged)
            if (Vector3.Distance(playerTransform.position, transform.position) < (transform.localScale.magnitude - 4.0f))
            {
                linkedGameObject.GetComponent<ppLinker>().startOfInteraction();
                GetComponent<Renderer>().material.color = Color.green;
                if (twoLinkedObjects) { linkedGameObject2.GetComponent<ppLinker>().startOfInteraction(); }
            }
            else
            {
            }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>() || collision.gameObject.GetComponent<Throwable>())
        {
            linkedGameObject.GetComponent<ppLinker>().startOfInteraction();
            GetComponent<Renderer>().material.color = Color.green;
            if (twoLinkedObjects) { linkedGameObject2.GetComponent<ppLinker>().startOfInteraction(); }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>() || collision.gameObject.GetComponent<Throwable>() || collision.gameObject.GetComponent<PlayerController>())
        {
            linkedGameObject.GetComponent<ppLinker>().endOfInteraction();
            GetComponent<Renderer>().material.color = Color.gray;
            if (twoLinkedObjects) { linkedGameObject2.GetComponent<ppLinker>().startOfInteraction(); }
        }
    }
}
