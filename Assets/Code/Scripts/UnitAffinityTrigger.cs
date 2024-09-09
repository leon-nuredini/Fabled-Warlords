using System;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Grid;
using UnityEngine;

public class UnitAffinityTrigger : MonoBehaviour
{
    [SerializeField] private List<LUnit> _enemyPlayerUnits = new List<LUnit>();

    private void OnEnable()
    {
        if (CellGrid.Instance != null)
        {
            CellGrid.Instance.TurnStarted += UpdateEnemyUnitList;
            CellGrid.Instance.TurnEnded += DisableCounterVisuals;
        }

        if (ObjectHolder.Instance != null)
        {
            ObjectHolder.Instance.OnSelectUnit += OnSelectedUnit;
            ObjectHolder.Instance.OnDeselectUnit += DisableCounterVisuals;
        }
    }

    private void OnDisable()
    {
        if (CellGrid.Instance != null)
        {
            CellGrid.Instance.TurnStarted -= UpdateEnemyUnitList;   
            CellGrid.Instance.TurnEnded -= DisableCounterVisuals;
        }

        if (ObjectHolder.Instance != null)
        {
            ObjectHolder.Instance.OnSelectUnit -= OnSelectedUnit;
            ObjectHolder.Instance.OnDeselectUnit -= DisableCounterVisuals;
        }
    }
    
    private void Start() => UpdateEnemyUnitList(null, null);

    private void UpdateEnemyUnitList(object o, EventArgs args)
    {
        if (CellGrid.Instance == null) return;
        _enemyPlayerUnits = CellGrid.Instance.Units
            .Where(u => u is LUnit && u is not LStructure && u.PlayerNumber != CellGrid.Instance.CurrentPlayerNumber)
            .OfType<LUnit>()
            .ToList();
    }

    private void OnSelectedUnit(LUnit lUnit)
    {
        for (int i = 0; i < _enemyPlayerUnits.Count; i++)
        {
            if (_enemyPlayerUnits[i] == null) continue;
            _enemyPlayerUnits[i].DisplayCounterIcon(lUnit);
        }
    }

    private void DisableCounterVisuals(object o, bool args)
    {
        for (int i = 0; i < _enemyPlayerUnits.Count; i++)
        {
            if (_enemyPlayerUnits[i] == null) continue;
            _enemyPlayerUnits[i].DisableCounterIcon();
        }
    }
}