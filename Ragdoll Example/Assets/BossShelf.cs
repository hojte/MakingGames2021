using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShelf : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("HeavyItem") || collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("test");
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(this.gameObject.GetComponent<Rigidbody>().velocity.x, 0, this.gameObject.GetComponent<Rigidbody>().velocity.z);

        }
    }
}