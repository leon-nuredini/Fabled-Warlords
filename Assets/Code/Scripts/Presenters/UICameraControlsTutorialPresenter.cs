using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraControlsTutorialPresenter : MonoBehaviour
{
    private UICameraControlsTutorial _uICameraControlsTutorial;

    private void Awake() => _uICameraControlsTutorial = GetComponent<UICameraControlsTutorial>();
    
    private void OnEnable()
    {
        TwoDCameraDrag.OnAnyMoveCameraLeft += OnAnyMoveCameraLeft;
        TwoDCameraDrag.OnAnyMoveCameraRight += OnAnyMoveCameraRight;
        TwoDCameraDrag.OnAnyMoveCameraUp += OnAnyMoveCameraUp;
        TwoDCameraDrag.OnAnyMoveCameraDown += OnAnyMoveCameraDown;
        TwoDCameraDrag.OnAnyZoomCameraIn += OnAnyZoomCameraIn;
        TwoDCameraDrag.OnAnyZoomCameraOut += OnAnyZoomCameraOut;
    }

    private void OnDisable()
    {
        TwoDCameraDrag.OnAnyMoveCameraLeft -= OnAnyMoveCameraLeft;
        TwoDCameraDrag.OnAnyMoveCameraRight -= OnAnyMoveCameraRight;
        TwoDCameraDrag.OnAnyMoveCameraUp -= OnAnyMoveCameraUp;
        TwoDCameraDrag.OnAnyMoveCameraDown -= OnAnyMoveCameraDown;
        TwoDCameraDrag.OnAnyZoomCameraIn -= OnAnyZoomCameraIn;
        TwoDCameraDrag.OnAnyZoomCameraOut -= OnAnyZoomCameraOut;
    }

    private void OnAnyMoveCameraLeft() => _uICameraControlsTutorial.OnMoveLeft();
    private void OnAnyMoveCameraRight() => _uICameraControlsTutorial.OnMoveRight();
    private void OnAnyMoveCameraUp() => _uICameraControlsTutorial.OnMoveUp();
    private void OnAnyMoveCameraDown() => _uICameraControlsTutorial.OnMoveDown();
    private void OnAnyZoomCameraIn() => _uICameraControlsTutorial.OnZoomIn();
    private void OnAnyZoomCameraOut() => _uICameraControlsTutorial.OnZoomOut();
}
