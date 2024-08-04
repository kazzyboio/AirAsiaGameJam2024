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
    public GameObject pluckedSprite;
    public float explosionForce = 10f;
    public float pluckedLifetime = 10f;
    public Sprite bloomSprite; 
    public Sprite sproutSprite; 
    public Sprite wiltSprite;

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
        ScoreManager.instance.currentCombo = 0;
        RemovePlant();
    }

    public void HarvestPlant()
    {
        if (currentStage != "Died" && currentStage != "Harvested")
        {
            StopAllCoroutines();
            CancelInvoke("RemovePlant");
            spawnPluckedSprite();
            SoundManager.instance.PlayHarvestSound();

            ScoreManager.instance.AddToScore(givenScore);
            if (currentStage == "Blooming")
            {
                ScoreManager.instance.currentCombo += 1;
                ScoreManager.instance.AddToPluckCounter();
            }
            else 
            {
                ScoreManager.instance.currentCombo = 0;

                if (currentStage == "Wilting")
                {
                    SoundManager.instance.Play("Wilted");
                }
                else if (currentStage == "Sprouting")
                {
                    SoundManager.instance.Play("Sprouting");
                }
            }
            currentStage = "Harvested";
            Invoke("RemovePlant", 0.1f);
        }
    }

    public void spawnPluckedSprite()
    {
        GameObject spawnedSprite = Instantiate(pluckedSprite, transform.position, Quaternion.identity);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        float randomAngle = Random.Range(80f, 100f);
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));

        SpriteRenderer spriteRenderer = spawnedSprite.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            switch (currentStage.ToLower())
            {
                case "blooming":
                    spriteRenderer.sprite = bloomSprite;
                    break;

                case "sprouting":
                    spriteRenderer.sprite = sproutSprite;
                    break; 

                case "wilting":
                    spriteRenderer.sprite = wiltSprite;
                    break;

            }
        }

        Rigidbody2D rb = spawnedSprite.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 force = direction * explosionForce;
            rb.velocity = force;
            StartCoroutine(ApplyGravity(rb));
        }

        Destroy(spawnedSprite, pluckedLifetime);
    }

    private IEnumerator ApplyGravity(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(0.05f); 
        rb.gravityScale = 2f; 
    }


    public void RemovePlant()
    {
        Destroy(gameObject);
    }
}
