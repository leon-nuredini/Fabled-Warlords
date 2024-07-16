using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectsController : MonoBehaviour
{
    private LUnit _lUnit;

    [SerializeField] private List<StatusEffect> _statusEffectList;

    #region Properties

    public List<StatusEffect> StatusEffectList => _statusEffectList;

    #endregion

    private void Awake() => _lUnit = GetComponent<LUnit>();
    private void OnEnable() => _lUnit.OnTurnEndUnitReset += DecreaseStatusEffectDuration;
    private void OnDisable() => _lUnit.OnTurnEndUnitReset -= DecreaseStatusEffectDuration;

    public void ApplyStatusEffect<T>(int turns)
    {
        for (int i = 0; i < _statusEffectList.Count; i++)
        {
            StatusEffect statusEffect = _statusEffectList[i];
            if (statusEffect.StatusEffectType is T)
            {
                statusEffect.IsApplied = true;
                statusEffect.DurationInTurns = turns;
                _statusEffectList[i] = statusEffect;
            }
        }
    }

    private void DecreaseStatusEffectDuration()
    {
        for (int i = 0; i < _statusEffectList.Count; i++)
        {
            StatusEffect statusEffect = _statusEffectList[i];
            if (statusEffect.IsApplied)
            {
                statusEffect.DurationInTurns--;
                if (statusEffect.DurationInTurns <= 0)
                    statusEffect.IsApplied = false;
                _statusEffectList[i] = statusEffect;
            }
        }
    }

    public bool IsStatusApplied<T>()
    {
        for (int i = 0; i < _statusEffectList.Count; i++)
        {
            StatusEffect statusEffect = _statusEffectList[i];
            if (statusEffect.StatusEffectType is T && statusEffect.IsApplied)
                return true;
        }

        return false;
    }

    #region Getters

    public T GetStatus<T>()
    {
        for (int i = 0; i < _statusEffectList.Count; i++)
        {
            StatusEffect statusEffect = _statusEffectList[i];
            if (statusEffect is T t)
                return t;
        }

        return default(T);
    }

    #endregion
}