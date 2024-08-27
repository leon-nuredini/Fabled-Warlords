using System;
using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    [SerializeField] private float _offsetMultiplier = 1f;
    [SerializeField] private float _smoothTime = .3f;

    private Vector2 _startPosition;
    private Vector3 _velocity;

    private Camera _mainCamera;

    private void Awake() => _mainCamera = Camera.main;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        Vector2 offset = _mainCamera.ScreenToViewportPoint(Input.mousePosition);
        offset.x = Mathf.Clamp(offset.x, 0f, 1f);
        offset.y = Mathf.Clamp(offset.y, 0f, 1f);
        Vector3 destination = _startPosition + (offset * _offsetMultiplier);
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, _smoothTime);
    }
}