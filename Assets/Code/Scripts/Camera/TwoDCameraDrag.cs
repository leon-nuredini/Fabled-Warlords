using System;
using Lean.Touch;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using CW.Common;

public class TwoDCameraDrag : MonoBehaviour
{
    public static event Action OnAnyMoveCameraLeft;
    public static event Action OnAnyMoveCameraRight;
    public static event Action OnAnyMoveCameraUp;
    public static event Action OnAnyMoveCameraDown;
    public static event Action OnAnyZoomCameraIn;
    public static event Action OnAnyZoomCameraOut;
    
    private Camera _cam;
    [Expandable]
    public CameraData cameraData;

    [Header("Camera Movement")] [HideInInspector]
    public Dc2dZoomTarget ztTarget;

    [HideInInspector]public bool dcTranslate = false;
    [HideInInspector] public bool dcZoom = false;
    [Range(0.1f, 10f)] [HideInInspector] public float dcZoomTargetIn = 4f;
    [Range(0.01f, 1f)] [HideInInspector] public float dcZoomTranslateSpeed = 0.5f;
    [Range(0.1f, 10f)] [HideInInspector] public float dcZoomTargetOut = 10f;
    [Range(0.01f, 1f)] [HideInInspector] public float dcZoomSpeed = 0.5f;
    private bool _dcZoomedOut = false;


    [HideInInspector] [Header("Follow Object")]
    public GameObject followTarget;

    [Range(0.01f, 1f)] [HideInInspector] public float lerpSpeed = 0.5f;
    [HideInInspector] public Vector3 offset = new Vector3(0, 0, -10);

    private  CameraBounds bounds;
    [HideInInspector] public Dc2dDolly dollyRail;


    public enum MouseButton
    {
        Left = 0,
        Middle = 2,
        Right = 1
    }

    // private vars
    private Vector3 _bl;
    private Vector3 _tr;

    [HideInInspector] public Dc2dSnapBox snapTarget;

    
    private void Start()
    {
        if (_cam != null) return;
        if (GetComponent<Camera>() != null)
        {
            _cam = GetComponent<Camera>();
        }

        if (Camera.main != null)
        {
            _cam = Camera.main;
        }
        
        AddCameraBounds();
    }

    private void LateUpdate()
    {
        if (cameraData.dragEnabled)
        {
            PanControl();
        }

        if (cameraData.enableEdgeScroll)
        {
            if (cameraData.edgeBoundary > 0)
            {
                EdgeScroll();
            }
        }


        if (cameraData.zoomEnabled)
        {
            ZoomControl();
        }

        if (snapTarget != null)
        {
            //using snap targets do da snap
            ConformToSnapTarget();
        }
        else
        {
            if (followTarget != null)
            {
                transform.position =
                    Vector3.Lerp(transform.position, followTarget.transform.position + offset, lerpSpeed);
            }
        }

        if (dcTranslate || dcZoom)
        {
            ZoomTarget();
        }

        if (cameraData.clampCamera)
        {
            CameraClamp();
        }


        if (dollyRail != null)
        {
            StickToDollyRail();
        }
    }

    private void ZoomTarget()
    {
        if (ztTarget == null)
        {
            throw new UnassignedReferenceException(
                "No Dc2dZoomTarget object. Please add one to your scene from the prefab folder, create an object with the Dc2dZoomTarget script or turn off Double Click zoom actions");
        }

        if (ztTarget.zoomToMe && dcTranslate)
        {
            Vector3 targetLoc = ztTarget.transform.position;
            targetLoc.z = offset.z; // lock ofset to cams offset
            transform.position = Vector3.Lerp(transform.position, targetLoc, 0.3f);
            if (ztTarget.zoomToMe && Vector3.Distance(transform.position, targetLoc) < 0.2f)
            {
                ztTarget.zoomToMe = false;
            }
        }

        if (dcZoom && !ztTarget.zoomComplete)
        {
            if (_dcZoomedOut)
            {
                _cam.orthographicSize = Mathf.Lerp(dcZoomTargetIn, _cam.orthographicSize, 0.1f);
                if (_cam.orthographicSize == dcZoomTargetIn)
                {
                    ztTarget.zoomComplete = true;
                    _dcZoomedOut = !_dcZoomedOut;
                }
            }
            else
            {
                _cam.orthographicSize = Mathf.Lerp(dcZoomTargetOut, _cam.orthographicSize, 0.1f);
                if (_cam.orthographicSize == dcZoomTargetOut)
                {
                    ztTarget.zoomComplete = true;
                    _dcZoomedOut = !_dcZoomedOut;
                }
            }
        }
    }

