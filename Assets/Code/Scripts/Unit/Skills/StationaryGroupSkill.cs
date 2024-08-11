using System;
using System.Collections;
using TbsFramework.Units.Abilities;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Units;

public class StationaryGroupSkill : Ability
{
    public event Action OnUnitAlerted;

    [SerializeField] private List<LUnit> _unitGroupList;

    [SerializeField] private bool _isStationary = true;

    [SerializeField] private int _alertRange = 3;

    private LUnit _lUnit;

    private void Awake() => _lUnit = GetComponent<LUnit>();

    private void OnEnable()
    {
        for (int i = 0; i < _unitGroupList.Count; i++)
        {
            _unitGroupList[i].OnDie += Alert;
            _unitGroupList[i].OnGetHit += Alert;
            if (_unitGroupList[i].TryGetComponent(out StationaryGroupSkill _stationaryGroupSkill))
                _stationaryGroupSkill.OnUnitAlerted += Alert;
        }
    }

    public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
    {
        if (!_isStationary) yield break;

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
            _isStationary = false;
        }

        if (_isStationary)
            UnitReference.SetMovementPoints(0);

        yield return 0;
    }

    private void Alert() => _isStationary = false;
    private void Alert(UnitDirection direction) => _isStationary = false;

    public override void OnTurnStart(CellGrid cellGrid)
    {
        if (!_isStationary) return;
        StartCoroutine(Act(cellGrid, false));
    }

    public override void OnTurnEnd(CellGrid cellGrid)
    {
        if (!_isStationary) return;
        StartCoroutine(Act(cellGrid, false));
    }
}