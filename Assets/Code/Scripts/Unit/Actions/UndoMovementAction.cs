using NaughtyAttributes;
using TbsFramework.Cells;
using TbsFramework.Grid;
using UnityEngine;

public class UndoMovementAction : MonoBehaviour
{
    [SerializeField] private LUnit _lUnit;
    [SerializeField] private Cell _startingCell;

    [SerializeField] private GameObject _undoMovementGobj;

    [SerializeField] private bool _isUndoingMovement;
    [SerializeField] private bool _disableUndoMovement;

    private UnitDirection _unitDirection;

    public bool IsUndoingMovement
    {
        get => _isUndoingMovement;
        set => _isUndoingMovement = value;
    }

    public UnitDirection UnitDirection
    {
        get => _unitDirection;
        set => _unitDirection = value;
    }

    public bool DisableUndoMovement
    {
        get => _disableUndoMovement;
        set => _disableUndoMovement = value;
    }

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
        UnitDirection = _lUnit.CurrentUnitDirection;
        DisableUndoMovement = false;
    }

    [Button("Test Undo movement")]
    private void UndoMovement()
    {
        if (MovementUndoController.Instance.LastMovedUnit != this) return;
        if (DisableUndoMovement) return;
        IsUndoingMovement = true;
        if (_startingCell == null) return;
        _lUnit.SetMovementPoints(_lUnit.TotalMovementPoints);
        var path = _lUnit.FindPath(CellGrid.Instance.Cells, _startingCell);
        StartCoroutine(_lUnit.Move(_startingCell, path));
        MovementUndoController.Instance.Reset();
        if (ObjectHolder.Instance != null)
            ObjectHolder.Instance.CurrSelectedUnit = null;

        for (int i = 0; i < CellGrid.Instance.Cells.Count; i++)
            CellGrid.Instance.Cells[i].UnMark();
    }
}