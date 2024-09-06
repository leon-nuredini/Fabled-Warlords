using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TbsFramework.Grid;
using TbsFramework.Units;
using UnityEngine;

public class NextAvailableUnitSelector : MonoBehaviour
{
    [SerializeField] private List<Unit> _playerUnits = new List<Unit>();
    private int _currSelectedUnitIndex = 0;

    private void OnEnable()
    {
        if (CellGrid.Instance != null) CellGrid.Instance.TurnStarted += UpdateUnitList;
    }

    private void OnDisable()
    {
        if (CellGrid.Instance != null) CellGrid.Instance.TurnStarted -= UpdateUnitList;
    }

    private void Start() => UpdateUnitList(null, null);

    private void UpdateUnitList(object o, EventArgs args)
    {
        _currSelectedUnitIndex = 0;
        if (CellGrid.Instance != null)
            _playerUnits = CellGrid.Instance.GetPlayerUnits(0).ToList();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Period))
            SelectNextAvailableUnit();
        else if (Input.GetKeyDown(KeyCode.Comma))
            SelectPreviousAvailableUnit();
    }
    
    private void SelectNextAvailableUnit()
    {
        Unit selectedUnit = null;
        if (ObjectHolder.Instance != null) selectedUnit = ObjectHolder.Instance.CurrSelectedUnit;
        if (CellGrid.Instance != null && CellGrid.Instance.CurrentPlayerNumber == 0)
        {
            for (int i = _currSelectedUnitIndex; i < _playerUnits.Count; i++)
            {
                if (_playerUnits[i] == null) continue;
                if (_playerUnits[i].ActionPoints == 0) continue;
                if (selectedUnit is not null)
                {
                    if (_playerUnits[i].Equals(selectedUnit)) continue;
                }

                if (_playerUnits[i] is LStructure) continue;

                _playerUnits[i].OnMouseDown();
                _currSelectedUnitIndex = i;
                return;
            }
        }

        _currSelectedUnitIndex = 0;
    }

    private void SelectPreviousAvailableUnit()
    {
        Unit selectedUnit = null;
        if (ObjectHolder.Instance != null) selectedUnit = ObjectHolder.Instance.CurrSelectedUnit;
        if (CellGrid.Instance != null && CellGrid.Instance.CurrentPlayerNumber == 0)
        {
            for (int i = _currSelectedUnitIndex; i >= 0; i--)
            {
                if (_playerUnits[i] == null) continue;
                if (_playerUnits[i].ActionPoints == 0) continue;
                if (selectedUnit is not null)
                {
                    if (_playerUnits[i].Equals(selectedUnit)) continue;
                }

                if (_playerUnits[i] is LStructure) continue;

                _playerUnits[i].OnMouseDown();
                _currSelectedUnitIndex = i;
                return;
            }
        }

        _currSelectedUnitIndex = 0;
    }
}