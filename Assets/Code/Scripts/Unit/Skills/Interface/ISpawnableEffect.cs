using UnityEngine;

public interface ISpawnableEffect
{
    GameObject Effect { get; }
    void SpawnEffect(Transform targetTransform);
}
