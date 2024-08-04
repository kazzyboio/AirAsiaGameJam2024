using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public void restartButtonPressed()
    {

        SceneChanger sceneChanger = FindObjectOfType<SceneChanger>();
        if (sceneChanger != null)
        {
            SoundManager.instance.Play("MenuTap");
            sceneChanger.ChangeScene("GameScene");
        }

    }

    public void homeButtonPressed()
    {

        SceneChanger sceneChanger = FindObjectOfType<SceneChanger>();
        if (sceneChanger != null)
        {
            SoundManager.instance.Play("MenuTap");
            sceneChanger.ChangeScene("StartScene");
        }

    }

}
