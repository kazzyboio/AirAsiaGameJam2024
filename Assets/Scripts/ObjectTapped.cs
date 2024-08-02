using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTapped : MonoBehaviour
{
    void OnMouseDown()
    {
        // Destroy the game object this script is attached to
        Destroy(gameObject);
    }
}
