using Lean.Pool;
using UnityEngine;

public class PoisonSkill : MonoBehaviour, IStatusEffectSkill, ISpawnableEffect
{
    [SerializeField] private string _skillName = "Poision";

    [SerializeField] private string _skillDescription =
        "Inflicts poison on enemies upon striking them, causing them to take damage over one turn.";

    [SerializeField] private int _durationInTurns = 1;
    [Range(0f, 100f)] [SerializeField] private float _procChance = 50;
    
    [SerializeField] private GameObject _effect;

    public string SkillName        => _skillName;
    public string SkillDescription => _skillDescription;
    public int    DurationInTurns  => _durationInTurns;
    public float ProcChance => _procChance;
    public GameObject Effect => _effect;
    
    public void SpawnEffect(Transform targetTransform)
    {
        if (Effect == null) return;
        LeanPool.Spawn(Effect, targetTransform.localPosition, Effect.transform.rotation);
    }
}