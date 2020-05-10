using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;

    //Settings for camera
    public float zoomSensitivity;

    void Update()
    {
        float camZoom = mainCamera.orthographicSize;
        camZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        mainCamera.orthographicSize = camZoom;
    }
}
