using NaughtyAttributes;
using UnityEngine;

public class RageSkill : MonoBehaviour, IAttackSkill
{
    private LUnit _lUnit;

    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;

    [BoxGroup("Damage Multiplier")] [SerializeField] [Range(1, 10)]
    private int _berserkDamageFactor = 2;

    [BoxGroup("Damage Multiplier")] [SerializeField] [Range(0.1f, .8f)]
    private float _hitPointsSkillActivationPercentage = 0.4f;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;

    [field: SerializeField] public bool CanBeActivatedDuringEnemyTurn { get; set; } = true;

    private void Awake() => _lUnit = GetComponent<LUnit>();

    public int GetDamageFactor()
    {
        if ((float)_lUnit.HitPoints / _lUnit.TotalHitPoints > _hitPointsSkillActivationPercentage)
            return 0;
        return _berserkDamageFactor;
    }
}