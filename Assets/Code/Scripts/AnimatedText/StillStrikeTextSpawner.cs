using Singleton;
using UnityEngine;

public class StillStrikeTextSpawner : SceneSingleton<StillStrikeTextSpawner>
{
    private StatusEffectSpawner _statusEffectSpawner;

    [SerializeField] protected GameObject _textGameObject;
    [SerializeField] private float _textSpawnDelay = 0.75f;

    private void OnEnable() => _statusEffectSpawner = GetComponentInParent<StatusEffectSpawner>();

    public void SpawnTextGameObject(LUnit lUnit, Vector3 spawnPosition)
    {
        if (_textGameObject == null) return;
        _statusEffectSpawner.QueueStatusTextEffect(lUnit, _textGameObject, spawnPosition, _textSpawnDelay);
    }
}
