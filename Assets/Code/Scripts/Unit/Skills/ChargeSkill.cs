using NaughtyAttributes;
using TbsFramework.Units;
using UnityEngine;

public class ChargeSkill : MonoBehaviour, IAttackSkill
{
    [SerializeField] private string _skillName = "Charge";

    [SerializeField] private string _skillDescription =
        "Deals extra damage when attacking after moving at least four tiles away from its starting position.";

    [BoxGroup("Attack Amount")] [SerializeField] [Range(1, 5)]
    private int _attackPowerFactor = 3;

    [SerializeField] private int _totalTilesDistanceAmount = 4;

    private bool _applyCharge;

    private LUnit _lUnit;
    private UndoMovementAction _undoMovementAction;

    #region Properties

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;
    public bool CanBeActivatedDuringEnemyTurn { get; set; } = true;

    #endregion

    private void Awake()
    {
        _lUnit = GetComponent<LUnit>();
        _undoMovementAction = GetComponent<UndoMovementAction>();
    }

    private void OnEnable()
    {
        _lUnit.OnMovementFinished += OnMoveToAnotherCell;
        _lUnit.OnTurnEndUnitReset += ResetTileDistance;
    }

    private void OnDisable()
    {
        _lUnit.OnMovementFinished -= OnMoveToAnotherCell;
        _lUnit.OnTurnEndUnitReset -= ResetTileDistance;
    }

    public int GetDamageFactor()
    {
        if (_applyCharge)
            return _attackPowerFactor;
        return 0;
    }

    private void OnMoveToAnotherCell(MovementEventArgs movementEventArgs, bool isUndoing)
    {
        if (isUndoing)
        {
            _applyCharge = false;
            return;
        }
        
        _applyCharge = movementEventArgs.Path.Count >= _totalTilesDistanceAmount;
        if (_applyCharge && ChargeTextSpawner.Instance != null)
            ChargeTextSpawner.Instance.SpawnTextGameObject(_lUnit, transform.position);
    }

    private void ResetTileDistance() => _applyCharge = false;
    
}