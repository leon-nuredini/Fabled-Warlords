using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class StatusEffectsController : MonoBehaviour
{
    private LUnit _lUnit;

    [SerializeField] private List<StatusEffect> _statusEffectList;

    private GameObject _spawnedStunEffect;

    #region Properties

    public List<StatusEffect> StatusEffectList => _statusEffectList;

    #endregion

    #region Poison

    [SerializeField] private Color _poisonedTintColor;
    private Color _startingColor;

    #endregion

    private void Awake()
    {
        _lUnit = GetComponent<LUnit>();
        _startingColor = _lUnit.MaskSpriteRenderer.color;
    }

    private void OnEnable()
    {
        _lUnit.OnTurnEndUnitReset += DecreaseStatusEffectDuration;
        _lUnit.OnDie += ResetStatusEffects;
    }

    private void OnDisable()
    {
        _lUnit.OnTurnEndUnitReset -= DecreaseStatusEffectDuration;
        _lUnit.OnDie -= ResetStatusEffects;
    }

    public void ApplyStatusEffect<T>(int turns)
    {
        if (_lUnit is LStructure) return;
        
        for (int i = 0; i < _statusEffectList.Count; i++)
        {
            StatusEffect statusEffect = _statusEffectList[i];
            if (statusEffect.StatusEffectType is T)
            {
                statusEffect.IsApplied = true;
                if (statusEffect.DurationInTurns < turns)
                    statusEffect.DurationInTurns = turns;
                _statusEffectList[i] = statusEffect;
                TryToSpawnStatusEffect(statusEffect.StatusEffectType);
                TryToApplyStatusEffectColor(statusEffect);
            }
        }
    }

    private void DecreaseStatusEffectDuration()
    {
        if (_lUnit is LStructure) return;
        
        for (int i = 0; i < _statusEffectList.Count; i++)
        {
            StatusEffect statusEffect = _statusEffectList[i];
            if (statusEffect.IsApplied)
            {
                statusEffect.DurationInTurns--;
                if (statusEffect.DurationInTurns <= 0)
                {
                    statusEffect.IsApplied = false;
                    TryToDespawnStatusEffect(statusEffect.StatusEffectType);
                    TryToApplyStatusEffectColor(statusEffect);
                }

                _statusEffectList[i] = statusEffect;
            }
        }
    }
    

    public void ResetStatusEffects(UnitDirection unitDirection)
    {
        if (_lUnit is LStructure) return;
        
        for (int i = 0; i < _statusEffectList.Count; i++)
        {
            StatusEffect statusEffect = _statusEffectList[i];
            if (statusEffect.IsApplied)
            {
                statusEffect.DurationInTurns = 0;
                statusEffect.IsApplied = false;
                TryToDespawnStatusEffect(statusEffect.StatusEffectType);
                _statusEffectList[i] = statusEffect;
            }
        }
    }

    public bool IsStatusApplied<T>()
    {
        if (_lUnit is LStructure) return false;
        
        for (int i = 0; i < _statusEffectList.Count; i++)
        {
            StatusEffect statusEffect = _statusEffectList[i];
            if (statusEffect.StatusEffectType is T && statusEffect.IsApplied)
                return true;
        }

        return false;
    }

    #region Effect spawning/despawning

    private void TryToSpawnStatusEffect(StatusEffectType statusEffectType)
    {
        if (_lUnit is LStructure) return;
        if (statusEffectType.Effect == null) return;
        if (statusEffectType is Stun stun)
        {
            Vector3 spawnPosition = transform.localPosition;
            spawnPosition.y += .8f;
            if (_spawnedStunEffect == null)
                _spawnedStunEffect = LeanPool.Spawn(stun.Effect, spawnPosition, Quaternion.identity);
        }
    }

    private void TryToDespawnStatusEffect(StatusEffectType statusEffectType)
    {
        if (_lUnit is LStructure) return;
        if (statusEffectType.Effect == null) return;
        if (statusEffectType is Stun stun)
        {
            if (_spawnedStunEffect != null)
            {
                LeanPool.Despawn(_spawnedStunEffect);
                _spawnedStunEffect = null;
            }
        }
    }

    #endregion

    private void TryToApplyStatusEffectColor(StatusEffect statusEffect)
    {
        if (_lUnit is LStructure) return;
        if (statusEffect.StatusEffectType is Poison)
            _lUnit.MaskSpriteRenderer.material.color = statusEffect.IsApplied ? _poisonedTintColor : _startingColor;
    }

    #region Getters

    public T GetStatus<T>() where T : class
    {
        if (_lUnit is LStructure) return null;
        for (int i = 0; i < _statusEffectList.Count; i++)
        {
            StatusEffect statusEffect = _statusEffectList[i];
            if (statusEffect.StatusEffectType is T t)
                return t;
        }

        return null;
    }

    #endregion
}