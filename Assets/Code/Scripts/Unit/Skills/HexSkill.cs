using Lean.Pool;
using UnityEngine;

public class HexSkill : MonoBehaviour, IStatusEffectSkill, ISpawnableEffect
{
    [SerializeField] private string _skillName = "Hex";

    [SerializeField] private string _skillDescription =
        "Debuffs an enemy unit, reducing their attack power and defense for one turn.";

    [SerializeField] private int _durationInTurns = 1;

    [SerializeField] private GameObject _effect;

    public string SkillName        => _skillName;
    public string SkillDescription => _skillDescription;
    public int    DurationInTurns  => _durationInTurns;
    public GameObject Effect => _effect;

    public void SpawnEffect(Transform targetTransform)
    {
        if (Effect == null) return;
        LeanPool.Spawn(Effect, targetTransform.localPosition, Effect.transform.rotation);
    }
}