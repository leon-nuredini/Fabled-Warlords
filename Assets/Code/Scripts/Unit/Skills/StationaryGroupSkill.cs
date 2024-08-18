using System;
using System.Collections;
using TbsFramework.Units.Abilities;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Units;

public class StationaryGroupSkill : Ability
{
    public event Action OnUnitAlerted;

    [SerializeField] private List<LUnit> _unitGroupList;

    [SerializeField] private bool _isStationary = true;

    [SerializeField] private int _alertRange = 3;

    [BoxGroup("Attack after X amount of turns")] [SerializeField]
    private bool _canAttackAfterSomeTurns;

    [BoxGroup("Attack after X amount of turns")] [ShowIf("_canAttackAfterSomeTurns")] [SerializeField]
    private int _turnsToWaitBeforeAttack = 15;

    private int _turnsPassed;

    private LUnit _lUnit;
    private MoveAbility _moveAbility;

    public bool IsStationary
    {
        get => _isStationary;
        set
        {
            _isStationary = value;
            if (_moveAbility != null)
                _moveAbility.CanMove = !_isStationary;
        }
    }

    private void Awake()
    {
        _lUnit = GetComponent<LUnit>();
        _moveAbility = GetComponent<MoveAbility>();

        if (!IsStationary)
            _moveAbility.CanMove = false;
    }

    private void OnEnable()
    {
        for (int i = 0; i < _unitGroupList.Count; i++)
        {
            if (_unitGroupList[i] == null) continue;
            _unitGroupList[i].OnDie += Alert;
            _unitGroupList[i].OnGetHit += Alert;
            if (_unitGroupList[i].TryGetComponent(out StationaryGroupSkill _stationaryGroupSkill))
                _stationaryGroupSkill.OnUnitAlerted += Alert;
        }
    }

    public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
    {
        if (!IsStationary) yield break;

        Player player = CellGrid.Instance.Players.First(player => player.PlayerNumber == 0);
        var humanPlayerUnitList = cellGrid.GetPlayerUnits(player);

        var unitsInRange = humanPlayerUnitList.Where(u =>
                u.Cell.GetDistance(UnitReference.Cell) <= _alertRange && u is not LStructure)
            .ToList();

        var tempUnits = new List<Unit>();

        for (int i = 0; i < unitsInRange.Count; i++)
        {
            if (unitsInRange[i].TryGetComponent(out PrisonerAbility prisonerAbility))
                if (prisonerAbility.IsPrisoner)
                    tempUnits.Add(unitsInRange[i]);
        }

        for (int i = 0; i < tempUnits.Count; i++)
            unitsInRange.Remove(tempUnits[i]);

        if (unitsInRange.Count > 0)
        {
            OnUnitAlerted?.Invoke();
            IsStationary = false;
        }

        if (IsStationary)
            UnitReference.SetMovementPoints(0);

        yield return 0;
    }

    public void Alert() => IsStationary = false;
    private void Alert(UnitDirection direction) => IsStationary = false;

    public override void OnTurnStart(CellGrid cellGrid)
    {
        if (_canAttackAfterSomeTurns)
        {
            _turnsPassed++;
            if (_turnsPassed >= _turnsToWaitBeforeAttack)
                Alert();
        }

        if (!IsStationary) return;
        StartCoroutine(Act(cellGrid, false));
    }

    public override void OnTurnEnd(CellGrid cellGrid)
    {
        if (!IsStationary) return;
        StartCoroutine(Act(cellGrid, false));
    }
}