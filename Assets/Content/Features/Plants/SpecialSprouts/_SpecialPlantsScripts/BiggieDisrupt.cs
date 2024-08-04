using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BiggieDisrupt : MonoBehaviour
{
    public GameObject screenDisrupt;
    public float blockDuration = 1.5f;
    public int despawnTime = 4;
 
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool stopTouch = false, despawning = false;

    public GameObject pluckedSprite;
    public float explosionForce = 12f;
    public float pluckedLifetime = 5f;
    public Sprite biggieSprite;

    private void Awake()
    {
        screenDisrupt = GameObject.Find("BiggieScreenDisrupt");
    }

    private void Start()
    {
        StartCoroutine(KillCountdown(despawnTime));
        Spawner.instance.listOfPlants[1].plantSpawnChance = 0f;
        screenDisrupt.GetComponent<Image>().color = Color.clear;
    }

    void Update()
    {
        Swipe();
    }

    void Swipe()
    {
        // Touch Input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                currentTouchPosition = touch.position;
                Vector2 distance = currentTouchPosition - startTouchPosition;

                if (!stopTouch)
                {

                    float normalizedDistance = distance.y / Screen.height;

                    // Higher value for mobile input
                    if (normalizedDistance > 250.0f)
                    {
                        Vector2 touchWorldPosition = Camera.main.ScreenToWorldPoint(startTouchPosition);
                        RaycastHit2D hit = Physics2D.Raycast(touchWorldPosition, Vector2.zero);

                        if (hit.collider != null && hit.collider.transform == transform)
                        {
                            StartDisrupt();
                            stopTouch = true;
                        }
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                stopTouch = false;
                startTouchPosition = currentTouchPosition = Vector2.zero;
            }
        }

        // Mouse Input
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            currentTouchPosition = Input.mousePosition;
            Vector2 distance = currentTouchPosition - startTouchPosition;

            if (!stopTouch)
            {
                float normalizedDistance = distance.y / Screen.height;

                // Lower value for mouse input
                if (normalizedDistance > 0.1f)
                {
                    Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(startTouchPosition);
                    RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);

                    if (hit.collider != null && hit.collider.transform == transform)
                    {
                        StartDisrupt();
                        stopTouch = true;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            stopTouch = false;
            startTouchPosition = currentTouchPosition = Vector2.zero;
        }
    }

    private void StartDisrupt()
    {
        if (!despawning)
        {
            despawning = true;
            StopAllCoroutines();
            SoundManager.instance.Play("Hehehe");
            StartCoroutine(DisruptScreen());
            spawnPluckedSprite();
        }
    }


    private IEnumerator DisruptScreen()
    {
        ScoreManager.instance.currentCombo = 0;
        Animator animator = screenDisrupt.GetComponent<Animator>();

        if (animator != null)
        {
            animator.Play("Splat");
        }

        yield return new WaitForSeconds(blockDuration);

        if (animator != null)
        {
            animator.Play("SlideOut");
        }

        RemovePlant();
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
            spriteRenderer.sprite = biggieSprite;
        }

        Rigidbody2D rb = spawnedSprite.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 force = direction * explosionForce;
            rb.velocity = force;
        }

        Destroy(spawnedSprite, pluckedLifetime);

    }

    IEnumerator KillCountdown(int seconds)
    {
        int counter = seconds;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }
        despawning = true;
        yield return new WaitForSeconds(0.25f);
        RemovePlant();
    }

    public void RemovePlant()
    {
        Spawner.instance.listOfPlants[1].plantSpawnChance = 0.1f;
        Destroy(gameObject);
    }
}
