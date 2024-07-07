using TbsFramework.Grid;
using UnityEngine;

public class VictoryValorSkill : MonoBehaviour, ISkill
{
    private LUnit _lUnit;
    
    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;

    private void Awake() => _lUnit = GetComponent<LUnit>();

    public bool TryGetAdditionalActionPoint(LUnit enemyUnit)
    {
        if (enemyUnit == null) return false;
        if (enemyUnit.HitPoints > 0) return false;
        if (_lUnit == null) return false;
        if (CellGrid.Instance.CurrentPlayerNumber != _lUnit.PlayerNumber) return false;
        _lUnit.ActionPoints++;
        return true;
    }
}