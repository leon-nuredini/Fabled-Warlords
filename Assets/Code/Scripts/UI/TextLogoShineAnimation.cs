using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TextLogoShineAnimation : MonoBehaviour
{
    [SerializeField] private float _shineDuration = .4f;
    [SerializeField] private float _shineDelay = 1f;

    private Image _image;
    private Material _material;
    private Tween _delayTween;
    private Tween _shineTween;

    private readonly string _shineOnly = "_ShineLocation";

    private void Awake()
    {
        _image = GetComponent<Image>();
        _material = _image.material;
    }

    private void Start() => InitAnimation();

    public void InitAnimation()
    {
        _material.SetFloat(_shineOnly, 0f);
        if (_delayTween != null) return;
        if (_shineTween != null) return;
        _delayTween = DOVirtual.DelayedCall(_shineDelay, AnimateShine);
    }

    private void AnimateShine()
    {
        _shineDelay = 0f;
        _shineTween = DOVirtual.Float(0f, 1f, _shineDuration, value => { _material.SetFloat(_shineOnly, value); })
            .OnComplete(
                () =>
                {
                    _delayTween.Kill();
                    _shineTween.Kill();
                    _delayTween = null;
                    _shineTween = null;
                });
    }
}