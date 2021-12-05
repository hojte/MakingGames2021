using System.Collections.Generic;
using UnityEngine;

public class TowerSpawnerController : MonoBehaviour
{
    public List<GameObject> itemsToSpawn = new List<GameObject>();
    public float timeBetweenSpawns = 3.5f;
    private float _counter = 0;
    [HideInInspector]
    public bool startSpawning = false;
    void Update()
    {
        if (startSpawning == true)
        {
            _counter += Time.deltaTime;
            if (!(_counter > timeBetweenSpawns) || itemsToSpawn.Count == 0) return;
            var itemToSpawn = itemsToSpawn[new System.Random().Next(itemsToSpawn.Count)];
            Instantiate(itemToSpawn, transform.position, Quaternion.identity);
            gameObject.GetComponent<ppLinker>().numSpawned += 1;
            if (gameObject.GetComponent<ppLinker>().numSpawned >= gameObject.GetComponent<ppLinker>().maxToSpawn)
            {
                startSpawning = false;
                return;
            }
            _counter = 0;
        }
    }
}
