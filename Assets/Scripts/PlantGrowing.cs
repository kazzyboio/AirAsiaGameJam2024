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
    private int sproutTime = 1, bloomTime = 2, wiltTime = 1, sproutScore = 100, bloomedScore = 300, wiltedScore = 100;
    private SpriteRenderer sprite;
    private string[] growthStages = new string[5] {"Sprouting", "Blooming", "Wilting", "Harvested", "Died"};

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(BloomCountdown(sproutTime));
        givenScore = sproutScore;
        currentStage = "Sprouting";
    }

    IEnumerator BloomCountdown(int seconds)
    {
        int counter = seconds;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }
        currentStage = "Blooming";
        givenScore = bloomedScore;
        StartCoroutine(WiltCountdown(bloomTime));
    }

    IEnumerator WiltCountdown(int seconds)
    {
        int counter = seconds;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }
        currentStage = "Wilting";
        givenScore = wiltedScore;
        // play wilt animation
        StartCoroutine(KillCountdown(wiltTime));
    }

    IEnumerator KillCountdown(int seconds)
    {
        int counter = seconds;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }
        currentStage = "Died";
        yield return new WaitForSeconds(0.25f);
        ScoreManager.instance.currentCombo = 1;
        RemovePlant();
    }

    public void HarvestPlant()
    {
        if (currentStage != "Died" && currentStage != "Harvested")
        {
            StopAllCoroutines();
            CancelInvoke("RemovePlant");

            if (currentStage == "Blooming")
            {
                ScoreManager.instance.currentCombo += 1;
            }
            else
            {
                ScoreManager.instance.currentCombo = 1;
            }
            ScoreManager.instance.AddToScore(givenScore);
            //play harvest animation
            currentStage = "Harvested";
            Invoke("RemovePlant", 0.25f);
        }
    }

    public void RemovePlant()
    {
        Destroy(gameObject);
    }
}
