using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject objectToSpawn;
    public float respawnDelay = 2f;


    void Start()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnObjectAt(spawnPoint);
        }
    }

    public void ObjectDestroyed(Transform spawnPoint)
    {
        StartCoroutine(RespawnAfterDelay(spawnPoint));
    }

    private IEnumerator RespawnAfterDelay(Transform spawnPoint)
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnObjectAt(spawnPoint);
    }

    private void SpawnObjectAt(Transform spawnPoint)
    {
        GameObject obj = Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        ObjectTapped tapToDisappear = obj.GetComponent<ObjectTapped>();
        if (tapToDisappear != null)
        {
            tapToDisappear.Initialize(this, spawnPoint);
        }
    }
}
