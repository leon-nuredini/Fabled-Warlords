using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextSizeTweener : MonoBehaviour, ITween
{
    private TextMeshPro _text;

    [SerializeField] private float _startFontSize = 4f;
    [SerializeField] private float _fontSizeDuration = .7f;
    [SerializeField] private Ease _fontSizeEase = Ease.OutBounce;

    [SerializeField] private float _originalFontSize;

    private Tween _fontSizeTween;

    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
        _originalFontSize = _text.fontSize;
    }

    private void OnEnable()
    {
        _text.fontSize = _startFontSize;
        Execute();
    }

    private void OnDisable() => KillTween();

    public void Execute()
    {
        KillTween();
        _fontSizeTween = _text.DOFontSize(_originalFontSize, _fontSizeDuration).SetEase(_fontSizeEase);
    }

    public void KillTween()
    {
        if (_fontSizeTween != null) _fontSizeTween.Kill();
    }
}