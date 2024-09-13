using Singleton;
using TbsFramework.Units.Abilities;
using UnityEngine;

public class MovementUndoController : SceneSingleton<MovementUndoController>
{
    [SerializeField] private bool _isEnabled = true;
    [SerializeField] private UndoMovementAction _lastMovedUnit;

    public UndoMovementAction LastMovedUnit
    {
        get => _lastMovedUnit;
        set
        {
            if (_lastMovedUnit is not null && !_lastMovedUnit.Equals(value))
                _lastMovedUnit.DisableUndoMovement = true;
            _lastMovedUnit = value;
        }
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set => _isEnabled = value;
    }

    private void OnEnable()
    {
        AttackAbility.OnAnyAbilityPerformed += DisableUndoActionOnUnit;
        RecruitmentController.OnAnyNewUnitRecruited += DisableUndoActionOnUnit;
    }

    private void OnDisable()
    {
        AttackAbility.OnAnyAbilityPerformed -= DisableUndoActionOnUnit;
        RecruitmentController.OnAnyNewUnitRecruited -= DisableUndoActionOnUnit;
    }

    private void DisableUndoActionOnUnit()
    {
        if (_lastMovedUnit is not null)
        {
            _lastMovedUnit.DisableUndoMovement = true;
            _lastMovedUnit.DisableUndoButton();
            Reset();
        }
    }

    public void Reset() => _lastMovedUnit = null;
}