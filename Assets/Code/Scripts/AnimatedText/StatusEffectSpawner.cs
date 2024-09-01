using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class StatusEffectSpawner : MonoBehaviour
{
    private Queue<StatusEffectText> _textQueue = new Queue<StatusEffectText>();

    private Coroutine _coroutine;

    public void QueueStatusTextEffect(LUnit lUnit, GameObject textGameObject, Vector3 position, float textSpawnDelay)
    {
        StatusEffectText statusEffectText = new StatusEffectText(lUnit, textGameObject, position, textSpawnDelay);
        _textQueue.Enqueue(statusEffectText);

        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(SpawnQueuedTextEffects());
    }

    private IEnumerator SpawnQueuedTextEffects()
    {
        while (_textQueue.Count > 0)
        {
            StatusEffectText queuedStatusEffectText = _textQueue.Dequeue();
            yield return new WaitForSeconds(queuedStatusEffectText.TextSpawnDelay);
            if (queuedStatusEffectText.LUnit == null || queuedStatusEffectText.LUnit.HitPoints <= 0) continue;
            LeanPool.Spawn(queuedStatusEffectText.TextGameObject, queuedStatusEffectText.SpawnPosition,
                Quaternion.identity);
        }
    }
}

public class StatusEffectText
{
    private LUnit _lUnit;
    private GameObject _textGameObject;
    private Vector3 _spawnPosition;
    private float _textSpawnDelay;

    public StatusEffectText(LUnit lUnit, GameObject gameObject, Vector3 position, float textSpawnDelay)
    {
        _lUnit = lUnit;
        _textGameObject = gameObject;
        _spawnPosition = position;
        _textSpawnDelay = textSpawnDelay;
    }

    public GameObject TextGameObject => _textGameObject;
    public Vector3 SpawnPosition => _spawnPosition;
    public LUnit LUnit => _lUnit;
    public float TextSpawnDelay => _textSpawnDelay;
}