using DG.Tweening;
using UnityEngine;
using System;

public class CanvasGroupFadeOutTweener : MonoBehaviour, ITween
{
    public event Action OnFadeOutComplete;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float       _duration = 0.5f;
    [SerializeField] private Ease        _fadeEase = Ease.OutQuad;

    private Tween _tween;

    public void Execute()
    {
        KillTween();
        _canvasGroup.DOFade(0f, _duration).SetEase(_fadeEase).OnComplete(OnFadeOut);
    }

    public void KillTween()
    {
        if (_tween != null)
            _tween.Kill();
    }

    private void OnFadeOut() => OnFadeOutComplete?.Invoke();
}