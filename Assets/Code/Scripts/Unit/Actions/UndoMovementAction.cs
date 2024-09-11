using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Units;
using UnityEngine;

public class UndoMovementAction : MonoBehaviour
{
    [SerializeField] private LUnit _lUnit;
    [SerializeField] private Cell _startingCell;

    [SerializeField] private GameObject _undoMovementGobj;

    private void Awake() => _lUnit = GetComponent<LUnit>();

    private void OnEnable()
    {
        _lUnit.OnTurnStartEvent += SetStartingCell;
    }

    private void OnDisable()
    {
        _lUnit.OnTurnStartEvent -= SetStartingCell;
    }

    private void SetStartingCell()
    {
        _startingCell = _lUnit.Cell;
    }

    [Button("Test Undo movement")]
    private void UndoMovement()
    {
        if (_startingCell == null) return;
        _lUnit.SetMovementPoints(_lUnit.TotalMovementPoints);
        var path = _lUnit.FindPath(CellGrid.Instance.Cells, _startingCell);
        _lUnit.Move(_startingCell, path);
    }
}