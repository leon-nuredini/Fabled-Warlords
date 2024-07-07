using Lean.Pool;
using UnityEngine;

public class HitAction : MonoBehaviour
{
    private LUnit _lUnit;
    private ITween _tween;

    [SerializeField] private GameObject _hitEffect;

    private void Awake()
    {
        _tween = GetComponentInChildren<ITween>();
        _lUnit = GetComponent<LUnit>();
    }
    private void OnEnable()
    {
        if (_lUnit == null) return;
        _lUnit.OnTakeDamage += Execute;
    }

    private void OnDisable()
    {
        if (_lUnit == null) return;
        _lUnit.OnTakeDamage -= Execute;
    }

    private void Execute()
    {
        if (_hitEffect != null)
            LeanPool.Spawn(_hitEffect, transform.localPosition, Quaternion.identity);
        if (_tween != null)
            _tween.Execute();
    }
}