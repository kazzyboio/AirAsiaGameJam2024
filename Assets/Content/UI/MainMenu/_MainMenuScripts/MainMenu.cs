using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public float freezeInputSeconds = 0f;
    private bool acceptInputs = false;

    void Start()
    {
        StartCoroutine(unFreezeInput());
    }

    IEnumerator unFreezeInput()
    {
        yield return new WaitForSeconds(freezeInputSeconds);
        acceptInputs = true;
    }

    void Update()
    {
        if (acceptInputs == true && Input.anyKeyDown)
        {
            SceneChanger sceneChanger = FindObjectOfType<SceneChanger>();
            if (sceneChanger != null)
            {
                sceneChanger.ChangeScene("GameScene");
                FindObjectOfType<SoundManager>().Play("MenuTap");
            }
        }
    }
}
