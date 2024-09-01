using DG.Tweening;
using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;

public class FlagTweener : MonoBehaviour
{
    [SerializeField] private float _newYPosition = 1f;
    [SerializeField] private float _startScale = .4f;
    [SerializeField] private float _finalScale = .75f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _fadeDelay = .4f;
    [SerializeField] private float _fadeOutDuration = 1f;
    [SerializeField] private Ease _ease = Ease.OutCubic;

    [BoxGroup("Sprites"), SerializeField] private SpriteRenderer _stand;
    [BoxGroup("Sprites"), SerializeField] private SpriteRenderer _flag;

    private Tween _moveTween;
    private Tween _scaleTween;
    private Tween _fadeTween;

    private Color _standColor;
    private Color _flagColor;

    private void Awake()
    {
        _standColor = _stand.color;
        _flagColor = _flag.color;
        transform.localScale = new Vector3(_startScale, _startScale, _startScale);
    }

    private void OnEnable()
    {
        _standColor.a = 1f;
        _flagColor.a = 1f;

        _stand.color = _standColor;
        _flag.color = _flagColor;
        
        Vector3 newPosition = transform.position;
        newPosition.y += _newYPosition;

        _moveTween = transform.DOMove(newPosition, _duration).SetEase(_ease).OnComplete(FadeOutFlag);
        _scaleTween = transform.DOScale(Vector3.one * _finalScale, _duration).SetEase(_ease);
    }

    private void FadeOutFlag()
    {
        _fadeTween = DOVirtual.Float(1f, 0f, _fadeOutDuration, value =>
        {
            _standColor.a = value;
            _flagColor.a = value;

            _stand.color = _standColor;
            _flag.color = _flagColor;
        }).SetDelay(_fadeDelay).SetEase(_ease).OnComplete(DisableGameObject);
    }

    private void DisableGameObject()
    {
        transform.localScale = new Vector3(_startScale, _startScale, _startScale);
        LeanPool.Despawn(gameObject);
    }

    private void OnDisable()
    {
        if (_moveTween != null) _moveTween.Kill();
        if (_scaleTween != null) _scaleTween.Kill();
        if (_fadeTween != null) _fadeTween.Kill();
    }
}