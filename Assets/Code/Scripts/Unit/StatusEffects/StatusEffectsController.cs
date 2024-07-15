using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsController : MonoBehaviour
{
    private LUnit _lUnit;

    [SerializeField] private List<StatusEffect> _statusEffectList;

    #region Properties

    public List<StatusEffect> StatusEffectList => _statusEffectList;

    #endregion

    private void Awake()     => _lUnit = GetComponent<LUnit>();
    private void OnEnable()  => _lUnit.OnTurnEndUnitReset += DecreaseStatusEffectDuration;
    private void OnDisable() => _lUnit.OnTurnEndUnitReset -= DecreaseStatusEffectDuration;

    public void ApplyStatusEffect<T>(int turns)
    {
        for (int i = 0; i < _statusEffectList.Count; i++)
        {
            StatusEffect statusEffect = _statusEffectList[i];
            if (statusEffect.StatusEffectType is T)
            {
                statusEffect.IsApplied       = true;
                statusEffect.DurationInTurns = turns;
                _statusEffectList[i]         = statusEffect;
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

    public bool IsWeakenApplied()
    {
        for (int i = 0; i < _statusEffectList.Count; i++)
        {
            StatusEffect statusEffect = _statusEffectList[i];
            if (statusEffect.StatusEffectType is Weaken && statusEffect.IsApplied)
                return true;
        }

        return false;
    }
}