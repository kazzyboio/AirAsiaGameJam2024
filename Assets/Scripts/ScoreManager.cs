using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager instance;

    [HideInInspector]
    public int currentCombo = 1;

    [SerializeField]
    private TextMeshProUGUI scoreText, comboText;
    private int totalScore;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        scoreText.text = totalScore.ToString();
        comboText.text = currentCombo.ToString();
    }

    public void AddToScore(int score)
    { 
        totalScore += score * currentCombo;
    }
}
