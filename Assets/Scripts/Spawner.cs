using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float minSpawnDelay = 1.5f, maxSpawnDelay = 2.5f;

    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();
    private float spawnDelay = 1.5f;

    private void Awake()
    {
        foreach (Transform t in GetComponentsInChildren<Transform>())
        { 
            spawnPoints.Add(t);
        }

        spawnPoints.Remove(transform);
    }

    void Start()
    {
        StartCoroutine(StartSpawning());
    }

    private IEnumerator StartSpawning()
    {
        spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(spawnDelay);

        bool full = false;

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnPoints[i].childCount < 1)
            {
                break;
            }

            if (i == spawnPoints.Count - 1)
            {
                full = true;
            }
        }

        bool spawned = false;
        int indexToSpawn = 0;

        while (!spawned && !full)
        {
            int ran = Random.Range(0, spawnPoints.Count);

            if (spawnPoints[ran].childCount < 1)
            { 
                spawned = true;
                indexToSpawn = ran;
            }
        }

        if (!full)
        {
            SpawnObjectAt(spawnPoints[indexToSpawn]);
        }

        StartCoroutine(StartSpawning());
    }

    private void SpawnObjectAt(Transform spawnPoint)
    {
        GameObject obj = Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        obj.transform.parent = spawnPoint.transform;
    }
}
