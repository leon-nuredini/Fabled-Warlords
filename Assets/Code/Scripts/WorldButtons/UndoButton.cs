using System;
using UnityEngine;

public class UndoButton : MonoBehaviour
{
    public event Action OnClickUndoButton;

    [SerializeField] private float _alpha = 200f;
    [SerializeField] private SpriteRenderer[] _spriteRenderers;

    private float _defaultAlpha;

    private void Start()
    {
        _defaultAlpha = _alpha / 255f;
        UpdateAlpha(_defaultAlpha);
    }

    private void OnDisable() => UpdateAlpha(_defaultAlpha);

    private void OnMouseEnter() => UpdateAlpha(1f);

    private void OnMouseDown()
    {
        gameObject.SetActive(false);
        UpdateAlpha(_defaultAlpha);
        OnClickUndoButton?.Invoke();
    }

    private void OnMouseExit() => UpdateAlpha(_defaultAlpha);

    private void UpdateAlpha(float value)
    {
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            Color color = _spriteRenderers[i].color;
            color.a = value;
            _spriteRenderers[i].color = color;
        }
    }
}