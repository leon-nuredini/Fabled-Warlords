using Lean.Pool;
using UnityEngine;

public class RootGraspSkill : MonoBehaviour, ISkill, ISpawnableEffect
{
    [SerializeField] private string _skillName = "Root Grasp";

    [SerializeField] private string _skillDescription =
        "Entagle an enemy, preventing them from moving and reducing their attack power for one turn.";

    [SerializeField] private int _durationInTurns = 1;
    [Range(0f, 100f)] [SerializeField] private float _procChance = 100;
    
    [SerializeField] private GameObject _effect;
    
    private TreantSpike _treantSpike;

    public string SkillName        => _skillName;
    public string SkillDescription => _skillDescription;
    public int    DurationInTurns  => _durationInTurns;
    public float ProcChance => _procChance;
    public GameObject Effect => _effect;
    public TreantSpike TreantSpike => _treantSpike;

    public void SpawnEffect(Transform targetTransform)
    {
        if (Effect == null) return;
        GameObject spikes = LeanPool.Spawn(Effect, targetTransform.localPosition, Effect.transform.rotation);
        _treantSpike = spikes.GetComponent<TreantSpike>();
    }
}