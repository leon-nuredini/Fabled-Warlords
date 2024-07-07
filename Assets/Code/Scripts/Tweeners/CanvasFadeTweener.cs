using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class CanvasFadeTweener : MonoBehaviour, ITween
{
    private CanvasGroup _canvasGroup;

    [SerializeField] private float _startFadeAmount = 0.25f;
    [SerializeField] private float _fadeDuration    = .35f;

    [ShowIf("_shouldFadeOut")] [SerializeField]
    private float _fadeOutDelay = .6f;

    [SerializeField] private Ease _fadeEase      = Ease.OutQuad;
    [SerializeField] private bool _shouldFadeOut = true;
    [SerializeField] private bool _tweenAtStart  = true;

    private Tween _fadeInTween;
    private Tween _fadeOutTween;

    private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

    private void OnEnable()
    {
        if (!_tweenAtStart) return;
        _fadeInTween = _canvasGroup.DOFade(_startFadeAmount, 0f);
        Execute();
    }

    private void OnDisable() => KillTween();

    public void Execute()
    {
        KillTween();
        _fadeInTween = _canvasGroup.DOFade(1f, _fadeDuration).SetEase(_fadeEase).OnComplete(() =>
        {
            if (!_shouldFadeOut) return;
            _fadeOutTween = _canvasGroup.DOFade(0f, _fadeDuration).SetEase(_fadeEase)
                .SetDelay(_fadeOutDelay);
        });
    }

    public void KillTween()
    {
        if (_fadeInTween  != null) _fadeInTween.Kill();
        if (_fadeOutTween != null) _fadeOutTween.Kill();
    }
}