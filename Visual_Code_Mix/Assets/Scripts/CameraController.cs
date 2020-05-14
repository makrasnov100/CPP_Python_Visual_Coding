using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;

    //Settings for camera
    public float zoomSensitivity;

    void Update()
    {
        float camZoom = mainCamera.orthographicSize;
        if(Input.GetAxis("Mouse ScrollWheel") != 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            camZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
            mainCamera.orthographicSize = camZoom;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            initPos = getCameraWorldPos();
            isNodeClicked = checkNodeHit();
        }

        if (!isNodeClicked && Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            UpdateCameraDrag();
    }


    //CAMERA MOVEMENT
    //settings
    public float cameraMovementSmoothing;
    //instance variables
    bool isNodeClicked = false;
    Vector3 initPos = Vector3.zero;
    Vector3 targetPos = Vector3.zero;
    void UpdateCameraDrag()
    {
        targetPos = getCameraWorldPos();
        Vector3 changePos = initPos - targetPos;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, mainCamera.transform.position + changePos, cameraMovementSmoothing);
    }

    Vector3 getCameraWorldPos()
    {
        Vector3 newPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        return new Vector3(newPos.x, newPos.y, mainCamera.transform.position.z);
    }

    bool checkNodeHit()
    {
        Vector3 groundPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        Vector3 mouseFloatPos = new Vector3(groundPos.x, groundPos.y, -10);
        Ray ray = new Ray(mouseFloatPos, Vector3.forward);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 50);

        //Draw debug position (if needed)
        //Instantiate(debugTarget, groundPos, Quaternion.identity, gameObject.transform);

        if (hit.collider != null)
            return true;
        else
            return false;
    }
}
