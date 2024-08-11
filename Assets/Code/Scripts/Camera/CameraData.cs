using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CameraData", menuName = "CameraData/CameraSettings")]
public class CameraData : ScriptableObject
{
    [Header("Drag")]
    public bool dragEnabled;
    public float dragSpeed = -0.06f;
    public TwoDCameraDrag.MouseButton mouseButton;
    [Header("Edge Scroll")] 
    public bool enableEdgeScroll;
    public int edgeBoundary = 20;

    [Range(0, 10)] [Tooltip("Speed the camera moves Mouse enters screen edge.")]
    public float edgeSpeed = 1f;

    [Header("Keyboard Input")]
    public bool keyboardInput;
    public bool inverseKeyboard;
    [Header("Zoom")] 
    public bool zoomEnabled;
    public float maxZoom;
    public float minZoom;
    [Range(0.01f,1f)]
    public float mouseScrollStepSize = 0.15f;
    [Range(0.01f,1f)]
    public float keyboardScrollStepSize = 0.15f;
    public bool linkedZoomDrag;
    public bool zoomToMouse;
    [Header("Zoom")]
    public bool clampCamera;

    [Header("Bounds")] 
    public float cameraBoundsX;
    public float cameraBoundsY;
    public Vector3 boundaryObjectPosition;
}