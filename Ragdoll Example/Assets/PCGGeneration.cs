﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCGGeneration : MonoBehaviour
{
    public GameObject Anchor1;
    public GameObject Anchor2;
    public GameObject Anchor3;
    public GameObject Anchor4;
    public GameObject TType1;
    public GameObject TType2;
    public bool jobDone = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!jobDone)
        {
            float firstRoll = Random.Range(0, 2);
            float secondRoll = Random.Range(0, 2);
            float thirdRoll = Random.Range(0, 2);
            float fourthRoll = Random.Range(0, 2);

            if (firstRoll > 0.0f)
                Instantiate(TType1, Anchor1.gameObject.transform.position, Anchor1.gameObject.transform.rotation);
            else
                Instantiate(TType2, Anchor1.gameObject.transform.position, Anchor1.gameObject.transform.rotation);

            if (secondRoll > 0.0f)
                Instantiate(TType1, Anchor2.gameObject.transform.position, Anchor2.gameObject.transform.rotation);
            else
                Instantiate(TType2, Anchor2.gameObject.transform.position, Anchor2.gameObject.transform.rotation);

            if (thirdRoll > 0.0f)
                Instantiate(TType1, Anchor3.gameObject.transform.position, Anchor3.gameObject.transform.rotation);
            else
                Instantiate(TType2, Anchor3.gameObject.transform.position, Anchor3.gameObject.transform.rotation);

            if (fourthRoll > 0.0f)
                Instantiate(TType1, Anchor4.gameObject.transform.position, Anchor4.gameObject.transform.rotation);
            else
                Instantiate(TType2, Anchor4.gameObject.transform.position, Anchor4.gameObject.transform.rotation);

            Debug.Log("Map generated by using PCG. Rolls: " + firstRoll + " " + secondRoll + " " + thirdRoll + " " + fourthRoll);

            jobDone = true;
        }
    }
}
