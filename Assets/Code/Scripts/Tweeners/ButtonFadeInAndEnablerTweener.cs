using DG.Tweening;
using UnityEngine;

public class ButtonFadeInAndEnablerTweener : MonoBehaviour, ITween
{
    [SerializeField] private float   _duration = .5f;
    [SerializeField] private float   _delay    = 3f;
    [SerializeField] private Ease    _ease     = Ease.OutQuad;

    private Tween         _tween;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup       = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }

    public void Execute()
    {
        KillTween();
        _canvasGroup.DOFade(1f, _duration).SetDelay(_delay).SetEase(_ease).OnComplete(() =>
        {
            _canvasGroup.blocksRaycasts = true;
        });
    }

    public void KillTween()
    {
        if (_tween == null) return;
        _tween.Kill();
    }
}