    private void EdgeScroll()
    {
        float x = 0;
        float y = 0;
        if (Input.mousePosition.x >= Screen.width - cameraData.edgeBoundary)
        {
            // Move the camera
            x = Time.deltaTime * cameraData.edgeSpeed;
        }

        if (Input.mousePosition.x <= 0 + cameraData.edgeBoundary)
        {
            // Move the camera
            x = Time.deltaTime * -cameraData.edgeSpeed;
        }

        if (Input.mousePosition.y >= Screen.height - cameraData.edgeBoundary)
        {
            // Move the camera
            y = Time.deltaTime * cameraData.edgeSpeed
                ;
        }

        if (Input.mousePosition.y <= 0 + cameraData.edgeBoundary)
        {
            // Move the camera
            y = Time.deltaTime * -cameraData.edgeSpeed
                ;
        }

        transform.Translate(x, y, 0);
    }

    public void AddCameraDolly()
    {
        if (dollyRail == null)
        {
            var go = new GameObject("Dolly");
            var dolly = go.AddComponent<Dc2dDolly>();

            var wp1 = new Dc2dWaypoint();
            wp1.position = new Vector3(0, 0, 0);

            var wp2 = new Dc2dWaypoint();
            wp2.position = new Vector3(1, 0, 0);

            var dc2dwaypoints = new Dc2dWaypoint[2];
            dc2dwaypoints[0] = wp1;
            dc2dwaypoints[1] = wp2;
            wp1.endPosition = wp2.position;


            dolly.allWaypoints = dc2dwaypoints;

            //Vector3[] waypoints = new Vector3[2];
            //waypoints[0] = new Vector3(0, 0, 0);
            //waypoints[1] = new Vector3(1, 1, 0);
            //dolly.dollyWaypoints = waypoints;

            //Vector3[] bezpoints = new Vector3[1];
            //bezpoints[0] = new Vector3(0.5f, 0.5f, 0);
            //dolly.bezierWaypoints = bezpoints;

            this.dollyRail = dolly;

#if UNITY_EDITOR
            Selection.activeGameObject = go;
            SceneView.FrameLastActiveSceneView();
#endif
        }
    }

    public void AddCameraBounds()
    {
        if (bounds != null) return;
        var go = new GameObject("CameraBounds");
        var cb = go.AddComponent<CameraBounds>();
        cb.guiColour = new Color(0, 0, 1f, 0.1f);
        cb.pointa = new Vector3(cameraData.cameraBoundsX, cameraData.cameraBoundsY, 0);
        go.transform.position = cameraData.boundaryObjectPosition;
        this.bounds = cb;
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    // adds a zoom target object to the scene to enable double click zooming
    public void AddZoomTarget()
    {
        if (ztTarget != null) return;
        var go = new GameObject("Dc2dZoomTarget");
        var zt = go.AddComponent<Dc2dZoomTarget>();
        this.ztTarget = zt;
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }


    //click and drag
    private void PanControl()
    {
        // if keyboard input is allowed
        if (cameraData.keyboardInput)
        {
            float x = -Input.GetAxis("Horizontal") * cameraData.dragSpeed;
            float y = -Input.GetAxis("Vertical") * cameraData.dragSpeed;

            if (cameraData.linkedZoomDrag)
            {
                var orthographicSize = _cam.orthographicSize;
                x *= orthographicSize;
                y *= orthographicSize;
            }

            if (cameraData.inverseKeyboard)
            {
                x = -x;
                y = -y;
            }
            
            if (x < 0)
                OnAnyMoveCameraLeft?.Invoke();
            
            if (x > 0)
                OnAnyMoveCameraRight?.Invoke();

            if (y > 0)
                OnAnyMoveCameraUp?.Invoke();
            
            if (y < 0)
                OnAnyMoveCameraDown?.Invoke();

            transform.Translate(x, y, 0);
        }


        // if mouse is down
        if (Input.GetMouseButton((int)cameraData.mouseButton))
        {
            float x = Input.GetAxis("Mouse X") * cameraData.dragSpeed;
            float y = Input.GetAxis("Mouse Y") * cameraData.dragSpeed;

            if (cameraData.linkedZoomDrag)
            {
                x *= _cam.orthographicSize;
                y *= _cam.orthographicSize;
            }

            transform.Translate(x, y, 0);
        }
    }

    private void ClampZoom()
    {
        _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize, cameraData.minZoom, cameraData.maxZoom);
        Mathf.Max(_cam.orthographicSize, 0.1f);
    }

