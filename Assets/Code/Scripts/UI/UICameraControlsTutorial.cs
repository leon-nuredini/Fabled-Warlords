using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class UICameraControlsTutorial : MonoBehaviour
{
    [BoxGroup("Panel")] [SerializeField] private GameObject _panel;
    [BoxGroup("Panel")] [SerializeField] private GameObject _objectives;

    [SerializeField] private TextMeshProUGUI _moveLeftText;
    [SerializeField] private TextMeshProUGUI _moveRightText;
    [SerializeField] private TextMeshProUGUI _moveUpText;
    [SerializeField] private TextMeshProUGUI _moveDownText;

    [SerializeField] private TextMeshProUGUI _zoomInText;
    [SerializeField] private TextMeshProUGUI _zoomOutText;

    private WaitForSeconds _wait;

    private bool _panCameraLeft;
    private bool _panCameraRight;
    private bool _panCameraUp;
    private bool _panCameraDown;
    private bool _zoomCameraIn;
    private bool _zoomCameraOut;

    private void Awake()
    {
        _panel.SetActive(false);
        _objectives.SetActive(false);
        _wait = new WaitForSeconds(.75f);
    }

    private void Start() => StartCoroutine(ShowPanel());

    private IEnumerator ShowPanel()
    {
        yield return _wait;
        _panel.SetActive(true);
        _objectives.SetActive(true);
    }

    public void OnMoveLeft()
    {
        if (!_panel.activeSelf) return;
        if (_panCameraLeft) return;
        _panCameraLeft = true;
        _moveLeftText.text = $"<s>{_moveLeftText.text}";
        TryDisableCanvas();
    }

    public void OnMoveRight()
    {
        if (!_panel.activeSelf) return;
        if (_panCameraRight) return;
        _panCameraRight = true;
        _moveRightText.text = $"<s>{_moveRightText.text}";
        TryDisableCanvas();
    }

    public void OnMoveUp()
    {
        if (!_panel.activeSelf) return;
        if (_panCameraUp) return;
        _panCameraUp = true;
        _moveUpText.text = $"<s>{_moveUpText.text}";
        TryDisableCanvas();
    }

    public void OnMoveDown()
    {
        if (!_panel.activeSelf) return;
        if (_panCameraDown) return;
        _panCameraDown = true;
        _moveDownText.text = $"<s>{_moveDownText.text}";
        TryDisableCanvas();
    }

    public void OnZoomIn()
    {
        if (!_panel.activeSelf) return;
        if (_zoomCameraIn) return;
        _zoomCameraIn = true;
        _zoomInText.text = $"<s>{_zoomInText.text}";
        TryDisableCanvas();
    }

    public void OnZoomOut()
    {
        if (!_panel.activeSelf) return;
        if (_zoomCameraOut) return;
        _zoomCameraOut = true;
        _zoomOutText.text = $"<s>{_zoomOutText.text}";
        TryDisableCanvas();
    }

    private void TryDisableCanvas()
    {
        if (!_panCameraLeft) return;
        if (!_panCameraRight) return;
        if (!_panCameraUp) return;
        if (!_panCameraDown) return;
        if (!_zoomCameraIn) return;
        if (!_zoomCameraOut) return;

        gameObject.SetActive(false);
    }
}