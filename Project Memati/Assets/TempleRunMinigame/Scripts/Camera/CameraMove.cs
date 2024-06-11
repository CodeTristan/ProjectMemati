using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    public float cameraSpeed;

    public void Init()
    {
        startMovement = true;
    }

    bool startMovement = false;
    void Update()
    {
        if(startMovement)
            transform.Translate(0, 0, cameraSpeed * Time.deltaTime);
    }
}
