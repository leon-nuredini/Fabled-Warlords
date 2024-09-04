using Singleton;
using TMPro;
using UnityEngine;

public class StunTextSpawner : SceneSingleton<StunTextSpawner>
{
    private StatusEffectSpawner _statusEffectSpawner;
    
    [SerializeField] private TextOutlineColors _textOutlineColors;
    [SerializeField] protected TextMeshPro _text;
    [SerializeField] private float _textSpawnDelay = 0.75f;
    
    private void OnEnable() => _statusEffectSpawner = GetComponentInParent<StatusEffectSpawner>();

    public void SpawnTextGameObject(LUnit lUnit, Vector3 spawnPosition)
    {
        if (_text is null) return;
        _statusEffectSpawner.QueueStatusTextEffect(lUnit, _text, spawnPosition, _textSpawnDelay, _textOutlineColors.StunColor);
    }
}
