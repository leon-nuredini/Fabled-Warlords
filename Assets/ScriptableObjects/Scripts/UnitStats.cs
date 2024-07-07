using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "UnitStats/Unit", order = 0)]
public class UnitStats : ScriptableObject
{
    [SerializeField]                 private int   _hitPoints;
    [SerializeField]                 private int   _attackRange;
    [SerializeField]                 private int   _attackFactor;
    [SerializeField]                 private int   _movementPoints;
    [SerializeField]                 private float _movementAnimationSpeed;
    [SerializeField]                 private int   _actionPoints;
    [Range(0, 100)] [SerializeField] private int   _evasionFactor;
    [SerializeField]                 private int   _cost;
    [SerializeField]                 private int   _upkeep;

    public int HitPoints    => _hitPoints;
    public int AttackRange  => _attackRange;
    public int AttackFactor => _attackFactor;
    public int   MovementPoints         => _movementPoints;
    public float MovementAnimationSpeed => _movementAnimationSpeed;
    public int   ActionPoints           => _actionPoints;
    public int   EvasionFactor          => _evasionFactor;
    public int   Cost                   => _cost;
    public int   Upkeep                 => _upkeep;
}