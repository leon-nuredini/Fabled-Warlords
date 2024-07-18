using NaughtyAttributes;
using UnityEngine;

public class AntiLargeSkill : MonoBehaviour, IAttackSkill
{
    private LUnit _aggressorUnit;

    [SerializeField] private string _skillName = "Anti-Large";
    [SerializeField] private string _skillDescription = "Deals additional damage to beasts, mounted, and large units.";

    [BoxGroup("Attack Amount")] [SerializeField] [Range(1, 5)]
    private int _attackPowerFactor = 2;

    #region Properties

    public string SkillName        => _skillName;
    public string SkillDescription => _skillDescription;
    
    public LUnit AggressorUnit
    {
        get => _aggressorUnit;
        set => _aggressorUnit = value;
    }

    #endregion

    [field: SerializeField] public bool CanBeActivatedDuringEnemyTurn { get; set; } = true;

    public int GetDamageFactor()
    {
        if (_aggressorUnit != null && (_aggressorUnit is IMonster || _aggressorUnit is IMounted))
            return _attackPowerFactor;
        return 0;
    }
}