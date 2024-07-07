using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class AbandonTweener : MonoBehaviour, ITween
{
    [BoxGroup("Colors")] [SerializeField] private Color _defaultColor;

    [BoxGroup("Tweening")] [SerializeField]
    private float _fadeDelay = 0.4f;

    private Material _material;
    private Tween    _tweenFade;

    private readonly string _hitEffectBlend = "_HitEffectBlend";

    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;
        _material = spriteRenderer.material;
    }

    public void Execute()
    {
        if (_material == null) return;
        KillTween();
        _material.SetFloat(_hitEffectBlend, 1f);
        _tweenFade = DOVirtual.DelayedCall(_fadeDelay, () => _material.SetFloat(_hitEffectBlend, 0f));
    }

    public void KillTween()
    {
        if (_tweenFade != null) _tweenFade.Kill();
    }
}