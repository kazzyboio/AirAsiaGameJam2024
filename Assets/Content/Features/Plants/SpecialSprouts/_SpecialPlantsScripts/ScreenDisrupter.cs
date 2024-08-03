using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenDisrupter : MonoBehaviour
{
    public static ScreenDisrupter Instance; 

    public GameObject screenDisrupt;
    public float fadeDuration = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        screenDisrupt.SetActive(false);
    }

    public void ShowDisruptScreen(float blockDuration)
    {
        StartCoroutine(DisruptScreen(blockDuration));
    }

    private IEnumerator DisruptScreen(float blockDuration)
    {
        screenDisrupt.SetActive(true);
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

        screenDisrupt.SetActive(false);
    }
}
