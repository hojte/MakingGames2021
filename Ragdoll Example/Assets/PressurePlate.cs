using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactions;
using PlayerScripts;

public class PressurePlate : MonoBehaviour
{
    Transform playerTransform;
    public GameObject linkedGameObject;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.gray;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.hasChanged)
            if (Vector3.Distance(playerTransform.position, transform.position) < (transform.localScale.magnitude-4.0f))
            {
                linkedGameObject.GetComponent<ppLinker>().startOfInteraction();
                GetComponent<Renderer>().material.color = Color.green;
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
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>() || collision.gameObject.GetComponent<Throwable>() || collision.gameObject.GetComponent<PlayerController>())
        {
            linkedGameObject.GetComponent<ppLinker>().endOfInteraction();
            GetComponent<Renderer>().material.color = Color.gray;
        }
    }
}
