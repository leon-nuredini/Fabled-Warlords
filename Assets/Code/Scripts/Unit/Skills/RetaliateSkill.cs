using TbsFramework.Grid;
using TbsFramework.Units.Abilities;
using UnityEngine;

public class RetaliateSkill : MonoBehaviour, ISkill
{
    private LUnit _aggressorUnit;
    private AttackAbility _attackAbility;

    [SerializeField] private int _retaliatePoints = 1;
    private int _totalRetaliatePoints;

    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;

    private bool _isRetaliating;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;

    public int RetaliatePoints
    {
        get => _retaliatePoints;
        set => _retaliatePoints = value;
    }

    public int TotalRetaliatePoints => _totalRetaliatePoints;

    public LUnit AggressorUnit
    {
        get => _aggressorUnit;
        set => _aggressorUnit = value;
    }

    public bool IsRetaliating
    {
        get => _isRetaliating;
        set => _isRetaliating = value;
    }

    private void Awake()
    {
        _attackAbility = GetComponent<AttackAbility>();
        _totalRetaliatePoints = RetaliatePoints;
    }

    private void OnEnable() => CellGrid.Instance.TurnEnded += OnTurnEnd;
    private void OnDisable() => CellGrid.Instance.TurnEnded -= OnTurnEnd;

    public bool IsInAttackRange()
    {
        if (_aggressorUnit.UnRetaliatableSkill != null) return false;
        if (RetaliatePoints <= 0) return false;
        if (!_attackAbility.UnitReference.IsUnitAttackable(AggressorUnit, _attackAbility.UnitReference.Cell))
            return false;
        RetaliatePoints--;
        IsRetaliating = true;
        return true;
    }

    private void OnTurnEnd(object sender, bool isNetworkInvoked)
    {
        RetaliatePoints = TotalRetaliatePoints;
    }
}