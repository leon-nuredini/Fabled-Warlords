using UnityEngine;

public class HealUnitSoundEvent : BaseSoundEvent
{
    private IAOEHealingSkill _healingSkill;

    private void Awake() => _healingSkill = GetComponent<IAOEHealingSkill>();

    private void OnEnable()
    {
        switch (_healingSkill)
        {
            case AOEHealingSkill aoeHealingSkill:
                aoeHealingSkill.OnHeal += TryToPlaySoundEvent;
                break;
            case VillageHealingSkill villageHealingSkill:
                villageHealingSkill.OnHeal += TryToPlaySoundEvent;
                break;
        }
    }

    private void OnDisable()
    {
        switch (_healingSkill)
        {
            case AOEHealingSkill aoeHealingSkill:
                aoeHealingSkill.OnHeal -= TryToPlaySoundEvent;
                break;
            case VillageHealingSkill villageHealingSkill:
                villageHealingSkill.OnHeal -= TryToPlaySoundEvent;
                break;
        }
    }

    private void TryToPlaySoundEvent(Transform[] spawnPositionArray)
    {
        if (spawnPositionArray.Length > 0)
            InvokeSoundEvent();
    }
}