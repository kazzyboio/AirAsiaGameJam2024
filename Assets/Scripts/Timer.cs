using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeRemaining = 30f;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject gameOverCanvas;

    private void Start()
    {
        gameOverCanvas.SetActive(false);
        UpdateTimeText();
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0) timeRemaining = 0; 
            UpdateTimeText();
        }
        else
        {
            gameOverCanvas.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void UpdateTimeText()
    {
        timeText.text = Mathf.CeilToInt(timeRemaining).ToString();
    }
}
