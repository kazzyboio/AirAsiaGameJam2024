using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowing : MonoBehaviour
{
    [HideInInspector]
    public int givenScore = 0;
    [HideInInspector]
    public string currentStage;

    [SerializeField]
    private int bloomTime = 5, wiltTime = 3, sproutScore = 100, bloomedScore = 300;
    private SpriteRenderer sprite;
    private string[] growthStages = new string[3] {"Sprouting", "Blooming", "Wilting"};

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(BloomCountdown(bloomTime));
        givenScore = sproutScore;
        sprite.color = Color.green;
        currentStage = growthStages[0];
    }

    IEnumerator BloomCountdown(int seconds)
    {
        int counter = seconds;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }
        sprite.color = Color.red;
        givenScore = sproutScore;
        currentStage = growthStages[1];
        StartCoroutine(WiltCountdown(wiltTime));
    }

    IEnumerator WiltCountdown(int seconds)
    {
        int counter = seconds;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }
        sprite.color = Color.black;
        givenScore = 0;
        currentStage = growthStages[2];
        Invoke("Wilt", 3);
    }

    public void Wilt()
    {
        Destroy(gameObject);
    }
}
