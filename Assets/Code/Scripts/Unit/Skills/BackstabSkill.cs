using NaughtyAttributes;
using TbsFramework.Units;
using UnityEngine;

public class BackstabSkill : MonoBehaviour, IAttackSkill
{
    private LUnit _lUnit;
    private LUnit _unitToAttack;

    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;

    [BoxGroup("Damage Multiplier")] [SerializeField] [Range(1, 10)]
    private int _backstabDamageFactor = 3;

    public string SkillName        => _skillName;
    public string SkillDescription => _skillDescription;

    public LUnit UnitToAttack { get => _unitToAttack; set => _unitToAttack = value; }

    [field: SerializeField] public bool CanBeActivatedDuringEnemyTurn { get; set; } = true;

    private void Awake() => _lUnit = GetComponent<LUnit>();

    public int GetDamageFactor()
    {
        int factor = 0;
        if (_lUnit.CurrentUnitDirection == _unitToAttack.CurrentUnitDirection)
            factor = _backstabDamageFactor;

        _unitToAttack = null;
        return factor;
    }
}