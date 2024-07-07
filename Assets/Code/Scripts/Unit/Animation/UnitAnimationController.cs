using System;
using NaughtyAttributes;
using UnityEngine;

public class UnitAnimationController : MonoBehaviour
{
    [BoxGroup("Idle")] [SerializeField] private UnitAnimation _idleHorizontal;
    [BoxGroup("Idle")] [SerializeField] private UnitAnimation _idleUp;
    [BoxGroup("Idle")] [SerializeField] private UnitAnimation _idleDown;
    [BoxGroup("Running")] [SerializeField] private UnitAnimation _runningHorizontal;
    [BoxGroup("Running")] [SerializeField] private UnitAnimation _runningUp;
    [BoxGroup("Running")] [SerializeField] private UnitAnimation _runningDown;
    [BoxGroup("Attack")] [SerializeField] private UnitAnimation _attackHorizontal;
    [BoxGroup("Attack")] [SerializeField] private UnitAnimation _attackUp;
    [BoxGroup("Attack")] [SerializeField] private UnitAnimation _attackDown;
    [BoxGroup("Death")] [SerializeField] private UnitAnimation _deathHorizontal;
    [BoxGroup("Death")] [SerializeField] private UnitAnimation _deathUp;
    [BoxGroup("Death")] [SerializeField] private UnitAnimation _deathDown;

    private Animator _animator;
    private LUnit _unit;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _unit = GetComponent<LUnit>();
    }

    private void OnEnable()
    {
        if (_unit != null)
        {
            _unit.OnIdle += PlayIdleAnimation;
            _unit.OnMove += PlayRunningAnimation;
            _unit.OnAttack += PlayAttackAnimation;
            _unit.OnDie += PlayDieAnimation;
        }
    }

    private void OnDisable()
    {
        if (_unit != null)
        {
            _unit.OnIdle -= PlayIdleAnimation;
            _unit.OnMove -= PlayRunningAnimation;
            _unit.OnAttack -= PlayAttackAnimation;
            _unit.OnDie -= PlayDieAnimation;
        }
    }

    private void PlayIdleAnimation(UnitDirection direction = UnitDirection.Right)
    {
        switch (direction)
        {
            case UnitDirection.Up:
                ExecuteAnimation(_idleUp);
                break;
            case UnitDirection.Down:
                ExecuteAnimation(_idleDown);
                break;
            default:
                ExecuteAnimation(_idleHorizontal);
                break;
        }
    }

    private void PlayRunningAnimation(UnitDirection direction = UnitDirection.Right)
    {
        switch (direction)
        {
            case UnitDirection.Up:
                ExecuteAnimation(_runningUp);
                break;
            case UnitDirection.Down:
                ExecuteAnimation(_runningDown);
                break;
            default:
                ExecuteAnimation(_runningHorizontal);
                break;
        }
    }

    private void PlayAttackAnimation(UnitDirection direction = UnitDirection.Right)
    {
        switch (direction)
        {
            case UnitDirection.Up:
                ExecuteAnimation(_attackUp);
                break;
            case UnitDirection.Down:
                ExecuteAnimation(_attackDown);
                break;
            default:
                ExecuteAnimation(_attackHorizontal);
                break;
        }
    }

    private void PlayDieAnimation(UnitDirection direction = UnitDirection.Right)
    {
        switch (direction)
        {
            case UnitDirection.Up:
                ExecuteAnimation(_deathUp);
                break;
            case UnitDirection.Down:
                ExecuteAnimation(_deathDown);
                break;
            default:
                ExecuteAnimation(_deathHorizontal);
                break;
        }
    }

    private void ExecuteAnimation(UnitAnimation unitAnimation) => unitAnimation.PlayAnimation(_animator);
}