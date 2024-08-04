using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideMenu : MonoBehaviour
{
    public void closeButton()
    {
        SoundManager.instance.Play("MenuTap");
        gameObject.SetActive(false);
    }
}
