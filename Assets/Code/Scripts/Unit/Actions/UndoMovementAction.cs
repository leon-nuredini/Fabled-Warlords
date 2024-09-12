using System;
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

    [SerializeField] private bool _isMovementPerformed;
    [SerializeField] private bool _isUndoingMovement;
    [SerializeField] private bool _disableUndoMovement;

    private UndoButton _undoButton;

    private UnitDirection _unitDirection;

    #region Properties

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

    public bool IsMovementPerformed
    {
        get => _isMovementPerformed;
        set
        {
            if (_disableUndoMovement)
                value = false;
            _isMovementPerformed = value;
        }
    }

    public bool IsUndoMovementButtonEnabled => _undoButton != null && _undoButton.gameObject.activeSelf;

    #endregion

    private void Awake()
    {
        _lUnit = GetComponent<LUnit>();
        _undoButton = GetComponentInChildren<UndoButton>(true);
    }

    private void OnEnable()
    {
        _lUnit.OnTurnStartEvent += SetStartingCell;
        _lUnit.UnitMoved += OnUnitMoved;
        _lUnit.UnitClicked += OnUnitClicked;
        _lUnit.UnitDeselected += DisableUndoButton;

        if (_undoButton != null)
            _undoButton.OnClickUndoButton += UndoMovement;
    }

    private void OnDisable()
    {
        _lUnit.OnTurnStartEvent -= SetStartingCell;
        _lUnit.UnitMoved -= OnUnitMoved;
        _lUnit.UnitClicked -= OnUnitClicked;
        _lUnit.UnitDeselected -= DisableUndoButton;

        if (_undoButton != null)
            _undoButton.OnClickUndoButton -= UndoMovement;
    }

    private void OnUnitClicked(object sender, EventArgs args) => ShowUndoGraphic();
    private void OnUnitMoved(object sender, MovementEventArgs movementEventArgs) => ShowUndoGraphic();

    private void ShowUndoGraphic()
    {
        if (DisableUndoMovement) return;
        if (!IsMovementPerformed) return;
        if (_undoButton is null) return;
        if (IsUndoingMovement) return;
        _undoButton.gameObject.SetActive(true);
    }

    private void DisableUndoButton(object sender, EventArgs args)
    {
        _undoButton.gameObject.SetActive(false);
    }

    private void SetStartingCell()
    {
        _startingCell = _lUnit.Cell;
        UnitDirection = _lUnit.CurrentUnitDirection;
        DisableUndoMovement = false;
    }

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