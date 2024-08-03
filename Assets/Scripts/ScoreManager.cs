using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager instance;

    [HideInInspector]
    public float currentCombo = 0, feverMultiplyer = 1;
    [HideInInspector]
    public int pluckCounter = 0;

    [SerializeField]
    private TextMeshProUGUI scoreText, comboText, feverText;
    [SerializeField]
    private float comboMultiplyer;
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

        if (feverMultiplyer == 1)
        {
            feverText.text = " ";
        }
        else
        {
            feverText.text = "HELL YEAH WE GOONING!!!";
        }
    }

    public void AddToScore(float score)
    {
        totalScore += (int)(score * (1 + (comboMultiplyer * currentCombo)) * feverMultiplyer);
    }

    public void AddToPluckCounter()
    {
        pluckCounter += 1;
    }
}
