using Lean.Pool;
using Singleton;
using UnityEngine;

public class ParryTextSpawner : SceneSingleton<ParryTextSpawner>
{
    [SerializeField] protected GameObject _textGameObject;

    public void SpawnTextGameObject(Vector3 spawnPosition, string healAmount = "")
    {
        if (_textGameObject == null) return;
        LeanPool.Spawn(_textGameObject, spawnPosition, Quaternion.identity);
    }
}