using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TappyAddTime : MonoBehaviour
{
    public Timer timer;
    public float increaseAmount;
    public int despawnTime = 4; 

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool stopTouch = false, despawning = false;

    public GameObject pluckedSprite;
    public float explosionForce = 12f;
    public float pluckedLifetime = 5f;
    public Sprite tappySprite;

    private void Awake()
    {
        timer = GameObject.Find("Timer").GetComponent<Timer>();
    }

    private void Start()
    {
        StartCoroutine(KillCountdown(despawnTime));
        Spawner.instance.listOfPlants[2].plantSpawnChance = 0f;
        ScoreManager.instance.pluckCounter = 0;
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
                            IncreaseTimer();
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
                        IncreaseTimer();
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

    void IncreaseTimer()
    {
        if (!despawning)
        {
            StopAllCoroutines();
            SoundManager.instance.Play("TimeGain");
            timer.timeRemaining = Mathf.Min(timer.timeRemaining + increaseAmount, 30f);
            Invoke("RemovePlant", 0.25f);
            spawnPluckedSprite();
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
            spriteRenderer.sprite = tappySprite;
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
        ScoreManager.instance.currentCombo = 0;
        RemovePlant();
    }

    public void RemovePlant()
    {
        Destroy(gameObject);
    }
}

