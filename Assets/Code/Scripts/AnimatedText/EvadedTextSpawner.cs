using Lean.Pool;
using Singleton;
using UnityEngine;

public class EvadedTextSpawner : SceneSingleton<EvadedTextSpawner>
{
    [SerializeField] protected GameObject _textGameObject;
    
    public void SpawnTextGameObject(Vector3 spawnPosition)
    {
        LeanPool.Spawn(_textGameObject, spawnPosition, Quaternion.identity);
    }
}