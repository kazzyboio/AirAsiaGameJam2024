using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager instance;
    static public GameObject scoreManagerObject;

    [HideInInspector]
    public float currentCombo = 0, highCombo = 0, recordCombo = 0, feverMultiplyer = 1;
    [HideInInspector]
    public int pluckCounter = 0, totalScore = 0, highScore = 0;

    [SerializeField]
    private TextMeshProUGUI scoreText, comboText, feverText;
    [SerializeField]
    private GameObject scoreContainer, comboContainer;
    public Animator feverTextAnim;
    [SerializeField]
    private float comboMultiplyer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (scoreManagerObject == null)
        {
            scoreManagerObject = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
            feverText.text = "FEVER MODE";
            feverTextAnim.Play("FeverAnim");
        }

        if (totalScore >= highScore)
        { 
            highScore = totalScore;
        }

        if (currentCombo >= highCombo)
        {
            highCombo = currentCombo;
        }

        if (highCombo >= recordCombo)
        {
            recordCombo = highCombo;
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

    public void EnableVisibility()
    { 
        feverText.enabled = true;
        scoreContainer.SetActive(true);
        comboContainer.SetActive(true);
    }

    public void DisableVisibility()
    {
        feverText.enabled = false;
        scoreContainer.SetActive(false);
        comboContainer.SetActive(false);
    }

    public void OnNewGameStart()
    {
        EnableVisibility();
        currentCombo = 0;
        highCombo = 0;
        feverMultiplyer = 1;
        totalScore = 0;
        pluckCounter = 0;
    }
}
