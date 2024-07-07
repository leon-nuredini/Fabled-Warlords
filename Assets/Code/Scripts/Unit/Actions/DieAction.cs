using System.Collections;
using NaughtyAttributes;
using TbsFramework.Grid;
using UnityEngine;

public class DieAction : MonoBehaviour
{
    [SerializeField] private float _destroyDelay = 0.25f;
    [SerializeField] private float _hitColorDelay = 0.4f;

    [BoxGroup("SpriteRenderer")] [SerializeField]
    private SpriteRenderer _unitSpriteRenderer;

    private const string HitEffectBlend = "_HitEffectBlend";

    private LUnit _lUnit;
    private WaitForSeconds _waitUntilDestroy;
    private WaitForSeconds _waitHitColor;
    private Material _material;

    private void Awake()
    {
        _waitUntilDestroy = new WaitForSeconds(_destroyDelay);
        _waitHitColor = new WaitForSeconds(_hitColorDelay);
        _lUnit = GetComponent<LUnit>();

        if (_unitSpriteRenderer == null) return;
        _material = _unitSpriteRenderer.material;
    }

    private void OnEnable() => _lUnit.OnDie += Execute;
    private void OnDisable() => _lUnit.OnDie -= Execute;

    private void Execute(UnitDirection unitDirection) => StartCoroutine(Execute());

    private IEnumerator Execute()
    {
        yield return _waitHitColor;
        _material.SetFloat(HitEffectBlend, 1f);
        yield return _waitUntilDestroy;
        CellGrid.Instance.ManualOnUnitDestroyed(_lUnit);
        Destroy(gameObject);
    }
}