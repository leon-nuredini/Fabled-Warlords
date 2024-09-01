using DG.Tweening;
using UnityEngine;
using System;

public class CanvasGroupFadeInTweener : MonoBehaviour, ITween
{
    public event Action OnFadeInBegin;
    public event Action OnFadeInComplete;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _delay = 1.5f;
    [SerializeField] private Ease _fadeEase = Ease.OutQuad;

    private Tween _tween;
    private Tween _delayTween;

    public void Execute()
    {
        KillTween();
        OnFadeInBegin?.Invoke();
        _tween = _canvasGroup.DOFade(1f, _duration).SetEase(_fadeEase).OnComplete(OnFadeIn);
    }

    public void KillTween()
    {
        if (_tween != null)
            _tween.Kill();
        if (_delayTween != null)
            _delayTween.Kill();
    }

    private void OnFadeIn() => _delayTween = DOVirtual.DelayedCall(_delay, () => OnFadeInComplete?.Invoke());
}