using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Transform transformPlayer;

    private void Awake()
    {
        transformPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        Vector3 positionCamera = transform.position;
        positionCamera.x = Mathf.Max(positionCamera.x, transformPlayer.position.x);
        transform.position = positionCamera;
    }
}
