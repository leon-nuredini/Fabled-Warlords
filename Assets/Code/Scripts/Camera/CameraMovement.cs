using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 _mouseScrollStartPos;
    private Camera _myCamera;
    private int _borderSize;

    private void Start()
    {
        _myCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleMiddleMouseScrolling();
    }

    private void HandleMiddleMouseScrolling()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _mouseScrollStartPos = _myCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2))
        {
            var movement = Vector3.zero;
            movement = _myCamera.ScreenToWorldPoint(Input.mousePosition) - _mouseScrollStartPos;
            _myCamera.transform.position -= movement;
        }
    }
}
