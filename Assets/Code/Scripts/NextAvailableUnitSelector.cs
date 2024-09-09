using System;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Units;
using UnityEngine;

public class NextAvailableUnitSelector : MonoBehaviour
{
    [SerializeField] private List<Unit> _playerUnits = new List<Unit>();
    private int _currSelectedUnitIndex = 0;
    private int _currSelectedBarrackIndex = 0;

    private Transform _cameraTransform;

    private void Awake() => _cameraTransform = Camera.main.transform;

    private void OnEnable()
    {
        if (CellGrid.Instance != null) CellGrid.Instance.TurnStarted += UpdateUnitList;
        NextPreviousUnitPreseneter.OnAnyClickNextUnitButton += SelectNextAvailableUnit;
        NextPreviousUnitPreseneter.OnAnyClickPreviousUnitButton += SelectPreviousAvailableUnit;
    }

    private void OnDisable()
    {
        if (CellGrid.Instance != null) CellGrid.Instance.TurnStarted -= UpdateUnitList;
        NextPreviousUnitPreseneter.OnAnyClickNextUnitButton -= SelectNextAvailableUnit;
        NextPreviousUnitPreseneter.OnAnyClickPreviousUnitButton -= SelectPreviousAvailableUnit;
    }

    private void Start() => UpdateUnitList(null, null);

    private void UpdateUnitList(object o, EventArgs args)
    {
        _currSelectedUnitIndex = 0;
        _currSelectedBarrackIndex = 0;
        if (CellGrid.Instance != null)
            _playerUnits = CellGrid.Instance.Units.ToList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Comma))
            SelectNextAvailableUnit();
        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Period))
            SelectPreviousAvailableUnit();

        if (Input.GetKeyDown(KeyCode.B))
            SelectNextBarrack();
    }

    #region UnitSelection

    private void SelectNextAvailableUnit()
    {
        if (_currSelectedUnitIndex == _playerUnits.Count - 1) _currSelectedUnitIndex = 0;
        Unit selectedUnit = null;
        if (ObjectHolder.Instance != null) selectedUnit = ObjectHolder.Instance.CurrSelectedUnit;
        if (TryToSelectTheNextUnit(selectedUnit)) return;

        _currSelectedUnitIndex = 0;
        TryToSelectTheNextUnit(selectedUnit);
    }

    private bool TryToSelectTheNextUnit(Unit selectedUnit)
    {
        if (CellGrid.Instance != null && CellGrid.Instance.CurrentPlayerNumber == 0)
        {
            for (int i = _currSelectedUnitIndex; i < _playerUnits.Count; i++)
            {
                if (_playerUnits[i] == null) continue;
                if (_playerUnits[i].PlayerNumber != CellGrid.Instance.CurrentPlayerNumber) continue;
                if (_playerUnits[i].ActionPoints == 0) continue;
                if (selectedUnit is not null)
                {
                    if (_playerUnits[i].Equals(selectedUnit)) continue;
                }

                if (_playerUnits[i] is LStructure) continue;
                if (_playerUnits[i] is LUnit lUnit && lUnit.PrisonerAbility != null &&
                    lUnit.PrisonerAbility.IsPrisoner) continue;

                SelectUnit(_playerUnits[i], i);
                return true;
            }
        }

        return false;
    }

    private void SelectPreviousAvailableUnit()
    {
        if (_currSelectedUnitIndex == 0) _currSelectedUnitIndex = _playerUnits.Count - 1;
        Unit selectedUnit = null;
        if (ObjectHolder.Instance != null) selectedUnit = ObjectHolder.Instance.CurrSelectedUnit;
        if (TryToSelectThePreviousUnit(selectedUnit)) return;

        _currSelectedUnitIndex = _playerUnits.Count - 1;
        TryToSelectThePreviousUnit(selectedUnit);
    }

    private bool TryToSelectThePreviousUnit(Unit selectedUnit)
    {
        if (CellGrid.Instance != null && CellGrid.Instance.CurrentPlayerNumber == 0)
        {
            for (int i = _currSelectedUnitIndex; i >= 0; i--)
            {
                if (_playerUnits[i] == null) continue;
                if (_playerUnits[i].PlayerNumber != CellGrid.Instance.CurrentPlayerNumber) continue;
                if (_playerUnits[i].ActionPoints == 0) continue;
                if (selectedUnit is not null)
                {
                    if (_playerUnits[i].Equals(selectedUnit)) continue;
                }

                if (_playerUnits[i] is LStructure) continue;
                if (_playerUnits[i] is LUnit lUnit && lUnit.PrisonerAbility != null &&
                    lUnit.PrisonerAbility.IsPrisoner) continue;

                SelectUnit(_playerUnits[i], i);
                return true;
            }
        }

        return false;
    }

    private void SelectUnit(Unit unit, int unitIndex)
    {
        if (unit is LUnit lUnit)
            lUnit.HandleMouseDown();

        _currSelectedUnitIndex = unitIndex;
        PositionCameraOnUnit(unit.transform.position);
    }

    #endregion

    #region BarrackSelection

    private void SelectNextBarrack()
    {
        if (_currSelectedBarrackIndex == _playerUnits.Count - 1) _currSelectedBarrackIndex = 0;
        Unit selectedUnit = null;
        if (ObjectHolder.Instance != null) selectedUnit = ObjectHolder.Instance.CurrSelectedUnit;
        if (TryToSelectNextBarrack(selectedUnit)) return;

        _currSelectedBarrackIndex = 0;
        TryToSelectNextBarrack(selectedUnit);
    }

    private bool TryToSelectNextBarrack(Unit selectedUnit)
    {
        if (CellGrid.Instance != null && CellGrid.Instance.CurrentPlayerNumber == 0)
        {
            for (int i = _currSelectedBarrackIndex; i < _playerUnits.Count; i++)
            {
                if (_playerUnits[i] == null) continue;
                if (_playerUnits[i].PlayerNumber != CellGrid.Instance.CurrentPlayerNumber) continue;
                if (selectedUnit is not null && _playerUnits[i].Equals(selectedUnit)) continue;
                if (_playerUnits[i] is not Stronghold && _playerUnits[i] is not Barrack) continue;
                
                SelectBarrack(_playerUnits[i], i);
                return true;
            }
        }

        return false;
    }
    
    private void SelectBarrack(Unit unit, int unitIndex)
    {
        if (unit is LUnit lUnit)
            lUnit.HandleMouseDown();

        _currSelectedBarrackIndex = unitIndex;
        PositionCameraOnUnit(unit.transform.position);
    }


    #endregion

    private void PositionCameraOnUnit(Vector3 unitPosition)
    {
        Vector3 newCameraPosition = _cameraTransform.position;
        newCameraPosition.x = unitPosition.x;
        newCameraPosition.y = unitPosition.y;
        _cameraTransform.position = newCameraPosition;
    }
}