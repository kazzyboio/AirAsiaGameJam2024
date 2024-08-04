using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public float freezeInputSeconds = 0f;
    private bool acceptInputs = false;
    [SerializeField] public GameObject GuideMenu = null;

    void Start()
    {
        StartCoroutine(unFreezeInput());
        GuideMenu.SetActive(false);
    }

    IEnumerator unFreezeInput()
    {
        yield return new WaitForSeconds(freezeInputSeconds);
        acceptInputs = true;
    }

    public void playButtonPressed()
    {
        if (acceptInputs == true)
        {
            SceneChanger sceneChanger = FindObjectOfType<SceneChanger>();
            if (sceneChanger != null)
            {
                sceneChanger.ChangeScene("GameScene");
            }
        }
        
    }

    public void guideButtonPressed()
    {
        if (acceptInputs == true)
        {
            GuideMenu.SetActive(true);
        }
    }
}
