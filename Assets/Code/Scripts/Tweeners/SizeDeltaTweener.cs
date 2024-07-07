using DG.Tweening;
using UnityEngine;

public class SizeDeltaTweener : MonoBehaviour, ITween
{
    [SerializeField] private Vector2 _sizeDelta;
    [SerializeField] private float   _duration = .5f;
    [SerializeField] private float   _delay    = 2f;
    [SerializeField] private Ease    _ease     = Ease.OutQuad;

    private Tween         _tween;
    private RectTransform _rectTransform;

    private void Awake() => _rectTransform = GetComponent<RectTransform>();

    public void Execute()
    {
        KillTween();
        _rectTransform.DOSizeDelta(_sizeDelta, _duration).SetDelay(_delay).SetEase(_ease);
    }

    public void KillTween()
    {
        if (_tween == null) return;
        _tween.Kill();
    }
}