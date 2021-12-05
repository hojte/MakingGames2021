using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public List<GameObject> itemsToSpawn = new List<GameObject>();
    public float timeBetweenSpawns = 3.5f;

    private float _counter = 0;
    void Update()
    {
        _counter += Time.deltaTime;
        if(!(_counter > timeBetweenSpawns) || itemsToSpawn.Count == 0) return;
        var itemToSpawn = itemsToSpawn[new System.Random().Next(itemsToSpawn.Count)];
        Instantiate(itemToSpawn, transform.position, Quaternion.identity);
        _counter = 0;
    }
}
