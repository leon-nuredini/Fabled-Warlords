using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class HitTweener : MonoBehaviour, ITween
{
    [BoxGroup("Sprite Renderer")] [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [BoxGroup("Colors")] [SerializeField] private Color _defaultColor;
    [BoxGroup("Colors")] [SerializeField] private Color _hitColor;

    [BoxGroup("Tweening")] [SerializeField]
    private float _fadeInDuration = 0.2f;

    [BoxGroup("Tweening")] [SerializeField]
    private float _fadeOutDuration = 0.4f;

    [BoxGroup("Tweening")] [SerializeField]
    private Ease _fadeEase = Ease.OutQuad;

    private Tween _tweenFadeIn;
    private Tween _tweenFadeOut;

    private void Awake()
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() => LevelManager.OnAnyNewLevelLoaded += KillTween;
    private void OnDisable() => LevelManager.OnAnyNewLevelLoaded -= KillTween;

    public void Execute()
    {
        if (_spriteRenderer == null) return;
        KillTween();
        _spriteRenderer.color = _hitColor;
        _tweenFadeIn = _spriteRenderer.DOColor(_hitColor, _fadeInDuration).SetEase(_fadeEase).OnComplete(() =>
            _tweenFadeOut = _spriteRenderer.DOColor(_defaultColor, _fadeOutDuration).SetEase(_fadeEase));
    }

    public void KillTween()
    {
        if (_tweenFadeIn != null) _tweenFadeIn.Kill();
        if (_tweenFadeOut != null) _tweenFadeOut.Kill();
    }
}