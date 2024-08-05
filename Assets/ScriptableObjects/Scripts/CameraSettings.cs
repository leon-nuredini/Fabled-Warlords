using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Settings/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    public Vector2 minBounds;
    public Vector2 maxBounds;
    public float minCamSize;
    public float maxCamSize;
    public float zoomSpeed;
}
