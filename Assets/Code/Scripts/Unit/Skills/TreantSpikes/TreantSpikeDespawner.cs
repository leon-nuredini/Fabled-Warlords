using System.Collections;
using Lean.Pool;
using UnityEngine;

[RequireComponent(typeof(TreantSpike))]
public class TreantSpikeDespawner : MonoBehaviour
{
    private TreantSpike _treantSpike;
    private Animator _animator;
    private WaitForSeconds _wait;

    [SerializeField] private float _despawnDelay = 1f;

    private readonly string _despawn = "despawn";

    private void Awake()
    {
        _treantSpike = GetComponent<TreantSpike>();
        _animator = GetComponent<Animator>();
        _wait = new WaitForSeconds(_despawnDelay);
    }

    private void OnEnable() => _treantSpike.OnDespawn += StartDespawning;
    private void OnDisable() => _treantSpike.OnDespawn -= StartDespawning;

    private void StartDespawning()
    {
        _animator.SetTrigger(_despawn);
        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return _wait;
        LeanPool.Despawn(gameObject);
    }
}