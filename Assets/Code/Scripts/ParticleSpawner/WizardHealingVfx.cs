using Lean.Pool;
using UnityEngine;

public class WizardHealingVfx : MonoBehaviour, IParticleSpawner
{
    [SerializeField] private GameObject _healingVfx;

    public GameObject ParticleToSpawn => _healingVfx;

    private AOEHealingSkill _aoeHealingSkill;

    private void Awake() => _aoeHealingSkill = GetComponent<AOEHealingSkill>();
    private void OnEnable() => _aoeHealingSkill.OnHeal += OnHeal;
    private void OnDisable() => _aoeHealingSkill.OnHeal -= OnHeal;

    private void OnHeal(Transform[] spawnPositionArray) => SpawnParticle(_healingVfx, spawnPositionArray);

    public void SpawnParticle(GameObject particleToSpawn, Transform[] spawnPositionArray)
    {
        if (particleToSpawn == null) return;
        for (int i = 0; i < spawnPositionArray.Length; i++)
            LeanPool.Spawn(_healingVfx, spawnPositionArray[i].position, Quaternion.identity);
    }
}