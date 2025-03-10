using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Timer : MonoBehaviour
{
    [Range (0f, 30f)]
    [SerializeField] public float timeRemaining = 30f;
    [SerializeField] public Slider timerSlider;
    [SerializeField] public Image timerFillImage;
    [SerializeField] public Color[] fillColors;
    [SerializeField] public Animator anim;
    [SerializeField] public float timesUpTime;

    private bool stopTimer = false;
    private float initialTime;
    private float t = 0f;

    private int startColorIndex = 2;
    private int endColorIndex = 1;
    private int currentPhase = 2;

    void Start()
    {
        initialTime = timeRemaining;
        timerSlider.maxValue = initialTime;
        timerSlider.value = timeRemaining;
        timerFillImage.color = fillColors[startColorIndex];
        anim.Play("Default");
        startTimer();
    }

    public void startTimer()
    {
        StartCoroutine(startTimerTick());
    }

    IEnumerator startTimerTick()
    {
        bool isShaking = false; 

        while (!stopTimer)
        {
            timeRemaining -= Time.deltaTime;

            if (currentPhase == 2 && timeRemaining <= (2 * initialTime / 3))
            {
                startColorIndex = 2;
                endColorIndex = 1;
                currentPhase = 1;
                t = 0f;
                if (!isShaking)
                {
                    anim.Play("Default");
                    isShaking = false;
                }
            }
            else if (currentPhase == 1 && timeRemaining <= (initialTime / 3))
            {
                startColorIndex = 1;
                endColorIndex = 0;
                currentPhase = 0;
                t = 0f;
                if (!isShaking)
                {
                    anim.Play("Shake");
                    isShaking = true;
                }
            }
            else if (currentPhase == 0 && timeRemaining > (initialTime / 3))
            {
                startColorIndex = 2;
                endColorIndex = 1;
                currentPhase = 2;
                if (isShaking)
                {
                    anim.Play("Default");
                    isShaking = false;
                }
            }

            if (currentPhase == 2 || currentPhase == 1)
            {
                t = Mathf.Clamp01(1 - (timeRemaining - (initialTime / 3)) / (initialTime / 3));
            }
            else
            {
                t = Mathf.Clamp01(1 - timeRemaining / (initialTime / 3));
            }

            timerFillImage.color = Color.Lerp(fillColors[startColorIndex], fillColors[endColorIndex], t);

            if (timeRemaining <= 0)
            {
                stopTimer = true;
                timeRemaining = 0;
                anim.Play("Default");
                anim.Play("TimesUp");
                FindObjectOfType<SoundManager>().Play("Whistle");
                StartCoroutine(SwitchScene());
            }

            IEnumerator SwitchScene()
            {
                yield return new WaitForSeconds(timesUpTime);

                ScoreManager.instance.DisableVisibility();

                SceneChanger sceneChanger = FindObjectOfType<SceneChanger>();
                if (sceneChanger != null)
                {
                    sceneChanger.ChangeScene("EndScene");
                }
                
            }

            timerSlider.value = timeRemaining;

            yield return null;
        }
    }
}
