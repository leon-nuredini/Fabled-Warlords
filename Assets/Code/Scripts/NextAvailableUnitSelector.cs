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

    private Transform _cameraTransform;

    private void Awake() => _cameraTransform = Camera.main.transform;
    
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
        if (Input.GetKeyDown(KeyCode.X))
            SelectNextAvailableUnit();
        else if (Input.GetKeyDown(KeyCode.Z))
            SelectPreviousAvailableUnit();
    }
    
    private void SelectNextAvailableUnit()
    {
        if (_currSelectedUnitIndex == _playerUnits.Count - 1) _currSelectedUnitIndex = 0;
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

                SelectUnit(_playerUnits[i], i);
                return;
            }
        }

        _currSelectedUnitIndex = 0;
    }

    private void SelectPreviousAvailableUnit()
    {
        if (_currSelectedUnitIndex == 0) _currSelectedUnitIndex = _playerUnits.Count - 1;
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

                SelectUnit(_playerUnits[i], i);
                return;
            }
        }

        _currSelectedUnitIndex = _playerUnits.Count - 1;
    }

    private void SelectUnit(Unit unit, int unitIndex)
    {
        unit.OnMouseDown();
        _currSelectedUnitIndex = unitIndex;
        PositionCameraOnUnit(unit.transform.position);
    }

    private void PositionCameraOnUnit(Vector3 unitPosition)
    {
        Vector3 newCameraPosition = _cameraTransform.position;
        newCameraPosition.x = unitPosition.x;
        newCameraPosition.y = unitPosition.y;
        _cameraTransform.position = newCameraPosition;
    }
}