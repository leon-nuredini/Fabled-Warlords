using Lean.Pool;
using UnityEngine;

public class EffectDespawner : MonoBehaviour
{
    private void DespawnEffect() => LeanPool.Despawn(gameObject);
}