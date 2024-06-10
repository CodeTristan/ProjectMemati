using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    [SerializeField] public float cameraSpeed = 9;

    void Update()
    {
        transform.Translate(0, 0, cameraSpeed * Time.deltaTime);
    }
}
