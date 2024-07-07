using Lean.Pool;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private void DespawnEffect() => LeanPool.Despawn(gameObject);
}