using Lean.Pool;
using UnityEngine;

public class VillageHealingVfx : MonoBehaviour, IParticleSpawner
{
    [SerializeField] private GameObject _healingVfx;

    public GameObject ParticleToSpawn => _healingVfx;

    private VillageHealingSkill _villageHealingSkill;

    private void Awake() => _villageHealingSkill = GetComponent<VillageHealingSkill>();
    private void OnEnable() => _villageHealingSkill.OnHeal += OnHeal;
    private void OnDisable() => _villageHealingSkill.OnHeal -= OnHeal;

    private void OnHeal(Transform[] spawnPositionArray) => SpawnParticle(_healingVfx, spawnPositionArray);

    public void SpawnParticle(GameObject particleToSpawn, Transform[] spawnPositionArray)
    {
        if (particleToSpawn == null) return;
        for (int i = 0; i < spawnPositionArray.Length; i++)
            LeanPool.Spawn(_healingVfx, spawnPositionArray[i].position, Quaternion.identity);
    }
}