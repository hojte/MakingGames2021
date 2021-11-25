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
        Vector3 dir = Player.transform.position - spawnPoint.transform.position - new Vector3(0,3,0);
        GameObject projectile = Instantiate(throwable, spawnPoint.transform.position + new Vector3(0,4,0) + 0.2f*dir, spawnPoint.rotation);
        projectile.GetComponent<Rigidbody>().AddForce(dir.normalized * 3000, ForceMode.Impulse);

    }
}
