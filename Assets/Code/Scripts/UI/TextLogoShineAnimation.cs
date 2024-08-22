using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TextLogoShineAnimation : MonoBehaviour
{
    [SerializeField] private float _shineDuration = .4f;
    [SerializeField] private float _shineDelay = 1f;

    private Image _image;
    private Material _material;

    private readonly string _shineOnly = "_ShineLocation";

    private void Awake()
    {
        _image = GetComponent<Image>();
        _material = _image.material;
    }

    private void Start()
    {
        DOVirtual.DelayedCall(_shineDelay, () => AnimateShine());
    }

    private void AnimateShine()
    {
        DOVirtual.Float(0f, 1f, _shineDuration, value =>
        {
            _material.SetFloat(_shineOnly, value);
        });
    }
}