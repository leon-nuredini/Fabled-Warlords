using Lean.Pool;
using Singleton;
using UnityEngine;

public class DamageTextSpawner : SceneSingleton<DamageTextSpawner>
{
    [SerializeField] protected GameObject _textGameObject;

    public void SpawnTextGameObject(Vector3 spawnPosition, string damage = "")
    {
        GameObject gObj = LeanPool.Spawn(_textGameObject, spawnPosition, Quaternion.identity);
        if (gObj.TryGetComponent(out DamageText damageText))
            damageText.UpdateTextValue(damage);
    }
}