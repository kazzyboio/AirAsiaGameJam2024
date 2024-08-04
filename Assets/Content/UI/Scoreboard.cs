using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI totalScoreText, highScoreText, highComboText, recordComboText;
    [SerializeField]
    private Texture2D lowScoreImage, highScoreImage;
    [SerializeField]
    private RawImage displayedImage;
    [SerializeField]
    private float scoreThreshold = 10000;

    private void Update()
    {
        totalScoreText.text = "Your Score: " + ScoreManager.instance.totalScore;
        highComboText.text = "Your Highest Combo: " + ScoreManager.instance.highCombo;
        highScoreText.text = "High Score: " + ScoreManager.instance.highScore;
        recordComboText.text = "Record Highest Combo: " + ScoreManager.instance.recordCombo;

        if (ScoreManager.instance.totalScore >= scoreThreshold)
        {
            displayedImage.texture = highScoreImage;
            displayedImage.SetNativeSize();
        }
        else
        {
            displayedImage.texture = lowScoreImage;
            displayedImage.SetNativeSize();
        }    
    }

    public void RestartGame()
    {
        SceneChanger sceneChanger = FindObjectOfType<SceneChanger>();
        if (sceneChanger != null)
        {
            sceneChanger.ChangeScene("StartScene");
            FindObjectOfType<SoundManager>().Play("MenuTap");
        }
    }
}
