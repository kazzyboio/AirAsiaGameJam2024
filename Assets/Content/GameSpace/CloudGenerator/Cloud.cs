using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float _speed;
    public float _endPosX;

    public void startFloating(float speed, float endPosX)
    {
        _speed = speed;
        _endPosX = endPosX;
    }

    void Update()
    {
        transform.Translate(Vector3.left * (Time.deltaTime * _speed));

        if(transform.position.x < _endPosX)
        {
            Destroy(gameObject);
        }
    }
}
