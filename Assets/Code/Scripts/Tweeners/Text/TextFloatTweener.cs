using System;
using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;

public class TextFloatTweener : MonoBehaviour, ITween
{
    [SerializeField] private float _yOffset = 1.5f;
    [SerializeField] private float _floatUpDuration = 1f;
    [SerializeField] private Ease _floatUpEase = Ease.OutSine;

    private Transform _cachedTransform;

    private Tween _tween;
    private Tween _disableTween;

    private void Awake() => _cachedTransform = transform;
    private void OnEnable() => Execute();
    private void OnDisable() => KillTween();

    public void Execute()
    {
        KillTween();
        float yDestination = _cachedTransform.localPosition.y + _yOffset;
        _tween = _cachedTransform.DOLocalMoveY(yDestination, _floatUpDuration)
            .SetEase(_floatUpEase);
    }

    public void KillTween()
    {
        if (_tween != null) _tween.Kill();
        if (_disableTween != null) _disableTween.Kill();
    }
}