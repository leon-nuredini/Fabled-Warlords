using TbsFramework.Units;
using UnityEngine;

public class Deer : LUnit, IMonster
{
    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is LUnit lUnit && lUnit.UnitClassCounter != null)
            newDamage *= lUnit.UnitClassCounter.VSMonsterFactor;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }

    protected override int CalculateDamage(AttackAction baseVal, Unit unitToAttack)
    {
        float totalFactorDamage = 0;
        int baseDamage = baseVal.Damage;

        if (StatusEffectsController.IsStatusApplied<Weaken>())
        {
            float weakenedFactor = StatusEffectsController.GetStatus<Weaken>().weakenFactor;
            baseDamage = Mathf.RoundToInt(baseDamage * weakenedFactor);
        }

        if (!IsEvaluating)
        {
            for (int i = 0; i < AttackSkillArray.Length; i++)
            {
                if (IsRetaliating && !AttackSkillArray[i].CanBeActivatedDuringEnemyTurn) continue;
                if (AttackSkillArray[i] is ChargeSkill chargeSkill && unitToAttack is not LStructure)
                    totalFactorDamage += AttackSkillArray[i].GetDamageFactor();
            }
        }

        int factoredDamage = totalFactorDamage > 0 ? baseDamage * (int)totalFactorDamage : baseDamage;
        return factoredDamage;
    }
}