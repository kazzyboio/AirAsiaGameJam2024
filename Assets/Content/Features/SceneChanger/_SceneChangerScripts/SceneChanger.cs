using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    enum FadeStatus
    {
        fading_in,
        fading_out,
        none
    }

    public static SceneChanger Instance;
    public Animator anim; 
    public float fadeDuration;
    public float sceneLoadDelay = 1f;

    private FadeStatus currentFadeStatus = FadeStatus.none;
    private float fadeTimer;
    private string sceneToLoad;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Scene loaded, prepare to fade in
        if (anim != null)
        {
            anim.Play("FadeIn");
            currentFadeStatus = FadeStatus.fading_in;
        }
        else
        {
            Debug.LogWarning("Animator is null!");
        }
    }

    public void ChangeScene(string _name)
    {
        sceneToLoad = _name;
        currentFadeStatus = FadeStatus.fading_out;

        if (anim != null)
        {
            anim.Play("FadeOut");
            StartCoroutine(LoadSceneAfterFadeOut());
        }
    }

    IEnumerator LoadSceneAfterFadeOut()
    {
        yield return new WaitForSeconds(fadeDuration + sceneLoadDelay);
        SceneManager.LoadScene(sceneToLoad);
    }

    void Update()
    {
        if (currentFadeStatus == FadeStatus.none) return;

        fadeTimer += Time.deltaTime;

        if (fadeTimer > fadeDuration) // Done fading
        {
            fadeTimer = 0;

            if (currentFadeStatus == FadeStatus.fading_out)
            {
                SceneManager.LoadScene(sceneToLoad);
                currentFadeStatus = FadeStatus.none;
            }
            else if (currentFadeStatus == FadeStatus.fading_in)
            {
                currentFadeStatus = FadeStatus.none;
            }
        }
    }
}
