using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;

public class TextFadeTweener : MonoBehaviour, ITween
{
    private TMP_Text _text;

    [SerializeField] private float _startFadeAmount = 0.25f;
    [SerializeField] private float _fadeDuration = .5f;
    [SerializeField] private float _fadeOutDelay = 1f;
    [SerializeField] private Ease _fadeEase = Ease.OutQuad;
    [SerializeField] private bool _canDespawnGameobject;

    private Tween _fadeInTween;
    private Tween _fadeOutTween;

    private void Awake() => _text = GetComponent<TMP_Text>();

    private void OnEnable()
    {
        _fadeInTween = _text.DOFade(_startFadeAmount, 0f);
        Execute();
    }

    private void OnDisable() => KillTween();

    public void Execute()
    {
        KillTween();
        _fadeInTween = _text.DOFade(1f, _fadeDuration).SetEase(_fadeEase).OnComplete(() =>
        {
            _fadeOutTween = _text.DOFade(0f, _fadeDuration).SetEase(_fadeEase)
                .OnComplete(() =>
                {
                    if (_canDespawnGameobject)
                        LeanPool.Despawn(gameObject);
                })
                .SetDelay(_fadeOutDelay);
        });
    }

    public void KillTween()
    {
        if (_fadeInTween != null) _fadeInTween.Kill();
        if (_fadeOutTween != null) _fadeOutTween.Kill();
    }
}