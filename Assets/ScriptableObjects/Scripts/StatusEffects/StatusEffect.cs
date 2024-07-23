using System;
using UnityEngine;

[Serializable]
public struct StatusEffect
{
    [SerializeField] private StatusEffectType _statusEffectType;
    [SerializeField] private bool _isApplied;
    [SerializeField] private int _durationInTurns;

    public bool IsApplied
    {
        get => _isApplied;
        set => _isApplied = value;
    }

    public int DurationInTurns
    {
        get => _durationInTurns;
        set => _durationInTurns = value;
    }

    public StatusEffectType StatusEffectType => _statusEffectType;
}