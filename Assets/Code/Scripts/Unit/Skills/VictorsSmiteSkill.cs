using TbsFramework.Grid;
using UnityEngine;

public class VictorsSmiteSkill : MonoBehaviour, ISkill
{
    private LUnit _lUnit;

    [SerializeField] private string _skillName = "Victor's Smite";

    [SerializeField] private string _skillDescription =
        "Once per turn, when defeating an enemy, this unit gains an additional action point to perform an additional attack this turn.";

    public string SkillName        => _skillName;
    public string SkillDescription => _skillDescription;

    private bool _smiteActivated;

    private void Awake() => _lUnit = GetComponent<LUnit>();

    private void OnEnable()  => _lUnit.OnTurnEndUnitReset += ResetSmite;
    private void OnDisable() => _lUnit.OnTurnEndUnitReset += ResetSmite;

    public bool TryGetAdditionalActionPoint(LUnit enemyUnit)
    {
        if (_smiteActivated) return false;
        if (enemyUnit                             == null) return false;
        if (enemyUnit.HitPoints                   > 0) return false;
        if (_lUnit                                == null) return false;
        if (CellGrid.Instance.CurrentPlayerNumber != _lUnit.PlayerNumber) return false;
        _lUnit.ActionPoints++;
        _smiteActivated = true;
        if (SmiteTextSpawner.Instance != null)
            SmiteTextSpawner.Instance.SpawnTextGameObject(_lUnit, transform.position);
        return true;
    }

    private void ResetSmite() => _smiteActivated = false;
}