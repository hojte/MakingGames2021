using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewdriverAttack : MonoBehaviour
{
    public GameObject throwable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attack(Transform spawnPoint, Transform Player)
    {
        Instantiate(throwable, spawnPoint.transform.position, spawnPoint.rotation);
    }
}
