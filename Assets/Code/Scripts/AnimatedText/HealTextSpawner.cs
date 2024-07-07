using Lean.Pool;
using Singleton;
using UnityEngine;

public class HealTextSpawner : SceneSingleton<HealTextSpawner>
{
    [SerializeField] protected GameObject _textGameObject;

    public void SpawnTextGameObject(Vector3 spawnPosition, string healAmount = "")
    {
        GameObject gObj = LeanPool.Spawn(_textGameObject, spawnPosition, Quaternion.identity);
        if (gObj.TryGetComponent(out HealText healText))
            healText.UpdateTextValue($"+{healAmount}");
    }
}
