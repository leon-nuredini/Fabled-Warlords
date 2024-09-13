using System;
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

    private float _remainingMovePoints;

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

    public Cell StartingCell
    {
        get => _startingCell;
        set => _startingCell = value;
    }

    public float RemainingMovePoints
    {
        get => _remainingMovePoints;
        set => _remainingMovePoints = value;
    }

    #endregion

    private void Awake()
    {
        _lUnit = GetComponent<LUnit>();
        _undoButton = GetComponentInChildren<UndoButton>(true);
    }

    private void OnEnable()
    {
        _lUnit.OnTurnStartEvent += UpdateStartingCell;
        _lUnit.UnitMoved += OnUnitMoved;
        _lUnit.UnitClicked += OnUnitClicked;
        _lUnit.OnTurnEndUnitReset += OnTurnEnd;
        _lUnit.OnStartedMoving += DisableUndoButton;

        if (_undoButton != null)
            _undoButton.OnClickUndoButton += UndoMovement;
    }

    private void OnDisable()
    {
        _lUnit.OnTurnStartEvent -= UpdateStartingCell;
        _lUnit.UnitMoved -= OnUnitMoved;
        _lUnit.UnitClicked -= OnUnitClicked;
        _lUnit.OnTurnEndUnitReset -= OnTurnEnd;
        _lUnit.OnStartedMoving -= DisableUndoButton;

        if (_undoButton != null)
            _undoButton.OnClickUndoButton -= UndoMovement;
    }

    private void OnUnitClicked(object sender, EventArgs args) => ShowUndoGraphic();
    private void OnUnitMoved(object sender, MovementEventArgs movementEventArgs) => ShowUndoGraphic();

    private void ShowUndoGraphic()
    {
        if (_lUnit.PlayerNumber != 0) return;
        if (DisableUndoMovement) return;
        if (!IsMovementPerformed) return;
        if (_undoButton is null) return;
        if (IsUndoingMovement) return;
        if (!MovementUndoController.Instance.IsEnabled) return;
        if (_lUnit.IsMoving)
        {
            DisableUndoButton();
            return;
        }

        _undoButton.gameObject.SetActive(true);
    }

    private void OnTurnEnd()
    {
        if (_lUnit.PlayerNumber != 0) return;
        if (!MovementUndoController.Instance.IsEnabled) return;
        DisableUndoMovement = false;
        IsMovementPerformed = false;
        IsUndoingMovement = false;
        DisableUndoButton();
    }

    public void DisableUndoButton()
    {
        if (_lUnit.PlayerNumber != 0) return;
        if (!MovementUndoController.Instance.IsEnabled) return;
        _undoButton.gameObject.SetActive(false);
    }

    public void UpdateStartingCell()
    {
        if (_lUnit.PlayerNumber != 0) return;
        if (!MovementUndoController.Instance.IsEnabled) return;
        StartingCell = _lUnit.Cell;
        UnitDirection = _lUnit.CurrentUnitDirection;
        DisableUndoMovement = false;
        _remainingMovePoints = _lUnit.MovementPoints;
    }

    private void UndoMovement()
    {
        if (_lUnit.IsMoving) return;
        if (MovementUndoController.Instance.LastMovedUnit != this) return;
        if (DisableUndoMovement) return;
        IsUndoingMovement = true;
        if (StartingCell == null) return;
        _lUnit.SetMovementPoints(_remainingMovePoints);
        var path = _lUnit.FindPath(CellGrid.Instance.Cells, StartingCell);
        StartCoroutine(_lUnit.Move(StartingCell, path));
        MovementUndoController.Instance.Reset();
        if (ObjectHolder.Instance != null)
            ObjectHolder.Instance.CurrSelectedUnit = null;

        for (int i = 0; i < CellGrid.Instance.Cells.Count; i++)
            CellGrid.Instance.Cells[i].UnMark();
    }
}