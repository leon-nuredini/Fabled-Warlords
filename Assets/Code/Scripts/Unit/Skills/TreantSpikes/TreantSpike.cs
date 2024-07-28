using Lean.Pool;
using UnityEngine;
using System;

public class TreantSpike : MonoBehaviour
{
    public event Action OnDespawn;

    private LUnit _enemyUnit;

    public LUnit EnemyUnit
    {
        get => _enemyUnit;
        set
        {
            _enemyUnit = value;
            _enemyUnit.OnDie += OnUnitDie;
            _enemyUnit.OnTurnEndUnitReset += OnUnitTurnEnd;
        }
    }

    private void OnUnitDie(UnitDirection direction) => Despawn();

    private void OnUnitTurnEnd()
    {
        if (!_enemyUnit.StatusEffectsController.IsStatusApplied<Stun>())
            Despawn();
    }

    private void Despawn() => OnDespawn?.Invoke();
}