using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BiggieDisrupt : MonoBehaviour
{
    public GameObject screenDisrupt;
    public float blockDuration = 2.0f, fadeDuration = 1.0f;
    public int despawnTime = 4;
 
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool stopTouch = false, despawning = false;

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
            StartCoroutine(DisruptScreen());
        }
    }


    private IEnumerator DisruptScreen()
    {
        ScoreManager.instance.currentCombo = 0;
        screenDisrupt.GetComponent<Image>().color = Color.red;
        Image image = screenDisrupt.GetComponent<Image>();

        if (image != null)
        {
            Color color = image.color;
            color.a = 1f;
            image.color = color;
        }

        yield return new WaitForSeconds(blockDuration);

        if (image != null)
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                Color color = image.color;
                color.a = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                image.color = color;
                yield return null;
            }

            Color finalColor = image.color;
            finalColor.a = 0f;
            image.color = finalColor;
        }

        screenDisrupt.GetComponent<Image>().color = Color.clear;
        RemovePlant();
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