    private void ZoomOrthoCamera(Vector3 zoomTowards, float amount)
    {
        if (amount > 0)
            OnAnyZoomCameraIn?.Invoke();
        else if (amount < 0)
            OnAnyZoomCameraOut?.Invoke();
        
        // Calculate how much we will have to move towards the zoomTowards position
        float multiplier = (1.0f / _cam.orthographicSize * amount);
        // Move camera
        transform.position += (zoomTowards - transform.position) * multiplier;
        // Zoom camera
        _cam.orthographicSize -= amount;
        // Limit zoom
        _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize, cameraData.minZoom, cameraData.maxZoom);
    }
    
    private void ZoomControl()
{
    var zoomInput = Input.GetAxis("Mouse ScrollWheel");
    var keyboardZoomInput = Input.GetAxis("Camera Zoom");
    
    var pinchRatio = LeanGesture.GetPinchRatio();

    if (cameraData.zoomEnabled)
    {
        if (zoomInput != 0)
        {
            if (cameraData.zoomToMouse)
            {
                if (zoomInput > 0 && cameraData.minZoom < _cam.orthographicSize)
                {
                    ZoomOrthoCamera(_cam.ScreenToWorldPoint(Input.mousePosition), cameraData.mouseScrollStepSize);
                }

                if (zoomInput < 0 && cameraData.maxZoom > _cam.orthographicSize)
                {
                    ZoomOrthoCamera(_cam.ScreenToWorldPoint(Input.mousePosition), -cameraData.mouseScrollStepSize);
                }
            }
            else
            {
                if (zoomInput > 0 && cameraData.minZoom < _cam.orthographicSize)
                {
                    _cam.orthographicSize -= cameraData.mouseScrollStepSize;
                }

                if (zoomInput < 0 && cameraData.maxZoom > _cam.orthographicSize)
                {
                    _cam.orthographicSize += cameraData.mouseScrollStepSize;
                }
            }

            ClampZoom();
        }
        
        if (keyboardZoomInput != 0)
        {
            if (keyboardZoomInput > 0 && cameraData.minZoom < _cam.orthographicSize)
            {
                _cam.orthographicSize -= cameraData.keyboardScrollStepSize;
            }

            if (keyboardZoomInput < 0 && cameraData.maxZoom > _cam.orthographicSize)
            {
                _cam.orthographicSize += cameraData.keyboardScrollStepSize;
            }

            ClampZoom();
        }
        
        if (pinchRatio > 0)
        {
            if (pinchRatio > 1 && cameraData.minZoom < _cam.orthographicSize) 
            {
                _cam.orthographicSize -= cameraData.pinchZoomStepSize * (pinchRatio - 1);
            }
            else if (pinchRatio < 1 && cameraData.maxZoom > _cam.orthographicSize) 
            {
                _cam.orthographicSize += cameraData.pinchZoomStepSize * (1 - pinchRatio);
            }
            
            ClampZoom();
        }
    }
}


    private bool _lfxmax = false;
    private bool _lfxmin = false;
    private bool _lfymax = false;
    private bool _lfymin = false;

    // Clamp Camera to bounds
    private void CameraClamp()
    {
        _tr = _cam.ScreenToWorldPoint(new Vector3(_cam.pixelWidth, _cam.pixelHeight, -transform.position.z));
        _bl = _cam.ScreenToWorldPoint(new Vector3(0, 0, -transform.position.z));

        if (bounds == null)
        {
            Debug.Log("Clamp Camera Enabled but no Bounds has been set.");
            return;
        }

        var boundsMaxX = bounds.pointa.x;
        var boundsMinX = bounds.transform.position.x;
        var boundsMaxY = bounds.pointa.y;
        var boundsMinY = bounds.transform.position.y;

        if (_tr.x > boundsMaxX && _bl.x < boundsMinX)
        {
            Debug.Log("User tried to zoom out past x axis bounds - locked to bounds");
            _cam.orthographicSize = _cam.orthographicSize - cameraData.mouseScrollStepSize; // zoomControl in to compensate
            ClampZoom();
        }

        if (_tr.y > boundsMaxY && _bl.y < boundsMinY)
        {
            Debug.Log("User tried to zoom out past y axis bounds - locked to bounds");
            _cam.orthographicSize = _cam.orthographicSize - cameraData.mouseScrollStepSize; // zoomControl in to compensate
            ClampZoom();
        }

        var tfxmax = false;
        var tfxmin = false;
        var tfymax = false;
        var tfymin = false;

        if (_tr.x > boundsMaxX)
        {
            if (_lfxmin)
            {
                _cam.orthographicSize = _cam.orthographicSize - cameraData.mouseScrollStepSize; // zoomControl in to compensate
                ClampZoom();
            }
            else
            {
                transform.position = new Vector3(transform.position.x - (_tr.x - boundsMaxX), transform.position.y,
                    transform.position.z);
                tfxmax = true;
            }
        }

        if (_tr.y > boundsMaxY)
        {
            if (_lfymin)
            {
                _cam.orthographicSize = _cam.orthographicSize - cameraData.mouseScrollStepSize; // zoomControl in to compensate
                ClampZoom();
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - (_tr.y - boundsMaxY),
                    transform.position.z);
                tfymax = true;
            }
        }

        if (_bl.x < boundsMinX)
        {
            if (_lfxmax)
            {
                _cam.orthographicSize = _cam.orthographicSize - cameraData.mouseScrollStepSize; // zoomControl in to compensate
                ClampZoom();
            }
            else
            {
                transform.position = new Vector3(transform.position.x + (boundsMinX - _bl.x), transform.position.y,
                    transform.position.z);
                tfxmin = true;
            }
        }

        if (_bl.y < boundsMinY)
        {
            if (_lfymax)
            {
                _cam.orthographicSize = _cam.orthographicSize - cameraData.mouseScrollStepSize; // zoomControl in to compensate
                ClampZoom();
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + (boundsMinY - _bl.y),
                    transform.position.z);
                tfymin = true;
            }
        }

        _lfxmax = tfxmax;
        _lfxmin = tfxmin;
        _lfymax = tfymax;
        _lfymin = tfymin;
    }

    private void StickToDollyRail()
    {
        if (dollyRail != null && followTarget != null)
        {
            Vector3 campos = dollyRail.getPositionOnTrack(followTarget.transform.position);
            transform.position = new Vector3(campos.x, campos.y, transform.position.z);
        }
    }

    public void ConformToSnapTarget()
    {
        float cx = (snapTarget.endPoint.x - snapTarget.transform.position.x) / 2 + snapTarget.transform.position.x;
        float cy = (snapTarget.endPoint.y - snapTarget.transform.position.y) / 2 + snapTarget.transform.position.y;
        transform.position =
            Vector3.Lerp(transform.position, new Vector3(cx, cy, transform.position.z),
                0.5f); // has to be fast to counter zoom jitter
        ExpandToZoomTarget();
        snapTarget = null; // target set rest for next frame
    }

    private void ExpandToZoomTarget()
    {
        if (snapTarget != null)
        {
            //contractzoom
            if (snapTarget.expandMode == Dc2dSnapBox.Mode.CONTRACTY || snapTarget.expandMode == Dc2dSnapBox.Mode.BOTHY)
            {
                if (_cam.WorldToViewportPoint(snapTarget.getExpandYUpperBound()).y < 1.049f)
                {
                    _cam.orthographicSize = _cam.orthographicSize - snapTarget.zoomSpeed;
                    //Debug.Log(snapTarget.sbName + ":CONTRACTY for:" + frameid);
                }
            }
            else if (snapTarget.expandMode == Dc2dSnapBox.Mode.CONTRACTX ||
                     snapTarget.expandMode == Dc2dSnapBox.Mode.BOTHX)
            {
                if (_cam.WorldToViewportPoint(snapTarget.getExpandXUpperBound()).x < 1.049f)
                {
                    _cam.orthographicSize = _cam.orthographicSize - snapTarget.zoomSpeed;
                    //Debug.Log(snapTarget.sbName + ":CONTRACTX for:" + frameid);
                }
            }

            //expandzoom
            if (snapTarget.expandMode == Dc2dSnapBox.Mode.EXPANDY || snapTarget.expandMode == Dc2dSnapBox.Mode.BOTHY)
            {
                if (_cam.WorldToViewportPoint(snapTarget.getExpandYUpperBound()).y > 0.95f)
                {
                    _cam.orthographicSize = _cam.orthographicSize + snapTarget.zoomSpeed;
                    //Debug.Log(snapTarget.sbName + ":EXPANDY for:" + frameid);
                }
            }
            else if (snapTarget.expandMode == Dc2dSnapBox.Mode.EXPANDX ||
                     snapTarget.expandMode == Dc2dSnapBox.Mode.BOTHX)
            {
                if (_cam.WorldToViewportPoint(snapTarget.getExpandXUpperBound()).x > 0.95f)
                {
                    _cam.orthographicSize = _cam.orthographicSize + snapTarget.zoomSpeed;
                    //Debug.Log(snapTarget.sbName + ":EXPANDX for:" + frameid);
                }
            }
        }
    }

    public void SetSnapTarget(Dc2dSnapBox sb)
    {
        if (snapTarget == null)
        {
            //Debug.Log("New snapboxes:"+ frameid);
            snapTarget = sb;
            return;
        }

        if (sb.priority > snapTarget.priority)
        {
            //Debug.Log("Swapping snapboxes:"+ frameid);
            snapTarget = sb;
        }
    }
}