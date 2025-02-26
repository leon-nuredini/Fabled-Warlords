using Singleton;
using UnityEngine;
using System;
using System.Collections;
using NaughtyAttributes;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Units.Abilities;

public class ObjectHolder : SceneSingleton<ObjectHolder>
{
    public event Action<LUnit> OnSelectUnit;
    public event EventHandler<bool> OnDeselectUnit;
    public event Action<LSquare> OnSelectCell;

    [BoxGroup("Unit")] [SerializeField] private LUnit _currentSelectedUnit;
    [BoxGroup("Unit")] [SerializeField] private LUnit _currentEnemyMarkedUnit;

    [BoxGroup("Cell")] [SerializeField] private LSquare _currentSelectedSquare;
    [BoxGroup("Cell")] [SerializeField] private float _cellResetDuration = 0.15f;

    private Coroutine _coroutine;
    private WaitForSeconds _wait;

    #region Properties

    public LUnit CurrSelectedUnit
    {
        get => _currentSelectedUnit;
        set
        {
            if (_currentSelectedUnit is not null && _currentSelectedUnit != value &&
                _currentSelectedUnit.UndoMovementAction != null)
                _currentSelectedUnit.UndoMovementAction.DisableUndoButton();

            _currentSelectedUnit = value;
            if (_currentSelectedUnit == null)
            {
                OnDeselectUnit?.Invoke(this, true);
                return;
            }

            OnSelectUnit?.Invoke(_currentSelectedUnit);
        }
    }

    public LUnit CurrentEnemyMarkedUnit
    {
        get => _currentEnemyMarkedUnit;
        set => _currentEnemyMarkedUnit = value;
    }

    public LSquare CurrentSelectedSquare
    {
        get => _currentSelectedSquare;
        set => _currentSelectedSquare = value;
    }

    #endregion

    private void Awake() => _wait = new WaitForSeconds(_cellResetDuration);

    private void OnEnable()
    {
        LSquare.OnAnyClickCell += OnCellSelected;
        LSquare.OnAnyRightClickCell += OnCellSelectedAlternative;
    }

    private void OnDisable()
    {
        LSquare.OnAnyClickCell -= OnCellSelected;
        LSquare.OnAnyRightClickCell -= OnCellSelectedAlternative;
    }

    private void OnCellSelected(LSquare selectedCell)
    {
        if (selectedCell.TerrainDescription == null) return;

        if (_coroutine != null) StopCoroutine(_coroutine);

        if (CheckCellSelectCondition(selectedCell))
        {
            _currentSelectedSquare = null;
            OnSelectCell?.Invoke(selectedCell);
            return;
        }

        _currentSelectedSquare = selectedCell;
        _coroutine = StartCoroutine(ResetSelectedCell());
    }

    private void OnCellSelectedAlternative(LSquare selectedCell)
    {
        if (selectedCell.TerrainDescription == null) return;
        if (CurrSelectedUnit != null)
        {
            CurrSelectedUnit.UnmarkSelection();
            MoveAbility moveAbility = CurrSelectedUnit.GetComponent<MoveAbility>();
            moveAbility.CleanUp(CellGrid.Instance);
            if (CellGrid.Instance != null)
                CellGrid.Instance.cellGridState = new CellGridStateWaitingForInput(CellGrid.Instance);
            CurrSelectedUnit = null;
        }

        _currentSelectedSquare = null;
        OnSelectCell?.Invoke(selectedCell);
    }

    private bool CheckCellSelectCondition(LSquare selectedCell)
    {
        if (_currentSelectedSquare == null) return false;
        return _currentSelectedSquare.Equals(selectedCell);
    }

    private IEnumerator ResetSelectedCell()
    {
        yield return _wait;
        _currentSelectedSquare = null;
    }
}