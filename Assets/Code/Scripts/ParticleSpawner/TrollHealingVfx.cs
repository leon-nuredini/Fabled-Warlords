using Lean.Pool;
using UnityEngine;

public class TrollHealingVfx : MonoBehaviour, IParticleSpawner
{
    [SerializeField] private GameObject _healingVfx;

    public GameObject ParticleToSpawn => _healingVfx;

    private RegenerationSkill _regenerationSkilll;

    private void Awake() => _regenerationSkilll = GetComponent<RegenerationSkill>();
    private void OnEnable() => _regenerationSkilll.OnHeal += OnHeal;
    private void OnDisable() => _regenerationSkilll.OnHeal -= OnHeal;

    private void OnHeal(Transform[] spawnPositionArray) => SpawnParticle(_healingVfx, spawnPositionArray);

    public void SpawnParticle(GameObject particleToSpawn, Transform[] spawnPositionArray)
    {
        if (particleToSpawn == null) return;
        for (int i = 0; i < spawnPositionArray.Length; i++)
            LeanPool.Spawn(_healingVfx, spawnPositionArray[i].position, Quaternion.identity);
    }
}