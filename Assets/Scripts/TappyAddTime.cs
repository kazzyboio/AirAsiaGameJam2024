using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TappyAddTime : MonoBehaviour
{
    public Timer timer;
    public float increaseAmount = 5f;
    public int despawnTime = 4; 

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool stopTouch = false, despawning = false;
    private float swipeRange = 150.0f;

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
                    if (distance.y > swipeRange)
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
                if (distance.y > swipeRange)
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
            timer.timeRemaining = Mathf.Min(timer.timeRemaining + increaseAmount, 30f);
            Invoke("RemovePlant", 0.25f);
        }
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

