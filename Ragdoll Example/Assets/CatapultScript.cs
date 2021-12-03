using UnityEngine;
using PlayerScripts;
using Interactions;

public class CatapultScript : MonoBehaviour
{
    Vector3 forceDirection;
    // Start is called before the first frame update
    void Start()
    {
        forceDirection = (transform.Find("endPoint").gameObject.transform.position - transform.Find("startingPoint").gameObject.transform.position).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("entry");
        if (other.gameObject.GetComponent<PlayerController>())
        {
            Debug.Log(forceDirection);
            other.gameObject.GetComponent<ForceSimulator>().AddImpact(forceDirection, 500);
            other.gameObject.GetComponent<BetterMovement>().flyRagdoll(other.gameObject, 10);
        }
        
        if (other.gameObject.GetComponent<Throwable>())
        {
            // Debug.Log("entry2" + forceDirection);
            other.gameObject.GetComponent<Rigidbody>().AddForce(3000*forceDirection, ForceMode.Impulse);
        }

        if (other.gameObject.GetComponent<EnemyController>())
        {
            Debug.Log(forceDirection);
            
            other.gameObject.GetComponent<EnemyController>().getCatapulted(forceDirection);
            //other.gameObject.GetComponent<Rigidbody>().AddForce(800 * forceDirection, ForceMode.Impulse);
        }
    }
}
