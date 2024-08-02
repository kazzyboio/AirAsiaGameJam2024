using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTapped : MonoBehaviour
{
    private Spawner spawner;
    private Transform spawnPoint;

    public void Initialize(Spawner spawner, Transform spawnPoint)
    {
        this.spawner = spawner;
        this.spawnPoint = spawnPoint;
    }

    void OnMouseDown()
    {
        spawner.ObjectDestroyed(spawnPoint);
        gameObject.SetActive(false);
    }
}
