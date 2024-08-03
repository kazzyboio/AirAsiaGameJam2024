using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    static public Spawner instance;
    public float minSpawnDelay = 1.5f, maxSpawnDelay = 2.5f;
    public List<Plant> listOfPlants = new List<Plant>();

    [SerializeField]
    private int tappyPluckStreakRequirement = 10;
    [SerializeField]
    private float oguComboRequirement = 20;
    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();
    private float spawnDelay = 1.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        foreach (Transform t in GetComponentsInChildren<Transform>())
        { 
            spawnPoints.Add(t);
        }

        spawnPoints.Remove(transform);
    }

    private void Start()
    {
        StartCoroutine(StartSpawning());
        StartCoroutine(StartSpawning());
        StartCoroutine(StartSpawning());
        StartCoroutine(SpawningAmp());
    }

    private void Update()
    {
        // Tappy spawn rates (Timer increase)
        if (ScoreManager.instance.pluckCounter >= tappyPluckStreakRequirement)
        {
            listOfPlants[2].plantSpawnChance = 0.15f + (0.02f * (float)(ScoreManager.instance.pluckCounter - tappyPluckStreakRequirement));
        }

        // Ogu spawn rates (Fever mode)
        if (ScoreManager.instance.currentCombo % oguComboRequirement == 0.0f && ScoreManager.instance.currentCombo != 0.0f)
        {
            listOfPlants[3].plantSpawnChance = 0.3f;
        }
    }

    private GameObject DetermineSpawnedPlant()
    {
        listOfPlants[0].plantSpawnChance = 1f;

        foreach (Plant plant in listOfPlants)
        {
            if (plant.plantName == "Normal")
            {
                continue;
            }

            listOfPlants[0].plantSpawnChance -= plant.plantSpawnChance;
        }

        float ran = Random.Range(0f, 1f);
        float cumulativeChance = 0f;
        GameObject plantToSpawn = null;

        foreach (Plant plant in listOfPlants)
        {
            cumulativeChance += plant.plantSpawnChance;

            if (ran <= cumulativeChance)
            {
                plantToSpawn = plant.plantPrefab;
                break;
            }
        }

        return plantToSpawn;
    }


    private IEnumerator SpawningAmp()
    {
        yield return new WaitForSeconds(10f);
        StartCoroutine(StartSpawning());
        StartCoroutine(SpawningAmper());
    }

    private IEnumerator SpawningAmper()
    {
        yield return new WaitForSeconds(10f);
        StartCoroutine(StartSpawning());
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
        GameObject obj = Instantiate(DetermineSpawnedPlant(), spawnPoint.position + new Vector3(0f, 0.4f), spawnPoint.rotation);
        obj.transform.parent = spawnPoint.transform;
    }
}

[System.Serializable]
public class Plant
{
    public string plantName;
    public GameObject plantPrefab;
    public float plantSpawnChance;
}