using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{

    [SerializeField] GameObject[] clouds;
    [SerializeField] GameObject endPoint;
    [SerializeField] float minSpawnInterval = 2f;
    [SerializeField] float maxSpawnInterval = 5f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        spawnCloud();
        ScheduleNextSpawn();
    }

    void spawnCloud()
    {
        int randomIndex = UnityEngine.Random.Range(0, clouds.Length);
        GameObject cloud = Instantiate(clouds[randomIndex]);

        float startY = UnityEngine.Random.Range(startPos.y - 3f, startPos.y + 1f);
        cloud.transform.position = new Vector3(startPos.x, startY, startPos.z);

        float scale = UnityEngine.Random.Range(0.8f, 1.2f);
        cloud.transform.localScale = new Vector2(scale, scale);

        float speed = UnityEngine.Random.Range(0.5f, 1.5f);
        cloud.GetComponent<Cloud>().startFloating(speed, endPoint.transform.position.x);
    }

    void ScheduleNextSpawn()
    {
        float randomInterval = UnityEngine.Random.Range(minSpawnInterval, maxSpawnInterval);
        Invoke("AttemptSpawn", randomInterval);
    }

    void AttemptSpawn()
    {
        spawnCloud();
        ScheduleNextSpawn();
    }
}
