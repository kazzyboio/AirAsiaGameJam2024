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

    public Animator anim;
    public float fadeDuration;
    public float sceneLoadDelay = 1f;

    private FadeStatus currentFadeStatus = FadeStatus.none;
    private float fadeTimer;
    private string sceneToLoad;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Scene loaded, prepare to fade in
        if (anim != null)
        {
            anim.Play("FadeIn");
            currentFadeStatus = FadeStatus.fading_in;
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
