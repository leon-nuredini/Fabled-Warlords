using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using TbsFramework.Units.Abilities;
using UnityEngine;

public class MovementUndoController : SceneSingleton<MovementUndoController>
{
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

    private void OnEnable()
    {
        AttackAbility.OnAnyAbilityPerformed += DisableUndoActionOnUnit;
    }
    
    private void OnDisable()
    {
        AttackAbility.OnAnyAbilityPerformed -= DisableUndoActionOnUnit;
    }

    private void DisableUndoActionOnUnit()
    {
        if (_lastMovedUnit is not null)
        {
            _lastMovedUnit.DisableUndoMovement = true;
            Reset();
        }
    }

    public void Reset()
    {
        _lastMovedUnit = null;
    }
}