using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GraphicRaycaster;

public class ObjectTapped : MonoBehaviour
{
    private Spawner spawner;
    private Transform spawnPoint;

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool stopTouch = false;
    private float swipeRange = 300.0f;

    public void Initialize(Spawner spawner, Transform spawnPoint)
    {
        this.spawner = spawner;
        this.spawnPoint = spawnPoint;
    }

    void Update()
    {
        Swipe();
    }

    void Swipe()
    {
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
                            // Destroy the game object this script is attached to
                            Destroy(gameObject);
                            spawner.ObjectDestroyed(spawnPoint);
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
    }
}
