using TbsFramework.Units;
using UnityEngine;

public class StoneElemental : LUnit
{
    protected override int Defend(Unit other, int damage)
    {
        Agressor = other;
        float defenceFactor = CalculateDefense();
        float defenceAmount = damage * defenceFactor;
        int newDamage = Mathf.RoundToInt(damage - defenceAmount);

        if (StatusEffectsController.IsStatusApplied<Weaken>())
        {
            float weakenedFactor = StatusEffectsController.GetStatus<Weaken>().weakenFactor;
            newDamage = Mathf.RoundToInt(newDamage * weakenedFactor);
        }

        bool isRetalationResilenceActive = TryUseRetaliationResilence();
        if (isRetalationResilenceActive) newDamage /= 2;
        if (newDamage <= 0) newDamage = 1;
        TempDamageReceived = newDamage;
        return newDamage;
    }

    protected override float CalculateDefense()
    {
        float defenceAmount = 0;
        for (int i = 0; i < DefendSkillArray.Length; i++)
        {
            if (DefendSkillArray[i] is StoneWillSkill)
                defenceAmount = DefendSkillArray[i].GetDefenceAmount();
        }

        return defenceAmount;
    }
}